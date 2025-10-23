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
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
        
        Vector3 move = camForward * input.direction.z + camRight * input.direction.x;
        if (input.jumpPressed && this.controller.Grounded)
            this.controller.Jump();
        this.controller.Move(move * this.speed * this.Runner.DeltaTime);
        
            
    }
}
