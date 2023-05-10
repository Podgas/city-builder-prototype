using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WorldManager : MonoBehaviour
{

    [SerializeField]
    public int _width;
    [SerializeField]
    public int _height;
    [SerializeField]
    public int _depth;
    [SerializeField]
    public int _cellSize;
    [SerializeField]
    private Transform worldContainer;

    [SerializeField]
    public TerrainTypes[] regions;

    private Grid<MapTerrain> _worldGrid;
    private Vector3 worldCenter;
   

    [Space(10)]
    [Header("Noise")]
    [Space(10)]
    [SerializeField]
    private float noiseScale;

    [SerializeField]
    private int octaves;
    [SerializeField]
    [Range(0f, 1f)]
    private float persistance;
    [SerializeField]
    private float lacunarity;
    [SerializeField]
    private int seed;
    [SerializeField]
    private Vector2 offset;

    float[,] noiseMap;


    public static WorldManager Instance { get; private set; }

    private void Awake()
    {

        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        GenerateWorld();
    }
    public void InitializeGrid()
    {
        _worldGrid = new Grid<MapTerrain>(_width, _height, _depth, _cellSize, Vector3.zero, (Grid<MapTerrain> g, int x, int y, int z) => new MapTerrain(x,y,z, g, regions[0]));
    }
    public void GenerateWorld()
    {
        GenerateNoiseMap();
        ClearWorld();
        InitializeGrid();
        for(int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for(int z = 0; z < _depth; z++)
                {
                    float currentHeight = noiseMap[x, z];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (currentHeight <= regions[i].height)
                        {
                            _worldGrid.GetGridObject(x, y, z).SetTerrainType(regions[i]);
                            
                            GameObject go = GameObject.Instantiate(regions[i].prefab, _worldGrid.GetCellCenter(x, y, z), Quaternion.identity, worldContainer);
                            MapTerrainVisual mtv = go.GetComponent<MapTerrainVisual>();
                            _worldGrid.GetGridObject(x, y, z).SetMapTerrainVisual(mtv);
                            mtv.SetTerrain(_worldGrid.GetGridObject(x,y,z));
                            break;
                        }
                    }
                }
            }
        }
        worldCenter = _worldGrid.GetGridWorldCenter();
    }


    private void GenerateNoiseMap()
    {
        noiseMap = Noise.GenerateNoiseMap(_width, _depth, seed, noiseScale, octaves, persistance, lacunarity, offset);
    }
    public void ClearWorld()
    {
        IterateChildren(worldContainer.gameObject, delegate (GameObject go) { GameObject.DestroyImmediate(go); }, false);

    }
    public Vector3 GetWorldCenter()
    {
        return worldCenter;
    }
    public Grid<MapTerrain> GetGrid()
    {
        return _worldGrid;
    } 

    [System.Serializable]
   
    #region ITERATE PARENT CHILDREN TO CLEAR OBJECTS
    public delegate void ChildHandler(GameObject child);

    /// <summary>
    /// Iterates all children of a game object
    /// </summary>
    /// <param name="gameObject">A root game object</param>
    /// <param name="childHandler">A function to execute on each child</param>
    /// <param name="recursive">Do it on children? (in depth)</param>
    public static void IterateChildren(GameObject gameObject, ChildHandler childHandler, bool recursive)
    {
        DoIterate(gameObject, childHandler, recursive);
    }

    /// <summary>
    /// NOTE: Recursive!!!
    /// </summary>
    /// <param name="gameObject">Game object to iterate</param>
    /// <param name="childHandler">A handler function on node</param>
    /// <param name="recursive">Do it on children?</param>
    private static void DoIterate(GameObject gameObject, ChildHandler childHandler, bool recursive)
    {
        Transform[] childrens = gameObject.GetComponentsInChildren<Transform>();

        for (int i=1; i<childrens.Length; i++)
        {
            childHandler(childrens[i].gameObject);
            if (recursive)
                DoIterate(childrens[i].gameObject, childHandler, true);
        }
    }
    #endregion


}
[System.Serializable]
public struct TerrainTypes
{
    public string name;
    public float height;
    public GameObject prefab;
    
}
