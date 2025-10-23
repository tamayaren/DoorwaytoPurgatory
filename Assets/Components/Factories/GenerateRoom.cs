using DG.Tweening;
using Fusion;
using UnityEngine;

public class GenerateRoom : NetworkBehaviour
{
    public static GenerateRoom instance;
    [SerializeField] private GameMetadata metadata;
    [SerializeField] private int rooms = 2;
    
    public void Awake() => instance = this;

    private Bounds GetBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);

        return bounds;
    }
    
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
        
        this.rooms++;
        int index = Random.Range(0, this.metadata.rooms.Length);
        GameObject chosenRoom = this.metadata.rooms[index];

        NetworkObject createdRoom = this.Runner.Spawn(chosenRoom, Vector3.zero);
        createdRoom.transform.SetParent(room.transform.parent);
        Transform createdRoot = createdRoom.transform.Find("Root").transform;

        float size = 1f;
        if (this.rooms == 2) size = 2f;
        Bounds bound = GetBounds(createdRoom.gameObject);
        
        Debug.Log(bound.size);
        Vector3 offset = new Vector3(-(bound.size.x + (size * this.rooms)), -bound.size.y/2f, Random.Range(-10f, 10f));
        Vector3 transformedPosition = room.transform.position + offset;
        
        createdRoom.transform.position = room.transform.position + new Vector3(0f, -10f, 0f);
        createdRoom.transform.DOMove(transformedPosition, .25f);
        roomState.state = RoomStateType.HasSpawned;
    }
}
