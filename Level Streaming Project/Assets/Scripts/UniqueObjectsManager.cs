using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UniqueObjectsManager : MonoBehaviour
{
    public UniqueObjectClass uniqueObjects;
    public GameObject player;
    public List<GameObject> loadedObjects = new List<GameObject>();
    public float loadDistance;

#if (UNITY_EDITOR)
    [ContextMenu("Save Map")]
    void saveMap()
    {
        GameObject[] UniqueObjectReferences = GameObject.FindGameObjectsWithTag("unique");
        UniqueObjectClass saveObject = new UniqueObjectClass();
        saveObject.UniqueGameObject = new UniqueObjectClass.UniqueObjects[UniqueObjectReferences.Length];
        for (int i = 0; i < UniqueObjectReferences.Length; i++)
        {
            string name = UniqueObjectReferences[i].name;
            int index = name.LastIndexOf(" (");
            if (index > 0)
                name = name.Substring(0, index);

            saveObject.UniqueGameObject[i].name = name;
            saveObject.UniqueGameObject[i].x = UniqueObjectReferences[i].transform.position.x;
            saveObject.UniqueGameObject[i].y = UniqueObjectReferences[i].transform.position.y;
            saveObject.UniqueGameObject[i].z = UniqueObjectReferences[i].transform.position.z;
            saveObject.UniqueGameObject[i].rotX = UniqueObjectReferences[i].transform.rotation.eulerAngles.x;
            saveObject.UniqueGameObject[i].rotY = UniqueObjectReferences[i].transform.rotation.eulerAngles.y;
            saveObject.UniqueGameObject[i].rotZ = UniqueObjectReferences[i].transform.rotation.eulerAngles.z;
            saveObject.UniqueGameObject[i].scaleX = UniqueObjectReferences[i].transform.localScale.x;
            saveObject.UniqueGameObject[i].scaleY = UniqueObjectReferences[i].transform.localScale.z;
            saveObject.UniqueGameObject[i].scaleZ = UniqueObjectReferences[i].transform.localScale.y;
            saveObject.UniqueGameObject[i].materialName = UniqueObjectReferences[i].GetComponent<MeshRenderer>().sharedMaterial.name;
            saveObject.UniqueGameObject[i].meshName = UniqueObjectReferences[i].GetComponent<MeshFilter>().sharedMesh.name;
        }
        string chunkData = JsonUtility.ToJson(saveObject);
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/UniqueObjects.json", chunkData);
    }
#endif

    private void Start()
    {
        LoadFromMap();
    }
    void Update()
    {
        foreach (var item in loadedObjects)
        {
            float dist = Vector3.Distance(player.transform.position, item.transform.position);
            if (dist < loadDistance)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }
    public void LoadFromMap()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/UniqueObjects.json");
        uniqueObjects = (UniqueObjectClass.CreateFromJSON(json));
        foreach (var item in uniqueObjects.UniqueGameObject)
        {
            GameObject UniqueObject = new GameObject();
            UniqueObject.transform.parent = this.gameObject.transform;
            UniqueObject.transform.position = new Vector3(item.x, item.y, item.z);
            UniqueObject.transform.rotation = Quaternion.Euler(item.rotX, item.rotY, item.rotZ);
            UniqueObject.transform.localScale = new Vector3(item.scaleX, item.scaleY, item.scaleZ);
            UniqueObject.AddComponent<MeshFilter>();
            UniqueObject.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("UniqueModelData/Meshes/" + item.meshName);
            UniqueObject.AddComponent<MeshCollider>().sharedMesh = Resources.Load<Mesh>("UniqueModelData/Meshes/" + item.meshName);
            UniqueObject.AddComponent<MeshRenderer>().sharedMaterial = Resources.Load<Material>("UniqueModelData/Materials/" + item.materialName);

            loadedObjects.Add(UniqueObject);
            UniqueObject.SetActive(false);
        }
    }
}
