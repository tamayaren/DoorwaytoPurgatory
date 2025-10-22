using Fusion;
using UnityEngine;

public class GenerateRoom : MonoBehaviour
{
    public static GenerateRoom instance;
    [SerializeField] private GameMetadata metadata;
    
    public void Awake() => instance = this;
    public void Generate(GameObject room)
    {
        Debug.Log("Generate Room");
        Transform root = room.transform.Find("Root")?.transform;
        Transform spawn = room.transform.Find("Spawn")?.transform;
        room.TryGetComponent(out RoomState roomState);
        
        Debug.Log("Room State: " + roomState);
        Debug.Log("Room Name: " + room.name);
        Debug.Log("Room Spawn: " + spawn.name);
        if (!root) return;
        if (!spawn) return;
        if (!roomState) return;
        
        int index = Random.Range(0, this.metadata.rooms.Length);
        GameObject chosenRoom = this.metadata.rooms[index];

        GameObject createdRoom = Instantiate(chosenRoom, room.transform.parent, true);
        Transform createdRoot = createdRoom.transform.Find("Root").transform;
        
        Vector3 offset = room.transform.position - createdRoot.position;
        createdRoom.transform.position = root.position + offset;
        roomState.state = RoomStateType.HasSpawned;
    }
}
