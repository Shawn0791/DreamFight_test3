using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject dialogueBox;

    public Text characterNameText;
    public Text dialogueLiineText;
    public GameObject pressE;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleDialogueBox(bool _isActive)
    {
        dialogueBox.gameObject.SetActive(_isActive);
    }

    public void TogglePressE(bool _isActive)
    {
        pressE.gameObject.SetActive(_isActive);
    }

    public void SetupDialogue(string _name,string _line,int _size)
    {
        characterNameText.text = _name;
        dialogueLiineText.text = _line;
        dialogueLiineText.fontSize = _size;

        ToggleDialogueBox(true);
    }
}
