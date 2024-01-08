using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera_1 : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
