using System;
using System.Collections.Generic;
using MyBox;
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

		public int Level { get; private set; }

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
				return;
			}

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

			ChangeScene(overWorlds[Level]);
		}

		public void GoIntoDungeon()
		{
			ChangeScene(dungeon);
		}

		public void Lose()
		{
			// TODO show effects / screen

			ChangeScene(overWorlds[Level]);
		}

		private void ChangeScene(SceneReference sceneRef)
		{
			//  TODO add loading effects

			sceneRef?.LoadScene();
		}
    }
}
