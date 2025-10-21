using Fusion;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    private Camera camera;

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    
    private void Start()
    {
        NetworkManager.PlayerSpawned.AddListener(player =>
        {
            Debug.Log("Player Spawned");
            if (player.HasInputAuthority)
                this.target = player.transform;
        });
        
        this.camera = Camera.main;
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (!this.HasStateAuthority) return;
        if (!this.target) return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (!GetInput(out NetworkInputData input))
        {
            Debug.Log("No input returned");
            return;
        }
        Debug.Log("Nano");
        
        
        this.camera.transform.position = this.target.position + this.offset;
        this.camera.transform.localRotation = Quaternion.Euler(input.view.x, 0, input.view.y);
    }
}
