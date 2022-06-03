using UnityEngine;
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

			// Spawn corpse
			if (cadaverSprite != null)
			{
				var cadaver = new GameObject();
				SpriteRenderer cadaverSpriteRenderer = cadaver.AddComponent<SpriteRenderer>();
				cadaverSpriteRenderer.sprite = cadaverSprite;
				cadaverSpriteRenderer.sortingLayerID = SortingLayer.NameToID("Ground");
				cadaverSpriteRenderer.sortingOrder = 1;

				Transform tr = transform;
				cadaver.transform.position = tr.position;
				cadaver.transform.localScale = tr.localScale;
			}

			if (!ReferenceEquals(DungeonEnemiesCount, null))
			{
				DungeonEnemiesCount.EnemiesCount--;
			}

			Destroy(gameObject);
		}
	}
}