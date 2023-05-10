using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[System.Serializable]
public class MapTerrain
{
    [SerializeField]
    private Vector3Int _gridPosition;
    [SerializeField]
    private Grid<MapTerrain> _grid;
    [SerializeField]
    private TerrainTypes _terrainType;
    [SerializeField]
    MapTerrainVisual mapTerrainVisual;


    public MapTerrain(int x, int y, int z, Grid<MapTerrain> grid, TerrainTypes type)
    {
        _gridPosition = new Vector3Int(x,y,z);
        _grid = grid;
        _terrainType = type;

    }

    public void SetTerrainType(TerrainTypes type)
    {
        _terrainType = type;
    }
    public void SetMapTerrainVisual(MapTerrainVisual mtv)
    {
        mapTerrainVisual = mtv;
    }
    public Vector3 GetWorldPosition()
    {
        return _grid.GetWorldPosition(_gridPosition.x, _gridPosition.y, _gridPosition.z);
    }
    public TerrainTypes GetTerrainType()
    {
        return _terrainType;
    }

}
