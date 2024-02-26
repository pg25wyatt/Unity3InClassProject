using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _fillBar;

    private void OnValidate()
    {
        // find health component automatically
        if(_health == null) _health = GetComponentInParent<Health>();
    }

    private void Update()
    {
        if (_health == null) return;        // do nothing if dead

        // fill bar - lazy/bad "polling" method, we'll improve this later
        _fillBar.fillAmount = _health.Percentage;

        // ensure health bar faces camera correctly
        transform.rotation = Camera.main.transform.rotation;
    }
}