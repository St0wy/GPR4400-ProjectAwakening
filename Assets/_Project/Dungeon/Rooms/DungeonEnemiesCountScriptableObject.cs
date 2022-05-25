using UnityEngine;

namespace ProjectAwakening.Dungeon.Rooms
{
	[CreateAssetMenu(fileName = "dungeonEnemiesCount", menuName = "Dungeon Enemies Count", order = 0)]
	public class DungeonEnemiesCountScriptableObject : ScriptableObject
	{
		public delegate void NoMoreEnemiesEvent();

		private int enemiesCount;

		public int EnemiesCount
		{
			get => enemiesCount;
			set
			{
				enemiesCount = value;
				if (enemiesCount <= 0)
				{
					OnNoMoreEnemies?.Invoke();
				}
			}
		}

		public NoMoreEnemiesEvent OnNoMoreEnemies { get; set; }
	}
}