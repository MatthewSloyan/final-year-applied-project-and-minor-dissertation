using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPaths : MonoBehaviour
{

    public Transform[] target;
    public Transform[] target1;
    public float speed;

    private int current;
    private int lastNode;
    void Start()
    {
        current = 1;
        GameObject container = GameObject.Find("NPC1targets");
        target1 = container.GetComponentsInChildren<Transform>();

        foreach (var item in target1)
        {
            Debug.Log(item.name);
        }

        lastNode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastNode < target1.Length-1){
            if(transform.position != target1[current].position ){

                Vector3 pos = Vector3.MoveTowards(transform.position,target1[current].position,speed * Time.deltaTime);

                GetComponent<Rigidbody>().MovePosition(pos);

            }else{
                current = (current+1) % target1.Length;
                lastNode++;
                Debug.Log("Node" + lastNode);
            }
        }
    }
}
