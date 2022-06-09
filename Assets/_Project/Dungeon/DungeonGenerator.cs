using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using ProjectAwakening.Dungeon.Rooms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectAwakening.Dungeon
{
	/// <summary>
	/// A "The Binding of Isaac" like dungeon generator.
	/// Based on : https://www.boristhebrave.com/2020/09/12/dungeon-generation-in-binding-of-isaac/
	/// </summary>
	[Serializable]
	public class DungeonGenerator
	{
		[field: SerializeField] public Size Size { get; set; } = new(10, 10);
		[field: SerializeField] public int NumberOfRooms { get; set; } = 10;
		[field: SerializeField, Range(0, 1)] public float ChanceToGiveUp { get; set; } = 0.5f;
		[field: SerializeField, Range(0, 1)] public float FillPercentage { get; set; } = 0.8f;

		public int MaxNumberOfRooms => Mathf.FloorToInt(Size.Width * Size.Height * FillPercentage);

		public Room[,] Generate()
		{
			var rooms = new Room[Size.Width, Size.Height];

			if (NumberOfRooms > MaxNumberOfRooms)
			{
				NumberOfRooms = MaxNumberOfRooms;
				Debug.LogWarning($"Too many rooms for the size of the map. (Max : {MaxNumberOfRooms})");
			}

			int count;
			do
			{
				GenerateDungeon(rooms);
				count = rooms.Cast<Room>().Count(room => !IsRoomEmpty(room));
			} while (count != NumberOfRooms);

			return rooms;
		}

		private void GenerateDungeon(Room[,] rooms)
		{
			EmptyRooms(rooms);

			var roomQueue = new Queue<Room>();
			var endRooms = new List<Room>();
			
			void AddRoom(Room room)
			{
				rooms[room.Pos.x, room.Pos.y] = room;
				roomQueue.Enqueue(room);
			}

			// Generate start room
			var startPos = new Vector2Int(Size.Width / 2, Size.Height / 2);
			var startRoom = new Room(RoomType.Start, startPos);
			AddRoom(startRoom);

			while (roomQueue.Count > 0)
			{
				Room room = roomQueue.Dequeue();
				var addCount = 0;
				for (int x = -1; x <= 1; x++)
				{
					for (int y = -1; y <= 1; y++)
					{
						if (!IsValidOffset(x, y)) continue;
						int roomCount = rooms.Cast<Room>().Count(r => !IsRoomEmpty(r));
						if (roomCount >= NumberOfRooms) continue;
						var neighborPos = new Vector2Int(room.Pos.x + x, room.Pos.y + y);
						if (IsOutOfBounds(Size, neighborPos.x, neighborPos.y)) continue;
						Room neighborRoom = rooms[neighborPos.x, neighborPos.y];
						if (!IsRoomEmpty(neighborRoom)) continue;
						if (HasMoreThanOneFilledNeighbor(rooms, neighborPos)) continue;
						if (Random.Range(0f, 1f) <= ChanceToGiveUp) continue;

						addCount++;
						var newRoom = new Room(RoomType.Basic, neighborPos);
						AddRoom(newRoom);

						UpdateNeighbor(rooms, newRoom);
					}
				}

				if (addCount == 0)
				{
					endRooms.Add(room);
				}
			}

			// The last of end rooms is the one furthest from the start
			endRooms.Last().Type = RoomType.Final;
		}

		[SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
		private void UpdateNeighbor(Room[,] rooms, Room room)
		{
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (!IsValidOffset(x, y)) continue;
					var neighborPos = new Vector2Int(room.Pos.x + x, room.Pos.y + y);
					if (IsOutOfBounds(Size, neighborPos.x, neighborPos.y)) continue;
					Room neighborRoom = rooms[neighborPos.x, neighborPos.y];
					if (IsRoomEmpty(neighborRoom)) continue;

					if (x == -1)
					{
						room.Neighborhood.Left = true;
						neighborRoom.Neighborhood.Right = true;
					}
					else if (x == 1)
					{
						room.Neighborhood.Right = true;
						neighborRoom.Neighborhood.Left = true;
					}
					else if (y == 1)
					{
						room.Neighborhood.Bottom = true;
						neighborRoom.Neighborhood.Top = true;
					}
					else if (y == -1)
					{
						room.Neighborhood.Top = true;
						neighborRoom.Neighborhood.Bottom = true;
					}
				}
			}
		}

		public static bool IsRoomEmpty(Room room)
		{
			if (room == null) return true;
			return room.Type == RoomType.Empty;
		}

		private void EmptyRooms(Room[,] rooms)
		{
			for (var x = 0; x < Size.Width; x++)
			{
				for (var y = 0; y < Size.Height; y++)
				{
					rooms[x, y] = null;
				}
			}
		}

		private bool IsOutOfBounds(Size size, int x, int y) => x < 0 || x >= size.Width || y < 0 || y >= size.Height;

		private bool IsValidOffset(int x, int y)
		{
			bool isCurrentRoom = (x == 0 && y == 0);
			bool isCornerRoom = Math.Abs(x) == Math.Abs(y);
			return !isCurrentRoom && !isCornerRoom;
		}

		private bool HasMoreThanOneFilledNeighbor(Room[,] rooms, Vector2Int pos)
		{
			var neighborCount = 0;
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					if (!IsValidOffset(x, y)) continue;

					var neighborPos = new Vector2Int(pos.x + x, pos.y + y);
					if (IsOutOfBounds(Size, neighborPos.x, neighborPos.y)) continue;
					Room neighborRoom = rooms[neighborPos.x, neighborPos.y];
					if (!IsRoomEmpty(neighborRoom))
					{
						neighborCount++;
					}
				}
			}

			return neighborCount > 1;
		}
	}
}