using UnityEngine;

using System;
using ProjectAwakening.Dungeon.Rooms;

namespace ProjectAwakening.Enemies
{
    public class EnemyLife : Life
    {
		[Header("Corpse")]
		[SerializeField]
		private Sprite cadaverSprite;

		public DungeonEnemiesCountScriptableObject DungeonEnemiesCount { get; set; }

		protected override void Die()
		{
			base.Die();

			//Spawn corpse
			if (cadaverSprite != null)
			{
				GameObject cadaver = new GameObject();
				cadaver.AddComponent<SpriteRenderer>().sprite = cadaverSprite;
				cadaver.transform.position = transform.position;
				cadaver.transform.localScale = transform.localScale;
			}

			if (!ReferenceEquals(DungeonEnemiesCount, null))
			{
				DungeonEnemiesCount.EnemiesCount--;
			}

			Destroy(gameObject);
		}
	}
}