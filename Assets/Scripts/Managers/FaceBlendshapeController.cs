using UnityEngine;

public enum FaceEmotion
{
    Neutral,
    Smile,
    Sad
}

public class FaceBlendshapeController : MonoBehaviour
{
    [Header("Renderer")]
    public SkinnedMeshRenderer skinned;

    [Header("Smile Indices")]
    public int smileBrowIndex   = 8;   // Brow-Joy
    public int smileEyeIndex    = 17;  // Eye-Joy

    [Header("Sad Indices")]
    public int sadBrowIndex     = 9;   // Brow-Sorrow
    public int sadEyeIndex      = 20;  // Eye-Sorrow

    [Header("Neutral Indices")]
    public int neutralEyeIndex   = 11; // Eye-Natural

    [Header("Weights")]
    [Range(0, 100)] public float smileWeight = 100;
    [Range(0, 100)] public float sadWeight   = 100;

    [Header("Smoothing")]
    public float changeSpeed = 500f;

    private FaceEmotion _current = FaceEmotion.Neutral;
    private float _smileT; // 0–1
    private float _sadT;   // 0–1

    private void Reset()
    {
        skinned = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void SetEmotion(FaceEmotion emotion)
    {
        _current = emotion;
    }

    private void LateUpdate()
    {
        if (skinned == null) return;

        // Smoothly blend between emotions
        float targetSmile = (_current == FaceEmotion.Smile) ? 1f : 0f;
        float targetSad   = (_current == FaceEmotion.Sad)   ? 1f : 0f;

        _smileT = Mathf.Lerp(_smileT, targetSmile, Time.deltaTime * changeSpeed);
        _sadT   = Mathf.Lerp(_sadT,   targetSad,   Time.deltaTime * changeSpeed);

        // Clear all relevant shapes first
        SetWeightSafe(smileBrowIndex,   0);
        SetWeightSafe(smileEyeIndex,    0);
        
        SetWeightSafe(sadBrowIndex,     0);
        SetWeightSafe(sadEyeIndex,      0);
        
        SetWeightSafe(neutralEyeIndex,  0);
        

        // Neutral baseline
        float neutralFactor = 1f - Mathf.Max(_smileT, _sadT);
        SetWeightSafe(neutralEyeIndex,   100f * neutralFactor);
        

        // Smile
        SetWeightSafe(smileBrowIndex,  smileWeight * _smileT);
        SetWeightSafe(smileEyeIndex,   smileWeight * _smileT);
        

        // Sad
        SetWeightSafe(sadBrowIndex,   sadWeight * _sadT);
        SetWeightSafe(sadEyeIndex,    sadWeight * _sadT);
    }

    private void SetWeightSafe(int index, float weight)
    {
        if (index < 0 || index >= skinned.sharedMesh.blendShapeCount) return;
        skinned.SetBlendShapeWeight(index, weight);
    }
}
