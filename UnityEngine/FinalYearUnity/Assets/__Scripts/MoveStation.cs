using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStation : MonoBehaviour
{
    private bool onTrain;
    private float speed;
    void Start()
    {
        onTrain = false;
        speed = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if(onTrain == false){
            onTrain = GameObject.Find("ColStick").GetComponent<PersonCollider>().onTrain;
        }

        if(onTrain == true){
           // Debug.Log("Move Train");
            
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed, gameObject.transform.position.y, gameObject.transform.position.z);

        }
    }

    public void StopMoving(){
        speed = 0.0f;
    }
}
