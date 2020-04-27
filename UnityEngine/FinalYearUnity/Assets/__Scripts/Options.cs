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
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        if (soundToggle.isOn.ToString() == "True"){
            AudioController.Instance.turnSoundOnOff(1);
        }
        else {
            AudioController.Instance.turnSoundOnOff(0);
        }
    }
}
