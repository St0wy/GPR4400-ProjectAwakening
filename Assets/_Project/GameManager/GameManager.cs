using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using ProjectAwakening.Loading;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening
{
	public class GameManager : MonoBehaviour
	{
		// Instance of the locator pattern
		public static GameManager Instance;

		[SerializeField]
		private List<SceneReference> overWorlds;

		[SerializeField]
		private SceneReference dungeon;
			
		[SerializeField]
		private GameObject loadingScreenVisuals;

		private int playerLife;
		private GameObject loadingScreenInstance = null;

		public int Level { get; private set; }

		public int PlayerLife
		{
			get
			{
				HasNewLife = false;
				return playerLife;
			}
			set
			{
				playerLife = value;
				HasNewLife = true;
			}
		}

		public bool HasNewLife { get; private set; }

		private void Awake()
		{
			// Check if there's already an instance of the game manager to avoid duplicate
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;

			DontDestroyOnLoad(gameObject);

			if (overWorlds.IsNullOrEmpty())
			{
				this.LogError("No scenes in game manager, stoupid");
			}

			// Create loading screen
			if (loadingScreenInstance == null)
			{
				loadingScreenInstance = Instantiate(loadingScreenVisuals);
				DontDestroyOnLoad(loadingScreenInstance);
			}
			SetLoadingScreen(false);

			// ChangeScene(overWorlds[0]);
		}

		public void GoBackToOverworld()
		{
			// TODO
			throw new NotImplementedException();
		}

		public void GoToNextLevel()
		{
			Level++;

			if (overWorlds.Count <= Level)
			{
				// TODO Reach end scene
				this.LogError("END REACHED; no more levels");
				return;
			}

			StartCoroutine(ChangeScene(overWorlds[Level]));
		}

		public void GoIntoDungeon()
		{
			StartCoroutine(ChangeScene(dungeon));
		}

		public void Lose()
		{
			// TODO show effects / screen

			StartCoroutine(ChangeScene(overWorlds[Level]));
		}

		private IEnumerator ChangeScene(SceneReference sceneRef)
		{
			// Check scene exists
			if (sceneRef == null)
			{
				Debug.LogError("SceneReference is null");
				yield break;
			}

			// LoadScreen
			SetLoadingScreen(true);

			// start loading
			AsyncOperation sceneLoading = sceneRef.LoadSceneAsync();

			// Wait for loading to be completed
			do
			{
				// Update loading screen
				loadingScreenInstance.GetComponentInChildren<LoadingBar>().UpdateProgress(sceneLoading.progress);

				yield return null;
			} while (!sceneLoading.isDone);

			// Disable loading screen
			SetLoadingScreen(false);
		}

		private void SetLoadingScreen(bool enable)
		{
			if (loadingScreenInstance == null)
				return;

			loadingScreenInstance.SetActive(enable);
		}
	}
}