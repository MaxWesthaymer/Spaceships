using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameConstants;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public Data Data { get; private set; }
    public int CurrentShip { get; private set; }
    [SerializeField] public GameConfig gameConfig;
    void Awake()
    {
        //Unity singleton 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadData();
    }

    private void SaveData(object obj)
    {
        var str = JsonUtility.ToJson(obj);
        Debug.Log(str);
        PlayerPrefs.SetString("savingdata", str);
    }
    
    public void Save()
    {
        SaveData(Data);
    }
    
    public void SetCurrentShip(int value)
    {
        CurrentShip = value;
    }
    
    private void LoadData()
    {
        Debug.Log("Loading");
        var str = PlayerPrefs.GetString("savingdata");
        if (str == String.Empty)
        {
            Data = new Data();
            Data.spaceships = new SpaceshipData[gameConfig.SpaceshipInfos.Length];

            foreach (var spaceship in Data.spaceships)
            {
                spaceship.modules = new Dictionary<Vector2Int, ModuleType>();
            }
        }
        else
        {
            Data = JsonUtility.FromJson<Data>(str);
        }
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    private void OnApplicationPause()
    {
        Save();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus)
            Save();
    }
}

[Serializable]
public class Data
{
    public SpaceshipData[] spaceships;
}

[Serializable]
public class SpaceshipData
{
    public Dictionary<Vector2Int, ModuleType> modules;
}

