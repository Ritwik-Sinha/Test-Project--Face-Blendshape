using System;
using System.Collections.Generic;
using UnityEngine;

public class PhonemeBlendshapeController : MonoBehaviour
{
    [Serializable]
    public struct PhonemeBlendshape
    {
        public Phoneme phoneme;
        public int blendShapeIndex;
        [Range(0, 100)] public float maxWeight;
    }
    
    [Header("Renderer")]
    public SkinnedMeshRenderer skinned;

    [Header("Mapping")]
    public List<PhonemeBlendshape> phonemeMap = new List<PhonemeBlendshape>();

    [Header("Smoothing")]
    [Tooltip("How quickly to move between phonemes.")]
    public float blendSpeed = 40f;

    private readonly Dictionary<Phoneme, PhonemeBlendshape> _map =
        new Dictionary<Phoneme, PhonemeBlendshape>();

    private Phoneme _currentPhoneme = Phoneme.Rest;

    // cache weights per index so we can smooth them
    private float[] _weights;

    private void Awake()
    {
        if (skinned == null)
            skinned = GetComponentInChildren<SkinnedMeshRenderer>();

        _map.Clear();
        foreach (var p in phonemeMap)
        {
            if (!_map.ContainsKey(p.phoneme))
                _map.Add(p.phoneme, p);
        }

        if (skinned != null && skinned.sharedMesh != null)
            _weights = new float[skinned.sharedMesh.blendShapeCount];
    }

    public void SetPhoneme(Phoneme phoneme)
    {
        _currentPhoneme = phoneme;
    }

    private void LateUpdate()
    {
        if (skinned == null || skinned.sharedMesh == null) return;
        if (_weights == null || _weights.Length != skinned.sharedMesh.blendShapeCount)
            _weights = new float[skinned.sharedMesh.blendShapeCount];

        foreach (var entry in phonemeMap)
        {
            float target = 0f;

            if (entry.phoneme == _currentPhoneme)
                target = entry.maxWeight;

            int index = entry.blendShapeIndex;
            if (index < 0 || index >= _weights.Length) continue;

            float current = _weights[index];
            float next = Mathf.MoveTowards(current, target, blendSpeed * Time.deltaTime);
            _weights[index] = next;

            skinned.SetBlendShapeWeight(index, next);
        }
    }
}
