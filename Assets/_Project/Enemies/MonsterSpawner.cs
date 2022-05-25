using System.Collections.Generic;
using ProjectAwakening.Enemies.Spawning;
using UnityEngine;

namespace ProjectAwakening.Enemies
{
	public class MonsterSpawner : MonoBehaviour
	{
		[SerializeField]
		private SpawnEventScriptableObject spawnEvent;

		[SerializeField]
		private List<GameObject> spawnableMonsters;

		[SerializeField]
		private int spawnsLeft = 1;

		public void SpawnMonster()
		{
			if (spawnableMonsters.Count == 0 || spawnsLeft <= 0)
				return;

			// Store the transform in a var to not access it through the getter twice
			Transform t = transform;
			Instantiate(GetRandomMonster(), t.position, t.rotation);

			spawnsLeft--;
		}

		private GameObject GetRandomMonster() => spawnableMonsters[Random.Range(0, spawnableMonsters.Count)];

		private void OnEnable()
		{
			spawnEvent.OnSpawnEnemies += SpawnMonster;
		}

		private void OnDisable()
		{
			spawnEvent.OnSpawnEnemies -= SpawnMonster;
		}
	}
}