using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Character : MonoBehaviour
{
    [SerializeField, Required, InlineEditor] private CharacterData _data;
}
