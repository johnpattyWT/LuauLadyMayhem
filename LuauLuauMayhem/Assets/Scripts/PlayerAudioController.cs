using UnityEngine;
using System.Collections;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Header("Idle Clips")]
    public AudioClip[] idleClips;
    public float minIdleDelay = 10f;
    public float maxIdleDelay = 30f;

    [Header("Jump Clips")]
    public AudioClip[] jumpClips;

    [Header("Hurt Clips")]
    public AudioClip[] hurtClips;

    [Header("Kill Clips")]
    public AudioClip[] killClips;
    [Range(0, 1)] public float killClipChance = 0.1f;

    private void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        
        StartCoroutine(IdleVoiceLoop());

        // Subscribe to the kill event in the new GameCore system
        if (GameCore.Instance != null)
        {
            GameCore.Instance.OnKill += HandleOnKill;
        }
    }

    private void Update()
    {
        // Plays jump sound when space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayRandomClip(jumpClips);
        }
    }

    private IEnumerator IdleVoiceLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minIdleDelay, maxIdleDelay);
            yield return new WaitForSeconds(waitTime);

            // Only play idle lines if the character isn't already talking
            if (audioSource != null && !audioSource.isPlaying)
            {
                PlayRandomClip(idleClips);
            }
        }
    }

    public void PlayHurtClip()
    {
        PlayRandomClip(hurtClips);
    }

    private void HandleOnKill()
    {
        if (Random.value <= killClipChance)
        {
            PlayRandomClip(killClips);
        }
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0 || audioSource == null) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }

    private void OnDestroy()
    {
        // Clean up the subscription when the player is destroyed or the scene changes
        if (GameCore.Instance != null)
        {
            GameCore.Instance.OnKill -= HandleOnKill;
        }
    }
}