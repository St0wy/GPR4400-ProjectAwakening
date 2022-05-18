using MyBox;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	public class Room
	{
		private SceneReference scene;
		private Vector2Int pos;

		public Room(SceneReference scene, Vector2Int pos)
		{
			this.scene = scene;
			this.pos = pos;
		}

		public bool IsFinished { get; set; } = false;
	}
}