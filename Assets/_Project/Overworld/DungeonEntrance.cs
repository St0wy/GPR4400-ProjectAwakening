using ProjectAwakening.Player.Character;
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
			}

			// TODO make the entrance delayed and add effects and sound

			GameManager.Instance.GoIntoDungeon();
		}
	}
}