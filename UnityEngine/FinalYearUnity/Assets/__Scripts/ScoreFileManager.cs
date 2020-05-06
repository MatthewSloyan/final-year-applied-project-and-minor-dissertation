using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreFileManager
{
    public void WriteScoreFile(string json)
    {
        File.WriteAllText(getPath(), json);
    }

    public string LoadScoreFile()
    {
        return File.ReadAllText(getPath());
    }

    private string getPath()
    {
        return Application.persistentDataPath + "/Score.txt";
    }
}
