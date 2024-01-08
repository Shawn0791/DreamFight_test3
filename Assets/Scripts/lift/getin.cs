using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getin : MonoBehaviour
{
    public GameObject doorleft, doorright;
    public GameObject wall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        doorleft.SetActive(false);
        doorright.SetActive(false);
        wall.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        doorleft.SetActive(true);
        doorright.SetActive(true);
        wall.SetActive(true);
    }
}
