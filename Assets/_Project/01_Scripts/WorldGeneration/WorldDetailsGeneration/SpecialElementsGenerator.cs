using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.WorldGeneration
{
    public class SpecialElementsGenerator : MonoBehaviour
    {
		[Tooltip("GameObjects to generate once, in the largest area")]
		[SerializeField]
		List<GameObject> uniqueGameObjects;

		[SerializeField]
		Vector3 offset;

		[SerializeField]
		float minDistance;

		[SerializeField]
		int maxTries;

		GameObject iObjectsParent = null;

        public void GenerateElementsInArea(List<Vector3> possiblePositions)
		{
			if (Application.isEditor)
				DestroyImmediate(iObjectsParent);
			else
				Destroy(iObjectsParent);

			iObjectsParent = new GameObject();

			List<Vector3> selectedPos = new List<Vector3>();

			foreach(GameObject uGameObject in uniqueGameObjects)
			{
				Vector3 randPos;
				int tries = 0;

				do
				{
					randPos = possiblePositions[Random.Range(0, possiblePositions.Count)] + offset;
				}
				while (selectedPos.Exists(pos => Vector3.Distance(pos, randPos) < minDistance - tries++));

				Instantiate(uGameObject, randPos,
					Quaternion.identity, iObjectsParent.transform);
			}
		}
    }
}
