using System;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.Dungeon
{
	public class EndStairsBehaviour : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D col)
		{
			// TODO : Load next level
			this.LogSuccess("End of dungeon. GG WP");
			GameManager.Instance.GoToNextLevel();
		}
	}
}