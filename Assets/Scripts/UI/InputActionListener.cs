using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionListener : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;

    public UnityEvent OnInput;

    private void OnEnable()
    {
        // += behaves similar to AddListener(), which we used to listen to UnityEvents
        // in this case, action.performed isn't a UnityEvent, it's a normal C# event
        if (_actionReference != null) _actionReference.action.performed += Performed;
    }

    private void OnDisable()
    {
        if (_actionReference != null) _actionReference.action.performed -= Performed;
    }

    private void Performed(InputAction.CallbackContext context)
    {
        OnInput.Invoke();
    }

    // allows us to invoke OnInput from outside scripts
    public void ForcePerform()
    {
        OnInput.Invoke();
    }
}