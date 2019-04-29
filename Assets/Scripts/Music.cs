using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;

    [Range(0,1)]
    public float volume = 1;

    protected AudioSource source;

    protected virtual void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = volume;

        StartCoroutine(Play());
    }

    protected virtual IEnumerator Play()
    {
        source.loop = false;
        source.PlayOneShot(intro);

        yield return new WaitUntil(() => !source.isPlaying);

        source.loop = true;
        source.clip = loop;
        source.Play();
    }
}
