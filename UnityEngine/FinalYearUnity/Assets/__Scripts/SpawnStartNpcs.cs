using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStartNpcs : MonoBehaviour
{
    public GameObject NPC;
    public GameObject NPC2;
    public GameObject NPC3;
    private GameObject npcCopy;
    IEnumerator SpawnDelay()
    {


        
        while(true){
            int rand = Random.Range(0,3);
            Debug.Log("RAND: " + rand);
            if(rand == 0){
                npcCopy = NPC;
            }
            else if(rand == 1){
                npcCopy = NPC2;
            }else if(rand == 2){
                npcCopy = NPC3;
            }
            Instantiate(npcCopy,gameObject.transform.position,Quaternion.Euler(0, -90, 0));
            //Debug.Log("Spawned");
            yield return new WaitForSecondsRealtime(3);
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
