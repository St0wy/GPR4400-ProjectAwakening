using System;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration
{
	[Serializable]
	public class Room
	{
		public Neighborhood Neighborhood;

		public Room() : this(RoomType.Empty, new Vector2Int(0, 0)) { }

		public Room(RoomType type, Vector2Int pos)
		{
			Type = type;
			Pos = pos;
		}

		[field: SerializeField] public RoomType Type { get; set; }
		[field: SerializeField] public Vector2Int Pos { get; private set; }
	}
}