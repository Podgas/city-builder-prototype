using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator)), CanEditMultipleObjects]
public class MapGeneratorEditor : Editor { 
    

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            mapGen.GenerateMap();
        }
        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
