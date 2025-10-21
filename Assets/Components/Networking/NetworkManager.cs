using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkManager instance;
    [SerializeField] private NetworkPrefabRef playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();
    private NetworkRunner networkRunner;

    public static UnityEvent<NetworkObject> PlayerSpawned = new UnityEvent<NetworkObject>();
    private void Awake() => instance = this;

    private void Start()
    {
        
    }
    
    async void OnGameStart(GameMode mode)
    {
        this.networkRunner = this.AddComponent<NetworkRunner>();
        this.networkRunner.ProvideInput = true;

        SceneRef scene = SceneRef.FromIndex(0);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();

        if (!scene.IsValid) return;
        sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);
        
        await networkRunner.StartGame(new StartGameArgs()
        {
            Scene = scene,
            SessionName = "RunTest",
            GameMode = mode,
            SceneManager = this.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void OnGUI()
    {
        if (this.networkRunner == null)
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Start Game"))
                OnGameStart(GameMode.AutoHostOrClient);
            if (GUI.Button(new Rect(10, 40, 100, 30), "Host"))
                OnGameStart(GameMode.Host);
            if (GUI.Button(new Rect(10, 80, 100, 30), "Client"))
                OnGameStart(GameMode.Client);
        }
    }

    #region PhotonCallbacks
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player joined");

        if (runner.IsServer)
        {
            Vector3 spawnPosition = this.transform.position;
            NetworkObject playerObject = runner.Spawn(this.playerPrefab, spawnPosition, Quaternion.identity, player);
            
            this.players.Add(player, playerObject);
            PlayerSpawned.Invoke(playerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player left");

        if (this.players.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            this.players.Remove(player);
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData networkInput = new NetworkInputData();
        
        networkInput.direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        networkInput.view = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * -1);
        input.Set(networkInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
    
    #endregion
}
