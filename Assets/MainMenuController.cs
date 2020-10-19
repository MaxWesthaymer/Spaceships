using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private ShipButton shipButtonPrefab;
    [SerializeField] private Transform buttonsContainer;
    private void Start()
    {
        for (var shipId = 0; shipId < GameData.Instance.gameConfig.SpaceshipInfos.Length; shipId++)
        {
            var shipBtn = Instantiate(shipButtonPrefab);
            shipBtn.transform.parent = buttonsContainer;
            shipBtn.transform.localScale = Vector3.one;
            shipBtn.SetShipButton(shipId, LoadBuildingLevel);
        }
    }

    private void LoadBuildingLevel(int shipId)
    {
        GameData.Instance.SetCurrentShip(shipId);
        SceneManager.LoadScene(1);
    }
}
