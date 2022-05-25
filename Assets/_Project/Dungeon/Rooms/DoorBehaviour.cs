using System;
using MyBox;
using ProjectAwakening.Enemies.Spawning;
using ProjectAwakening.Player;
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
		// [SerializeField] private TileBase wallBottom;
		// [SerializeField] private TileBase wallLeft;
		// [SerializeField] private TileBase wallRight;
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

		private void Start() { }

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
			this.Log("no more");
			IsOpen = true;
		}

		private void OnSpawnEnemies()
		{
			if (!roomBehaviour.HasEnemies) return;

			IsOpen = false;
			this.Log("Spawned");
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
			this.Log("close");
			switch (direction)
			{
				case Direction.Up:
					tilemap.SetTile(TopDoorPos[0], wallTop);
					tilemap.SetTile(TopDoorPos[2], wallTop);
					tilemap.SetTile(TopDoorPos[1], closedDoor);
					// Matrix4x4 rotationTop = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0));
					// tilemap.SetTransformMatrix(TopDoorPos[1], rotationTop);
					break;
				case Direction.Right:
					tilemap.SetTile(RightDoorPos[0], wallTop);
					tilemap.SetTile(RightDoorPos[2], wallTop);
					tilemap.SetTile(RightDoorPos[1], closedDoor);
					// Matrix4x4 rotationRight = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));
					// tilemap.SetTransformMatrix(RightDoorPos[1], rotationRight);
					break;
				case Direction.Down:
					tilemap.SetTile(BottomDoorPos[0], wallTop);
					tilemap.SetTile(BottomDoorPos[2], wallTop);
					tilemap.SetTile(BottomDoorPos[1], closedDoor);
					// Matrix4x4 rotationBottom = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
					// tilemap.SetTransformMatrix(BottomDoorPos[1], rotationBottom);
					break;
				case Direction.Left:
					tilemap.SetTile(LeftDoorPos[0], wallTop);
					tilemap.SetTile(LeftDoorPos[2], wallTop);
					tilemap.SetTile(LeftDoorPos[1], closedDoor);
					// Matrix4x4 rotationLeft = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270));
					// tilemap.SetTransformMatrix(LeftDoorPos[1], rotationLeft);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			tilemap.RefreshAllTiles();
		}

		private void OpenDoor()
		{
			this.Log("Open");
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