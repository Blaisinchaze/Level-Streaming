using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZoneClass
{
    [System.Serializable]
    public struct ZonesClass
    {
        public string jsonName;
        public ZoneLocation[] location;
    }

    [System.Serializable]
    public struct ZoneLocation
    {
        public float x;
        public float y;
        public float z;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
    }
    public ZoneClass[] zones;

    public static ZoneClass CreateFromJSON(string jsonString)
    {
        return JsonConvert.DeserializeObject<ZoneClass>(jsonString);
        //return JsonUtility.FromJson<Simulations>(jsonString);
    }
}
