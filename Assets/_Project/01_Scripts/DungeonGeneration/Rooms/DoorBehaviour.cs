using System;
using MyBox;
using ProjectAwakening.Player;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	public class DoorBehaviour : MonoBehaviour
	{
		[SerializeField] private Direction direction;
		[SerializeField] private RoomEventScriptableObject roomEvent;

		private void OnTriggerEnter2D(Collider2D other)
		{
			roomEvent.OnOpenDoor(direction);
		}
	}
}