using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [field: SerializeField] public int Team { get; private set; } = 0;
    [field: SerializeField] public bool IsTargetable { get; set; } = true;
    [field: SerializeField] public Transform VisibilityTransform { get; private set; }

    private void OnValidate()
    {
        if(VisibilityTransform == null)
        {
            // find all child transforms and return the first one called "Spine_02"
            // we're using LINQ to find the bone, it is expensive so don't use it during runtime
            VisibilityTransform = transform.GetComponentsInChildren<Transform>().
                Where(t => t.name.Equals("Spine_02")).FirstOrDefault();
        }
    }
}