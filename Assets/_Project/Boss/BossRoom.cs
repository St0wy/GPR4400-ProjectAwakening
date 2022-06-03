using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening
{
    public class BossRoom : MonoBehaviour
    {
		[SerializeField]
		Enemies.Spawning.SpawnEventScriptableObject spawnEvent;

		[SerializeField]
		Dungeon.Rooms.RoomEventScriptableObject roomEvent;

		private void OnEnable()
		{
			roomEvent.OnOpenDoor += Exit;
		}

		private void OnDisable()
		{
			roomEvent.OnOpenDoor -= Exit;
		}

		private void Exit(Direction direction)
		{
			GameManager.Instance.GoToNextLevel();
		}

		private void Start()
		{
			spawnEvent.SpawnEnemies();
		}
	}
}
