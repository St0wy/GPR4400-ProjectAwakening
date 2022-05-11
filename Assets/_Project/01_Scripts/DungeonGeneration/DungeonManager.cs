using UnityEngine;

namespace ProjectAwakening.DungeonGeneration
{
	public class DungeonManager : MonoBehaviour
	{
		[SerializeField] private DungeonGenerator dungeonGenerator;
		[SerializeField] private DungeonDrawer dungeonDrawer;

		private Room[,] dungeon;

		public void Generate()
		{
			dungeon = dungeonGenerator.Generate();
			dungeonDrawer.DrawDungeon(dungeon);
		}

	}
}