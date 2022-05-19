using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Player
{
    public class PlayerLife : Life
    {
		protected override void Die()
		{
			base.Die();

			//Communicate with gameManager that we died
			GameManager.INSTANCE?.Lose();
		}
	}
}
