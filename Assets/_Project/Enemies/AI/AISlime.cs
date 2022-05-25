using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies
{
    public class AISlime : AIBase
    {
		[Header("Slime movement")]
		[SerializeField]
		float jumpSpeed;
		[SerializeField]
		float jumpTime;

		[SerializeField]
		float restTime;

		bool isJumping = false;

		Rigidbody2D rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
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

		protected override void Move()
		{
			if (isJumping)
				return;

			FindWhereToGo();
			StartCoroutine(Jump((goal - rb.position).normalized));
		}

		IEnumerator Jump(Vector2 direction)
		{
			isJumping = true;

			Vector2 initialPos = rb.position;

			//Move
			rb.velocity = direction * jumpSpeed;
			yield return new WaitForSeconds(jumpTime);
			rb.velocity = Vector2.zero;

			//Wait for rest period
			yield return new WaitForSeconds(restTime + Random.Range(0.0f, restTime / 2.0f));

			isJumping = false;
		}
	}
}
