using ProjectAwakening.Player;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	[CreateAssetMenu(fileName = "roomEvent", menuName = "Room Event", order = 0)]
	public class RoomEventScriptableObject : ScriptableObject
	{
		public delegate void OpenDoorEvent(Direction direction);

		public delegate void SpawnEnemiesEvent();

		public OpenDoorEvent OnOpenDoor { get; set; }
		public SpawnEnemiesEvent OnSpawnEnemies { get; set; }

		public void OpenDoor(Direction direction)
		{
			OnOpenDoor?.Invoke(direction);
		}

		public void SpawnEnemies()
		{
			OnSpawnEnemies?.Invoke();
		}
	}
}