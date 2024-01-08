using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goingdown : MonoBehaviour
{
    public GameObject wall1,wall2,wall3,wall4,wall5,wall6;
    //public GameObject wall_U1, wall_U2, wall_U3, wall_U4, wall_U5, wall_U6;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wall1.SetActive(true);
            wall2.SetActive(true);
            wall3.SetActive(true);
            wall4.SetActive(true);
            wall5.SetActive(true);
            wall6.SetActive(true);

            /*wall_U1.SetActive(true);
            wall_U2.SetActive(true);
            wall_U3.SetActive(true);
            wall_U4.SetActive(true);
            wall_U5.SetActive(true);
            wall_U6.SetActive(true);*/
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wall1.SetActive(false);
            wall2.SetActive(false);
            wall3.SetActive(false);
            wall4.SetActive(false);
            wall5.SetActive(false);
            wall6.SetActive(false);

            /*wall_U1.SetActive(false);
            wall_U2.SetActive(false);
            wall_U3.SetActive(false);
            wall_U4.SetActive(false);
            wall_U5.SetActive(false);
            wall_U6.SetActive(false);*/
        }
    }
}
