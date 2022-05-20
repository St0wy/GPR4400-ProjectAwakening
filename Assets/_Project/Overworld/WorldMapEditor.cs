using UnityEditor;
using UnityEngine;

namespace ProjectAwakening.Overworld
{
	[CustomEditor(typeof(WorldMap))]
    public class WorldMapEditor : Editor
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
