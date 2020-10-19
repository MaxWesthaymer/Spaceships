using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public Dictionary<Vector2Int, Cell> ModuleCells { get; private set; }

    private void Start()
    {
        Debug.Log("Module start!!!");
        var allCells = transform.GetComponentsInChildren<Cell>();

        ModuleCells = new Dictionary<Vector2Int, Cell>();
        foreach (var it in allCells)
        {
            var cellPosition = it.transform.position;
            ModuleCells.Add(new Vector2Int((int)cellPosition.x, (int)cellPosition.y), it);
        }
    }
    
    public Dictionary<Vector2Int, Cell> GetModuleCells()
    {
        var allCells = transform.GetComponentsInChildren<Cell>();

        var moduleCells = new Dictionary<Vector2Int, Cell>();
        foreach (var it in allCells)
        {
            var cellPosition = it.transform.position;
            moduleCells.Add(new Vector2Int((int)cellPosition.x, (int)cellPosition.y), it);
        }
        return moduleCells;
    }
}
