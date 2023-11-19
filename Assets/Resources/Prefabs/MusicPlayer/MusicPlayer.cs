using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance => FindFirstObjectByType<MusicPlayer>();

    public AudioSource Source;
    public AudioClip NormalMusic;
    public AudioClip RadStormMusic;

    void Awake()
    {
        PlaySong(NormalMusic);
    }

    public void PlaySong(AudioClip which)
    {
        Source.Stop();
        Source.clip = which;
        Source.Play();
    }

}