using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	public class RoomBehaviour : MonoBehaviour
	{
		[SerializeField] private RoomEventScriptableObject roomEvent;
		[SerializeField] private GameObject[] enemiesSpawnPoint;

		private void OnEnable()
		{
			roomEvent.OnSpawnEnemies += OnSpawnEnemies;
		}

		private void OnDisable()
		{
			roomEvent.OnSpawnEnemies -= OnSpawnEnemies;
		}

		private void OnSpawnEnemies()
		{
			// TODO : Spawn enemies
			this.Log("Enemies spawned");
		}
	}
}