using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    void Start()
    {
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

        Debug.Log(json);

        // Convert string to object. Object returned has the updated score.
        Game game = new Utilities().ToObject<Game>(json);

        Text t = GameObject.Find("TimeGameOver").gameObject.GetComponent<Text>();
        t.text = game.gameTime;

        Text s = GameObject.Find("ScoreGameOver").gameObject.GetComponent<Text>();
        s.text = "Score: " + game.gameScore.ToString();

        // Scroll list
        GameObject grid = GameObject.FindGameObjectWithTag("list");
        float position = 0f;

        foreach (var npc in game.npcs)
        {
            GameObject temp = Resources.Load("Container") as GameObject;
            GameObject container = Instantiate(temp, new Vector3(temp.transform.position.x, temp.transform.position.y - position, temp.transform.position.z), temp.transform.rotation) as GameObject;

            // Get the filename depending on the voice.
            string fileName;

            switch (npc.voiceName)
            {
                case "en-US-GuyNeural":
                    fileName = "male_1.png";
                    break;
                case "en-IE-Sean":
                    fileName = "male_1.png";
                    break;
                case "en-US-JessaNeural":
                    fileName = "female_1.png";
                    break;
                case "de-DE-KatjaNeural":
                    fileName = "female_2.png";
                    break;
                default:
                    fileName = "default";
                    break;
            }

            Debug.Log(fileName);

            Sprite image = Resources.Load("Images/" + fileName) as Sprite;
            container.transform.GetChild(0).GetComponent<Image>().sprite = image;

            container.transform.GetChild(1).transform.GetComponent<Text>().text = "Rating: " + npc.score;
            container.transform.SetParent(grid.transform, false);

            position += 45;
        }
    }
    #endregion
}
