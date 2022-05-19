using System;
using System.Collections;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.Player.Sword
{
	public class SwordBehaviour : MonoBehaviour
	{
		[SerializeField] private Animator swordAnimator;

		private BoxCollider2D boxCollider;

		private void Awake()
		{
			boxCollider = GetComponent<BoxCollider2D>();
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
			transform.rotation = Quaternion.Euler(0, 0, angle);

			// Enable hit box and sword game object
			swordAnimator.gameObject.SetActive(true);

			float attackDuration = swordAnimator.GetCurrentAnimatorStateInfo(0).length;
			swordAnimator.Play(SwordAnimations.Attack);

			StartCoroutine(StopAttackCoroutine(attackDuration));
			return attackDuration;
		}

		private IEnumerator StopAttackCoroutine(float timeToStop)
		{
			yield return new WaitForSeconds(timeToStop);

			swordAnimator.gameObject.SetActive(false);
		}
	}
}