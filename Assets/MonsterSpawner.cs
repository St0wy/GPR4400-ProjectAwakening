using System.Collections;
using System.Collections.Generic;
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

		 	Instantiate(spawnableMonsters[Random.Range(0, spawnableMonsters.Count)], transform);

			spawnsLeft--;
        }

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
