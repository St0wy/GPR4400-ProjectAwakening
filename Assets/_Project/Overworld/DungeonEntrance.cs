using UnityEngine;

namespace ProjectAwakening.Overworld
{
	public class DungeonEntrance : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.CompareTag("Player"))
				return;

			// TODO make the entrance delayed and add effects and sound

			if (GameManager.GameManager.Instance != null)
			{
				GameManager.GameManager.Instance.GoIntoDungeon();
			}
		}
	}
}