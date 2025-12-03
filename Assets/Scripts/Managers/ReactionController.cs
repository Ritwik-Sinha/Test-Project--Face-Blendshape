using System.Collections;
using UnityEngine;
using Animancer;

public class ReactionController : MonoBehaviour
{
    [Header("Animancer")]
    public AnimancerComponent animancer;
    public AnimationClip idleClip;
    public AnimationClip smileBodyClip; // happy body move
    public AnimationClip sadBodyClip;   // sad body move

    [Header("Face + Lips")]
    public FaceBlendshapeController face;
    public AudioClip dialogueClip;

    [Header("Timings (seconds)")]
    public float crossFadeTime = 0.25f;

    [SerializeField] private LipSyncPlayer lipSyncPlayer;
    [SerializeField] private bool triggerLipSyncOnReaction = true;

    private bool _sequenceRunning;
    private bool _queued;

    private void Start()
    {
        if (animancer != null && idleClip != null)
        {
            animancer.Play(idleClip);
        }

        if (face != null)
            face.SetEmotion(FaceEmotion.Neutral);
    }
    public void OnPlayReactionButton()
    {
        if (_sequenceRunning)
        {
            _queued = true;
            return;
        }

        StartCoroutine(RunSequenceLoop());
    }

    private IEnumerator RunSequenceLoop()
    {
        _sequenceRunning = true;

        do
        {
            _queued = false;
            if (triggerLipSyncOnReaction && lipSyncPlayer != null)
            {
                lipSyncPlayer.Play(lipSyncPlayer.reactionLipSyncClip);
            }
            yield return StartCoroutine(PlaySmileSadSmileSad());
        }
        while (_queued);

        _sequenceRunning = false;
    }

    private IEnumerator PlaySmileSadSmileSad()
    {
        // 1) Smile
        yield return StartCoroutine(PlayEmotion(
            FaceEmotion.Smile,
            smileBodyClip,
            2));

        // 2) Sad
        yield return StartCoroutine(PlayEmotion(
            FaceEmotion.Sad,
            sadBodyClip,
            2));

        // 3) Smile
        yield return StartCoroutine(PlayEmotion(
            FaceEmotion.Smile,
            smileBodyClip,
            6));

        // 4) Sad
        yield return StartCoroutine(PlayEmotion(
            FaceEmotion.Sad,
            sadBodyClip,
            2));

        // Back to idle + neutral face
        if (animancer != null && idleClip != null)
        {
            animancer.Play(idleClip, crossFadeTime);
        }

        if (face != null)
            face.SetEmotion(FaceEmotion.Neutral);
    }

    private IEnumerator PlayEmotion(FaceEmotion emotion, AnimationClip bodyClip, float duration)
    {
        if (face != null)
            face.SetEmotion(emotion);

        if (animancer != null && bodyClip != null)
        {
            animancer.Play(bodyClip, crossFadeTime);
        }

        yield return new WaitForSeconds(duration);
    }
}