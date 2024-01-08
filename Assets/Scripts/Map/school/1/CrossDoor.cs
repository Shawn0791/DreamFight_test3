using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossDoor : MonoBehaviour
{
    public GameObject wall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wall.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wall.SetActive(true);
        }
    }

}
