using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPoints.asset", menuName = "Database/Spawn Points")]
public class SpawnPoints : ScriptableObject
{
    [System.Serializable]
    public class LookStateToVector2
    {
        public Player.LookStates LookState;
        public Vector2 SpawnCoord;
    }

    public LookStateToVector2[] DirectionToCoord;
}
