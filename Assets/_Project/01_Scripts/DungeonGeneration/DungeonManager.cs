using System;
using System.Collections.Generic;
using MyBox;
using ProjectAwakening.DungeonGeneration.Rooms;
using ProjectAwakening.DungeonGeneration.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectAwakening.DungeonGeneration
{
	public class DungeonManager : MonoBehaviour
	{
		[SerializeField] private DungeonGenerator dungeonGenerator;
		[SerializeField] private UIDungeonDrawer dungeonDrawer;

		[SerializeField] private bool randomSeed = true;

		[ConditionalField(nameof(randomSeed), true)] [SerializeField]
		private int seed = 5;

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

		private RoomMap[,] mapDungeon;
		private Room[,] dungeon;

		public int Level { get; set; } = 1;

		private void Start()
		{
			if (!randomSeed)
				Random.InitState(seed);
			dungeonGenerator.NumberOfRooms = Random.Range(0, 2) + 5 + (int) (Level * 2.6);
			GenerateDungeonMap();
			CreateDungeonWithScenes();
		}

		public void GenerateDungeonMap()
		{
			mapDungeon = dungeonGenerator.Generate();
			// dungeonDrawer.DrawDungeon(mapDungeon);
		}

		private void CreateDungeonWithScenes()
		{
			int width = mapDungeon.GetLength(0);
			int height = mapDungeon.GetLength(1);

			dungeon = new Room[width, height];

			foreach (RoomMap roomMap in mapDungeon)
			{
				if (roomMap.Type == RoomType.Empty) continue;
				SceneReference scene = GetScene(roomMap);
				Room room = new(scene, roomMap.Pos);
				dungeon[roomMap.Pos.x, roomMap.Pos.y] = room;
			}
		}

		private static SceneReference GetSceneFromType(RoomType type, IReadOnlyList<SceneReference> scenes) =>
			type == RoomType.Start ? scenes[0] : scenes[Random.Range(0, scenes.Count)];

		private SceneReference GetScene(RoomMap roomMap)
		{
			return roomMap.Neighborhood.Type switch
			{
				NeighborhoodType.Bottom => GetSceneFromType(roomMap.Type, b),
				NeighborhoodType.BottomLeft => GetSceneFromType(roomMap.Type, bl),
				NeighborhoodType.BottomLeftRight => GetSceneFromType(roomMap.Type, blr),
				NeighborhoodType.BottomRight => GetSceneFromType(roomMap.Type, br),
				NeighborhoodType.Left => GetSceneFromType(roomMap.Type, l),
				NeighborhoodType.LeftRight => GetSceneFromType(roomMap.Type, lr),
				NeighborhoodType.Right => GetSceneFromType(roomMap.Type, r),
				NeighborhoodType.Top => GetSceneFromType(roomMap.Type, t),
				NeighborhoodType.TopBottom => GetSceneFromType(roomMap.Type, tb),
				NeighborhoodType.TopBottomLeft => GetSceneFromType(roomMap.Type, tbl),
				NeighborhoodType.TopBottomLeftRight => GetSceneFromType(roomMap.Type, tblr),
				NeighborhoodType.TopBottomRight => GetSceneFromType(roomMap.Type, tbr),
				NeighborhoodType.TopLeft => GetSceneFromType(roomMap.Type, tl),
				NeighborhoodType.TopLeftRight => GetSceneFromType(roomMap.Type, tlr),
				NeighborhoodType.TopRight => GetSceneFromType(roomMap.Type, tr),
				NeighborhoodType.None => throw new ArgumentOutOfRangeException(),
				_ => throw new ArgumentOutOfRangeException(),
			};
		}
	}
}