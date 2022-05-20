using System;
using UnityEditor;

namespace ProjectAwakening.Overworld.WorldDetailsGeneration
{
    [CustomEditor(typeof(SpecialElementsGenerator))]
    public class SpecialElementsGenEditor : Editor
    {
	    private SpecialElementsGenerator targetScript;

        private void Awake()
        {
            targetScript = (SpecialElementsGenerator) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            throw new NotImplementedException($"Target: {targetScript}");
        }
    }
}
