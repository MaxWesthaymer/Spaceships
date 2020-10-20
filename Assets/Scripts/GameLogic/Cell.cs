using UnityEngine;

public class Cell : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _emptyColor;
    #endregion
    
    #region Propierties
    public bool IsEmpty { get; set;}
    public Vector2Int moduleOwnerId { get; set;}
    #endregion
    
    #region PublicMethods
    public void SetHighlight(bool isHighlighted)
    {
        spriteRenderer.color = isHighlighted ? _highlightColor : _emptyColor;
    }
    #endregion
}
