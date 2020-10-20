using System.Collections.Generic;
using GameConstants;
using UnityEngine;

public class Module : MonoBehaviour
{
    #region InspectorFields
    [SerializeField]private MeshRenderer textMeshRenderer;
    #endregion
    
    #region PrivateFields
    private bool _isTaken;
    #endregion
    
    #region Propierties
    public ModuleType ModuleType { get; set; }
    #endregion

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
            moduleCells.Add(new Vector2Int(Mathf.RoundToInt(cellPosition.x), Mathf.RoundToInt(cellPosition.y)), it);
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
