using UnityEngine;

namespace ProjectAwakening.Player
{
	public class PlayerLife : Life
	{
		[SerializeField]
		TransformReferenceScriptableObject playerTransform;

		private void Awake()
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