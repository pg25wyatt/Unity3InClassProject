using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class SetGammaValue : SetSliderValue
{
    [SerializeField] private VolumeProfile _profile;

    public override void SetValue(float value)
    {
        base.SetValue(value);

        // check for existing LiftGammaGain component
        if(_profile.TryGet(out LiftGammaGain liftGammaGain))
        {
            float gammaValue = GetRemappedValue(value);
            liftGammaGain.gamma.value = Vector4.one * gammaValue;
        }
        else
        {
            Debug.LogWarning("No LiftGammaGain component found on volume!", _profile);
        }
    }

    private void OnEnable()
    {
        // check for correct profile and slider component
        if(_profile.TryGet(out LiftGammaGain liftGammaGain) && TryGetComponent(out Slider slider))
        {
            // get current gamma (we're grabbing x (red channel), but all channels should be equal)
            float current = liftGammaGain.gamma.value.x;
            // remap current gamma to normalized 0 to 1 value
            // inverse lerp does the opposite of lerp, it takes A and B, and tells us what linear value T sits at
            float normalizedValue = Mathf.InverseLerp(_outMinMax.x, _outMinMax.y, current);

            // set slider without triggering unity event
            slider.SetValueWithoutNotify(normalizedValue);
        }
    }
}