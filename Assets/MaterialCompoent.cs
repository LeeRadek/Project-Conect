using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public struct ColorComponent : IComponentData
{
    public float4 color;
    public Entity materialEntity;

}

public class MaterialCompoent : MonoBehaviour
{
    public float4 color;
    public Material material;

    class Baker : Baker<MaterialCompoent>
    {
        public override void Bake(MaterialCompoent authoring)
        {


            var entity = GetEntity(TransformUsageFlags.None);
            var materialEntity = CreateAdditionalEntity(TransformUsageFlags.None);

            AddComponent(entity, new ColorComponent { color = authoring.color, materialEntity = materialEntity });

            AddSharedComponentManaged(materialEntity, new RenderMesh
            {
                material = authoring.material,
                mesh = GetComponent<MeshFilter>().sharedMesh
            });

        }
    }
}
