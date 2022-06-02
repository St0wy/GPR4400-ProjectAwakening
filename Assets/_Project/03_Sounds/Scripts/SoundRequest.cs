using UnityEngine;
using UnityEngine.Events;

namespace ProjectAwakening
{
	[CreateAssetMenu(fileName = "SoundRequest", menuName = "ScriptableObjects/SoundRequests", order = 4)]
	public class SoundRequest : ScriptableObject
	{
		public UnityAction<AudioClip> OnRequest = null;

		public void Request(AudioClip clip)
		{
			if (clip != null)
				OnRequest?.Invoke(clip);
		}
	}
}