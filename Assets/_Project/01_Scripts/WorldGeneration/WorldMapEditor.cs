using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectAwakening.WorldGeneration.Editors
{
	[CustomEditor(typeof(WorldMap))]
    public class WorldMapEditor : Editor
    {
		WorldMap targetScript;

		private void Awake()
		{
			targetScript = (WorldMap) target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("GENERATE"))
			{
				targetScript.Generate();
			}
		}
	}
}
