namespace ProjectAwakening.Player
{
	public class PlayerLife : Life
	{
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