namespace ProjectAwakening.Enemies
{
    public class EnemyLife : Life
    {
		protected override void Die()
		{
			base.Die();

			Destroy(gameObject);
		}
	}
}
