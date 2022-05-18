using System;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	[Serializable]
	public class RoomMap
	{
		public Neighborhood Neighborhood;

		public RoomMap() : this(RoomType.Empty, new Vector2Int(0, 0)) { }

		public RoomMap(RoomType type, Vector2Int pos)
		{
			Type = type;
			Pos = pos;
		}

		[field: SerializeField] public RoomType Type { get; set; }
		[field: SerializeField] public Vector2Int Pos { get; private set; }
	}
}