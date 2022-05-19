using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace ProjectAwakening
{
    public class GameManager : MonoBehaviour
	{
		//Instance of the locator patern
		public static GameManager INSTANCE = null;

		[SerializeField]
		private List<SceneReference> overWorlds;

		[SerializeField]
		private SceneReference dungeon;

		public int Level { get; private set; } = 0;

		private void Awake()
		{
			if (INSTANCE != null)
			{
				Destroy(gameObject);
				return;
			}

			INSTANCE = this;

			DontDestroyOnLoad(gameObject);

			if (overWorlds.IsNullOrEmpty())
			{
				Debug.LogError("No scenes in game manager, stoupid");
				return;
			}

			ChangeScene(overWorlds[0]);
		}

		public void GoBackToOverworld()
		{
			//TODO
		}

		public void GoToNextLevel()
		{
			Level++;

			if (overWorlds.Count <= Level)
			{
				//TODO Reach end scene
				Debug.Log("END REACHED");
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
			//TODO show effects / screen

			ChangeScene(overWorlds[Level]);
		}

		private void ChangeScene(SceneReference sceneRef)
		{
			//TODO add loading effects

			sceneRef?.LoadScene();
		}
    }
}
