using System.Collections;
using UnityEngine;

namespace ProjectAwakening.Player.Bow
{
	public class ArrowBehaviour : MonoBehaviour
	{
		[Header("Properties")]
		[SerializeField]
		private bool isFriendly = true;

		[SerializeField]
		private bool piercing;

		[SerializeField]
		private float speed = 35.0f;

		[SerializeField]
		private int damage = 1;

		[SerializeField]
		private float knockbackMod = 1.0f;

		[Header("Components")]
		[SerializeField]
		private Rigidbody2D rb;

		[SerializeField]
		private Collider2D col;

		// Start is called before the first frame update
		private void Start()
		{
			rb.velocity = transform.up * speed;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (isFriendly && collision.CompareTag("Player"))
				return;

			if (!isFriendly && collision.CompareTag("Enemy"))
				return;

			// Check for opponent life script
			if (collision.TryGetComponent(out Life life))
			{
				// Deal Damage	
				life.Damage(damage, transform.position, knockbackMod);

				if(!piercing)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Destroy(gameObject);
			}

		}
	}
}