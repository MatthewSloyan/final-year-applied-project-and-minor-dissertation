using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PersonCollider : MonoBehaviour
{
    private string[] replies = { "You've already checked my ticket, piss off!", "You've already checked my ticket.", "Sorry, but you have checked my ticket."};
    public bool onTrain = false;
    public bool playOnce = false;
    private bool startDeleting;
    private Queue chunks;
    public bool stopSpawning;
    private int chunkCount;
    private GameObject newChunk;
    private GameObject station;
    void Start(){
        stopSpawning = false;
        chunks = new Queue();
        startDeleting = false;
        chunkCount = 0;
        station = GameObject.Find("StationHolder");
        newChunk = GameObject.Find("CityChunkContainer");
    } 

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("person"))
        {
            TextToSpeech.Collision = col;

            try
            {
                if (Math.Round(col.gameObject.transform.rotation.eulerAngles.y, 0) == 90){
                   col.gameObject.GetComponent<Animator>().SetTrigger("TurnHeadLeft");
                }
                else
                {
                   col.gameObject.GetComponent<Animator>().SetTrigger("TurnHeadRight");
                }
            }
            catch{}

            // Get the NPC script attached to NPC and start to listen for speech.
            NPC npc = col.gameObject.GetComponent<NPC>();

            // Change ring colour to blue to to signify that the player can talk to the NPC.
            CompleteRing cr = col.gameObject.GetComponentInChildren<CompleteRing>();

            // Find satisfaction meter gameobject and set to active so it's displayed.
            GameObject temp = col.gameObject.transform.Find("SatisfactionMeterContainer(Clone)").gameObject;
            temp.SetActive(true);

            SpeechToText.IsPersonActive = true;

            // Check if ticket has been checked or not.
            if (cr.getCurrentColour() == "Green")
            {
                TextToSpeech.Instance.ConvertTextToSpeech(replies[npc.GetPersona()], npc.GetVoiceName(), true);
            }
            else
            {
                cr.changeRingColour("Blue");

                SpeechToText.Instance.convertSpeechToText(npc);
            }
        }
        
        if(col.gameObject.CompareTag("OnTrain")){

            if (playOnce == false){
                AudioController.Instance.PlayAudioOnce(2);
                playOnce = true;
            }

            AudioController.Instance.PlayAudio(1);
            Debug.Log("OnTrain");
            GameObject.Find("SlidingDoors").GetComponent<Animator>().SetBool("ontrain",true);
            onTrain = true;
        }
        
        if(col.gameObject.CompareTag("NextChunk")){
            if(stopSpawning == false){
                Debug.Log("Next CHunk");
                
                chunks.Enqueue(Instantiate(newChunk,new Vector3(col.gameObject.transform.position.x-190f,col.gameObject.transform.position.y-78.25f,col.gameObject.transform.position.z+24.75f),Quaternion.Euler(0, 0, 0)));
                //Instantiate(newChunk,new Vector3(col.gameObject.transform.position.x-272.5f,col.gameObject.transform.position.y-78.25f,col.gameObject.transform.position.z+24.75f),Quaternion.Euler(0, 0, 0));
                
                if(startDeleting == false){
                    if(chunkCount == 2){

                        startDeleting = true;

                    }else{
                        chunkCount++;
                    }
                }

                if(startDeleting == true){
                    Destroy(chunks.Dequeue() as GameObject);
                }
            }else{
                Debug.Log("SPAWNING STOPPED");
                
                station.transform.position = new Vector3(col.gameObject.transform.position.x-100f,station.transform.position.y,station.transform.position.z);
                //Destroy(chunks.Dequeue() as GameObject);
                
            }

        }
        if(col.gameObject.CompareTag("StopTrain") && stopSpawning == true){
            station.GetComponent<MoveStation>().StopMoving();

            foreach (GameObject chunk in chunks)
            {
                chunk.GetComponent<MoveStation>().StopMoving();
            }

            newChunk.GetComponent<MoveStation>().StopMoving();
            GameObject.Find("SlidingDoors").GetComponent<Animator>().SetBool("ontrain",false);
        }
    }

    public void GameComplete(){
        stopSpawning = true;
        //GameObject.Find("Audio Source").GetComponent<AudioController>().PlayAudioOnce(1);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("person"))
        {
            // Check if colour is green.
            string colour = col.gameObject.GetComponentInChildren<CompleteRing>().getCurrentColour();

            if (colour != "Green")
            {
                // Change ring colour back to initial colour when player walks away from NPC.
                col.gameObject.GetComponentInChildren<CompleteRing>().changeRingColour("Red");
            }

            SpeechToText.IsPersonActive = false;

            // Find satisfaction meter gameobject and set to not active so it's hidden.
            GameObject temp = col.gameObject.transform.Find("SatisfactionMeterContainer(Clone)").gameObject;
            temp.SetActive(false);
        }
    }
}
