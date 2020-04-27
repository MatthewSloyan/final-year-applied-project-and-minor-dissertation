using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatisfactionMeter : MonoBehaviour
{
    private int satifaction;

    void Start()
    {
        satifaction = 3;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DecreaseSatifaction();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreaseSatifaction();
        }
    }

    public void DecreaseSatifaction()
    {
        transform.Rotate(0f, 0f, 38);
        satifaction--;
    }

    public void IncreaseSatifaction()
    {
        if (satifaction < 5)
        {
            transform.Rotate(0f, 0f, -38);
            satifaction++;
        }
    }
}
