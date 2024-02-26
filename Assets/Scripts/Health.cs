using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[HideMonoScript] // hides script name at top of component
public class Health : MonoBehaviour
{
    [BoxGroup("Stats"), InlineButton("TestDamage"), SerializeField] private float _current = 100f;
    [BoxGroup("Stats"), SerializeField] private float _max = 100f;

    [BoxGroup("Death"), SerializeField] private string _corpseLayerName = "Corpse";

    // properties (public get, no set)
    [BoxGroup("Debug"), ShowInInspector] public float Percentage => _current / _max;
    [BoxGroup("Debug"), ShowInInspector] public bool IsAlive => _current >= 1f;

    [FoldoutGroup("Events")] public UnityEvent<DamageInfo> OnDamage;
    [FoldoutGroup("Events")] public UnityEvent<DamageInfo> OnDeath;
    [FoldoutGroup("Events")] public UnityEvent<float> OnChanged;

    public void Damage(DamageInfo damageInfo)
    {
        if(!IsAlive) return;                    // stop if dead
        if (damageInfo.Amount < 1f) return;     // ignore invalid damage amounts

        // reduce current health and clamp
        _current -= damageInfo.Amount;
        _current = Mathf.Clamp(_current, 0f, _max);

        // invoke damage event
        OnDamage.Invoke(damageInfo);
        OnChanged.Invoke(Percentage);

        // handle death
        if(!IsAlive)
        {
            // move to corpse layer
            gameObject.layer = LayerMask.NameToLayer(_corpseLayerName);

            OnDeath.Invoke(damageInfo);
        }
    }

    [Button, HideInEditorMode]
    public void TestDamage()
    {
        // damage self for 10% of max health
        DamageInfo damageInfo = new DamageInfo(_max * 0.1f, gameObject, gameObject, gameObject, DamageType.Emotional);
        Damage(damageInfo);
    }
}