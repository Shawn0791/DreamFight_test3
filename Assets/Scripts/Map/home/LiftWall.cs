using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            transform.Find("Wall").gameObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            transform.Find("Wall").gameObject.SetActive(false);
    }
}
