using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ProjectAwakening.WorldGeneration;

[CustomEditor(typeof(TileSetScriptable))]
public class TileSetEditor : Editor
{
    TileSetScriptable _targetScript;

    private void Awake()
    {
        _targetScript =  (TileSetScriptable) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Calculate Neighbours"))
        {
            _targetScript.CalculateNeighbours();
        }

        if (GUILayout.Button("PrintTilesAndNeighbours"))
        {
            _targetScript.PrintTiles();
        }
    }
}
