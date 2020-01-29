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
            IncreaseSatifaction();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DecreaseSatifaction();
        }
    }

    public void IncreaseSatifaction()
    {
        if (satifaction > 1)
        {
            transform.Rotate(0f, 0f, 38);
            satifaction--;
            Debug.Log(satifaction);
        }
    }

    public void DecreaseSatifaction()
    {
        if (satifaction < 5)
        {
            transform.Rotate(0f, 0f, -38);
            satifaction++;
            Debug.Log(satifaction);
        }
    }
}
