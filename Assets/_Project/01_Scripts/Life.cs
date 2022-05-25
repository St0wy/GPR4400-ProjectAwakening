using System.Collections;
using UnityEngine;

namespace ProjectAwakening
{
	public class Life : MonoBehaviour
	{
		[Header("DamagedVariables")]
		[SerializeField]
		protected float invincibilityTime = 0.5f;
		[SerializeField]
		protected float blinkTime;
		[SerializeField]
		protected float blinkSpeed = 1.0f;
		[SerializeField]
		protected float knockbackForce;

		[SerializeField]
		protected float knockbackTime = 0.2f;

		[SerializeField]
		protected SpriteRenderer sp;
		[SerializeField]
		protected Rigidbody2D rb;

		// ReSharper disable InconsistentNaming
		protected bool canBeDamaged = true;
		protected Coroutine currentBlink;
		// ReSharper restore InconsistentNaming

		[field: SerializeField]
		public int Lives { get; protected set; } = 5;

		public bool IsDead { get; protected set; }

		public bool IsBeingKnockedBack { get; protected set; }

		/// <summary>
		/// Damages the entity, reducing life and inflicting knockback.
		/// </summary>
		/// <param name="damageAmount">The amount of damage to inflict.</param>
		/// <returns>Whether the damage could be applied.</returns>
		public bool Damage(int damageAmount)
		{
			return Damage(damageAmount, Vector2.up);
		}

		/// <summary>
		/// Damages the entity, reducing life and inflicting knockback.
		/// </summary>
		/// <param name="damageAmount">The amount of damage to inflict.</param>
		/// <param name="damageOrigin">The origin of the damages.</param>
		/// <param name="knockbackMod">Modifier for the intensity of the knockback.</param>
		/// <returns>Whether the damage could be applied.</returns>
		public virtual bool Damage(int damageAmount, Vector2 damageOrigin, float knockbackMod = 1.0f)
		{
			if (!canBeDamaged)
				return false;

			Lives -= damageAmount;

			if (Lives <= 0)
			{
				Die();
			}

			OnDamageEffects(damageOrigin, knockbackMod);

			return true;
		}

		protected void OnDamageEffects(Vector2 damageOrigin, float knockbackMod)
		{
			Vector2 damageDir = -((Vector2) transform.position - damageOrigin).normalized;

			Knockback(damageDir, knockbackMod);

			if (!IsDead)
			{
				StartBlinking();
				BecomeInvincible();
			}

			PlaySound();
		}

		protected virtual void Die()
		{
			IsDead = true;
			canBeDamaged = false;
		}

		protected void Knockback(Vector2 damageDir, float knockBackMod)
		{
			if (rb != null)
				rb.AddForce(damageDir * -1.0f * knockbackForce * knockBackMod);

			IsBeingKnockedBack = true;
			StartCoroutine(StopKnockBackCoroutine());
		}

		IEnumerator StopKnockBackCoroutine()
		{
			yield return new WaitForSeconds(knockbackTime);

			IsBeingKnockedBack = false;
		}

		protected void StartBlinking()
		{
			if (sp == null) return;

			if (currentBlink != null)
				StopCoroutine(currentBlink);

			currentBlink = StartCoroutine(BlinkCoroutine());
		}

		protected IEnumerator BlinkCoroutine()
		{
			float timeLeft = blinkTime;

			while (timeLeft > 0.0f)
			{
				sp.enabled = !sp.enabled;

				float waitTime = 1 / blinkSpeed;
				timeLeft -= waitTime;

				yield return new WaitForSeconds(waitTime);
			}

			sp.enabled = true;
		}

		protected void BecomeInvincible()
		{
			canBeDamaged = false;

			StartCoroutine(RemoveInvincibilityCoroutine());
		}

		/// <summary>
		/// Removes the invincibility with a delay.
		/// </summary>
		protected IEnumerator RemoveInvincibilityCoroutine()
		{
			yield return new WaitForSeconds(invincibilityTime);

			canBeDamaged = true;
		}

		protected void PlaySound()
		{
			// TODO actually play a sound
			if (IsDead) { }
			else { }
		}
	}
}