using System;
using MyBox;
using ProjectAwakening.Music;
using UnityEngine;

namespace ProjectAwakening.UI
{
	public class MainMenuBehaviour : MonoBehaviour
	{
		[SerializeField] private SceneReference world1Scene;
		[SerializeField] private SceneReference creditsScene;
		[SerializeField] private SceneReference mainMenuScene;

		private MenuMusicManager menuMusic;

		private void Awake()
		{
			menuMusic = FindObjectOfType<MenuMusicManager>();
		}

		public void StartGame()
		{
			world1Scene.LoadScene();
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
