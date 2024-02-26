using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private AnimationCurve _weightRemap;

    private void OnValidate()
    {
        _volume = GetComponent<Volume>();
    }

    public void SetWeight(float weight)
    {
        _volume.weight = _weightRemap.Evaluate(weight);
    }
}