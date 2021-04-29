using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float loadDistance = 100f;
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > loadDistance) Destroy(this.gameObject);
    }
}
