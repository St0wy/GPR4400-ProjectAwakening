using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectAwakening.Music
{
	public class MenuMusicSpawner : MonoBehaviour
	{
		[FormerlySerializedAs("menuMusicPrefab")] [SerializeField]
		private GameObject musicPrefab;

		private void Awake()
		{
			var musicManager = FindObjectOfType<MenuMusicManager>();
			if (musicManager == null)
			{
				Instantiate(musicPrefab);
			}
		}
	}
}