using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNPC : MonoBehaviour
{
    public GameObject npc;
    public GameObject satisfactionMeter;
    private GameObject container;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        // Empty game object container
        container = new GameObject("container");

        // List of all NPCS in the game to write scores to file.
        List<NPCList> list = new List<NPCList>();
        
        for (int i = -8; i < 0; i += 2)
        {
            // Instaniate a new person.
            GameObject copy = Instantiate(npc, new Vector3(5, 1, i), Quaternion.identity);
            copy.transform.parent = container.transform;

            // Instaniate satisfaction meter for every npc, and set as child to NPC
            GameObject sm = Instantiate(satisfactionMeter, new Vector3(6, 2.3f, i), Quaternion.identity);
            sm.transform.Rotate(0, 90, 0);
            sm.transform.parent = copy.transform;

            // Set meter to false, so invisible until the player interacts with the NPC
            sm.SetActive(false);

            // Create a new NPC List object to add data to, this is then added to the game object outside the loop.
            NPCList person = new NPCList();
            person.score = 0;

            // Get the sript from the new instaniated object.
            NPC npcScript = copy.GetComponent<NPC>();
            // Set and get NPC data, I tried by just getting the component from the copy gameObject but the values were null.
            npcScript.SetSessionId();
            npcScript.SetVoice();
            person.sessionId = npcScript.GetSessionID();
            person.voiceName = npcScript.GetVoiceName();

            list.Add(person);
        }

        // Add list to Game object and write JSON to text file.
        Game game = new Game();
        game.gameId = Random.Range(1, 10000);
        game.npcs = list;

        // Write object to file as JSON.
        new ScoreFileManager().WriteScoreFile(new Utilities().ToJsonString(game));
    }
}
