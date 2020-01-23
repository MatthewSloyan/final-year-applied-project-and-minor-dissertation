using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNPC : MonoBehaviour
{
    public GameObject npc;
    private GameObject container;
    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        container = new GameObject("container");



        for (int i = -8; i < -4; i += 2)
        {

            //container.transform.parent = npc.transform;
            //
            GameObject copy = Instantiate(npc, new Vector3(5, 1, i), Quaternion.identity); ;
            copy.transform.parent = container.transform;
            
        }


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
