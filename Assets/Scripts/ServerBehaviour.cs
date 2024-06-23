using Unity.Collections;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine;

public class ServerBehaviour : MonoBehaviour
{
    NetworkDriver networkDriver;
    NativeList<NetworkConnection> connections;

    void Start()
    {
        networkDriver = NetworkDriver.Create();
        connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        NetworkEndpoint endpoint = NetworkEndpoint.AnyIpv4.WithPort(7777);
        if(networkDriver.Bind(endpoint) != 0)
        {
            Debug.LogError("Fialed to bidn to port 7777");
            return;
        }
        networkDriver.Listen();

    }

    private void OnDestroy()
    {
        if (networkDriver.IsCreated)
        {
            networkDriver.Dispose();
            connections.Dispose();
        }
    }


    void Update()
    {
        networkDriver.ScheduleUpdate().Complete();

        for(int i = 0; i < connections.Length; i++)
        {
            if (connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                i--;
            }
        }

        NetworkConnection c;
        while((c = networkDriver.Accept()) != default)
        {
            connections.Add(c);
            Debug.Log("Accepted conncetion");
        }

        for(int i = 0; i < connections.Length; i++)
        {
            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while((cmd = networkDriver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if(cmd == NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadUInt();
                    Debug.Log($"6ot{number} from a client , adding 2 to it");
                    
                    number += 2;
                    networkDriver.BeginSend(NetworkPipeline.Null, connections[i], out var writer);
                    writer.WriteUInt(number);
                    networkDriver.EndSend(writer);
                }
                else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconected from the server");
                    connections[i] = default; 
                    break;
                }

            }
        }
    }
}
