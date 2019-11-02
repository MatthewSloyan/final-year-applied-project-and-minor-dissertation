using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    #region == Private Variables == 
    [SerializeField]
    private InputField m_TextInput;
    #endregion
    
    public void sendMessage()
    {
        if (m_TextInput.text != null)
        {
            Debug.Log(m_TextInput.text);
        }
    }
}
