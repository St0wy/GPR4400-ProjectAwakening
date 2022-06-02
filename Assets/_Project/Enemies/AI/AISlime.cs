using System.Collections;
using UnityEngine;

namespace ProjectAwakening.Enemies.AI
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

		private bool isJumping;

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

		private Pathfinding.Path path;

		private void Awake()
		{
			seeker.pathCallback = OnPathComplete;
		}

		private void OnPathComplete(Pathfinding.Path newPath)
		{
			path?.Release(path);
			path = newPath;
			path.Claim(path);
		}

		protected override void AIUpdate()
		{
			FindWhereToGo();
		}

		protected override void FindWhereToGo()
		{
			if (playerTransform.Transform == null)
				return;

			if (isJumping)
				return;

			goal = playerTransform.Transform.position;

			seeker.StartPath(transform.position, goal);
		}

		protected override void Move()
		{
			if (isJumping)
				return;

			// Random chance to bail on the attack
			if (Random.Range(0.0f, 1.0f) > moveChance)
				return;

			Vector2 target;

			// Charge straight towards the player if we have a clear view or if we don't have a path
			Vector3 position = transform.position;
			Vector3 playerPos = playerTransform.Transform.position;

			RaycastHit2D hit = Physics2D.Raycast(position,
				playerPos - position,
				(playerPos - position).magnitude,
				layerMask: LayerMask.GetMask("Default"));

			if (!hit || path == null)
			{
				target = playerTransform.Transform.position;
			}
			else
			{
				target = path.vectorPath[1];
			}

			StartCoroutine(Jump((target - rb.position).normalized));
		}

		private IEnumerator Jump(Vector2 direction)
		{
			isJumping = true;

			sp.sprite = squash;

			yield return new WaitForSeconds(telegraphTime);

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