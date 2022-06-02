using UnityEngine;

namespace ProjectAwakening.Player.Character
{
	public class PlayerLife : Life
	{
		public delegate void HurtEvent();

		[SerializeField] private TransformReferenceScriptableObject playerTransform;
		[SerializeField] private PlayerActions playerActions;

		public HurtEvent OnHurt { get; set; }

		private void Start()
		{
			if (GameManager.Instance.HasNewLife)
			{
				Lives = GameManager.Instance.PlayerLife;
				OnHurt?.Invoke();
			}

			playerTransform.SetReference(transform);
		}

		public override bool Damage(int damageAmount, Vector2 damageOrigin, float knockbackMod = 1)
		{
			if (playerActions.ActionState != ActionState.Shield)
			{
				return base.Damage(damageAmount, damageOrigin, knockbackMod);
			}

			damageAmount = Mathf.RoundToInt(damageAmount / 2.0f) - 1;
			if (damageAmount < 0)
			{
				// TODO : play sound

				return false;
			}

			knockbackMod /= 2.0f;

			return base.Damage(damageAmount, damageOrigin, knockbackMod);
		}

		protected override void OnDamageEffects(Vector2 damageOrigin, float knockbackMod)
		{
			base.OnDamageEffects(damageOrigin, knockbackMod);
			OnHurt?.Invoke();
		}

		protected override void Die()
		{
			base.Die();

			// Communicate with gameManager that we died
			if (GameManager.Instance != null)
			{
				GameManager.Instance.Lose();
			}
		}
	}
}