using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    public bool ThemeIsPlaying { get; set; }

    private void Awake()
    {
        instance = this;
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    private void Start()
    {
        Play("Theme");
        ThemeIsPlaying = true;
    }

    public void Play(string name)
    {
        foreach(Sound s in sounds)
        {
            if(s.name == name)
            {
                s.source.Play();
            }
        }
    }

    public void Stop(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Stop();
            }
        }
    }
}
