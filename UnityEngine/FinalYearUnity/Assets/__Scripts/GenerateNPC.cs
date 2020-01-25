using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNPC : MonoBehaviour
{
    public GameObject npc;
    private GameObject container;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        // Empty game object container
        container = new GameObject("container");

        // List of all NPCS in the game to write scores to file.
        List<NPCS> list = new List<NPCS>();
        
        for (int i = -8; i < -4; i += 2)
        {
            //container.transform.parent = npc.transform;
            //
            GameObject copy = Instantiate(npc, new Vector3(5, 1, i), Quaternion.identity);
            copy.transform.parent = container.transform;

            // Create a new NPCS object to add to list.
            NPCS person = new NPCS();
            person.score = 0;
            person.sessionId = copy.GetComponent<NPC>().GetSessionID();
            person.voiceName = "en - US - JessaNeural";

            list.Add(person);
        }

        // Add list to Game object and write JSON to text file.
        Game game = new Game();
        game.gameId = Random.Range(1, 1000);
        game.npcs = list;

        new ScoreFileManager().WriteScoreFile(new Utilities().ToJsonString(game));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
