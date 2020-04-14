using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStartNpcs : MonoBehaviour
{
    public GameObject NPC;
    
    IEnumerator SpawnDelay()
    {
        GameObject npcCopy = NPC;
        while(true){
            Instantiate(npcCopy,gameObject.transform.position,Quaternion.Euler(0, -90, 0));
            Debug.Log("Spawned");
            yield return new WaitForSecondsRealtime(5);
        }

       
    }
    
    void Start()
    {
        StartCoroutine("SpawnDelay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
