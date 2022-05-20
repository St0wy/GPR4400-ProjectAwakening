using ProjectAwakening.Enemies.Spawning;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.Dungeon.Rooms
{
	public class RoomBehaviour : MonoBehaviour
	{
		[SerializeField] private SpawnEventScriptableObject spawnEvent;
		[SerializeField] private GameObject[] enemiesSpawnPoint;

		private void OnEnable()
		{
			spawnEvent.OnSpawnEnemies += OnSpawnEnemies;
		}

		private void OnDisable()
		{
			spawnEvent.OnSpawnEnemies -= OnSpawnEnemies;
		}

		private void OnSpawnEnemies()
		{
			// TODO : Spawn enemies
			this.Log($"Enemies spawned: {enemiesSpawnPoint.Length}");
		}
	}
}