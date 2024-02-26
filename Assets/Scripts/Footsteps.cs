using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private EventReference _footstepSFX;

    // this is called from the attached Animator component, using animation events
    public void FootstepAnimEvent()
    {
        RuntimeManager.PlayOneShot(_footstepSFX, transform.position);
    }
}