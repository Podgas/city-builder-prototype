using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WorldManager))]
public class WorldEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WorldManager worldManager = (WorldManager)target;

        
        EditorGUI.BeginChangeCheck();


        worldManager._width = EditorGUILayout.IntField(new GUIContent("Width"), worldManager._width);
        worldManager._height = EditorGUILayout.IntField(new GUIContent("Height"), worldManager._height);
        worldManager._depth = EditorGUILayout.IntField(new GUIContent("Depth"), worldManager._depth);
        worldManager._cellSize = EditorGUILayout.IntField(new GUIContent("CellSize"), worldManager._cellSize);


        if (EditorGUI.EndChangeCheck())
        {

            worldManager.GenerateWorld();


        }

        if (GUILayout.Button("GenerateWorld"))
        {
            worldManager.GenerateWorld();
        }
        if (GUILayout.Button("Clear"))
        {
            worldManager.ClearWorld();
        }

        
    }
    
}
