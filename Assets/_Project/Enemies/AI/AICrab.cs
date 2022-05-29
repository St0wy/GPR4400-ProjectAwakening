using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies.AI
{
    public class AICrab : AIBase
    {
		[SerializeField]
		private GameObject projectile;

		[SerializeField]
		private float speed;

		[SerializeField]
		private Rigidbody2D rb;

		protected override void Move()
		{
			Vector2 direction = (goal - (Vector2) transform.position).normalized;
			rb.velocity = direction * speed;
		}

		protected override void FindWhereToGo()
		{
			if (playerTransform == null)
				return;

			//goal is inverse relative to us of player pos 
			goal = transform.position - playerTransform.Transform.position - transform.position;
		}

		void Shoot(Vector3 direction)
		{
			Instantiate(projectile, transform.position + direction, Quaternion.LookRotation(Vector3.forward, direction), null);
		}
    }
}
