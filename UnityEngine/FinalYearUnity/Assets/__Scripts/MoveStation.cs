using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that moves the enviroment around the train to give the illusion that the train is moving
public class MoveStation : MonoBehaviour
{
    private bool onTrain;
    private float speed;
    void Start()
    {
        onTrain = false;
        speed = 0.25f;
    }

    // Moves the station or City chunk when on the train
    void Update()
    {
        if(onTrain == false){
            onTrain = GameObject.Find("ColStick").GetComponent<PersonCollider>().onTrain;
        }

        if(onTrain == true){
            
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed, gameObject.transform.position.y, gameObject.transform.position.z);

        }
    }

    //Stops Station or city chunks from moving when the train re-enters station
    public void StopMoving(){
        speed = 0.0f;
    }
}
