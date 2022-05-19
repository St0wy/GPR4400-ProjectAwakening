using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
