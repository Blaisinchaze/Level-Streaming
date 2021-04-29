using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Scenes;

public class SubsceneLoader : ComponentSystem
{

    private SceneSystem sceneSystem;

    protected override void OnCreate()
    {
        sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    }
    protected override void OnUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    foreach (SubScene subScene in SubSceneReferences.Instance.maps)
        //    {
        //        Debug.Log("unload");
        //        UnloadSubScene(subScene);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    foreach (SubScene subScene in SubSceneReferences.Instance.maps)
        //    {
        //        Debug.Log("load");
        //        LoadSubScene(subScene);
        //    }
        //}
        Entities.WithAll<Tag_Player>().ForEach((ref Translation translation) =>
        {
            foreach (SubScene subScene in SubSceneReferences.Instance.maps)
            {
                float loadDistance = 100f;
                if (math.distance(translation.Value, subScene.transform.position) < loadDistance)
                {
                    LoadSubScene(subScene);
                }
                else
                {
                    UnloadSubScene(subScene);
                }
            }

        });
    }

    private void LoadSubScene(SubScene subScene)
    {
        sceneSystem.LoadSceneAsync(subScene.SceneGUID);
    }    
    
    private void UnloadSubScene(SubScene subScene)
    {
        sceneSystem.UnloadScene(subScene.SceneGUID);
    }
}
