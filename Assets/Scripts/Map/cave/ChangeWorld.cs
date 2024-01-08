using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWorld : MonoBehaviour
{
    public void ChangeTo2D()
    {
        //2.5D变2D
        transform.Find("2D").gameObject.SetActive(true);
        transform.Find("2.5D").gameObject.SetActive(false);
    }
}
