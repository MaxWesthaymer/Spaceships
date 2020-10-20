using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private readonly Color32 _highlightColor = new Color32(236, 200, 106, 255);
    private readonly Color32 _emptyColor = new Color32(142, 168, 236, 255);
    public bool IsEmpty;
    public Vector2Int moduleOwnerId;

    public void SetHighlight(bool isHighlighted)
    {
        spriteRenderer.color = isHighlighted ? _highlightColor : _emptyColor;
    }
}
