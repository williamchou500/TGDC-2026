using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bullshitOne;
    public AudioClip bullshitTwo;

    public void PlayOne()
    {
        audioSource.PlayOneShot(bullshitOne);
    }

    public void PlayTwo()
    {
        audioSource.PlayOneShot(bullshitTwo);
    }
}
