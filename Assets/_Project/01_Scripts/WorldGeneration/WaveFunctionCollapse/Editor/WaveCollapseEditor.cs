using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ProjectAwakening.WorldGeneration;

[CustomEditor(typeof(WaveCollapseMapMaker))]
public class WaveCollapseEditor : Editor
{
    WaveCollapseMapMaker _targetScript;

    private void Awake()
    {
        _targetScript = (WaveCollapseMapMaker) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create and draw map"))
        {
            _targetScript.CreateMap();
            _targetScript.CreateMapVisuals();
        }

        if (GUILayout.Button("CreateMap"))
        {
            _targetScript.CreateMap();
        }

        if (GUILayout.Button("DrawMap"))
        {
            _targetScript.CreateMapVisuals();
        }
    }
}
