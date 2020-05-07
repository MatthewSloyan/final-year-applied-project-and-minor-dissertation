using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatisfactionMeter : MonoBehaviour
{
    private int satifaction;

    void Start()
    {
        // Set initial satisfaction to 3.
        satifaction = 3;
    }

    // Decreases satisfaction arrow by one on each call.
    public void DecreaseSatifaction()
    {
        transform.Rotate(0f, 0f, 38);
        satifaction--;
    }

    // Increases satisfaction arrow by one on each call.
    public void IncreaseSatifaction()
    {
        if (satifaction < 5)
        {
            transform.Rotate(0f, 0f, -38);
            satifaction++;
        }
    }
}
