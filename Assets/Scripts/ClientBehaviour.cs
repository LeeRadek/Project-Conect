using Unity.Networking.Transport;
using UnityEngine;

public class ClientBehaviour : MonoBehaviour
{
    NetworkDriver networkDriver;
    NetworkConnection connection;

    void Start()
    {
        networkDriver = NetworkDriver.Create();

        NetworkEndpoint endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(7777);
        connection = networkDriver.Connect(endpoint);
    }

    private void OnDestroy()
    {
        networkDriver.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        networkDriver.ScheduleUpdate().Complete();

        if (!connection.IsCreated)
        {
            return;
        }

        Unity.Collections.DataStreamReader stream;
        NetworkEvent.Type cmd;
        while((cmd = connection.PopEvent(networkDriver, out stream)) != NetworkEvent.Type.Empty)
        {
            if(cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");

                uint value = 1;
                networkDriver.BeginSend(connection, out var writer);
                writer.WriteUInt(value);
                networkDriver.EndSend(writer);
            }
            else if(cmd == NetworkEvent.Type.Data)
            {
                uint value = stream.ReadUInt();
                Debug.Log($"Got the value {value} back form server");
                connection.Disconnect(networkDriver);
            }
            else if(cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconected from server");
                connection = default;
            }
        }
    }
}
