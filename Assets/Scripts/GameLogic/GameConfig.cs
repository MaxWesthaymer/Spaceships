using System;
using System.Collections;
using System.Collections.Generic;
using GameConstants;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public SpaceshipInfo[] SpaceshipInfos;
    public ModuleInfo[] ModuleInfos;
    public Category[] Categories;
}

[Serializable]
public class Category
{
    public Category(SubCategory[] subCategories, string name)
    {
        SubCategories = subCategories;
        Name = name;
    }
    public string Name;
    public SubCategory[] SubCategories;
}

[Serializable]
public class SubCategory
{
    public SubCategory(ModuleType[] moduleTypes, string name)
    {
        ModuleTypes = moduleTypes;
        Name = name;
    }
    public string Name;
    public ModuleType[] ModuleTypes;
}

[Serializable]
public class ModuleInfo
{
    public ModuleInfo(ModuleType moduleType, string name, GameObject prefab)
    {
        ModuleType = moduleType;
        Name = name;
        Prefab = prefab;
    }
    public ModuleType ModuleType;
    public string Name;
    public GameObject Prefab;
}

[Serializable]
public class SpaceshipInfo
{
    public SpaceshipInfo(Sprite icon, string name, GameObject prefab)
    {
        Icon = icon;
        Name = name;
        Prefab = prefab;
    }
    public Sprite Icon;
    public string Name;
    public GameObject Prefab;
}