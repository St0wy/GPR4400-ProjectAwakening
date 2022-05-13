using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.WorldGeneration
{
    public class SpecialElementsGenerator : MonoBehaviour
    {
		[SerializeField]
		GameObject player;

		[SerializeField]
		GameObject keyDungeon;

		[Tooltip("GameObjects to generate once, in the largest area")]
		[SerializeField]
		List<GameObject> uniqueGameObjects;

		[Header("Monster Gen")]
		[Tooltip("SpawnPoints possible")]
		[SerializeField]
		List<GameObject> spawners;

		[Tooltip("Total amount of monsters to generate")]
		[SerializeField]
		int monsters;

		[SerializeField]
		Vector3 offset;

		GameObject iObjectsParent = null;

        public void GenerateElementsInArea(List<Vector3> possiblePositions)
		{
			if (Application.isEditor)
				DestroyImmediate(iObjectsParent);
			else
				Destroy(iObjectsParent);

			iObjectsParent = new GameObject();

			List<Vector3> selectedPos = new List<Vector3>();

			//Add the player and the dungeon
			//Spawn the player
			Vector3 playerPos = possiblePositions[Random.Range(0, possiblePositions.Count)] + offset;
			Instantiate(player, playerPos,
					Quaternion.identity, iObjectsParent.transform);

			//Find the furthest point
			KeyValuePair<Vector3, float> furthestFromPlayer = new KeyValuePair<Vector3, float>();
			foreach (Vector3 point in possiblePositions)
			{
				float dist = Vector3.Distance(point, playerPos);
				if (dist > furthestFromPlayer.Value)
				{
					furthestFromPlayer = new KeyValuePair<Vector3, float>(point, dist);
				}
			}
			//Spawn the dungeon
			Instantiate(keyDungeon, furthestFromPlayer.Key + offset,
					Quaternion.identity, iObjectsParent.transform);

			//Generate unique Objects
			foreach (GameObject uGameObject in uniqueGameObjects)
			{
				Vector3 randPos;

				do
				{
					randPos = possiblePositions[Random.Range(0, possiblePositions.Count)] + offset;
				} while (selectedPos.Contains(randPos));

				selectedPos.Add(randPos);

				Instantiate(uGameObject, randPos,
					Quaternion.identity, iObjectsParent.transform);
			}

			//Generate monsters
			for (int m = 0; m < monsters; m++)
			{
				Vector3 randPos;

				do
				{
					randPos = possiblePositions[Random.Range(0, possiblePositions.Count)] + offset;
				} while (selectedPos.Contains(randPos));

				selectedPos.Add(randPos);
				Instantiate(spawners[Random.Range(0, spawners.Count)], randPos,
					Quaternion.identity, iObjectsParent.transform);
			}
		}
    }
}
