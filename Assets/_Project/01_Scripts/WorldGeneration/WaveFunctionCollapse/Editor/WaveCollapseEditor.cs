using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ProjectAwakening.WorldGeneration;

namespace ProjectAwakening.WorldGeneration.Editors
{
	[CustomEditor(typeof(WaveCollapseMapMaker))]
	public class WaveCollapseEditor : Editor
	{
		WaveCollapseMapMaker targetScript;

		private void Awake()
		{
			targetScript = (WaveCollapseMapMaker)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Create and draw map"))
			{
				targetScript.CreateMap();
				targetScript.CreateMapVisuals();
			}

			if (GUILayout.Button("CreateMap"))
			{
				targetScript.CreateMap();
			}

			if (GUILayout.Button("DrawMap"))
			{
				targetScript.CreateMapVisuals();
			}
		}
	}
}