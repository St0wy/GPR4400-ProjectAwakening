using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectAwakening.DungeonGeneration
{
	/// <summary>
	/// A "The Binding of Isaac" like dungeon generator.
	/// Based on : https://www.boristhebrave.com/2020/09/12/dungeon-generation-in-binding-of-isaac/
	/// </summary>
	[Serializable]
	public class DungeonGenerator
	{
		[field: SerializeField] public Size Size { get; set; }
		[field: SerializeField] public int NumberOfRooms { get; set; }
		[field: SerializeField, Range(0, 1)] public float ChanceToGiveUp { get; set; }
		[field: SerializeField, Range(0, 1)] public float FillPercentage { get; set; }

		public int MaxNumberOfRooms => Mathf.FloorToInt(Size.Width * Size.Height * FillPercentage);

		public Room[,] Generate()
		{
			var rooms = new Room[Size.Width, Size.Height];

			if (NumberOfRooms >= MaxNumberOfRooms)
			{
				NumberOfRooms = MaxNumberOfRooms;
				Debug.LogWarning($"Too many rooms for the size of the map. (Max : {MaxNumberOfRooms})");
			}

			var roomQueue = new Queue<Room>();
			var endRooms = new List<Room>();

			void AddRoom(Room room)
			{
				rooms[room.Pos.x, room.Pos.y] = room;
				roomQueue.Enqueue(room);
			}

			// Generate start room
			var pos = new Vector2Int(Size.Width / 2, Size.Height / 2);
			AddRoom(new Room(RoomType.Start, pos));

			foreach (Room room in roomQueue)
			{
				var addCount = 0;
				for (int x = -1; x <= 1; x++)
				{
					for (int y = -1; y < 1; y++)
					{
						if ((x == 0 && y == 0) || IsOutOfBounds(Size, x, y)) continue;
						if (roomQueue.Count >= NumberOfRooms) continue;
						var neighborPos = new Vector2Int(room.Pos.x + x, room.Pos.y + y);
						Room neighborRoom = rooms[neighborPos.x, neighborPos.y];
						bool isNeighborOccupied = neighborRoom != null;
						if (isNeighborOccupied) continue;
						if (HasMoreThanOneFilledNeighbor(rooms, neighborPos)) continue;
						if (Random.Range(0f, 1f) <= ChanceToGiveUp) continue;

						addCount++;
						AddRoom(new Room(RoomType.Basic, neighborPos));
					}
				}

				if (addCount == 0)
				{
					endRooms.Add(room);
				}
			}

			// The last of end rooms is the one furthest from the start
			endRooms.Last().Type = RoomType.Final;

			return rooms;
		}

		private bool IsOutOfBounds(Size size, int x, int y) => x < 0 || x >= size.Width || y < 0 || y >= size.Height;

		private bool HasMoreThanOneFilledNeighbor(Room[,] rooms, Vector2Int pos)
		{
			var neighborCount = 0;
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y < 1; y++)
				{
					if ((x == 0 && y == 0) || IsOutOfBounds(Size, x, y)) continue;
					var neighborPos = new Vector2Int(pos.x + x, pos.y + y);
					Room neighborRoom = rooms[neighborPos.x, neighborPos.y];
					if (neighborRoom != null)
					{
						neighborCount++;
					}
				}
			}

			// 2 because we ignore the room from where this one will be generated
			return neighborCount > 2;
		}
	}
}