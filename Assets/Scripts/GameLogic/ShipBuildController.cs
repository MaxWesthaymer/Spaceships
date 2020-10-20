using System.Collections.Generic;
using Extensions;
using GameConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBuildController : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private float camBoundsOffset;
    #endregion
    
    #region PrivateFields
    private GameObject _spaceShipObject;
    private GameObject _currentModuleObj;
    private Dictionary<Vector2Int, Cell>  _moduleCells;
    private Dictionary<Vector2Int, Cell> _shipCells;
    private Dictionary<Vector2Int, Module>  _moduleObjects;
    private int _currentShipIndex;
    #endregion
    
    #region UnityMethods
    private void Start()
    {
        InstantiateShip();
        FillShipCells();
        FillShipModules();
    }
    #endregion

    #region PublicMethods
    public void SpawnShipModule(int moduleId)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentModuleObj = Instantiate(GameData.Instance.gameConfig.ModuleInfos[moduleId].Prefab, mousePosition, Quaternion.identity);
        _moduleCells = _currentModuleObj.GetComponent<Module>().GetModuleCells();
        _currentModuleObj.GetComponent<Module>().SetTaken(true);
        _currentModuleObj.GetComponent<Module>().SetType((ModuleType)moduleId);
    }
    
    public void MoveShipModule()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentModuleObj.transform.position = mousePosition;

        foreach (var sipCell in _shipCells)
        {
            if (!sipCell.Value.IsEmpty)
            {
                continue;
            }
            var isHighlighted = false;

            foreach (var moduleCell in _moduleCells)
            {
                if (sipCell.Value.gameObject.Contains(moduleCell.Value.transform.position))
                {
                    isHighlighted = true;
                    break;
                }
            }
            sipCell.Value.SetHighlight(isHighlighted);
        }
    }

    public void ReleaseShipModule()
    {
        _currentModuleObj.GetComponent<Module>().SetTaken(false);
        Cell cell = null;
        Vector2Int cellCoordinate = new Vector2Int();
        foreach (var shipCell in _shipCells)
        {
            if (!shipCell.Value.gameObject.Contains(_currentModuleObj.transform.position))
            {
                continue;
            }
            cell = shipCell.Value;
            cellCoordinate = shipCell.Key;
            break;
        }

        var readyCells = GetReadyCells();

        if (cell != null && readyCells.Length == _moduleCells.Count)
        {
            _currentModuleObj.transform.position = cell.transform.position;
            _moduleObjects.Add(cellCoordinate, _currentModuleObj.GetComponent<Module>());
            SetUnderModuleCells(cellCoordinate, _moduleObjects[cellCoordinate].GetModuleCells(), false);
        }
        else
        {
            Destroy(_currentModuleObj);
        }
    }

    public bool HasEmptyCells()
    {
        foreach (var it in _shipCells)
        {
            if (it.Value.IsEmpty)
            {
                return true;
            }
        }
        return false;
    }
    
    public void SaveBuild()
    {
        GameData.Instance.Data.spaceships[_currentShipIndex].modules.Clear();
        foreach (var moduleObject in _moduleObjects)
        {
            var moduleData = new ModuleData(moduleObject.Key,moduleObject.Value.ModuleType);
            GameData.Instance.Data.spaceships[_currentShipIndex].modules.Add(moduleData);
        }
        GameData.Instance.Save();
        SceneManager.LoadScene(0);
    }
    #endregion

    #region PrivateMethods

    private void InstantiateShip()
    {
        _currentShipIndex = GameData.Instance.CurrentShip;
        _spaceShipObject = Instantiate(GameData.Instance.gameConfig.SpaceshipInfos[_currentShipIndex].Prefab, Vector3.zero,
            Quaternion.identity);

        SetupCamera();
    }

    private void FillShipCells()
    {
        var allCells = _spaceShipObject.GetComponentsInChildren<Cell>();
        foreach (var cell in allCells)
        {
            cell.IsEmpty = true;
        }
        
        _shipCells = new Dictionary<Vector2Int, Cell>();
        foreach (var it in allCells)
        {
            var cellPosition = it.transform.position;
            _shipCells.Add(new Vector2Int(Mathf.RoundToInt(cellPosition.x), Mathf.RoundToInt(cellPosition.y)), it);
        }
    }

    private void FillShipModules()
    {
        _moduleObjects = new Dictionary<Vector2Int, Module>();
        var modules = GameData.Instance.Data.spaceships[_currentShipIndex].modules;
        foreach (var module in modules)
        {
            var moduleObj = Instantiate(GameData.Instance.gameConfig.ModuleInfos[(int)module.moduleType].Prefab, 
                new Vector3(module.coordinate.x, module.coordinate.y, 0), Quaternion.identity);

            var moduleHandler = moduleObj.GetComponent<Module>();
            moduleHandler.SetType(module.moduleType);
            
            SetUnderModuleCells(module.coordinate, moduleHandler.GetModuleCells(), false);
            
            _moduleObjects.Add(module.coordinate, moduleObj.GetComponent<Module>());
            
        }
    }

    private void SetUnderModuleCells(Vector2Int mainCoordinate, Dictionary<Vector2Int, Cell> moduleCells , bool isEmpty)
    {
        foreach (var moduleCell in moduleCells)
        {
            var coordinate = mainCoordinate + moduleCell.Key;
            _shipCells[coordinate].IsEmpty = isEmpty;

            if (!isEmpty)
            {
                _shipCells[coordinate].moduleOwnerId = mainCoordinate;
            }
        }
    }
    private Cell[] GetReadyCells()
    {
        List<Cell> readyCells = new List<Cell>();
        foreach (var shipCell in _shipCells)
        {
            foreach (var moduleCell in _moduleCells)
            {
                if (shipCell.Value.gameObject.Contains(moduleCell.Value.transform.position))
                {
                    if (!shipCell.Value.IsEmpty)
                    {
                        SetUnderModuleCells(shipCell.Value.moduleOwnerId, _moduleObjects[shipCell.Value.moduleOwnerId].GetModuleCells(), true);
                        Destroy(_moduleObjects[shipCell.Value.moduleOwnerId].gameObject);
                        _moduleObjects.Remove(shipCell.Value.moduleOwnerId);
                    }
                    readyCells.Add(shipCell.Value);
                    break;
                }
            }
        }
        return readyCells.ToArray();
    }

    private void SetupCamera()
    {
        Camera cam = Camera.main;
        var size = _spaceShipObject.GetComponent<BoxCollider2D>().bounds.size;
        cam.orthographicSize = size.x / (2 * cam.aspect) + camBoundsOffset;
    }
    #endregion
}
