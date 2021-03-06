using System;
using MyBox;
using ProjectAwakening.Enemies.Spawning;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.Dungeon.Rooms
{
	public class DoorBehaviour : MonoBehaviour
	{
		private const string PlayerTag = "Player";
		private static readonly Vector3Int[] TopDoorPos = {new(-1, 4, 0), new(0, 4, 0), new(1, 4, 0)};
		private static readonly Vector3Int[] BottomDoorPos = {new(-1, -6, 0), new(0, -6, 0), new(1, -6, 0)};
		private static readonly Vector3Int[] LeftDoorPos = {new(-9, 0, 0), new(-9, -1, 0), new(-9, -2, 0)};
		private static readonly Vector3Int[] RightDoorPos = {new(9, 0, 0), new(9, -1, 0), new(9, -2, 0)};

		[SerializeField] private Direction direction;
		[SerializeField] private RoomEventScriptableObject roomEvent;
		[SerializeField] private SpawnEventScriptableObject spawnEvent;
		[SerializeField] private DungeonEnemiesCountScriptableObject dungeonEnemiesCount;

		[Foldout("Tiles", true)]
		[FormerlySerializedAs("DoorLeft")]
		[SerializeField]
		private TileBase doorLeft;
		[FormerlySerializedAs("DoorMiddle")] [SerializeField]
		private TileBase doorMiddle;
		[FormerlySerializedAs("DoorBottom")] [SerializeField]
		private TileBase doorBottom;
		[SerializeField] private TileBase wallTop;
		[SerializeField] private TileBase closedDoor;

		[ReadOnly]
		[SerializeField]
		private bool isOpen;

		private Tilemap tilemap;
		private RoomBehaviour roomBehaviour;

		public bool IsOpen
		{
			get => isOpen;
			private set
			{
				isOpen = value;
				UpdateOpen();
			}
		}

		private void Awake()
		{
			GameObject go = GameObject.Find("Tilemap");
			if (go == null)
			{
				this.LogError("Did not find the Tilemap.");
				return;
			}

			tilemap = go.GetComponent<Tilemap>();

			roomBehaviour = FindObjectOfType<RoomBehaviour>();
		}

		private void OnEnable()
		{
			dungeonEnemiesCount.OnNoMoreEnemies += OnNoMoreEnemies;
			spawnEvent.OnSpawnEnemies += OnSpawnEnemies;
		}

		private void OnDisable()
		{
			dungeonEnemiesCount.OnNoMoreEnemies -= OnNoMoreEnemies;
			spawnEvent.OnSpawnEnemies -= OnSpawnEnemies;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(PlayerTag))
			{
				roomEvent.OpenDoor(direction);
			}
		}

		private void OnNoMoreEnemies()
		{
			IsOpen = true;
		}

		private void OnSpawnEnemies()
		{
			if (!roomBehaviour.HasEnemies) return;

			IsOpen = false;
		}

		private void UpdateOpen()
		{
			if (isOpen)
			{
				OpenDoor();
			}
			else
			{
				CloseDoor();
			}
		}

		private void CloseDoor()
		{
			switch (direction)
			{
				case Direction.Up:
					tilemap.SetTile(TopDoorPos[0], wallTop);
					tilemap.SetTile(TopDoorPos[2], wallTop);
					tilemap.SetTile(TopDoorPos[1], closedDoor);
					break;
				case Direction.Right:
					tilemap.SetTile(RightDoorPos[0], wallTop);
					tilemap.SetTile(RightDoorPos[2], wallTop);
					tilemap.SetTile(RightDoorPos[1], closedDoor);
					break;
				case Direction.Down:
					tilemap.SetTile(BottomDoorPos[0], wallTop);
					tilemap.SetTile(BottomDoorPos[2], wallTop);
					tilemap.SetTile(BottomDoorPos[1], closedDoor);
					break;
				case Direction.Left:
					tilemap.SetTile(LeftDoorPos[0], wallTop);
					tilemap.SetTile(LeftDoorPos[2], wallTop);
					tilemap.SetTile(LeftDoorPos[1], closedDoor);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			tilemap.RefreshAllTiles();
		}

		private void OpenDoor()
		{
			switch (direction)
			{
				case Direction.Up:
					tilemap.SetTile(TopDoorPos[0], doorLeft);
					tilemap.SetTile(TopDoorPos[1], doorMiddle);
					tilemap.SetTile(TopDoorPos[2], doorBottom);
					break;
				case Direction.Right:
					tilemap.SetTile(RightDoorPos[0], doorLeft);
					tilemap.SetTile(RightDoorPos[1], doorMiddle);
					tilemap.SetTile(RightDoorPos[2], doorBottom);
					break;
				case Direction.Down:
					tilemap.SetTile(BottomDoorPos[0], doorBottom);
					tilemap.SetTile(BottomDoorPos[1], doorMiddle);
					tilemap.SetTile(BottomDoorPos[2], doorLeft);
					break;
				case Direction.Left:
					tilemap.SetTile(LeftDoorPos[0], doorBottom);
					tilemap.SetTile(LeftDoorPos[1], doorMiddle);
					tilemap.SetTile(LeftDoorPos[2], doorLeft);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			tilemap.RefreshAllTiles();
		}
	}
}