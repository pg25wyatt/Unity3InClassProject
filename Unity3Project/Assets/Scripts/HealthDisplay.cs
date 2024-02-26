using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Image _fillBar;

    public void SetFill(float fill)
    {
        _fillBar.fillAmount = fill;
    }
}