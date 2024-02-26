using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public float Amount { get; set; }
    public GameObject Victim { get; set; }      // person getting shot
    public GameObject Source { get; set; }      // bullet that hit the person
    public GameObject Instigator { get; set; }  // person that fired the bullet
    public DamageType DamageType { get; set; }

    public DamageInfo(float amount, GameObject victim, GameObject source, GameObject instigator, DamageType damageType)
    {
        Amount = amount;
        Victim = victim;
        Source = source;
        Instigator = instigator;
        DamageType = damageType;
    }
}

public enum DamageType
{
    None,
    Physical,
    Fire,
    Mental,
    Emotional
}