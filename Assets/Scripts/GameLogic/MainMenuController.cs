using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private ShipButton shipButtonPrefab;
    [SerializeField] private Transform buttonsContainer;
    #endregion
    
    #region UnityMethods
    private void Start()
    {
        InstantiateShips();
    }
    #endregion

    #region PrivateMethods
    private void LoadBuildingLevel(int shipId)
    {
        GameData.Instance.SetCurrentShip(shipId);
        SceneManager.LoadScene(1);
    }

    private void InstantiateShips()
    {
        for (var shipId = 0; shipId < GameData.Instance.gameConfig.SpaceshipInfos.Length; shipId++)
        {
            var shipBtn = Instantiate(shipButtonPrefab);
            shipBtn.transform.parent = buttonsContainer;
            shipBtn.transform.localScale = Vector3.one;
            shipBtn.SetShipButton(shipId, LoadBuildingLevel);
        }
    }
    #endregion
}
