using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private ElementUI element;
    [SerializeField] private GameObject elementPrefab;
    [SerializeField] private Transform spaceshipGridContainer;
    private GameObject currentObj;
    private Cell[] allCells;
    private Cell[] elementCells;
    private Cell highlightedCell;
    [SerializeField] private Dictionary<Vector2Int, GameObject> shipCells;
    void Start()
    {
        element.onBeginDrag += SpawnShipElement;
        element.onDrag += MoveShipElement;
        element.onEndDrag += EndDrag;
        allCells = spaceshipGridContainer.GetComponentsInChildren<Cell>();

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

    private void SpawnShipElement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentObj = Instantiate(elementPrefab, mousePosition, quaternion.identity);
        elementCells = currentObj.GetComponentsInChildren<Cell>();
    }
    
    private void MoveShipElement()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentObj.transform.position = mousePosition;
       // RaycastHit2D hit = Physics2D.Raycast(currentObj.transform.position, Vector3.back);
       // if (hit.collider != null) 
       // {
       //     if (hit.transform.gameObject.Contains(mousePosition))
       //     {
       //         Debug.Log(hit.transform.name);
       //     }
       // }

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
    
    private void HighlightUpdate()
    {
        foreach (var it in allCells)
        {
            if (!it.IsEmpty)
            {
                continue;
            }
            var isHighlighted = it.gameObject.Contains(gameObject.transform.position);
            it.SetHighlight(isHighlighted);
        }
    }

    private void EndDrag()
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
