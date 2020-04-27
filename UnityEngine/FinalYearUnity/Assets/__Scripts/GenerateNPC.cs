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

    private GameObject NPCSpawnersContainer;

    private Transform[] NPCSpawners;

    //en-US-JessaNeural
    // de-DE-KatjaNeural
    //en-IE-Sean
    //en-US-GuyNeural

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        NPCSpawnersContainer = GameObject.Find("npc_spawners");

        NPCSpawners = NPCSpawnersContainer.GetComponentsInChildren<Transform>();

        // Empty game object container
        container = new GameObject("container");

        // List of all NPCS in the game to write scores to file.
        List<NPCList> list = new List<NPCList>();
        
        for (int i = 0; i < NPCSpawners.Length; i++)
        {
            // Instaniate a new person.
            Debug.Log("Which cube: "+ NPCSpawners[i].name);

            if(NPCSpawners[i].name != "npc_spawners"){
                GameObject copy = npc;
                copy.GetComponent<NPC>().SetVoice();
                Debug.Log("VOICENAME: " + copy.GetComponent<NPC>().GetVoiceName());
                string npcVoice = copy.GetComponent<NPC>().GetVoiceName();
                
                if (copy.GetComponent<NPC>().GetVoiceName() == "en-US-AriaNeural" || copy.GetComponent<NPC>().GetVoiceName() == "de-DE-KatjaNeural")
                {
                    int rand = UnityEngine.Random.Range(0, 2);

                    if(rand == 0)
                    {
                        copy = Instantiate(npc2, new Vector3(NPCSpawners[i].position.x, NPCSpawners[i].position.y, NPCSpawners[i].position.z), Quaternion.Euler(0, NPCSpawners[i].rotation.eulerAngles.y, 0));
                        copy.GetComponent<NPC>().SetVoice(npcVoice);
                        copy.transform.parent = container.transform;
                    }else if(rand == 1)
                    {
                        copy = Instantiate(npc3, new Vector3(NPCSpawners[i].position.x, NPCSpawners[i].position.y, NPCSpawners[i].position.z), Quaternion.Euler(0, NPCSpawners[i].rotation.eulerAngles.y, 0));
                        copy.GetComponent<NPC>().SetVoice(npcVoice);
                        copy.transform.parent = container.transform;
                    }
                }
                else
                {
                    copy = Instantiate(npc1, new Vector3(NPCSpawners[i].position.x, NPCSpawners[i].position.y, NPCSpawners[i].position.z), Quaternion.Euler(0, NPCSpawners[i].rotation.eulerAngles.y, 0));
                    copy.GetComponent<NPC>().SetVoice(npcVoice);
                    copy.transform.parent = container.transform;
                }

                // Instaniate completion indicator for player to know if a ticket has been checked or not.
                // Also, instaniate satisfaction meter for every npc, and set as child to NPC
                // Check which way the NPC is facing to determine where to place ring and meter.
                GameObject cr;
                GameObject sm;
                if (NPCSpawners[i].rotation.eulerAngles.y == 90){
                    cr = Instantiate(completionRing, new Vector3(NPCSpawners[i].position.x + 0.8f, 0.6f, NPCSpawners[i].position.z), Quaternion.identity);
                    sm = Instantiate(satisfactionMeter, new Vector3(NPCSpawners[i].position.x + 0.7f, 2.5f, NPCSpawners[i].position.z - 0.5f), Quaternion.identity);
                }
                else
                {
                    cr = Instantiate(completionRing, new Vector3(NPCSpawners[i].position.x - 0.8f, 0.6f, NPCSpawners[i].position.z), Quaternion.identity);
                    sm = Instantiate(satisfactionMeter, new Vector3(NPCSpawners[i].position.x - 0.7f, 2.5f, NPCSpawners[i].position.z - 0.5f), Quaternion.identity);
                }

                // Rotate both ring and meter correctly and set parent container.
                cr.transform.Rotate(90, 0, 0);
                cr.transform.parent = copy.transform;
                sm.transform.Rotate(0, 180, 0);
                sm.transform.parent = copy.transform;

                // Set meter to false, so invisible until the player interacts with the NPC
                sm.SetActive(false);
                
                // Get the sript from the new instaniated object.
                NPC npcScript = copy.GetComponent<NPC>();

                // Initialise and add new NPC to list to be writen to file.
                list.Add(InitialiseNPCObject(npcScript));

                // Finally increase or decrease persona meter depending on the type of person (0 = rude, 1 = neutral, 2 = polite)
                SatisfactionMeter smScript = sm.GetComponentInChildren<SatisfactionMeter>();

                if (npcScript.GetPersona() == 0) {
                    smScript.DecreaseSatifaction();
                    smScript.DecreaseSatifaction();
                }
                else if (npcScript.GetPersona() == 2) {
                    smScript.IncreaseSatifaction();
                    smScript.IncreaseSatifaction();
                }
            }
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
        npcScript.SetHasTicket();
        
        //if (npcScript.GetVoiceName() == null)
        //npcScript.SetVoice();
        person.sessionId = npcScript.GetSessionID();
        person.voiceName = npcScript.GetVoiceName();

        return person;
    }
}
