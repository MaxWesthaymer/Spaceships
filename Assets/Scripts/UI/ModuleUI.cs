using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleUI : MonoBehaviour, IPointerDownHandler
{
    #region InspectorFields
    [SerializeField] private Image icon;
    [SerializeField] private Text name;
    #endregion
    
    #region Events
    public event Action<int> onBeginDrag;
    public event Action onDrag;
    public event Action onEndDrag;
    #endregion
    
    #region PrivateFields
    private int _moduleId;
    private bool _isDrag;
    private bool _isBeginDrag;
    private float startMouseYPosition;
    #endregion
    
    #region PublicMethods
    public void SetModule(int moduleId)
    { 
        _moduleId = moduleId;
       //icon.sprite = GameData.Instance.gameConfig.ModuleInfos[moduleId].Icon;
       name.text = GameData.Instance.gameConfig.ModuleInfos[moduleId].Name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isBeginDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startMouseYPosition = mousePosition.y;
            _isBeginDrag = true;
        }
    }
    #endregion

    #region UnityMethods
    private void Update()
    {
        if (Input.GetMouseButton(0) && _isBeginDrag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePosition.y > startMouseYPosition + 0.5f)
            {
                _isBeginDrag = false;
                _isDrag = true;
                onBeginDrag?.Invoke(_moduleId);

            }
        }
        if (Input.GetMouseButton(0) && _isDrag)
        {
            onDrag?.Invoke();
        }
        
        if (Input.GetMouseButtonUp(0) && _isBeginDrag)
        {
            _isBeginDrag = false;
        }
        
        if (Input.GetMouseButtonUp(0) && _isDrag)
        {
            onEndDrag?.Invoke();
            _isDrag = false;
        }
        #endregion
    }
}
