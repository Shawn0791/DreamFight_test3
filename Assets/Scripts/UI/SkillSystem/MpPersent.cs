using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpPersent : MonoBehaviour
{
    public GameObject HPandMP;

    private Text controlPersent;

    private void Start()
    {
        controlPersent = HPandMP.transform.Find("ControlPersent").GetComponent<Text>();
    }
    void Update()
    {
        GetComponent<Text>().text = controlPersent.text;
    }
}
