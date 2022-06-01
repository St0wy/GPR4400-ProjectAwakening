using ProjectAwakening.Overworld;
using UnityEditor;
using UnityEngine;

namespace ProjectAwakening.Editor
{
	[CustomEditor(typeof(WorldMap))]
    public class WorldMapEditor : UnityEditor.Editor
    {
	    private WorldMap targetScript;

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
