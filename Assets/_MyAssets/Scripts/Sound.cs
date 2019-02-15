using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop = false;

    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Range(-3f, 3f)]
    public float pitch = 1.0f;

    [Range(0f, 1f)]
    public float spatialBlend = 0f;

    [HideInInspector]
    public AudioSource source;
    
    
}
