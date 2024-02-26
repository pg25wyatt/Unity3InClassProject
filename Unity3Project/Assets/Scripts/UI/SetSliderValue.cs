using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSliderValue : MonoBehaviour
{
    [SerializeField] protected Vector2 _outMinMax = new Vector2(-0.5f, 0.5f);

    public virtual void SetValue(float value)
    {

    }

    protected float GetRemappedValue(float value)
    {
        // remap from slider to outMinMax range
        float remappedValue = Mathf.Lerp(_outMinMax.x, _outMinMax.y, value);
        return remappedValue;
    }
}