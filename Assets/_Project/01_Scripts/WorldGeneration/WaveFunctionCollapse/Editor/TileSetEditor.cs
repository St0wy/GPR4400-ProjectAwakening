using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ProjectAwakening.WorldGeneration;

[CustomEditor(typeof(TileSetScriptable))]
public class TileSetEditor : Editor
{
    TileSetScriptable targetScript;

    private void Awake()
    {
        targetScript =  (TileSetScriptable) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Calculate Neighbours"))
        {
            targetScript.CalculateNeighbours();
        }

        if (GUILayout.Button("PrintTilesAndNeighbours"))
        {
            targetScript.PrintTiles();
        }
    }
}
