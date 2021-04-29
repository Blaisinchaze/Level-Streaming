using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTargetTest : MonoBehaviour
{
    public GameObject agent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
    }
}
