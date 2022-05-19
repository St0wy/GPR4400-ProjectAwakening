using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProjectAwakening.WorldGeneration.Editors
{
    [CustomEditor(typeof(SpecialElementsGenerator))]
    public class SpecialElementsGenEditor : Editor
    {
        SpecialElementsGenerator targetScript;

        private void Awake()
        {
            targetScript = (SpecialElementsGenerator) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


        }
    }
}
