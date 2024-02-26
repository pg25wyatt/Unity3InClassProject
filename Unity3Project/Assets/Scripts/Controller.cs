using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent enforces components on gameobject
[RequireComponent(typeof(CustomCharacterMovement))]
public class Controller : MonoBehaviour
{
    // this is a property - we can specify get/set accessibility explicitly
    // [field: SerializeField] allows us to see properties in the inspector
    [field: Header("Components")]
    [field: SerializeField] protected CustomCharacterMovement Movement { get; private set; }
    [field: SerializeField] protected Targetable Targetable { get; private set; }
    [field: SerializeField] protected Vision Vision { get; private set; }

    [field: Header("Weapons")]
    [field: SerializeField, InlineButton("FindWeapons")] protected Weapon[] Weapons { get; private set; }

    // OnValidate runs when inspector values change
    private void OnValidate()
    {
        Movement = GetComponent<CustomCharacterMovement>();
        Targetable = GetComponent<Targetable>();
        Weapons = GetComponentsInChildren<Weapon>();
        Vision = GetComponent<Vision>();
    }

    private void FindWeapons()
    {
        Weapons = GetComponentsInChildren<Weapon>();
    }
}