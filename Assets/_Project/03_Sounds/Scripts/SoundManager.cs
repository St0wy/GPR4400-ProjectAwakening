using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] SoundRequests _requests;
    [SerializeField] int _maxSourcesAtOnce = 16;
    int _currentSources = 0;

    List<AudioClip> _clipsBacklog = new List<AudioClip>();
    bool _emptyingBacklog = false;
    List<AudioSource> _inUseSources = new List<AudioSource>();
    List<AudioSource> _unUsedSources = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        _requests.OnRequest += OnRequest;
    }

    private void OnDestroy()
    {
        _requests.OnRequest -= OnRequest;
    }

    private void FixedUpdate()
    {
        //Add all sources that aren't playing to unused
        _unUsedSources.AddRange(_inUseSources.FindAll(x => !x.isPlaying));
        //Remove them from inUse
        _inUseSources.RemoveAll(x => !x.isPlaying);
    }

    void OnRequest(AudioClip clip)
    {
        if (_inUseSources.Count == _maxSourcesAtOnce)
        {
            //Start emptying backlog
            if (_clipsBacklog.Count == 0)
                StartCoroutine(EmptyBacklog());

            //Add to backlog
            _clipsBacklog.Add(clip);

            return;
        }

        Play(clip);
    }

    bool Play(AudioClip clip)
    {
        if (_unUsedSources.Count > 0)
        {
            //Play the clip
            _unUsedSources[0].PlayOneShot(clip);

            //move the source from unused to used
            _inUseSources.Add(_unUsedSources[0]);
            _unUsedSources.RemoveAt(0);
        }
        else if (_currentSources < _maxSourcesAtOnce)
        {
            _currentSources++;

            //Create the new source
            AudioSource source = gameObject.AddComponent<AudioSource>();
            //Make the source play the clip
            source.PlayOneShot(clip);

            //Add the source to our list of used sources
            _inUseSources.Add(source);
        }
        else
        {
            return false;
        }

        return true;
    }

    IEnumerator EmptyBacklog()
    {
        //Security to avoid being executed twice
        if (_emptyingBacklog)
            yield break;
        _emptyingBacklog = true;

        do
        {
            yield return null;

            //Try to play a clip if there's an unused source
            if (_unUsedSources.Count > 0)
            {
                if (Play(_clipsBacklog[_clipsBacklog.Count - 1]))
                {
                    //Remove the clip if we managed to play it
                    _clipsBacklog.RemoveAt(_clipsBacklog.Count - 1);
                }
            }
        } while (_clipsBacklog.Count > 0);
    }
}
