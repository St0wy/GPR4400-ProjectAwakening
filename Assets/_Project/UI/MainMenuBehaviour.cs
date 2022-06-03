using MyBox;
using ProjectAwakening.Music;
using UnityEngine;

namespace ProjectAwakening.UI
{
	public class MainMenuBehaviour : MonoBehaviour
	{
		[SerializeField] private SceneReference creditsScene;
		[SerializeField] private SceneReference mainMenuScene;

		private MenuMusicManager menuMusic;

		private void Start()
		{
			menuMusic = FindObjectOfType<MenuMusicManager>();
		}

		public void StartGame()
		{
			GameManager.Instance.StartFirstLevel();
			StopMusic();
		}

		public void StopMusic()
		{
			menuMusic.StopMusic();
		}

		public void ShowCreditsMenu()
		{
			creditsScene.LoadScene();
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public void CloseCredits()
		{
			mainMenuScene.LoadScene();
		}
	}
}
