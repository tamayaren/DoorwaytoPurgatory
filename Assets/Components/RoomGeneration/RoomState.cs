using UnityEngine;

public class RoomState : MonoBehaviour
{
    public RoomStateType state = RoomStateType.NoSpawn;
}

public enum RoomStateType
{
    NoSpawn,
    HasSpawned
}
