using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPaths : MonoBehaviour
{

    //public Transform[] target;
    public Transform[] target;
    public float speed;

    private int current;
    private int lastNode;
    GameObject path;

    //Sets which path the NPC takes
    void Start()
    {
        
        int rand = Random.Range(0,2);
        if(rand == 1){
            path = GameObject.Find("path1");
            target = path.GetComponentsInChildren<Transform>();
        }else{
            path = GameObject.Find("path2");
            target = path.GetComponentsInChildren<Transform>();
        }

        lastNode = 0;
        current = 1;
    }

    // In the Start screen this update method moves NPCs towards the next object in the path
    void Update()
    {
        if(lastNode < target.Length-1){
            if(transform.position != target[current].position ){

                Vector3 pos = Vector3.MoveTowards(transform.position,target[current].position,speed * Time.deltaTime);

                GetComponent<Rigidbody>().MovePosition(pos);

            }else{
                current = (current+1) % target.Length;
                lastNode++;
            }
        }else{
            if(!gameObject.name.Equals("NPC_NoDestroy") && !gameObject.name.Equals("NPC2_NoDestroy")&& !gameObject.name.Equals("NPC3_NoDestroy")){
                Destroy(gameObject);
            }
            
        }
    }
}
