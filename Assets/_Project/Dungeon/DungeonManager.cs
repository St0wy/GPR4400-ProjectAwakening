using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using ProjectAwakening.Dungeon.Rooms;
using ProjectAwakening.Enemies.Spawning;
using ProjectAwakening.Player.Character;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace ProjectAwakening.Dungeon
{
	public class DungeonManager : MonoBehaviour
	{
		[SerializeField] private DungeonGenerator dungeonGenerator;

		[Foldout("Settings", true)]
		[SerializeField] private bool randomSeed = true;

		[ConditionalField(nameof(randomSeed), true)] [SerializeField]
		private int seed = 7;

		[SerializeField] private int baseNumberOfRooms = 12;

		[SerializeField]
		private RoomEventScriptableObject roomEvent;

		[SerializeField]
		private SpawnEventScriptableObject spawnEvent;

		[SerializeField]
		private DungeonEnemiesCountScriptableObject dungeonEnemiesCount;

		[SerializeField]
		private PlayerMovement player;

		[SerializeField]
		private AstarPath path;

		[Foldout("TP Points", true)]
		[SerializeField] private Transform TPTop;
		[SerializeField] private Transform TPBottom;
		[SerializeField] private Transform TPLeft;
		[SerializeField] private Transform TPRight;

		#region Scenes

		[Foldout("Room Scenes", true)]
		[SerializeField] private SceneReference[] b;
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
		[SerializeField] private SceneReference tEnd;
		[SerializeField] private SceneReference bEnd;
		[SerializeField] private SceneReference lEnd;
		[SerializeField] private SceneReference rEnd;

		#endregion

		private Room[,] dungeon;
		private Room currentRoom;

		public static int Level => GameManager.Instance.Level;

		#region Unity Events

		private void OnEnable()
		{
			roomEvent.OnOpenDoor += OnOpenDoor;
			dungeonEnemiesCount.OnNoMoreEnemies += OnNoMoreEnemies;
		}

		private void Start()
		{
			if (!randomSeed)
				Random.InitState(seed);
			dungeonGenerator.NumberOfRooms = Random.Range(0, 2) + baseNumberOfRooms + (int) (Level * 2.6);
			GenerateDungeonMap();
			FillScenesInDungeon();
			LoadStartScene();
		}

		private void OnDisable()
		{
			roomEvent.OnOpenDoor -= OnOpenDoor;
			dungeonEnemiesCount.OnNoMoreEnemies -= OnNoMoreEnemies;
		}

		#endregion

		#region Methods

		public void GenerateDungeonMap()
		{
			dungeon = dungeonGenerator.Generate();
		}

		private void OnNoMoreEnemies()
		{
			currentRoom.IsFinished = true;
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

			room.Scene.LoadSceneAsync(LoadSceneMode.Additive).completed += _ =>
			{
				room.Scene.SetActive();
				currentRoom = room;

				path.Scan();

				if (!room.IsFinished)
				{
					spawnEvent.SpawnEnemies();
				}
			};
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
			switch (room.Type)
			{
				case RoomType.Basic:
				case RoomType.Start:
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
				case RoomType.Final:
					return room.Neighborhood.Type switch
					{
						NeighborhoodType.Top => tEnd,
						NeighborhoodType.Bottom => bEnd,
						NeighborhoodType.Left => lEnd,
						NeighborhoodType.Right => rEnd,
						_ => tblr[0],
					};
				case RoomType.Empty:
				default:
					return tblr[0];
			}
		}

		#endregion
	}
}