using ProjectAwakening.Overworld.WaveFunctionCollapse;
using UnityEditor;
using UnityEngine;

namespace ProjectAwakening.Editor
{
	[CustomEditor(typeof(WaveCollapseMapMaker))]
	public class WaveCollapseEditor : UnityEditor.Editor
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