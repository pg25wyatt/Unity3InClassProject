using FMOD;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFMODValue : SetSliderValue
{
    [SerializeField] private string _parameterName;

    public override void SetValue(float value)
    {
        base.SetValue(value);

        if(!string.IsNullOrEmpty(_parameterName))
        {
            float remappedValue = GetRemappedValue(value);
            RESULT result = RuntimeManager.StudioSystem.setParameterByName(_parameterName, remappedValue);
            if(result != RESULT.OK)
            {
                UnityEngine.Debug.LogWarning($"FMOD parameter set fail: {result}");
            }
        }
    }

    private void OnEnable()
    {
        if(!string.IsNullOrEmpty(_parameterName) && TryGetComponent(out Slider slider))
        {
            RESULT result = RuntimeManager.StudioSystem.getParameterByName(_parameterName, out float value);
            if(result == RESULT.OK)
            {
                slider.SetValueWithoutNotify(value);
            }
        }
    }
}