using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies.AI
{
    public class AICrab : AIBase
    {
		[SerializeField]
		private GameObject projectile;

		[Tooltip("Distance at which the projectile spawns")]
		[SerializeField]
		private float initialProjectileDistance = 0.7f;

		[Tooltip("The angle of the cone in which we detect and try to shoot at the player")]
		[SerializeField]
		private float coneDetectionAngle;

		[SerializeField]
		private float timeBeforeShoot;

		[SerializeField]
		private float timeRecoil;

		[SerializeField]
		private float timeBetweenShots;

		[SerializeField]
		private float speed;

		[SerializeField]
		private Rigidbody2D rb;

		private bool canShoot = true;
		private bool canMove = true;

		protected override void Move()
		{
			if (!canMove)
				return;

			Vector2 direction = (goal - (Vector2) transform.position).normalized;
			rb.velocity = direction * speed;
		}

		protected override void FindWhereToGo()
		{
			if (playerTransform == null)
				return;

			//goal is inverse relative to us of player pos 
			goal = transform.position - (playerTransform.Transform.position - transform.position);
		}

		protected override void AIUpdate()
		{
			base.AIUpdate();

			//TODO Raycast to check if player is visible

			//Find if player is within an error margin of our cardinal directions
			//Get player direction vector
			Vector2 playerDir = playerTransform.Transform.position - transform.position;

			//Find minimum angle between player and our four cardinal directions.
			float angle = Vector2.Angle(playerDir, Vector2.left);
			float angle2 = Vector2.Angle(playerDir, Vector2.up);
			if (angle > angle2)
				angle = angle2;
			angle2 = Vector2.Angle(playerDir, Vector2.right);
			if (angle > angle2)
				angle = angle2;
			angle2 = Vector2.Angle(playerDir, Vector2.down);
			if (angle > angle2)
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
			if (!canShoot)
				yield break;

			canShoot = false;
			canMove = false;

			//Find direction
		 	Direction cardinal = DirectionUtils.VectorToEnumDirection(direction);

			//Turn around
			transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, DirectionUtils.GetAngle(cardinal)));

			//Communicate start shoot
			yield return new WaitForSeconds(timeBeforeShoot);

			//shoot
			Shoot(direction);

			yield return new WaitForSeconds(timeRecoil);

			canMove = true;

			yield return new WaitForSeconds(timeBetweenShots);

			canShoot = true;
		}

		void Shoot(Vector3 direction)
		{
			Instantiate(projectile, transform.position + direction.normalized * initialProjectileDistance,
				Quaternion.LookRotation(Vector3.forward, direction), null);
		}
    }
}
