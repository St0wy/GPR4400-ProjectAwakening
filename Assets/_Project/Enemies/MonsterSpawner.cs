using System.Collections.Generic;
using ProjectAwakening.Dungeon.Rooms;
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
		private DungeonEnemiesCountScriptableObject dungeonEnemiesCount;

		[SerializeField]
		private bool spawnAllAtOnce = true;

		[field: SerializeField] public int SpawnsLeft { get; private set; } = 1;

		public void SpawnMonster()
		{
			do
			{
				if (spawnableMonsters.Count == 0 || SpawnsLeft <= 0)
					return;

				// Store the transform in a var to not access it through the getter twice
				Transform t = transform;
				GameObject enemy = Instantiate(GetRandomMonster(), t.position, t.rotation);

				if (dungeonEnemiesCount != null)
				{
					var enemyLife = enemy.GetComponent<EnemyLife>();
					if (enemyLife == null) return;

					enemyLife.DungeonEnemiesCount = dungeonEnemiesCount;
				}

				SpawnsLeft--;
			} while (spawnAllAtOnce);
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