using System;
using UnityEngine;
using UnityEngine.UI;

public class ShipButton : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private Image icon;
    [SerializeField] private Text name;
    #endregion
    
    #region PublicMethods
    public void SetShipButton(int shipId, Action<int> action)
    {
        icon.sprite = GameData.Instance.gameConfig.SpaceshipInfos[shipId].Icon;
        name.text = GameData.Instance.gameConfig.SpaceshipInfos[shipId].Name;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            action?.Invoke(shipId);
        });
    }
    #endregion
}
