using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
[EditorTool("Custom snap", typeof(CustomSnap))]
public class CustomSnapTool : EditorTool
{
    public Texture2D toolIcon;
    private Transform oldTarget;
    private CustomSnapPoint[] allPoints;
    private CustomSnapPoint[] targetPoints;
        
    public override GUIContent toolbarIcon => new GUIContent
    {
        image = toolIcon, 
        text = "Custom snap tool",
        tooltip = "Custom snap tool"
    };

    public override void OnToolGUI(EditorWindow window)
    {
        Transform targetTransform = ((CustomSnap)target).transform;

        if (targetTransform != oldTarget)
        {
            allPoints = targetTransform.parent.GetComponentsInChildren<CustomSnapPoint>();
            targetPoints = targetTransform.GetComponentsInChildren<CustomSnapPoint>();
        }
        EditorGUI.BeginChangeCheck();
        var newPosition = Handles.PositionHandle(targetTransform.position, quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetTransform, "Custom snap tool move");
            MoveWithSnapping(targetTransform, newPosition);
        }
    }

    private void MoveWithSnapping(Transform targetTransform, Vector3 newPosition)
    {
        Vector3 bestPosition = newPosition;
        float closestDistance = float.PositiveInfinity;

        foreach (var point in allPoints)
        {
            if(point.transform.parent == targetTransform) continue;

            foreach (var ownPoint in targetPoints)
            {
                Vector3 targetPosition = point.transform.position - (ownPoint.transform.position - targetTransform.position);
                float distance = Vector3.Distance(targetPosition, newPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestPosition = targetPosition;
                }
            }
        }

        if (closestDistance < 0.5f)
        {
            targetTransform.position = bestPosition;
        }
        else
        {
            targetTransform.position = newPosition;
        }
    }
}
