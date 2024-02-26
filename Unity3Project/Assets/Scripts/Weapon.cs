using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using FMODUnity;

// the 'abstract' keyword prevents this class/component from being used directly
public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField, BoxGroup("Weapon")] public float EffectiveRange { get; protected set; } = 5f;       // distance at which AI will use weapon
    [field: SerializeField, BoxGroup("Weapon")] public float Damage { get; protected set; } = 5f;
    [field: SerializeField, BoxGroup("Weapon")] public float Cooldown { get; protected set; } = 0.5f;
    [field: SerializeField, BoxGroup("Weapon")] public float Duration { get; protected set; } = 0.5f;

    // SFX
    [field: SerializeField, BoxGroup("SFX")] public EventReference AttackSFX { get; protected set; }
    // animation
    [field: SerializeField, BoxGroup("Animation")] public Animator Animator { get; protected set; }
    [field: SerializeField, BoxGroup("Animation")] public string AnimationTrigger { get; protected set; }
    // TODO: add weapon VFX

    private float _lastAttackTime;

    private void OnValidate()
    {
        if(Animator == null) Animator = GetComponentInParent<Animator>();
    }

    public bool TryAttack(Vector3 aimPosition, int team, GameObject instigator)
    {
        // current time is less that next attack time
        if (Time.time < _lastAttackTime + Cooldown) return false;
        // set last time to now
        _lastAttackTime = Time.time;

        Attack(aimPosition, team, instigator);

        return true;
    }

    protected virtual void Attack(Vector3 aimPosition, int team, GameObject instigator)
    {
        // play SFX
        if (!AttackSFX.IsNull) RuntimeManager.PlayOneShot(AttackSFX, transform.position);
        // play animation
        if (!string.IsNullOrEmpty(AnimationTrigger)) Animator.SetTrigger(AnimationTrigger);
    }
}