using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnAI : MonoBehaviour
{
    public float wanderRadius;

    public List<GameObject> npcs = new List<GameObject>();
    public GameObject npcPrefab;
    public GameObject npcHolder;
    public LevelManager levelManager;
    // Use this for initialization
    void OnEnable()
    {
    }

    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (npcs.Count<= 300 && levelManager.currentGameMode == LevelManager.GameMode.INGAME)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            if(newPos != new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity))
            {
                GameObject npc = Instantiate(npcPrefab, newPos, Quaternion.identity,npcHolder.transform);
                npc.GetComponent<WanderingAI>().player = this.gameObject;
                npcs.Add(npc);
            }


        }
    }

    public void DestroyAllNPCs()
    {
        for (int i = 0; i < npcHolder.transform.childCount; i++)
        {
            Destroy(npcHolder.transform.GetChild(i));
        }
        npcs.Clear();
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
