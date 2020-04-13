using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("watch"))
        {
            
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("watch"))
        {
           
        }
    }
}
