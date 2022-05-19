using System.Collections;
using UnityEngine;

namespace ProjectAwakening.Player
{
	public class ArrowBehaviour : MonoBehaviour
	{
		[Header("Properties")]
		[SerializeField]
		private float speed = 35.0f;

		[SerializeField]
		private int damage = 1;

		[SerializeField]
		private float knockbackMod = 1.0f;

		[Tooltip("Time during which the collider is disabled")]
		[SerializeField]
		private float inactiveTime = 0.3f;

		[Header("Components")]
		[SerializeField]
		private Rigidbody2D rb;

		[SerializeField]
		private Collider2D col;

		// Start is called before the first frame update
		private void Start()
		{
			rb.velocity = transform.up * speed;

			//Collider starts disabled then gets enabled
			col.enabled = false;
			StartCoroutine(DelayedActivate());
		}

		private IEnumerator DelayedActivate()
		{
			yield return new WaitForSeconds(inactiveTime);
			col.enabled = true;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			//Check for opponent life script
			Life life;
			if (collision.TryGetComponent(out life))
			{
				//Deal Damage	
				life.Damage(damage, transform.position, knockbackMod);
			}
			//Die
			Destroy(gameObject);
		}
	}
}