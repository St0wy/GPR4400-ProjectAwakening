using MyBox;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	public class DoorBehaviour : MonoBehaviour
	{
		[SerializeField] private SceneReference roomScene;
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			roomScene.LoadSceneAsync();
		}
	}
}
