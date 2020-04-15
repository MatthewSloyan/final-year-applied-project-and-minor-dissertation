using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCollider : MonoBehaviour
{
    private string[] replies = { "You've already checked my ticket, piss off!", "You've already checked my ticket.", "Sorry, but you have checked my ticket."};
    public bool onTrain = false;
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

                SpeechToText.Instance.convertSpeechToText(npc.GetSessionID(), npc.GetPersona(), npc.GetVoiceName());
            }
        }
        
        if(col.gameObject.CompareTag("OnTrain")){
            Debug.Log("OnTrain");
            //GameObject.Find("StationContainer").GetComponent<Animator>().SetBool("OnTrain",true);
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
                Destroy(chunks.Dequeue() as GameObject);
                
            }

        }
        if(col.gameObject.CompareTag("StopTrain") && stopSpawning == true){
            station.GetComponent<MoveStation>().StopMoving();

            foreach (GameObject chunk in chunks)
            {
                chunk.GetComponent<MoveStation>().StopMoving();
            }

            newChunk.GetComponent<MoveStation>().StopMoving();

        }
    }

    public void GameComplete(){
        stopSpawning = true;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("person"))
        {
            //var renderer = col.gameObject.GetComponent<Renderer>();
            //renderer.material.SetColor("_Color", Color.white);

            // Check if colour is green.
            string colour = col.gameObject.GetComponentInChildren<CompleteRing>().getCurrentColour();
            //Debug.Log(colour);
            //Debug.Log(tempColour);

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
