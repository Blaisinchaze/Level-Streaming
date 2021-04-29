using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float3 offset = float3.zero;

    private EntityManager manager;

    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        if(entityToFollow == null) { return; }

        Translation entPos = manager.GetComponentData<Translation>(entityToFollow);
        transform.position = entPos.Value + offset;
    }
}
