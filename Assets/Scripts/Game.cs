using System.Net;
using System.Net.NetworkInformation;
using Unity.NetCode;
using UnityEngine;

[UnityEngine.Scripting.Preserve]
public class GameBootstrap : ClientServerBootstrap
{

    public override bool Initialize(string defaultWorldName)
    {
        
        AutoConnectPort = 7777;
        return base.Initialize(defaultWorldName);
    }

    private void Awake()
    {
        Application.runInBackground = true;
    }
}
