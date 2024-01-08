using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMapSystem : MonoBehaviour
{

    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }


    void Update()
    {
        
    }
}
