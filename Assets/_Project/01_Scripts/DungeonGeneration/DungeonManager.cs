using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using ProjectAwakening.DungeonGeneration.Rooms;
using ProjectAwakening.Player;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace ProjectAwakening.DungeonGeneration
{
	public class DungeonManager : MonoBehaviour
	{
		[SerializeField] private DungeonGenerator dungeonGenerator;
		// [SerializeField] private UIDungeonDrawer dungeonDrawer;

		[Foldout("Settings")]
		[SerializeField] private bool randomSeed = true;

		[Foldout("Settings")]
		[ConditionalField(nameof(randomSeed), true)] [SerializeField]
		private int seed = 5;

		[Foldout("Settings")] [SerializeField] private RoomEventScriptableObject roomEvent;

		[Foldout("Settings")] [SerializeField] private PlayerMovement player;

		[Foldout("TP Points", true)]
		[SerializeField] private Transform TPTop;
		[SerializeField] private Transform TPBottom;
		[SerializeField] private Transform TPLeft;
		[SerializeField] private Transform TPRight;

		#region Scenes

		[Foldout("Room Scenes", true)] [SerializeField]
		private SceneReference[] b;

		[SerializeField] private SceneReference[] bl;
		[SerializeField] private SceneReference[] blr;
		[SerializeField] private SceneReference[] br;
		[SerializeField] private SceneReference[] l;
		[SerializeField] private SceneReference[] lr;
		[SerializeField] private SceneReference[] r;
		[SerializeField] private SceneReference[] t;
		[SerializeField] private SceneReference[] tb;
		[SerializeField] private SceneReference[] tbl;
		[SerializeField] private SceneReference[] tblr;
		[SerializeField] private SceneReference[] tbr;
		[SerializeField] private SceneReference[] tl;
		[SerializeField] private SceneReference[] tlr;
		[SerializeField] private SceneReference[] tr;

		#endregion

		private Room[,] dungeon;
		private Room currentRoom;

		[field: SerializeField, Foldout("Settings")]
		public int Level { get; set; } = 1;

		#region Unity Events

		private void OnEnable()
		{
			roomEvent.OnOpenDoor += OnOpenDoor;
		}

		private void Start()
		{
			if (!randomSeed)
				Random.InitState(seed);
			dungeonGenerator.NumberOfRooms = Random.Range(0, 2) + 5 + (int) (Level * 2.6);
			GenerateDungeonMap();
			FillScenesInDungeon();
			LoadStartScene();
		}

		private void OnDisable()
		{
			roomEvent.OnOpenDoor -= OnOpenDoor;
		}

		#endregion

		#region Methods

		public void GenerateDungeonMap()
		{
			dungeon = dungeonGenerator.Generate();
			// dungeonDrawer.DrawDungeon(mapDungeon);
		}

		private void OnOpenDoor(Direction direction)
		{
			// Unload the old room
			currentRoom.Scene.UnloadSceneAsync();

			// Get the position of the new room
			Vector2Int newPos = currentRoom.Pos + DirectionUtils.GetDirectionVector(direction);

			// Load the new room
			LoadRoom(dungeon[newPos.x, newPos.y]);

			TeleportPlayer(direction);
		}

		private void TeleportPlayer(Direction doorDirection)
		{
			player.Direction = doorDirection;
			player.gameObject.transform.position = doorDirection switch
			{
				Direction.Up => TPBottom.position,
				Direction.Down => TPTop.position,
				Direction.Left => TPRight.position,
				Direction.Right => TPLeft.position,
				_ => throw new ArgumentOutOfRangeException(nameof(doorDirection), doorDirection, null)
			};
		}

		private void LoadStartScene()
		{
			Room startRoom = dungeon.Cast<Room>().FirstOrDefault(room => room is {Type: RoomType.Start});
			currentRoom = startRoom;

			if (startRoom == null)
			{
				this.LogError("No start room found in the dungeon.");
				return;
			}

			LoadRoom(startRoom);
		}

		private void LoadRoom(Room room)
		{
			if (room == null)
			{
				this.LogError("Tried to load a null room.");
				return;
			}

			room.Scene.LoadScene(LoadSceneMode.Additive);
			currentRoom = room;

			if (!room.IsFinished)
			{
				roomEvent.SpawnEnemies();
			}
		}

		private void FillScenesInDungeon()
		{
			foreach (Room room in dungeon)
			{
				if (room == null || room.Type == RoomType.Empty) continue;
				SceneReference scene = GetScene(room);
				room.Scene = scene;
			}
		}

		private static SceneReference GetSceneFromType(RoomType type, IReadOnlyList<SceneReference> scenes) =>
			type == RoomType.Start ? scenes[0] : scenes[Random.Range(0, scenes.Count)];

		private SceneReference GetScene(Room room)
		{
			return room.Neighborhood.Type switch
			{
				NeighborhoodType.Bottom => GetSceneFromType(room.Type, b),
				NeighborhoodType.BottomLeft => GetSceneFromType(room.Type, bl),
				NeighborhoodType.BottomLeftRight => GetSceneFromType(room.Type, blr),
				NeighborhoodType.BottomRight => GetSceneFromType(room.Type, br),
				NeighborhoodType.Left => GetSceneFromType(room.Type, l),
				NeighborhoodType.LeftRight => GetSceneFromType(room.Type, lr),
				NeighborhoodType.Right => GetSceneFromType(room.Type, r),
				NeighborhoodType.Top => GetSceneFromType(room.Type, t),
				NeighborhoodType.TopBottom => GetSceneFromType(room.Type, tb),
				NeighborhoodType.TopBottomLeft => GetSceneFromType(room.Type, tbl),
				NeighborhoodType.TopBottomLeftRight => GetSceneFromType(room.Type, tblr),
				NeighborhoodType.TopBottomRight => GetSceneFromType(room.Type, tbr),
				NeighborhoodType.TopLeft => GetSceneFromType(room.Type, tl),
				NeighborhoodType.TopLeftRight => GetSceneFromType(room.Type, tlr),
				NeighborhoodType.TopRight => GetSceneFromType(room.Type, tr),
				NeighborhoodType.None => throw new ArgumentOutOfRangeException(),
				_ => throw new ArgumentOutOfRangeException(),
			};
		}

		#endregion
	}
}