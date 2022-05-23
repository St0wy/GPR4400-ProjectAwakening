using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies
{
    public class OnTouchDamage : MonoBehaviour
    {
		[SerializeField]
		protected bool dealDamageOnTouch = true;
		[SerializeField]
		protected int touchDamage = 1;
		[SerializeField]
		protected float touchKnockbackMod = 3.0f;

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (!dealDamageOnTouch)
				return;

			if (collision.collider.CompareTag("Player"))
			{
				collision.collider.GetComponent<Life>().Damage(touchDamage, transform.position, touchKnockbackMod);
			}
		}
	}
}
