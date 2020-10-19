using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class ShipBuildController : MonoBehaviour
{
    private GameObject _spaceShipObject;
    
    private GameObject currentObj;
    private Cell[] allCells;
    private Cell[] elementCells;
    private Cell highlightedCell;
    [SerializeField] private Dictionary<Vector2Int, GameObject> shipCells;
    void Start()
    {
        _spaceShipObject = Instantiate(GameData.Instance.gameConfig.SpaceshipInfos[GameData.Instance.CurrentShip].Prefab, Vector3.zero,
            Quaternion.identity);
        allCells = _spaceShipObject.GetComponentsInChildren<Cell>();

        foreach (var cell in allCells)
        {
            cell.IsEmpty = true;
        }
        
        shipCells = new Dictionary<Vector2Int, GameObject>();
        foreach (var it in allCells)
        {
            var cellPosition = it.transform.position;
            shipCells.Add(new Vector2Int((int)cellPosition.x, (int)cellPosition.y), it.transform.gameObject);
        }
        Debug.Log(shipCells[new Vector2Int(-1,0)].transform.name);
    }

    public void SpawnShipElement(int moduleId)
    {
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

        List<Cell> redyCells = new List<Cell>();
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
                    redyCells.Add(it);
                    break;
                }
            }
        }

        if (cell != null && redyCells.Count == elementCells.Length)
        {
            currentObj.transform.position = cell.transform.position;

            foreach (var redy in redyCells)
            {
                redy.IsEmpty = false;
                redy.gameObjectOnCell = currentObj;
            }
        }
        else
        {
            Destroy(currentObj);
        }
    }
}
