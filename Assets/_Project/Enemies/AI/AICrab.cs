using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies.AI
{
    public class AICrab : AIBase
    {
		[SerializeField]
		private GameObject projectile;

		[Tooltip("The angle of the cone in which we detect and try to shoot at the player")]
		[SerializeField]
		private float coneDetectionAngle;

		[SerializeField]
		private float speed;

		[SerializeField]
		private Rigidbody2D rb;

		private bool canShoot = true;

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

		protected override void AIUpdate()
		{
			base.AIUpdate();

			//Raycast to check if player is visible

			//Find if player is within an error margin of our cardinal directions
			//Get player direction vector
			Vector2 playerDir = playerTransform.Transform.position;

			//Find minimum angle between player and our four cardinal directions.
			float angle = Vector2.Angle(playerDir, Vector2.left);
			float angle2 = Vector2.Angle(playerDir, Vector2.up);
			if (angle < angle2)
				angle = angle2;
			angle2 = Vector2.Angle(playerDir, Vector2.right);
			if (angle < angle2)
				angle = angle2;
			angle2 = Vector2.Angle(playerDir, Vector2.down);
			if (angle < angle2)
				angle = angle2;

			//Check if angle within bounds
			if (angle <= coneDetectionAngle / 2.0f)
			{
				if (canShoot)
					StartCoroutine(PrepareShot(playerDir));
			}
		}

		IEnumerator PrepareShot(Vector2 direction)
		{
			//Turn around

			//Communicate start shoot

			//shoot

			yield return null;
		}

		void Shoot(Vector3 direction)
		{
			Instantiate(projectile, transform.position + direction, Quaternion.LookRotation(Vector3.forward, direction), null);
		}
    }
}
