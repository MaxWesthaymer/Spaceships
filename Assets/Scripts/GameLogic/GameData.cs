using System;
using System.Collections.Generic;
using GameConstants;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public Data Data { get; private set; }
    public int CurrentShip { get; private set; }
    [SerializeField] public GameConfig gameConfig;
    private void Awake()
    {
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
        var str = PlayerPrefs.GetString("savingdata");
        if (str == String.Empty)
        {
            Data = new Data();
            Data.spaceships = new List<SpaceshipData>();

            for (int i = 0; i < gameConfig.SpaceshipInfos.Length; i++)
            {
                Data.spaceships.Add(new SpaceshipData(new List<ModuleData>()));
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
}

[Serializable]
public class Data
{
    public List<SpaceshipData> spaceships;
}

[Serializable]
public class SpaceshipData
{
    public SpaceshipData(List<ModuleData> modules)
    {
        this.modules = modules;
    }
    public List<ModuleData> modules;
}
[Serializable]
public class ModuleData
{
    public ModuleData(Vector2Int coordinate, ModuleType type)
    {
        this.coordinate = coordinate;
        moduleType = type;
    }
    public Vector2Int coordinate;
    public ModuleType moduleType;
}

