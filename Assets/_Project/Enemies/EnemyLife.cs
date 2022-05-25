using UnityEngine;

namespace ProjectAwakening.Enemies
{
    public class EnemyLife : Life
    {
		[Header("Corpse")]
		[SerializeField]
		private Sprite cadaverSprite;

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

			Destroy(gameObject);
		}
	}
}
