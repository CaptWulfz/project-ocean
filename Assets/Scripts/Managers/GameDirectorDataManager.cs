using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDirectorMain
{
    public Vector3 SpawnToCheckpoint()
    {
        if (DataManager.Instance.GameSaveData.Checkpoint == null)
        {
            return Vector3.zero;
        }
        Vector3 location = new Vector3(
            DataManager.Instance.GameSaveData.Checkpoint.x, 
            DataManager.Instance.GameSaveData.Checkpoint.y,
            DataManager.Instance.GameSaveData.Checkpoint.z);
        if (location != null)
            return location;
        else
            return Vector3.zero;
    }

}