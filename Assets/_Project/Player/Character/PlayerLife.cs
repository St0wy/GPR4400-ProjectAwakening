using UnityEngine;

namespace ProjectAwakening.Player
{
	public class PlayerLife : Life
	{
		[SerializeField]
		TransformReferenceScriptableObject playerTransform;

		[SerializeField]
		PlayerActions playerActions;

		public override bool Damage(int damageAmount, Vector2 damageOrigin, float knockbackMod = 1)
		{
			if (playerActions.ActionState == ActionState.Shield)
			{
				damageAmount = Mathf.RoundToInt(damageAmount / 2.0f) - 1;
				if (damageAmount < 0)
				{
					//Todo play sound

					return false;
				}

				knockbackMod /= 2.0f;
			}

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
			if (GameManager.GameManager.Instance != null)
			{
				GameManager.GameManager.Instance.Lose();
			}
		}
	}
}