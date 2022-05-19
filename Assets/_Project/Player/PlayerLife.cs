using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening
{
    public class PlayerLife : Life
    {
		protected override void Die()
		{
			base.Die();

			//Todo Communicate with gameManager that we died
		}
	}
}
