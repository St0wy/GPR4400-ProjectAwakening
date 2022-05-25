using UnityEngine;

namespace ProjectAwakening
{
	[CreateAssetMenu(fileName = "SharedTransform", menuName = "SharedValues/Transform", order = 0)]
    public class TransformReferenceScriptableObject: ScriptableObject
    {
		public Transform Transform { get ; private set; }

		public void SetReference(Transform newValue)
		{
			Transform = newValue;
		}
    }
}
