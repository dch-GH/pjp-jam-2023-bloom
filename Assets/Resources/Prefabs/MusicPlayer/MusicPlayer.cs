using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    public AudioSource Source;
    public AudioClip NormalMusic;
    public AudioClip RadStormMusic;

    void Awake()
    {
        Instance = this;
        PlaySong(NormalMusic);
    }

    public void PlaySong(AudioClip which)
    {
        Source.Stop();
        Source.clip = which;
        Source.Play();
    }

}