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
            //gameObject.transform.localEulerAngles.z <= 76
            if (satifaction > 1)
            {
                transform.Rotate(0f, 0f, 38);
                satifaction--;
                Debug.Log(satifaction);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (satifaction < 5)
            {
                transform.Rotate(0f, 0f, -38);
                satifaction++;
                Debug.Log(satifaction);
            }
        }
    }
}
