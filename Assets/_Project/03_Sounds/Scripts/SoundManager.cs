using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening
{
	public class SoundManager : MonoBehaviour
	{
		[SerializeField] private SoundRequests requests;
		[SerializeField] private int maxSourcesAtOnce = 16;
		private int currentSources;

		private readonly List<AudioClip> clipsBacklog = new();
		private bool emptyingBacklog;
		private readonly List<AudioSource> inUseSources = new();
		private readonly List<AudioSource> unUsedSources = new();

		private void Start()
		{
			requests.OnRequest += OnRequest;
		}

		private void OnDestroy()
		{
			requests.OnRequest -= OnRequest;
		}

		private void FixedUpdate()
		{
			// Add all sources that aren't playing to unused
			unUsedSources.AddRange(inUseSources.FindAll(x => !x.isPlaying));
			
			// Remove them from inUse
			inUseSources.RemoveAll(x => !x.isPlaying);
		}

		private void OnRequest(AudioClip clip)
		{
			if (inUseSources.Count == maxSourcesAtOnce)
			{
				//Start emptying backlog
				if (clipsBacklog.Count == 0)
					StartCoroutine(EmptyBacklog());

				//Add to backlog
				clipsBacklog.Add(clip);

				return;
			}

			Play(clip);
		}

		private bool Play(AudioClip clip)
		{
			if (unUsedSources.Count > 0)
			{
				// Play the clip
				unUsedSources[0].PlayOneShot(clip);

				// move the source from unused to used
				inUseSources.Add(unUsedSources[0]);
				unUsedSources.RemoveAt(0);
			}
			else if (currentSources < maxSourcesAtOnce)
			{
				currentSources++;

				// Create the new source
				var source = gameObject.AddComponent<AudioSource>();
				// Make the source play the clip
				source.PlayOneShot(clip);

				// Add the source to our list of used sources
				inUseSources.Add(source);
			}
			else
			{
				return false;
			}

			return true;
		}

		private IEnumerator EmptyBacklog()
		{
			// Security to avoid being executed twice
			if (emptyingBacklog)
				yield break;
			emptyingBacklog = true;

			do
			{
				yield return null;

				//Try to play a clip if there's an unused source
				if (unUsedSources.Count <= 0) continue;
				
				if (Play(clipsBacklog[^1]))
				{
					//Remove the clip if we managed to play it
					clipsBacklog.RemoveAt(clipsBacklog.Count - 1);
				}
			} while (clipsBacklog.Count > 0);
		}
	}
}
