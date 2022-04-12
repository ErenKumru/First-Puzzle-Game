using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    private AudioSource audioSource;
    private int currentAudioClipIndex;

    private void Awake()
    {
        SimpleSingleton();

        audioSource = GetComponent<AudioSource>();

        PlayRandomAudio();
        StartCoroutine(PlayAudioSequentially());
    }

    private void SimpleSingleton()
    {
        int numOfSoundController = FindObjectsOfType<SoundController>().Length;

        if(numOfSoundController > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    private void PlayRandomAudio()
    {
        currentAudioClipIndex = Random.Range(0, audioClips.Count);
        audioSource.clip = audioClips[currentAudioClipIndex];
        audioSource.Play();
    }

    private IEnumerator PlayAudioSequentially()
    {
        yield return new WaitForSeconds(audioClips[currentAudioClipIndex].length);

        currentAudioClipIndex = (currentAudioClipIndex + 1) % audioClips.Count;
        audioSource.clip = audioClips[currentAudioClipIndex];
        audioSource.Play();
    }
}
