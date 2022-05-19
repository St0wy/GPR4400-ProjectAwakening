using System;
using MyBox;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	[Serializable]
	public class Room
	{
		public Neighborhood Neighborhood;

		public Room() : this(RoomType.Empty, new Vector2Int(0, 0)) { }

		public Room(RoomType type, Vector2Int pos, SceneReference scene = null)
		{
			Type = type;
			Pos = pos;
			Scene = scene;
		}

		[field: SerializeField] public RoomType Type { get; set; }
		[field: SerializeField] public Vector2Int Pos { get; private set; }
		[field: SerializeField] public SceneReference Scene { get; set; }
		[field: SerializeField] public bool IsFinished { get; set; } = false;
	}
}