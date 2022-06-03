using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAwakening.UI
{
	public class PauseMenuBehaviour : MonoBehaviour
	{
		[SerializeField] private GameObject content;
		[SerializeField] private SceneReference mainMenuScene;
		[SerializeField] private Button firstSelected;

		private bool isPaused;

		public bool IsPaused
		{
			get => isPaused;
			private set
			{
				isPaused = value;
				content.SetActive(isPaused);
				Time.timeScale = isPaused ? 0f : 1f;
			}
		}

		public void TogglePause()
		{
			if (IsPaused) Resume();
			else Pause();
		}

		public void Pause()
		{
			IsPaused = true;
			firstSelected.Select();
		}

		public void Resume()
		{
			IsPaused = false;
		}

		public void GoToMainMenu()
		{
			mainMenuScene.LoadScene();
			IsPaused = false;
		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}
}