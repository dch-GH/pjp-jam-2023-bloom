using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundCollection : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _clips;
    private AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayRandom()
    {
        _source.Stop();
        var randomIndex = UnityEngine.Random.Range(0, _clips.Length - 1);
        var clip = _clips[randomIndex];
        _source.PlayOneShot(clip);
    }
}