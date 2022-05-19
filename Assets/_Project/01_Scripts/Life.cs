using System.Collections;
using UnityEngine;

namespace ProjectAwakening
{
    public class Life : MonoBehaviour
    {
		[field: SerializeField]
		public int Lives { get; protected set; } = 5;
		
		public bool IsDead { get; protected set; }

		public bool IsBeingKnockedBack { get; protected set; }

		[Header("DamagedVariables")]

		[SerializeField] 
		protected float invincTime = 0.0f;
		[SerializeField]
		protected float blinkTime = 0.0f;
		[SerializeField]
		protected float blinkSpeed = 1.0f;
		[SerializeField]
		protected float knockbackForce = 0.0f;

		[SerializeField]
		protected SpriteRenderer sp;
		[SerializeField]
		protected Rigidbody2D rb;

		protected bool canBeDamaged = true;

		protected Coroutine currentBlink = null;

		/// <summary>
		/// Damages the entity, reducing damage and inflicting knockback
		/// </summary>
		/// <param name="damageAmount"></param>
		/// <returns>wether the damage could be applied</returns>
		public bool Damage(int damageAmount, Vector2? damageDir = null)
		{
			if (!canBeDamaged)
				return false;

			if (damageDir == null)
				damageDir = Vector2.up;

			Lives -= damageAmount;

			if (Lives <= 0)
			{
				Die();
			}

			OnDamageEffects((Vector2) damageDir);

			return true;
		}

		protected void OnDamageEffects(Vector2 damageDir)
		{
			Knockback(damageDir);

			if (!IsDead)
			{
				StartBlinking();
				BecomeInvincible();
			}

			PlaySound();
		}

		protected void Die()
		{
			IsDead = true;
			canBeDamaged = false;
		}

		protected void Knockback(Vector2 damageDir)
		{
			if (rb != null)
				rb.AddForce(damageDir * -1.0f);
		}

		protected void StartBlinking()
		{
			if (sp != null)
			{
				if (currentBlink != null)
					StopCoroutine(currentBlink);

				currentBlink = StartCoroutine(Blink());
			}
		}

		protected IEnumerator Blink()
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

			StartCoroutine(DelayedLoseInvis());
		}

		protected IEnumerator DelayedLoseInvis()
		{
			yield return new WaitForSeconds(invincTime);

			canBeDamaged = true;
		}

		protected void PlaySound()
		{
			//TODO actually play a sound
			if (IsDead)
			{ }
			else
			{ }
		}
    }
}
