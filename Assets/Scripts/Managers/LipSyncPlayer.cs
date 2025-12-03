using UnityEngine;
using Animancer;

public class LipSyncPlayer : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;
    public PhonemeBlendshapeController phonemeController;

    [Header("Animancer (optional)")]
    public AnimancerComponent animancer;
    public AnimationClip idleClip;        // idle humanoid animation

    [Header("Clip to play for reaction")]
    public LipSyncClip reactionLipSyncClip;

    private int _keyIndex;
    private float _startTime;
    private bool _isPlaying;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (phonemeController == null)
            phonemeController = GetComponentInChildren<PhonemeBlendshapeController>();
    }

    private void Start()
    {
        if (animancer != null && idleClip != null)
            animancer.Play(idleClip);

        if (phonemeController != null)
            phonemeController.SetPhoneme(Phoneme.Rest);
    }

    public void Play(LipSyncClip clip)
    {
        if (clip == null || clip.keys == null || clip.keys.Length == 0)
        {
            Debug.LogWarning("LipSyncPlayer: Invalid clip. - "+
                (clip == null ? "Clip is null" : 
                (clip.keys == null || clip.keys.Length == 0 ? " Keys are null or empty." : "")));
            // return;
        }

        // start audio
        audioSource.clip = clip.audioClip;
        audioSource.Play();

        // reset phoneme sequence
        _keyIndex = 0;
        _startTime = Time.time;
        _isPlaying = true;

        // set initial phoneme
        if (phonemeController != null)
        {
            phonemeController.SetPhoneme(clip.keys[0].phoneme);
        }
    }

    private void Update()
    {
        if (!_isPlaying || reactionLipSyncClip == null) return;

        float t = Time.time - _startTime;
        var keys = reactionLipSyncClip.keys;
        if (keys == null || keys.Length == 0) return;

        while (_keyIndex < keys.Length && t >= keys[_keyIndex].time)
        {
            if (phonemeController != null)
                phonemeController.SetPhoneme(keys[_keyIndex].phoneme);

            _keyIndex++;
        }

        if (!audioSource.isPlaying && t > reactionLipSyncClip.audioClip.length)
        {
            _isPlaying = false;

            if (phonemeController != null)
                phonemeController.SetPhoneme(Phoneme.Rest);

            if (animancer != null && idleClip != null)
                animancer.Play(idleClip);

            _keyIndex = 0;
        }
    }
}
