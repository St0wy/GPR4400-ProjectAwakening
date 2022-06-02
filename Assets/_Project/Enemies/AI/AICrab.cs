﻿using System.Collections;
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
		[SerializeField] private float coneDetectionAngle;

		[SerializeField] private float timeBeforeShoot;
		[SerializeField] private float timeRecoil; 
		[SerializeField] private float timeBetweenShots;

		[SerializeField] private float speed;

		[SerializeField] private float fleeRange;
		[Tooltip("Maximum distance a random point to wander to will be chos")]
		[SerializeField] private float wanderMaxRange;
		[Tooltip("Maximum time the crab will wander toward a single goal for")]
		[SerializeField] private float wanderMaxTime;
		[SerializeField] private float arrivalMargin;

		[SerializeField]
		private Rigidbody2D rb;

		[SerializeField]
		private SpriteRenderer sp;

		[SerializeField]
		private Animator animator;

		private Direction curDir = Direction.Down;

		private bool flee = false;
		private bool canShoot = true;
		private bool canMove = true;

		private bool canChooseWanderTarget = true;

		protected override void Move()
		{
			if (!canMove)
				return;

			//Don't move if we're close to our goal
			Vector2 toGoal = goal - (Vector2) transform.position;
			if (toGoal.sqrMagnitude < arrivalMargin)
			{
				animator.speed = 0.2f;
				return;
			}

			animator.speed = 1.0f;

			Vector2 direction = toGoal.normalized;
			rb.velocity = direction * speed;

			//Change direction
			FaceDirection(direction);
		}

		protected override void FindWhereToGo()
		{
			if (playerTransform == null)
				return;

			if (flee)
			{
				//goal is inverse relative to us of player pos 
				goal = transform.position - (playerTransform.Transform.position - transform.position);
			}
			else
			{
				//Check that we didn't pick a direction too quickly
				if (!canChooseWanderTarget)
					return;

				//Pick a random direction
				goal = (Vector2) transform.position + new Vector2(Random.Range(-wanderMaxRange, wanderMaxRange), Random.Range(-wanderMaxRange, wanderMaxRange));

				//Reset the time until we chose a new direction
				StartCoroutine(BlockWanderTargetTemp());
			}
		}

		private IEnumerator BlockWanderTargetTemp()
		{
			canChooseWanderTarget = false;

			yield return new WaitForSeconds(Random.Range(0.0f, wanderMaxTime));

			canChooseWanderTarget = true;
		}

		protected override void AIUpdate()
		{
			Vector2 playerDir = playerTransform.Transform.position - transform.position;

			//Try shoot
			if (CheckSightline())
			{
				
				StartCoroutine(PrepareShot(playerDir));
			}

			//Decide to flee or not
			if (playerDir.magnitude < fleeRange)
				flee = true;
			else
				flee = false;

			base.AIUpdate();

		}

		private bool CheckSightline()
		{
			if (!canShoot)
				return false;

			//Raycast to check if something is between us and the player is visible
			if (Physics2D.Raycast(transform.position, playerTransform.Transform.position - transform.position,
				(playerTransform.Transform.position - transform.position).magnitude, layerMask: LayerMask.GetMask("Default")))
				return false;

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
					return true;
			}

			return false;
		}

		private void FaceDirection(Vector2 dir)
		{
			//Find direction
			Direction cardinal = DirectionUtils.VectorToEnumDirection(dir);

			if (curDir == cardinal)
				return;

			curDir = cardinal;

			//Face toward it
			sp.flipX = false;
			switch (cardinal)
			{
				case Direction.Up:
					animator.SetInteger("Direction", 1);
					break;
				case Direction.Down:
					animator.SetInteger("Direction", -1);
					break;
				case Direction.Left:
					sp.flipX = true;
					animator.SetInteger("Direction", 0);
					break;
				case Direction.Right:
					animator.SetInteger("Direction", 0);
					break;
				default: break;
			}

			animator.SetTrigger("ChangeAnim");
		}

		private IEnumerator PrepareShot(Vector2 direction)
		{
			if (!canShoot)
				yield break;

			canShoot = false;
			canMove = false;

			//Communicate start shoot
			FaceDirection(direction);

			yield return null;

			//Stop moving
			animator.speed = 0.0f;

			yield return new WaitForSeconds(timeBeforeShoot);

			//shoot
			Shoot(direction);

			yield return new WaitForSeconds(timeRecoil);

			canMove = true;

			animator.speed = 1;

			yield return new WaitForSeconds(timeBetweenShots);

			canShoot = true;
		}

		private void Shoot(Vector3 direction)
		{
			Instantiate(projectile, transform.position + direction.normalized * initialProjectileDistance,
				Quaternion.LookRotation(Vector3.forward, direction), null);
		}
    }
}
