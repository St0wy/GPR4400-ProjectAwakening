using ProjectAwakening.Player.Character;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.Overworld
{
	public class DungeonEntrance : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player")) return;
			if (GameManager.Instance == null) return;

			if (collision.TryGetComponent(out PlayerLife life))
			{
				GameManager.Instance.PlayerLife = life.Lives;
				this.Log("YOooooo");
			}

			// TODO make the entrance delayed and add effects and sound

			GameManager.Instance.GoIntoDungeon();
		}
	}
}