using MyBox;
using UnityEngine;

namespace ProjectAwakening
{
	public class MainMenuBehaviour : MonoBehaviour
	{
		[SerializeField] private SceneReference world1Scene;
		[SerializeField] private SceneReference creditsScene;
		[SerializeField] private SceneReference mainMenuScene;
		
		public void StartGame()
		{
			world1Scene.LoadScene();
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
