using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    #region == Private Variables == 
    [SerializeField]
    private Toggle soundToggle;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Add listener for when the state of the Toggle changes, to take action
        // Code adapted from: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Toggle-onValueChanged.html
        soundToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(soundToggle);
        });

        // Check if sound option has been saved before.
        // E.g if not the first time playing the game, or sound has never been turned off.
        // If so then change toggle switch to false if sound is off.
        // If not then set playerPref to true, for again.
        if (PlayerPrefs.HasKey("Sound"))
        {
            bool toggle = Convert.ToBoolean(PlayerPrefs.GetString("Sound"));

            if (!toggle)
            {
                soundToggle.isOn = false;
            }
        }
        else
        {
            PlayerPrefs.SetString("Sound", soundToggle.isOn.ToString());
        }
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        PlayerPrefs.SetString("Sound", soundToggle.isOn.ToString());
    }
}
