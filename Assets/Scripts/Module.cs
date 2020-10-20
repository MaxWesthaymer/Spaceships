using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField]private MeshRenderer textMeshRenderer;
    private bool _isTaken;

    private void Start()
    {
        textMeshRenderer.sortingLayerName = _isTaken ? "TakenModule" : "Module";
    }
    
    public Dictionary<Vector2Int, Cell> GetModuleCells()
    {
        var allCells = transform.GetComponentsInChildren<Cell>();

        var moduleCells = new Dictionary<Vector2Int, Cell>();
        foreach (var it in allCells)
        {
            var cellPosition = it.transform.localPosition;
            moduleCells.Add(new Vector2Int((int)cellPosition.x, (int)cellPosition.y), it);
        }
        return moduleCells;
    }
    
    public void SetTaken(bool isTaken)
    {
        _isTaken = isTaken;
        var allSprites = transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (var it in allSprites)
        {
            it.sortingLayerName = isTaken ? "TakenCell" : "ModuleCell";
        }
        textMeshRenderer.sortingLayerName = isTaken ? "TakenModule" : "Module";
    }
}
