using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNPC : MonoBehaviour
{
    public GameObject npc;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;

    public GameObject satisfactionMeter;
    public GameObject completionRing;
    private GameObject container;
    //en-US-JessaNeural
    // de-DE-KatjaNeural
    //en-IE-Sean
    //en-US-GuyNeural

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        // Empty game object container
        container = new GameObject("container");

        // List of all NPCS in the game to write scores to file.
        List<NPCList> list = new List<NPCList>();
        
        for (int i = -4; i < 4; i += 2)
        {
            // Instaniate a new person.
            
            GameObject copy = npc;
            copy.GetComponent<NPC>().SetVoice();
            Debug.Log("VOICENAME: " + copy.GetComponent<NPC>().GetVoiceName());
            string npcVoice = copy.GetComponent<NPC>().GetVoiceName();
            
            if (copy.GetComponent<NPC>().GetVoiceName() == "en-US-JessaNeural" || copy.GetComponent<NPC>().GetVoiceName() == "de-DE-KatjaNeural")
            {
                int rand = UnityEngine.Random.Range(0, 2);

                if(rand == 0)
                {
                    copy = Instantiate(npc2, new Vector3(5, 0, i), Quaternion.Euler(0, -90, 0));
                    copy.GetComponent<NPC>().SetVoice(npcVoice);
                    copy.transform.parent = container.transform;
                }else if(rand == 1)
                {
                    copy = Instantiate(npc3, new Vector3(5, 0, i), Quaternion.Euler(0, -90, 0));
                    copy.GetComponent<NPC>().SetVoice(npcVoice);
                    copy.transform.parent = container.transform;
                }

            }
            else
            {
                copy = Instantiate(npc1, new Vector3(5, 0, i), Quaternion.Euler(0, -90, 0));
                copy.GetComponent<NPC>().SetVoice(npcVoice);
                copy.transform.parent = container.transform;
            }



            // Instaniate satisfaction meter for every npc, and set as child to NPC
            GameObject sm = Instantiate(satisfactionMeter, new Vector3(6, 2.3f, i), Quaternion.identity);
            sm.transform.Rotate(0, 90, 0);
            sm.transform.parent = copy.transform;

            // Instaniate completion indicator for player to know if a ticket has been checked or not.
            GameObject cr = Instantiate(completionRing, new Vector3(5, 0, i), Quaternion.identity);
            cr.transform.Rotate(90, 0, 0);
            cr.transform.parent = copy.transform;

            // Set meter to false, so invisible until the player interacts with the NPC
            sm.SetActive(false);
            
            // Get the sript from the new instaniated object.
            NPC npcScript = copy.GetComponent<NPC>();

            // Initialise and add new NPC to list to be writen to file.
            list.Add(InitialiseNPCObject(npcScript));
        }

        // Add list to Game object and write JSON to text file.
        Game game = new Game();
        game.gameId = UnityEngine.Random.Range(1, 10000);
        game.npcs = list;

        // Write object to file as JSON.
        new ScoreFileManager().WriteScoreFile(new Utilities().ToJsonString(game));
    }

    private NPCList InitialiseNPCObject(NPC npcScript)
    {
        // Create a new NPC List object to add data to, this is then added to the game object outside the loop.
        NPCList person = new NPCList();
        person.score = 0;
        person.complete = false;
        
        // Set and get NPC data, I tried by just getting the component from the copy gameObject but the values were null.
        
        npcScript.SetSessionId();
        npcScript.SetPersona();
        //if (npcScript.GetVoiceName() == null)
        //npcScript.SetVoice();
        person.sessionId = npcScript.GetSessionID();
        person.voiceName = npcScript.GetVoiceName();

        return person;
    }
}
