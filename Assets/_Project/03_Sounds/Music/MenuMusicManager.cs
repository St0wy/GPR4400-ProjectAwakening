using UnityEngine;

namespace ProjectAwakening.Music
{
	/// <summary>
	/// Script to put on the music audio source to be delete once a level is started.
	/// </summary>
	public class MenuMusicManager : MonoBehaviour
	{
		private AudioSource musicSource;

		private void Awake()
		{
			musicSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			DontDestroyOnLoad(this);
		}

		public void PlayMusic()
		{
			if (!musicSource.isPlaying)
			{
				musicSource.Play();
			}
		}

		public void StopMusic()
		{
			if (musicSource.isPlaying)
			{
				musicSource.Stop();
			}
		}
	}
}