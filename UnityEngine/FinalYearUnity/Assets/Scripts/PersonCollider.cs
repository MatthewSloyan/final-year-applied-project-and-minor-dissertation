using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCollider : MonoBehaviour
{
    SpeechToText speech;

    // Start is called before the first frame update
    void Start()
    {
        speech = new SpeechToText();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("person"))
        {
            Debug.Log("awake");
            var renderer = col.gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.blue);
            SpeechToText.IsPersonActive = true;
            
            speech.ButtonClick();
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
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
