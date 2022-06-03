using MyBox;
using ProjectAwakening.Player.Character;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.Dungeon
{
	public class EndStairsBehaviour : MonoBehaviour
	{
		[SerializeField] private SceneReference victoryScene;

		private void OnTriggerEnter2D(Collider2D col)
		{
			this.LogSuccess("End of dungeon. GG WP");
			if (!col.TryGetComponent(out PlayerLife playerLife)) return;

			GameManager.Instance.PlayerLife = playerLife.Lives;
			GameManager.Instance.GoToNextLevel();
		}
	}
}