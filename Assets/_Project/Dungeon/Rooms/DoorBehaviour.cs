using ProjectAwakening.Player;
using UnityEngine;

namespace ProjectAwakening.Dungeon.Rooms
{
	public class DoorBehaviour : MonoBehaviour
	{
		private const string PlayerTag = "Player";

		[SerializeField] private Direction direction;
		[SerializeField] private RoomEventScriptableObject roomEvent;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(PlayerTag))
			{
				roomEvent.OpenDoor(direction);
			}
		}
	}
}