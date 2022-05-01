using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //private DataManagerMain dataManagerMain;

    private const string FILE_NAME = "save_game.dat";

    private string filePath;

    private GameSaveData gameSaveData = null;

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    public GameSaveData GameSaveData
    {
        get { return this.gameSaveData; }
    }

    public void Initialize()
    {
        this.filePath = Path.Combine(Application.persistentDataPath, FILE_NAME);

        if (!File.Exists(filePath))
        {
            CreateNewSaveFile();
        } else
        {
            LoadData();
        }
    }

    /// <summary>
    /// Saves the necessary User Data into the storage
    /// </summary>
    public void SaveData()
    {
        GameSaveData data = new GameSaveData();
        SaveFile(data);
    }

    /// <summary>
    /// Loads the saved User Data from storage
    /// </summary>
    public void LoadData()
    {
        this.gameSaveData = LoadFile<GameSaveData>(this.filePath);
        if (this.gameSaveData != null)
            this.isDone = true;
    }

    /// <summary>
    /// Creates a new default save file
    /// </summary>
    private void CreateNewSaveFile()
    {
        this.gameSaveData = new GameSaveData();
        SaveFile(gameSaveData);
        if (this.gameSaveData != null)
            this.isDone = true;
    }

    /// <summary>
    /// Generic File Saving Method that encodes json files into base64 before saving
    /// </summary>
    /// <param name="data">Object data to be saved as a Json File and encoded into base 64</param>
    private void SaveFile(object data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        string base64 = Convert.ToBase64String(bytes);
        File.WriteAllText(filePath, base64);
        //string data64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        //File.WriteAllText(filePath, data64);
        //Debug.Log("Saved");
    }

    /// <summary>
    /// Generic File Loader Method that decodes the base64 file into a json format
    /// </summary>
    /// <typeparam name="T">The desired type the base64 decoded Json File is converted to</typeparam>
    /// <param name="filePath">Path of the desired file to be loaded</param>
    /// <returns></returns>
    private T LoadFile<T>(string filePath)
    {
        byte[] data64 = Convert.FromBase64String(File.ReadAllText(filePath));
        return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(data64));
    }

    public void SaveCheckPoint(Transform checkpoint)
    {
        this.gameSaveData.SetCheckpoint(checkpoint);
        Debug.Log("DATA FILE: " + this.gameSaveData);
        SaveFile(this.gameSaveData);
    }
}

[Serializable]
public class CheckPoint
{
    public float x, y, z;
}
[Serializable]
public class GameSaveData
{
    public int level;
    public CheckPoint checkpoint;

    public CheckPoint Checkpoint
    {
        get { return this.checkpoint; }
    }

    public GameSaveData(int level = 1)
    {
        this.level = level;
        this.checkpoint = new CheckPoint();
    }

    public void SetCheckpoint(Transform transform)
    {
        if (checkpoint == null)
        {
            this.checkpoint = new CheckPoint();
        }
        checkpoint.x = transform.position.x;
        checkpoint.y = transform.position.y;
        checkpoint.z = transform.position.z;
    }
}
