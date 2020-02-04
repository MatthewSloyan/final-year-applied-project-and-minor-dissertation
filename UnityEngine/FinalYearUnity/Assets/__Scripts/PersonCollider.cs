using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCollider : MonoBehaviour
{
    private SpeechToText speech;

    // Start is called before the first frame update
    void Start()
    {
        speech = SpeechToText.Instance;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("person"))
        {
            Debug.Log("awake");
            var renderer = col.gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.blue);
            SpeechToText.IsPersonActive = true;

            GameObject temp = col.gameObject.transform.Find("SatisfactionMeterContainer(Clone)").gameObject;
            temp.SetActive(true);

            NPC npc = col.gameObject.GetComponent<NPC>();
            speech.convertSpeechToText(npc.GetSessionID(),npc.GetPersona(), npc.GetVoiceName());
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("person"))
        {
            var renderer = col.gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.white);
            SpeechToText.IsPersonActive = false;
            Debug.Log("exit");

            GameObject temp = col.gameObject.transform.Find("SatisfactionMeterContainer(Clone)").gameObject;
            temp.SetActive(false);
        }
    }
}
