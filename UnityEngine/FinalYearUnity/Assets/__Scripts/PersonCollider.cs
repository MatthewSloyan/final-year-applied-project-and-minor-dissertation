using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCollider : MonoBehaviour
{
    private SpeechToText speech;
    private string tempColour;

    // Start is called before the first frame update
    void Start()
    {
        speech = SpeechToText.Instance;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("person"))
        {
            CompleteRing cr = col.gameObject.GetComponentInChildren<CompleteRing>();

            // Get initial colour
            tempColour = cr.getCurrentColour();

            // Change ring colour to blue to to signify that the player can talk to the NPC.
            cr.changeRingColour("Blue");

            SpeechToText.IsPersonActive = true;

            // Find satisfaction meter gameobject and set to active so it's displayed.
            GameObject temp = col.gameObject.transform.Find("SatisfactionMeterContainer(Clone)").gameObject;
            temp.SetActive(true);

            // Get the NPC script attached to NPC and start to listen for speech.
            NPC npc = col.gameObject.GetComponent<NPC>();
            speech.convertSpeechToText(npc.GetSessionID(),npc.GetPersona(), npc.GetVoiceName());
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
