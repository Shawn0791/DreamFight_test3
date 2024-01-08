using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestAndCave : MonoBehaviour
{
    public GameObject ForestBG;
    public GameObject CaveBG;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.y > -19)
            {
                ForestBG.SetActive(false);
                CaveBG.SetActive(true);
            }
            else
            {
                ForestBG.SetActive(true);
                CaveBG.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.y > -19)
            {
                ForestBG.SetActive(true);
                CaveBG.SetActive(false);
            }
            else
            {
                ForestBG.SetActive(false);
                CaveBG.SetActive(true);
            }
        }
    }
}
