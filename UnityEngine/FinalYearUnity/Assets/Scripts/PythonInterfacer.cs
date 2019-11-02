using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using Debug = UnityEngine.Debug;

public class PythonInterfacer : MonoBehaviour
{
    void Start()
    {
        var psi = new ProcessStartInfo();
        // point to python virtual env
        psi.FileName = @"C:\Users\aaron\Anaconda3\python.exe";

        // Provide arguments
        var script = @"C:\Users\aaron\OneDrive\Desktop\Final-Year-Applied-Project-And-Minor-Dissertation\Python Client\Client.py";
        var vidFileIn = "";
        var inputPath = @"";
        var outputPath = @"";

        psi.Arguments = string.Format("\"{0}\" -v \"{1}\" -i \"{2}\" -o \"{3}\"",
                                       script, vidFileIn, inputPath, outputPath);

        // Process configuration
        psi.UseShellExecute = false;
        psi.CreateNoWindow = false;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        // Execute process and get output
        var errors = "nothing";
        var results = "nothing";

        using (var process = Process.Start(psi))
        {
            errors = process.StandardError.ReadToEnd();
            results = process.StandardOutput.ReadToEnd();
        }

        Debug.Log("RUNNING");
        Debug.Log(results.ToString());
    }
}