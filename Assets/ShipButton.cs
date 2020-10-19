using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text name;

    public void SetShipButton(int shipId, Action<int> action)
    {
        icon.sprite = GameData.Instance.gameConfig.SpaceshipInfos[shipId].Icon;
        name.text = GameData.Instance.gameConfig.SpaceshipInfos[shipId].Name;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            action?.Invoke(shipId);
        });
    }
}
