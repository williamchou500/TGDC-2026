using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bullshitOne;
    public AudioClip bullshitTwo;
    public AudioClip bullshitThree;

    public void PlayOne()
    {
        audioSource.PlayOneShot(bullshitOne);
    }

    public void PlayTwo()
    {
        audioSource.PlayOneShot(bullshitTwo);
    }

    public void PlayThree()
    {
        audioSource.PlayOneShot(bullshitThree);
    }
}
