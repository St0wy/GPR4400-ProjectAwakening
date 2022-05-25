using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies
{
    public class AISlime : AIBase
    {
		[Header("Slime movement")]
		[Tooltip("The chance that the slime perform a move each fixed update")]
		[SerializeField]
		private float moveChance = 0.1f;

		[SerializeField]
		private float jumpSpeed;

		[Tooltip("How long the slime takes to start the jump")]
		[SerializeField]
		private float telegraphTime;

		[SerializeField]
		private float jumpTime;

		[Tooltip("Time the slime will spend squashed at the end")]
		[SerializeField]
		private float landTime;

		[Tooltip("Minimum time the slime will wait in normal mod before attempting to jump again")]
		[SerializeField]
		private float restTime;

		private bool isJumping = false;

		[SerializeField]
		private Rigidbody2D rb;

		[SerializeField]
		private SpriteRenderer sp;

		[SerializeField]
		private Pathfinding.Seeker seeker;

		[Header("visuals")]
		[SerializeField]
		private Sprite normal;
		[SerializeField]
		private Sprite squash;
		[SerializeField]
		private Sprite stretch;

		private void Start()
		{
			//seeker.pathCallback = OnPathComplete;
		}

		protected override void AIUpdate()
		{
			//Slimes don't recalculate at fixed intervals but rather just before they jump
		}

		protected override void FindWhereToGo()
		{
			if (playerTransform.Transform == null)
				return;

			if (isJumping)
				return;

			goal = playerTransform.Transform.position;
		}

		void OnPathComplete(Pathfinding.OnPathDelegate pathDelegate)
		{

		}

		protected override void Move()
		{
			if (isJumping)
				return;

			//Random chance to bail on the attack
			if (Random.Range(0.0f, 1.0f) > moveChance)
				return;

			FindWhereToGo();
			StartCoroutine(Jump((goal - rb.position).normalized));
		}

		IEnumerator Jump(Vector2 direction)
		{
			isJumping = true;

			sp.sprite = squash;

			yield return new WaitForSeconds(telegraphTime);

			Vector2 initialPos = rb.position;

			//Move
			rb.velocity = direction * jumpSpeed;

			sp.sprite = stretch;

			yield return new WaitForSeconds(jumpTime);
			rb.velocity = Vector2.zero;

			sp.sprite = squash;

			yield return new WaitForSeconds(landTime);

			sp.sprite = normal;

			//Wait for rest period
			yield return new WaitForSeconds(restTime + Random.Range(0.0f, restTime / 2.0f));

			isJumping = false;
		}
	}
}
