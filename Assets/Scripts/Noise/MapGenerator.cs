using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap}
    public DrawMode drawmode;
    [SerializeField]
    private int mapWidth;
    [SerializeField]
    private int mapheight;
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

    public TerrainTipes[] regions;
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapheight, seed, noiseScale,octaves,persistance,lacunarity,offset);

        Color[] colorMap = new Color[mapWidth * mapheight];
        for(int y = 0; y < mapheight; y++)
        {
            for(int x=0; x<mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for(int i=0; i<regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        //colorMap[y*mapWidth+x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawmode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if(drawmode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap,mapWidth,mapheight));
        }
            
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapheight < 1)
        {
            mapheight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1; ;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

    }
}

[System.Serializable]
public struct TerrainTipes
{
    public string name;
    public float height;
    public GameObject prefab;
}
