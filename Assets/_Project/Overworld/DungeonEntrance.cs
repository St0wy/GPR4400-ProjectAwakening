using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening
{
    public class DungeonEntrance : MonoBehaviour
    {
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player"))
				return;

			//TODO make the entrance delayed and add effects and sound

			GameManager.INSTANCE?.GoIntoDungeon();
		}
	}
}
