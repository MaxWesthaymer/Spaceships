using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryUI : MonoBehaviour
{
    [SerializeField] private Text categoryName;

    public void SetElement(string name)
    {
        categoryName.text = name;
    }
}
