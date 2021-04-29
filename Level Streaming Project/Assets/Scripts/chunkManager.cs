using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;


public class chunkManager : MonoBehaviour
{
    public Chunks chunks;
    public GameObject player;
    public List<GameObject> loadedChunks = new List<GameObject>();
    public string jsonFileName;
    public float loadDistance;
    public GameObject[] chunkReferences;
    public List<int> destroyedBuildings = new List<int>();
    private IEnumerator coroutine;
    private string json;
    private string jsonLocation;


#if (UNITY_EDITOR)
    [ContextMenu("Save Map")]
    public void saveMap()
    {
        //GameObject[] chunkReferences = GameObject.FindGameObjectsWithTag("chunk");
        Chunks saveChunks = new Chunks();
        saveChunks.chunks = new Chunks.ChunkClass[chunkReferences.Length];
        for (int i = 0; i < chunkReferences.Length; i++)
        {
            saveChunks.chunks[i].x = chunkReferences[i].transform.position.x;
            saveChunks.chunks[i].y = chunkReferences[i].transform.position.y;
            saveChunks.chunks[i].z = chunkReferences[i].transform.position.z;
            Chunks.ChunkObjects[] tempChunkObjects = new Chunks.ChunkObjects[chunkReferences[i].transform.childCount];
            for (int j = 0; j < chunkReferences[i].transform.childCount; j++)
            {
                string name = chunkReferences[i].transform.GetChild(j).name;
                int index = name.LastIndexOf(" (");
                if (index > 0)
                    name = name.Substring(0, index);

                tempChunkObjects[j].name = name;
                tempChunkObjects[j].id = j;
                if(chunkReferences[i].transform.GetChild(j).GetComponent<DestroyedBool>() != null)
                {
                    if(chunkReferences[i].transform.GetChild(j).GetComponent<DestroyedBool>().destroyed)
                        tempChunkObjects[j].destroyed = true;
                    else
                        tempChunkObjects[j].destroyed = false;
                }
                else
                    tempChunkObjects[j].destroyed = false;
                tempChunkObjects[j].x = chunkReferences[i].transform.GetChild(j).transform.position.x;
                tempChunkObjects[j].y = chunkReferences[i].transform.GetChild(j).transform.position.y;
                tempChunkObjects[j].z = chunkReferences[i].transform.GetChild(j).transform.position.z;
                tempChunkObjects[j].rotX = chunkReferences[i].transform.GetChild(j).transform.rotation.eulerAngles.x;
                tempChunkObjects[j].rotY = chunkReferences[i].transform.GetChild(j).transform.rotation.eulerAngles.y;
                tempChunkObjects[j].rotZ = chunkReferences[i].transform.GetChild(j).transform.rotation.eulerAngles.z;
                tempChunkObjects[j].scaleX = chunkReferences[i].transform.GetChild(j).transform.localScale.x;
                tempChunkObjects[j].scaleY = chunkReferences[i].transform.GetChild(j).transform.localScale.z;
                tempChunkObjects[j].scaleZ = chunkReferences[i].transform.GetChild(j).transform.localScale.y;
                saveChunks.chunks[i].chunkObjects = tempChunkObjects;
            }
        } 
        string chunkData = JsonUtility.ToJson(saveChunks);
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/mapData/" + jsonFileName + ".json", chunkData);
    }
#endif
    // Start is called before the first frame update
    void Start()
    {
        jsonLocation = Application.dataPath + "/Resources/currentInstance/" + jsonFileName + ".json";
         json = File.ReadAllText(jsonLocation);
        LoadFromMap();
        chunkReferences = loadedChunks.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in loadedChunks)
        {
            float dist = Vector3.Distance(player.transform.position, item.transform.position);
            if (dist < loadDistance)
            {
                LoadChunk(item);
            }
            else
            {
                UnloadChunk(item);
            }
        }
    }

    public void LoadFromMap()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/currentInstance/"+ jsonFileName + ".json");
        chunks = (Chunks.CreateFromJSON(json));
        foreach (var item in chunks.chunks)
        {
            GameObject chunkParent = new GameObject();
            chunkParent.transform.parent = this.gameObject.transform;
            chunkParent.transform.position = new Vector3(item.x, item.y, item.z);
            //chunkParent.AddComponent<DistanceChecker>();
            foreach (var item2 in item.chunkObjects)
            {
                if(!item2.destroyed)
                {
                    GameObject newObject = Instantiate(Resources.Load(item2.name, typeof(GameObject)), chunkParent.transform) as GameObject;
                    newObject.name = item2.id.ToString();
                    newObject.transform.position = new Vector3(item2.x, item2.y, item2.z);
                    newObject.transform.rotation = Quaternion.Euler(item2.rotX, item2.rotY, item2.rotZ);
                    newObject.transform.localScale = new Vector3(item2.scaleX, item2.scaleY, item2.scaleZ);
                }


            }
            loadedChunks.Add(chunkParent);
            foreach (var item4 in loadedChunks)
            {
                foreach (var item3 in item4.GetComponentsInChildren<MeshRenderer>())
                {
                    item3.enabled = false;
                }
            }
        }
    }    
    

    private void LoadChunk(GameObject chunk)
    {
        foreach (var item in chunk.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = true;
        }
    }    
    private void UnloadChunk(GameObject chunk)
    {
        foreach (var item in chunk.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
    }

    private void OnApplicationQuit()
    {
        DestroyArea();
    }

    public void DestroyArea()
    {
        StartCoroutine(WaitAndPrint());

    }

    private IEnumerator WaitAndPrint()
    {
        yield return StartCoroutine(SaveChunk());

        Destroy(this.gameObject);
    }

    public IEnumerator SaveChunk()
    {
        bool done = false;

        dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                if (transform.GetChild(i).transform.GetChild(j).GetComponent<DestroyedBool>() != null)
                {
                    if (transform.GetChild(i).transform.GetChild(j).GetComponent<DestroyedBool>().destroyed)
                    {

                        jsonObj["chunks"][i]["chunkObjects"][j]["destroyed"] = true;

                    }
                }


            }

        }        
        new Thread(() => {
            
        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(jsonLocation, output);
            done = true;
        }).Start();

        // Do nothing on each frame until the thread is done
        while (!done)
        {
            yield return null;
        }
    }


}



