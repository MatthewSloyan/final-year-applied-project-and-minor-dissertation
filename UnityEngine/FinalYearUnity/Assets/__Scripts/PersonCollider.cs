using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCollider : MonoBehaviour
{
    private string[] replies = { "You've already checked my ticket, piss off!", "You've already checked my ticket.", "Sorry, but you have checked my ticket."};
    public bool onTrain = false;
    private bool startDeleting;
    private Queue chunks;

    private int chunkCount;
    void Start(){
        chunks = new Queue();
        startDeleting = false;
        chunkCount = -1;
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
            Debug.Log("Next CHunk");
            GameObject newChunk = GameObject.Find("CityChunkContainer");
            chunks.Enqueue(Instantiate(newChunk,new Vector3(col.gameObject.transform.position.x-272.5f,col.gameObject.transform.position.y-78.25f,col.gameObject.transform.position.z+24.75f),Quaternion.Euler(0, 0, 0)));
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
        }
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
