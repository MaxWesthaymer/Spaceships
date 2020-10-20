using System.Collections;
using System.Collections.Generic;
using Extensions;
using GameConstants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBuildController : MonoBehaviour
{
    [SerializeField] private float camBoundsOffset;
    private GameObject _spaceShipObject;
    private GameObject _currentObj;
    private Dictionary<Vector2Int, Cell>  _moduleCells;
    private int _currentModuleId;
    private Dictionary<Vector2Int, Cell> _shipCells;
    [SerializeField]private Dictionary<Vector2Int, Module>  _moduleObjects;

    private void SetupCamera()
    {
        Camera cam = Camera.main;
        var size = _spaceShipObject.GetComponent<BoxCollider2D>().bounds.size;
        cam.orthographicSize = size.x / (2 * cam.aspect) + camBoundsOffset;
    }
    
    private void Start()
    {
        _spaceShipObject = Instantiate(GameData.Instance.gameConfig.SpaceshipInfos[GameData.Instance.CurrentShip].Prefab, Vector3.zero,
            Quaternion.identity);

        SetupCamera();
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
        
        _moduleObjects = new Dictionary<Vector2Int, Module>();
        var modules = GameData.Instance.Data.spaceships[GameData.Instance.CurrentShip].modules;
        foreach (var module in modules)
        {
            var moduleObj = Instantiate(GameData.Instance.gameConfig.ModuleInfos[(int)module.moduleType].Prefab, 
                new Vector3(module.coordinate.x, module.coordinate.y, 0), Quaternion.identity);
            moduleObj.GetComponent<Module>().ModuleType = module.moduleType;
            foreach (var moduleCell in moduleObj.GetComponent<Module>().GetModuleCells())
            {
                var coordinate = module.coordinate + moduleCell.Key;
                _shipCells[coordinate].IsEmpty = false;
                _shipCells[coordinate].moduleOwnerId = module.coordinate;
            }
            _moduleObjects.Add(module.coordinate, moduleObj.GetComponent<Module>());
        }
    }

    public void SpawnShipElement(int moduleId)
    {
        _currentModuleId = moduleId;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentObj = Instantiate(GameData.Instance.gameConfig.ModuleInfos[moduleId].Prefab, mousePosition, Quaternion.identity);
        _moduleCells = _currentObj.GetComponent<Module>().GetModuleCells();
        _currentObj.GetComponent<Module>().SetTaken(true);
        _currentObj.GetComponent<Module>().ModuleType = (ModuleType)moduleId;

    }
    
    public void MoveShipElement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentObj.transform.position = mousePosition;

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

    public void EndDrag()
    {
        _currentObj.GetComponent<Module>().SetTaken(false);
        Cell cell = null;
        Vector2Int cellCoordinate = new Vector2Int();
        foreach (var shipCell in _shipCells)
        {
            if (!shipCell.Value.gameObject.Contains(_currentObj.transform.position))
            {
                continue;
            }
            cell = shipCell.Value;
            cellCoordinate = shipCell.Key;
            break;
        }

        var readyCells = GetReadyCells();
        Debug.Log("!!!!!!!!!!!readyCells " + readyCells.Length);

        if (cell != null && readyCells.Length == _moduleCells.Count)
        {
            _currentObj.transform.position = cell.transform.position;
            
            _moduleObjects.Add(cellCoordinate, _currentObj.GetComponent<Module>());
            Debug.Log(_moduleObjects[cellCoordinate].transform.name);

            foreach (var moduleCell in _moduleObjects[cellCoordinate].GetModuleCells())
            {
                var coordinate = cellCoordinate + moduleCell.Key;
                _shipCells[coordinate].IsEmpty = false;
                _shipCells[coordinate].moduleOwnerId = cellCoordinate;
            }
        }
        else
        {
            Destroy(_currentObj);
            Debug.Log("Destroy(_currentObj);");
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

    private Cell[] GetReadyCells()
    {
        List<Cell> redyCells = new List<Cell>();
        foreach (var shipCell in _shipCells)
        {
            foreach (var moduleCell in _moduleCells)
            {
                if (shipCell.Value.gameObject.Contains(moduleCell.Value.transform.position))
                {
                    if (!shipCell.Value.IsEmpty)
                    {
                        foreach (var it in _moduleObjects[shipCell.Value.moduleOwnerId].GetModuleCells())
                        {
                            var coordinate = shipCell.Value.moduleOwnerId + it.Key;
                            _shipCells[coordinate].IsEmpty = true;
                        }
                        Destroy(_moduleObjects[shipCell.Value.moduleOwnerId].gameObject);
                        _moduleObjects.Remove(shipCell.Value.moduleOwnerId);
                    }
                    redyCells.Add(shipCell.Value);
                    break;
                }
            }
        }
        return redyCells.ToArray();
    }

    public void SaveBuild()
    {
        GameData.Instance.Data.spaceships[GameData.Instance.CurrentShip].modules.Clear();
        foreach (var it in _moduleObjects)
        {
            var moduleData = new ModuleData(it.Key,it.Value.ModuleType);
            GameData.Instance.Data.spaceships[GameData.Instance.CurrentShip].modules.Add(moduleData);
        }
        GameData.Instance.Save();
        SceneManager.LoadScene(0);
    }
    
    
}
