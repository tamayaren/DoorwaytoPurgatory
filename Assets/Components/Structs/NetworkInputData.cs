using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;
    public Vector3 direction;
    public Vector2 view;
    public NetworkBool jumpPressed;
}
