using System.Collections;
using UnityEngine;

namespace ProjectAwakening.Player.Sword
{
	public class SwordBehaviour : MonoBehaviour
	{
		[SerializeField] private BoxCollider2D box;
		[SerializeField] private Animator swordAnimator;
		[SerializeField] private Transform sword;
		[SerializeField] private int damage = 2;

		private bool isAttacking;
		private readonly Collider2D[] colliders = new Collider2D[15];
		private Coroutine stopAttackCoroutine;

		private void Awake()
		{
			GetComponent<BoxCollider2D>();
		}

		private void Update()
		{
			if (isAttacking)
			{
				HandleAttack();
			}
		}

		private void HandleAttack()
		{
			int amount = box.GetContacts(colliders);

			for (var i = 0; i < amount; i++)
			{
				// Check that we are not hitting ourself
				if (colliders[i].CompareTag(gameObject.tag)) continue;

				// Get the life component and hit if it exists
				if (colliders[i].TryGetComponent(out Life life))
				{
					life.Damage(damage, transform.position);
				}
			}
		}

		/// <summary>
		/// Triggers the attack of the sword
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public float Attack(Direction direction)
		{
			// Set the rotation of the sword
			float angle = DirectionUtils.GetAngle(direction);
			sword.rotation = Quaternion.Euler(0, 0, angle);

			// Enable hit box and sword game object
			swordAnimator.gameObject.SetActive(true);
			isAttacking = true;

			float attackDuration = swordAnimator.GetCurrentAnimatorStateInfo(0).length;
			swordAnimator.Play(SwordAnimations.Attack);

			stopAttackCoroutine = StartCoroutine(StopAttackCoroutine(attackDuration));
			return attackDuration;
		}

		public void StopAttackNow()
		{
			StopCoroutine(stopAttackCoroutine);
			StopAttack();
		}

		private IEnumerator StopAttackCoroutine(float timeToStop)
		{
			yield return new WaitForSeconds(timeToStop);
			StopAttack();
		}

		private void StopAttack()
		{
			swordAnimator.gameObject.SetActive(false);
			isAttacking = false;
		}
	}
}