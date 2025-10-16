using Fusion;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float speed = 5f;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        
        this.transform.position += this.transform.forward * this.speed * this.Runner.DeltaTime;
    }
}
