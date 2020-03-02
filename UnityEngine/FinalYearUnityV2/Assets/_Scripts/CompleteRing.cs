using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteRing : MonoBehaviour
{
    public Material green;
    public Material blue;
    public Material red;

    public void changeRingColour(string colour)
    {
        // Code adapted from: https://answers.unity.com/questions/13356/how-can-i-assign-materials-using-c-code.html
        Renderer rend = GetComponent<Renderer>();

        if (colour == "Green" && rend != null)
        {
            rend.material = green;
        }
        else if (colour == "Blue" && rend != null)
        {
            rend.material = blue;
        }
        else
        {
            rend.material = red;
        }
    }

    public string getCurrentColour()
    {
        Renderer rend = GetComponent<Renderer>();

        return rend.material.name.Replace(" (Instance)", "");
    }
}
