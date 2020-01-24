using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{

    public int sessionId;

    private void Start()
    {
        sessionId = Random.Range(1, 10000);
    }

    public int GetSessionID()
    {
        return sessionId;
    }

}
