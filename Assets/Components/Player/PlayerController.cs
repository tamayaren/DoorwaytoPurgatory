using System;
using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 10;
    private NetworkCharacterController controller;

    private void Awake()
    {
        this.controller = GetComponent<NetworkCharacterController>();
    }

    public override void Spawned()
    {
        base.Spawned();
        this.name = $"{this.Object.InputAuthority.PlayerId}";
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (!GetInput(out NetworkInputData input)) return;
        if (!this.HasStateAuthority) return;
        
        MovePlayer(input);
    }
    
    //
    private void MovePlayer(NetworkInputData input)
    {
        this.controller.Move(this.speed * input.direction * this.Runner.DeltaTime);
    }
}
