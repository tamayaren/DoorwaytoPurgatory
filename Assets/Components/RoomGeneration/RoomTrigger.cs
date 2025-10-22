using System;
using Fusion;
using UnityEngine;

public class RoomTrigger : NetworkBehaviour
{
    private GameObject room;
    private RoomState roomState;

    private void Start()
    {
        this.room = this.transform.parent.gameObject;
        this.room.TryGetComponent(out this.roomState);
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcRemoteGenerateNewRoom()
    {
        if (!this.roomState) return;
        if (this.roomState.state == RoomStateType.NoSpawn)
            GenerateRoom.instance.Generate(this.transform.parent.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!this.HasStateAuthority)
            return;
        
        if (other.TryGetComponent(out NetworkObject nObj))
            RpcRemoteGenerateNewRoom();
    }
}
