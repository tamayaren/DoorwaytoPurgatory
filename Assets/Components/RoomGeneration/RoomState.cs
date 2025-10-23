using Fusion;
using UnityEngine;

public class RoomState : NetworkBehaviour
{
    public RoomStateType state = RoomStateType.NoSpawn;
}

public enum RoomStateType
{
    NoSpawn,
    HasSpawned
}
