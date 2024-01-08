using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossDoor1 : MonoBehaviour
{
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wall1.SetActive(true);
            wall2.SetActive(true);
            wall3.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wall1.SetActive(false);
            wall2.SetActive(false);
            wall3.SetActive(false);
        }
    }
}
