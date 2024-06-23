using System.ComponentModel;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;


public struct CubeSpawner: IComponentData
{
    public Entity cube;

    

}
[DisallowMultipleComponent]
public class CubeSpawnerAutgoring : MonoBehaviour
{
    public GameObject cube;


    class Baker : Baker<CubeSpawnerAutgoring>
    {
        public override void Bake(CubeSpawnerAutgoring authoring)
        {
            CubeSpawner component = default(CubeSpawner);
            component.cube = GetEntity(authoring.cube, TransformUsageFlags.Dynamic);

            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, component);
            
        }
    }
}
