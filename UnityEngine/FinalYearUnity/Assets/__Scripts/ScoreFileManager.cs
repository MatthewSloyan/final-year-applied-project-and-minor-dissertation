using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreFileManager
{
    // Helper methods, used to write a json string to file.
    public void WriteScoreFile(string json)
    {
        File.WriteAllText(getPath(), json);
    }

    // Helper method to load in json file from memory.
    public string LoadScoreFile()
    {
        return File.ReadAllText(getPath());
    }

    // Get the path to the score file in memory.
    private string getPath()
    {
        return Application.persistentDataPath + "/Score.txt";
    }
}
