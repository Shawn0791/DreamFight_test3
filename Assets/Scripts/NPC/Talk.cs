using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    private GameObject talkable;
    private GameObject panel;

    private void Start()
    {
        talkable = transform.Find("talkable").gameObject;
        panel = transform.Find("Canvas").transform.Find("Panel").gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            talkable.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogSystem.index = 0;
            talkable.SetActive(false);
            panel.SetActive(false);
        }
    }
    void Update()
    {
        if (talkable.activeSelf && Input.GetKeyDown(KeyCode.E))
            panel.SetActive(true);
    }
}
