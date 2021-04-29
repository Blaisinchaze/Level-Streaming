using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunks
{
    [System.Serializable]
    public struct ChunkClass
    {
        public float x;
        public float y;
        public float z;
        public ChunkObjects[] chunkObjects;
    }

    [System.Serializable]
    public struct ChunkObjects
    {
        public string name;
        public int id;
        public float x;
        public float y;
        public float z;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
        public bool destroyed;
    }
    public ChunkClass[] chunks;

    public static Chunks CreateFromJSON(string jsonString)
    {
        return JsonConvert.DeserializeObject<Chunks>(jsonString);
        //return JsonUtility.FromJson<Simulations>(jsonString);
    }   
}

//[System.Serializable]
//public class ChunkObjects
//{
//    public string name;
//    public float x;
//    public float y;
//    public float z;
//    public float rotX;
//    public float rotY;
//    public float rotZ;
//    public float scaleX;
//    public float scaleY;
//    public float scaleZ;
//}
