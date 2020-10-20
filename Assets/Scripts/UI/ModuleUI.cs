using System;
using System.Collections;
using System.Collections.Generic;
using GameConstants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleUI : MonoBehaviour, IPointerDownHandler
{
   [SerializeField] private Image icon;
   [SerializeField] private Text name;
   
    public event Action<int> onBeginDrag;
    public event Action onDrag;
    public event Action onEndDrag;
    private int _moduleId;
    private bool _isDrag;
    private bool _isBeginDrag;
    private float startMouseYPosition;
    
    public void SetModule(int moduleId)
    {
        _moduleId = moduleId;
       icon.sprite = GameData.Instance.gameConfig.ModuleInfos[moduleId].Icon;
       name.text = GameData.Instance.gameConfig.ModuleInfos[moduleId].Name;
    }

    //public void OnBeginDrag(PointerEventData eventData)
  // {
  //     onBeginDrag?.Invoke();
  // }

  // public void OnDrag(PointerEventData eventData)
  // {
  //     onDrag?.Invoke();
  // }

  // public void OnEndDrag(PointerEventData eventData)
  // {
  //     onEndDrag?.Invoke();
  // }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isBeginDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startMouseYPosition = mousePosition.y;
            _isBeginDrag = true;
            Debug.Log("OnPointerDown");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && _isBeginDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePosition.y > startMouseYPosition + 0.5f)
            {
                Debug.Log(mousePosition.y);
                Debug.Log("startMouseYPosition " + startMouseYPosition);
                _isBeginDrag = false;
                _isDrag = true;
                onBeginDrag?.Invoke(_moduleId);
                Debug.Log("onBeginDrag");
            }
        }
        if (Input.GetMouseButton(0) && _isDrag)
        {
            onDrag?.Invoke();
            Debug.Log("onDrag");
        }
        
        if (Input.GetMouseButtonUp(0) && _isBeginDrag)
        {
            _isBeginDrag = false;
        }
        
        if (Input.GetMouseButtonUp(0) && _isDrag)
        {
            onEndDrag?.Invoke();
            Debug.Log("onEndDrag");
            _isDrag = false;
        }

    }
}
