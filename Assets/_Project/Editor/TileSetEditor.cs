using ProjectAwakening.Overworld.WaveFunctionCollapse;
using UnityEditor;
using UnityEngine;

namespace ProjectAwakening.Editor
{
	[CustomEditor(typeof(TileSetScriptable))]
	public class TileSetEditor : UnityEditor.Editor
	{
		private TileSetScriptable targetScript;

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
}
