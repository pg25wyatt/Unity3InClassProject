using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAutoSelect : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private bool _firstSelected = false;

    // added from IPointerEnterHandler, calls when mouse moves over UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        // select on pointer enter
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void Start()
    {
        if(_firstSelected) EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void OnEnable()
    {
        if(_firstSelected) EventSystem.current.SetSelectedGameObject(gameObject);
    }
}