using System;
using ProjectAwakening.Dungeon.Rooms;

namespace ProjectAwakening.Enemies
{
	public class EnemyLife : Life
	{
		public DungeonEnemiesCountScriptableObject DungeonEnemiesCount { get; set; }

		protected override void Die()
		{
			base.Die();

			if (!ReferenceEquals(DungeonEnemiesCount, null))
			{
				DungeonEnemiesCount.EnemiesCount--;
			}

			Destroy(gameObject);
		}
	}
}