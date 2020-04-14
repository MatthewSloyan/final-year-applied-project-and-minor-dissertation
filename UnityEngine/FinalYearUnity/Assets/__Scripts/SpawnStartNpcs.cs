using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStartNpcs : MonoBehaviour
{
    public GameObject NPC;
    public GameObject NPC2;
    private GameObject npcCopy;
    IEnumerator SpawnDelay()
    {


        
        while(true){
            int rand = Random.Range(0,2);
            Debug.Log("RAND: " + rand);
            if(rand == 0){
                npcCopy = NPC;
            }
            else if(rand == 1){
                npcCopy = NPC2;
            }
            Instantiate(npcCopy,gameObject.transform.position,Quaternion.Euler(0, -90, 0));
            //Debug.Log("Spawned");
            yield return new WaitForSecondsRealtime(1);
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
