using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public GameObject button;
    void Start()
    {
        
    }

    void Update()
    {
        button.GetComponent<Button>().Select();
    }
}
