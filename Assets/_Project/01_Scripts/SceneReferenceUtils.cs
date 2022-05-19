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
	}
}