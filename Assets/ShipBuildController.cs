using System.Collections;
using System.Collections.Generic;
using Extensions;
using GameConstants;
using UnityEngine;

public class ShipBuildController : MonoBehaviour
{
    private GameObject _spaceShipObject;
    
    private GameObject currentObj;
    private Cell[] allCells;
    private Cell[] elementCells;
    private Cell highlightedCell;
    private int currentModuleId;
    [SerializeField] private Dictionary<Vector2Int, Cell> shipCells;
    void Start()
    {
        _spaceShipObject = Instantiate(GameData.Instance.gameConfig.SpaceshipInfos[GameData.Instance.CurrentShip].Prefab, Vector3.zero,
            Quaternion.identity);
        allCells = _spaceShipObject.GetComponentsInChildren<Cell>();

        foreach (var cell in allCells)
        {
            cell.IsEmpty = true;
        }
        
        shipCells = new Dictionary<Vector2Int, Cell>();
        foreach (var it in allCells)
        {
            var cellPosition = it.transform.position;
            shipCells.Add(new Vector2Int((int)cellPosition.x, (int)cellPosition.y), it);
        }
        Debug.Log(shipCells[new Vector2Int(-1,0)].transform.name);

        foreach (var m in GameData.Instance.Data.spaceships[GameData.Instance.CurrentShip].modules)
        {
            var moduleObj = Instantiate(GameData.Instance.gameConfig.ModuleInfos[(int)m.moduleType].Prefab, 
                new Vector3(m.coordinate.x, m.coordinate.y, 0), Quaternion.identity);

            shipCells[m.coordinate].IsEmpty = false;

            foreach (var c in moduleObj.GetComponent<Module>().GetModuleCells())
            {
                var coordinate = m.coordinate + c.Key;
                shipCells[coordinate].IsEmpty = false;
            }
        }
    }

    public void SpawnShipElement(int moduleId)
    {
        currentModuleId = moduleId;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentObj = Instantiate(GameData.Instance.gameConfig.ModuleInfos[moduleId].Prefab, mousePosition, Quaternion.identity);
        elementCells = currentObj.GetComponentsInChildren<Cell>();
    }
    
    public void MoveShipElement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentObj.transform.position = mousePosition;

        foreach (var it in allCells)
        {
            if (!it.IsEmpty)
            {
                continue;
            }
            var isHighlighted = false;

            foreach (var elementCell in elementCells)
            {
                if (it.gameObject.Contains(elementCell.transform.position))
                {
                    isHighlighted = true;
                    break;
                }
            }
            it.SetHighlight(isHighlighted);
        }
    }

    public void EndDrag()
    {
        Cell cell = null;
        foreach (var it in allCells)
        {
            if (!it.IsEmpty || !it.gameObject.Contains(currentObj.transform.position))
            {
                continue;
            }
            cell = it;
            break;
        }

        var readyCells = GetReadyCells();

        if (cell != null && readyCells.Length == elementCells.Length)
        {
            currentObj.transform.position = cell.transform.position;

            foreach (var redy in readyCells)
            {
                redy.IsEmpty = false;
                redy.gameObjectOnCell = currentObj;
            }

            SaveToShip();
        }
        else
        {
            Destroy(currentObj);
        }
    }

    private Cell[] GetReadyCells()
    {
        List<Cell> redyCells = new List<Cell>();
        foreach (var it in allCells)
        {
            if (!it.IsEmpty)
            {
                continue;
            }

            foreach (var elementCell in elementCells)
            {
                if (it.gameObject.Contains(elementCell.transform.position))
                {
                    redyCells.Add(it);
                    break;
                }
            }
        }
        return redyCells.ToArray();
    }

    private void SaveToShip()
    {
        GameData.Instance.Data.spaceships[ GameData.Instance.CurrentShip].modules.Add(new ModuleData(new Vector2Int(0,0),(ModuleType)currentModuleId));
        Debug.Log("!!" + GameData.Instance.Data.spaceships[0].modules.Count);
    }
}
