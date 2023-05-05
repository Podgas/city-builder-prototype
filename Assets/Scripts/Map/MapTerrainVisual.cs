using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTerrainVisual : MonoBehaviour
{
    [SerializeField]
    MapTerrain _terrain;

    public void SetTerrain(MapTerrain terrain)
    {
        _terrain = terrain;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
