using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySky : MonoBehaviour
{
    public GameObject YellowSky;
    public GameObject BlackSky;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.x < -27)
            {
                YellowSky.SetActive(true);
                BlackSky.SetActive(false);
            }
            else
            {
                YellowSky.SetActive(false);
                BlackSky.SetActive(true);
            }
        }
    }
}
