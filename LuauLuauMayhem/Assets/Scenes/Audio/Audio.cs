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
    public float killClipChance = 0.1f; // 10%

    private void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        StartCoroutine(IdleVoiceLoop());

        // Subscribe to kill event (make sure Game.RegisterKill calls this)
        if (Game.instance != null)
        {
            Game.instance.OnKillRegistered += OnKillRegistered;
        }
    }

    private void Update()
    {
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

            if (!audioSource.isPlaying)
            {
                PlayRandomClip(idleClips);
            }
        }
    }

    public void PlayHurtClip()
    {
        PlayRandomClip(hurtClips);
    }

    public void OnKillRegistered()
    {
        if (Random.value <= killClipChance)
        {
            PlayRandomClip(killClips);
        }
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips.Length == 0 || audioSource == null) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }

    private void OnDestroy()
    {
        if (Game.instance != null)
        {
            Game.instance.OnKillRegistered -= OnKillRegistered;
        }
    }
}