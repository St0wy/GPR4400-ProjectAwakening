using ProjectAwakening.Enemies;
using ProjectAwakening.Enemies.Spawning;
using UnityEngine;

namespace ProjectAwakening.Dungeon.Rooms
{
	public class RoomBehaviour : MonoBehaviour
	{
		[SerializeField] private SpawnEventScriptableObject spawnEvent;
		[SerializeField] private DungeonEnemiesCountScriptableObject dungeonEnemiesCount;

		private int enemiesCount;

		[field: SerializeField] public bool HasEnemies { get; set; }

		private void Awake()
		{
			MonsterSpawner[] spawners = FindObjectsOfType<MonsterSpawner>();
			enemiesCount = spawners.Length;
		}

		private void OnEnable()
		{
			if (spawnEvent != null)
				spawnEvent.OnSpawnEnemies += OnSpawnEnemies;
		}

		private void OnDisable()
		{
			if (spawnEvent != null)
				spawnEvent.OnSpawnEnemies -= OnSpawnEnemies;
		}

		private void OnSpawnEnemies()
		{
			if (HasEnemies && enemiesCount > 0)
			{
				dungeonEnemiesCount.EnemiesCount = enemiesCount;
			}
		}
	}
}