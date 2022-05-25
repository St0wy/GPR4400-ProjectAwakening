using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAwakening
{
	public static class SceneReferenceUtils
	{
		public static AsyncOperation UnloadSceneAsync(this SceneReference sceneReference)
		{
			return SceneManager.UnloadSceneAsync(sceneReference.Scene.name);
		}

		public static bool SetActive(this SceneReference sceneReference)
		{
			return SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneReference.Scene.name));
		} 
	}
}