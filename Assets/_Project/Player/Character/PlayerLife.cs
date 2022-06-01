using UnityEngine;

namespace ProjectAwakening.Player.Character
{
	public class PlayerLife : Life
	{
		public delegate void HurtEvent(int damageAmount);
		
		[SerializeField] private TransformReferenceScriptableObject playerTransform;
		[SerializeField] private PlayerActions playerActions;

		public HurtEvent OnHurt { get; set; }

		public override bool Damage(int damageAmount, Vector2 damageOrigin, float knockbackMod = 1)
		{
			if (playerActions.ActionState != ActionState.Shield)
			{
				OnHurt?.Invoke(damageAmount);
				return base.Damage(damageAmount, damageOrigin, knockbackMod);
			}

			damageAmount = Mathf.RoundToInt(damageAmount / 2.0f) - 1;
			if (damageAmount < 0)
			{
				// TODO : play sound

				return false;
			}

			knockbackMod /= 2.0f;

			OnHurt?.Invoke(damageAmount);
			return base.Damage(damageAmount, damageOrigin, knockbackMod);
		}

		private void Start()
		{
			playerTransform.SetReference(transform);
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