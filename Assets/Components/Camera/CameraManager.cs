using Fusion;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    private Transform target;
    private Camera camera;

    [SerializeField] private Vector3 offset;
    
    private void Start()
    {
        NetworkManager.PlayerSpawned.AddListener(player =>
        {
            if (player.HasInputAuthority)
                this.target = player.transform;
        });

        this.camera = Camera.main;
    }

    private void Update()
    {
        if (!this.target) return;
        
        this.camera.transform.position = this.target.position + this.offset;
        this.camera.transform.LookAt(this.target.position, Vector3.up);
    }
}
