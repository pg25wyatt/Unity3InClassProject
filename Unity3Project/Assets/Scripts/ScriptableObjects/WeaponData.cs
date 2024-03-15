using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Data/New Weapon")]
public class WeaponData : ItemData
{
    [field: SerializeField, BoxGroup("Weapon")] public int Damage { get; set; } = 5;
    [field: SerializeField, BoxGroup("Weapon")] public float Range { get; set; } = 1f;
    [field: SerializeField, BoxGroup("Weapon")] public float Speed { get; set; } = 1f;
    [field: SerializeField, BoxGroup("Weapon")] public int Durability { get; set; } = 100;


}
