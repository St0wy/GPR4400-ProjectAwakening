using ProjectAwakening.Overworld.WorldDetailsGeneration;
using UnityEditor;

namespace ProjectAwakening.Editor
{
	[CustomEditor(typeof(SpecialElementsGenerator))]
	public class SpecialElementsGenEditor : UnityEditor.Editor
	{
		private SpecialElementsGenerator targetScript;

		private void Awake()
		{
			targetScript = (SpecialElementsGenerator) target;
		}
	}
}