using UnityEngine;

namespace ProjectAwakening.Dungeon.Rooms
{
	[CreateAssetMenu(fileName = "roomEvent", menuName = "Room Event", order = 0)]
	public class RoomEventScriptableObject : ScriptableObject
	{
		public delegate void OpenDoorEvent(Direction direction);

		public OpenDoorEvent OnOpenDoor { get; set; }
		

		public void OpenDoor(Direction direction)
		{
			OnOpenDoor?.Invoke(direction);
		}
	}
}