using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(DecalProjector))]
public class RandomDecalIndex : MonoBehaviour
{
    [SerializeField] private DecalProjector _decal;

    private void OnValidate()
    {
        _decal = GetComponent<DecalProjector>();
    }

    private void Awake()
    {
        _decal.material.SetFloat("_FlipbookIndex", Random.Range(0f, float.MaxValue));
        int index = Random.Range(0, 4);
        Material mat = Instantiate(_decal.material);
        mat.SetFloat("_FlipbookIndex", index);
        _decal.material = mat;
    }
}