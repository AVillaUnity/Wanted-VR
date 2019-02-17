using UnityEngine;
using Valve.VR;

public class SwingSound : MonoBehaviour
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Behaviour_Pose pose;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(pose.GetVelocity().magnitude > 1 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
