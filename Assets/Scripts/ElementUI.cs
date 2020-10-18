using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action onBeginDrag;
    public event Action onDrag;
    public event Action onEndDrag;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke();
    }
}
