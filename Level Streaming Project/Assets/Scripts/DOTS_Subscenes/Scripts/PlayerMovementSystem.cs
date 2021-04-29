using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Collections;

public class PlayerMovementSystem : ComponentSystem {

    protected override void OnUpdate()
    {
        Entities.WithAll<Tag_Player>().ForEach((ref Translation translation) =>
        {
            float moveZ = 0f;
            float moveX = 0f;
            float moveSpeed = 10f;
            if (Input.GetKey(KeyCode.UpArrow)) moveZ = -1f;
            if (Input.GetKey(KeyCode.DownArrow)) moveZ = 1f;
            if (Input.GetKey(KeyCode.RightArrow)) moveX = -1f;
            if (Input.GetKey(KeyCode.LeftArrow)) moveX = 1f;

            translation.Value += new float3(moveX,0,moveZ) * Time.DeltaTime * moveSpeed;
        });
        //Entities.WithAll<Tag_Player>().ForEach((ref Rotation rotation) =>
        //{
        //    float rotY = 0f;
        //    if (Input.GetKey(KeyCode.RightArrow)) rotY = +1f;
        //    if (Input.GetKey(KeyCode.LeftArrow)) rotY = -1f;

        //    float rotationSpeed = 100f;
        //    rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(rotY * rotationSpeed * Time.DeltaTime)));
        //});
    }
}
