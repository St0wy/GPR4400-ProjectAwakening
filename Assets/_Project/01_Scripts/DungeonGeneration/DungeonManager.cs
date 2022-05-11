using UnityEngine;

namespace ProjectAwakening.DungeonGeneration
{
	public class DungeonManager : MonoBehaviour
	{
		[SerializeField] private DungeonGenerator dungeonGenerator;
		[SerializeField] private DungeonDrawer dungeonDrawer;
		[SerializeField] private int seed;

		private Room[,] dungeon;

		public void Generate()
		{
			Random.InitState(seed);
			dungeon = dungeonGenerator.Generate();
			dungeonDrawer.DrawDungeon(dungeon);
		}

	}
}