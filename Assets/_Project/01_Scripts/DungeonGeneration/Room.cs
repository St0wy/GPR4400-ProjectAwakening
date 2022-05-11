using UnityEngine;

namespace ProjectAwakening.DungeonGeneration
{
	public enum RoomType
	{
		Empty = 0,
		Basic,
		Start,
		Final,
	}

	public class Room
	{
		public Room() : this(RoomType.Basic, new Vector2Int(0, 0)) { }

		public Room(RoomType type, Vector2Int pos)
		{
			Type = type;
			Pos = pos;
		}

		public RoomType Type { get; set; }
		public Vector2Int Pos { get; private set; }
	}
}