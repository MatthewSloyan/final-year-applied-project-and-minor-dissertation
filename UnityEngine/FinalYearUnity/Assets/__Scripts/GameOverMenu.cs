using UnityEngine;
using UnityEngine.UI;

//Class that controls the GameOver menu
public class GameOverMenu : MonoBehaviour
{
    // List of images used to store final npc profile pictures on game over menu.
    public Sprite[] images;

    void Start()
    {
        // Turn off game over menu on start.
        gameObject.GetComponent<Canvas>().enabled = false;
    }
    
    #region == Menu overlays (Pause/Gameover)
    
    // Closes any UI gameobject.
    public void GameOverUI()
    {
        // Turn back on menu.
        gameObject.GetComponent<Canvas>().enabled = true;

        // Get json string from file.
        string json = new ScoreFileManager().LoadScoreFile();

        // Convert string to object. Object returned has the updated score.
        Game game = new Utilities().ToObject<Game>(json);

        // Update time and score on menu.
        Text t = GameObject.Find("TimeGameOver").gameObject.GetComponent<Text>();
        t.text = game.gameTime;

        Text s = GameObject.Find("ScoreGameOver").gameObject.GetComponent<Text>();
        s.text = "Score: " + game.gameScore.ToString();

        // Get the Scroll list, to add npc data to.
        GameObject grid = GameObject.FindGameObjectWithTag("list");
        float position = 0f;

        // Loop through all NPC's in the game, allows for any number.
        foreach (var npc in game.npcs)
        {
            // Get and instaniate a new image and text game object container. 
            // This is a template resource that will be updated below.
            GameObject temp = Resources.Load("Container") as GameObject;
            GameObject container = Instantiate(temp, new Vector3(temp.transform.position.x, temp.transform.position.y - position, temp.transform.position.z), temp.transform.rotation) as GameObject;

            // Get the image depending on the voice.
            int imageIndex;

            switch (npc.voiceName)
            {
                case "en-US-GuyNeural":
                    imageIndex = 0;
                    break;
                case "en-GB-George-Apollo":
                    imageIndex = 0;
                    break;
                case "en-US-AriaNeural":
                    imageIndex = 1;
                    break;
                case "de-DE-KatjaNeural":
                    imageIndex = 2;
                    break;
                default:
                    imageIndex = 3;
                    break;
            }

            // Update the image in the template container, using the determined image.
            container.transform.GetChild(0).GetComponent<Image>().sprite = images[imageIndex];

            // Update the text in the template container using the score from file.
            container.transform.GetChild(1).transform.GetComponent<Text>().text = "Rating: " + npc.score;
            container.transform.SetParent(grid.transform, false);

            position += 45;
        }
    }
    #endregion
}
