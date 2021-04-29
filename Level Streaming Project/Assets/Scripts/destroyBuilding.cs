using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBuilding : MonoBehaviour
{
    public LevelManager levelManager;
    public GameObject explosion;
    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "destructable")
        {
            GameObject boom = Instantiate(explosion,new Vector3( other.transform.position.x,25,other.transform.position.z),Quaternion.identity);
            boom.transform.SetParent(null);
            levelManager.IncreaseScore(1000);
            other.transform.GetComponent<DestroyedBool>().destroyed = true;
            //other.GetComponent<MeshRenderer>().enabled = false;
            //other.GetComponent<MeshCollider>().enabled = false;
            other.gameObject.SetActive(false);
        }
    }
}
