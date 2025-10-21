using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    [SerializeField] private Camera camera;

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private float pitch;

    [SerializeField] private Transform head;
    [SerializeField] private Transform rootcamera;

    public override void Spawned()
    {
        base.Spawned();
        
        this.target = this.transform;
        this.camera = Camera.main;

        if (this.HasInputAuthority)
        {
            Debug.Log("Input");
            this.rootcamera = GameObject.FindGameObjectWithTag("Root").transform;
            this.head = this.rootcamera?.Find("Head").transform;
            this.camera.transform.parent = this.head;
        }
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
        
        this.rootcamera.transform.position = this.target.position + this.offset;
        this.rootcamera.Rotate(Vector3.up * input.view.x);

        this.pitch -= input.view.y;
        this.pitch = Mathf.Clamp(this.pitch, -85f, 85f);
        this.head.localRotation = Quaternion.Euler(this.pitch, 0f, 0f);
    }
}
