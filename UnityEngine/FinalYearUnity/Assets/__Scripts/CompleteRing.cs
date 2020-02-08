using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteRing : MonoBehaviour
{
    public Material green;

    public void ticketChecked()
    {
        // Code adapted from: https://answers.unity.com/questions/13356/how-can-i-assign-materials-using-c-code.html
        Renderer rend = GetComponent<Renderer>();

        if (rend != null)
        {
            rend.material = green;
        }
    }
}
