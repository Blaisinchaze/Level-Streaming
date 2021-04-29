using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UniqueObjectClass
{

    [System.Serializable]
    public struct UniqueObjects
    {
        public string name;
        public float x;
        public float y;
        public float z;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
        public string materialName;
        public string meshName;
    }
    public UniqueObjects[] UniqueGameObject;

    public static UniqueObjectClass CreateFromJSON(string jsonString)
    {
        return JsonConvert.DeserializeObject<UniqueObjectClass>(jsonString);
        //return JsonUtility.FromJson<Simulations>(jsonString);
    }
}
