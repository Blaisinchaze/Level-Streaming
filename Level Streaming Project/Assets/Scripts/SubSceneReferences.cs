using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Scenes;

public class SubSceneReferences : MonoBehaviour
{
    public static SubSceneReferences Instance { get; private set; }

    public SubScene[] maps;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
}
