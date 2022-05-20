using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace ProjectAwakening.Overworld.WorldDetailsGeneration
{
	public class SpecialElementsGenerator : MonoBehaviour
	{
		[Header("Parameters")]
		[Tooltip("Range from the player in which dangerous things can't spawn")]
		[SerializeField]
		private float safeRange;

		[Tooltip("Maximum number of tries to spawn outside the safe range")]
		[SerializeField]
		private int spawnTries;

		// GameObject that holds all the instantiated objects
		[HideInInspector]
		[SerializeField]
		private GameObject instantiatedObjects;

		[Header("Necessary objects")]
		[SerializeField]
		private GameObject player;

		[SerializeField]
		private CinemachineVirtualCamera vCam;

		[SerializeField] private GameObject keyDungeon;

		[Header("Additional objects")]
		[Tooltip("GameObjects to generate once, in the largest area")]
		[SerializeField]
		private List<GameObject> uniqueGameObjects;

		[Header("Monster Gen")]
		[Tooltip("SpawnPoints possible")]
		[SerializeField]
		private List<GameObject> spawners;

		[Tooltip("Total amount of monsters to generate")]
		[SerializeField]
		private int monsters;

		[SerializeField] private Vector3 offset;

		private Vector3 playerPos = Vector3.zero;

		public void GenerateElementsInArea(IEnumerable<Vector3> positions)
		{
			if (!Application.isPlaying)
				DestroyImmediate(instantiatedObjects);
			else
				Destroy(instantiatedObjects);

			instantiatedObjects = new GameObject
			{
				name = "InstantiatedObjects",
			};

			var possiblePositions = new List<Vector3>(positions);

			//Add the player and the dungeon
			//Spawn the player
			playerPos = RemoveRandomFromList(possiblePositions) + offset;
			GameObject playerInstance = Instantiate(player, playerPos,
				Quaternion.identity, instantiatedObjects.transform);

			//Link player with virtual cam
			vCam.Follow = playerInstance.transform;

			//Find the furthest point
			var furthestFromPlayer = new KeyValuePair<Vector3, float>();
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
				Quaternion.identity, instantiatedObjects.transform);
			possiblePositions.Remove(furthestFromPlayer.Key);

			//Generate unique Objects
			foreach (GameObject uGameObject in uniqueGameObjects)
			{
				Instantiate(uGameObject, RemoveRandomFromList(possiblePositions) + offset,
					Quaternion.identity, instantiatedObjects.transform);
			}

			//Generate monsters
			for (int m = 0; m < monsters; m++)
			{
				Instantiate(spawners[Random.Range(0, spawners.Count)],
					RemoveRandomFromList(possiblePositions, IsOutsidePlayerSafeRange, spawnTries) + offset,
					Quaternion.identity, instantiatedObjects.transform);
			}
		}

		private bool IsOutsidePlayerSafeRange(Vector3 position)
		{
			return Vector3.Distance(position + offset, playerPos) > safeRange;
		}

		public static T RemoveRandomFromList<T>(List<T> list, System.Predicate<T> predicate, int maxTries)
		{
			int chosenIndex;
			T chosenElement;

			var tries = 0;
			do
			{
				chosenIndex = Random.Range(0, list.Count);
				chosenElement = list[chosenIndex];
			} while (predicate(chosenElement) == false && ++tries < maxTries);

			list.RemoveAt(chosenIndex);

			return chosenElement;
		}

		public static T RemoveRandomFromList<T>(List<T> list)
		{
			int chosenIndex = Random.Range(0, list.Count);
			T chosenElement = list[chosenIndex];
			list.RemoveAt(chosenIndex);

			return chosenElement;
		}
	}
}