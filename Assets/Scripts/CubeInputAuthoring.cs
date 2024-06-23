using System;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
public struct CubeInput: IInputComponentData
{
    public int horizontal;
    public int vertical;

}
[DisallowMultipleComponent]
public class CubeInputAuthoring : MonoBehaviour
{
    class Baking : Unity.Entities.Baker<CubeInputAuthoring>
    {
        public override void Bake(CubeInputAuthoring authoring)
        {
            AddComponent<CubeInput>();
        }
    }
}
[UpdateInGroup(typeof(GhostInputSystemGroup))]
public partial struct SampleCubeInput : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        bool left = UnityEngine.Input.GetKey("left");
        bool right = UnityEngine.Input.GetKey("right");
        bool up = UnityEngine.Input.GetKey("up");
        bool down = UnityEngine.Input.GetKey("down");

        foreach (var playerInput in SystemAPI.Query<RefRW<CubeInput>>().WithAll<GhostOwnerIsLocal>())
        {
            playerInput.ValueRW = default;
            if(left)
            {
                playerInput.ValueRW.horizontal = -1;
            }
            if (right)
            {
                playerInput.ValueRW.horizontal = 1;
            }
            if (up)
            {
                playerInput.ValueRW.vertical = 1;
            }
            if (down)
            {
                playerInput.ValueRW.vertical = -1;
            }
        }
    }
}
