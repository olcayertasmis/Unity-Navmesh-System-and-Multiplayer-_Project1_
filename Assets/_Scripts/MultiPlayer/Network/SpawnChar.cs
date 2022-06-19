using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class SpawnChar : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefabBlue,playerPrefabRed,playerPrefabGreen;

    //Other Components
    CharacterInputHandler characterInputHandler;

    [SerializeField]
    int spawnCount = 0;

    [SerializeField]
    Material blueMaterial, greenMaterial, redMaterial;

    void Start()
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared)
        {
            Debug.Log("OnConnectedToServer, starting player prefabs as local player");
        }
        else
        {
            Debug.Log("OnConnectedToServer");
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log("OnPlayerJoined we are server.Spawning Player");
            switch (spawnCount)
            {
                case 0:
                    runner.Spawn(playerPrefabBlue, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
                    break;

                case 1:
                    runner.Spawn(playerPrefabGreen, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
                    break;

                case 2:
                    runner.Spawn(playerPrefabRed, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
                    break;
            }
        }
        else
        {
            Debug.Log("OnPlayerJoined");
        }
        spawnCount++;
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (characterInputHandler == null && NetworkPlayer.Local != null)
        {
            characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();
        }

        if (characterInputHandler != null)
        {
            input.Set(characterInputHandler.GetNetworkInput());
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        print("Player left.");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        print("Input missing.");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        print("Shutdown.");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        print("Disconnected from server");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        print("On connect request.");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        print("On connect failed.");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        print("User simulation message.");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        print("Session list updated.");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        print("Custom authentication response.");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        print("Host migration.");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        print("Reliable data received.");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        print("Scene load done.");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        print("Scene load start.");
    }
}
