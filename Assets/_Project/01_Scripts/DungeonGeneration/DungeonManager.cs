using MyBox;
using ProjectAwakening.DungeonGeneration.UI;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration
{
	public class DungeonManager : MonoBehaviour
	{
		[SerializeField] private DungeonGenerator dungeonGenerator;
		[SerializeField] private UIDungeonDrawer dungeonDrawer;

		[SerializeField] private bool randomSeed = true;

		[ConditionalField(nameof(randomSeed), true)] [SerializeField]
		private int seed = 5;

		private Room[,] dungeon;

		public void Generate()
		{
			if (!randomSeed)
				Random.InitState(seed);
			dungeon = dungeonGenerator.Generate();
			dungeonDrawer.DrawDungeon(dungeon);
		}
	}
}