using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_4_1MeetScarer : MonoBehaviour
{
    public GameObject talkUI;
    public GameObject sprouter;

    private bool enter;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            talkUI.SetActive(true);
            enter = true;

            //切断人物控制
            GameManager.instance.gameMode = GameManager.GameMode.DialogueMoment;
            //立刻静止为idle状态
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("isMoving", false);
        }
    }

    private void Update()
    {
        if (enter)
        {
            if (talkUI.activeSelf == false)
            {
                sprouter.GetComponent<Sprouter>().findTarget = true;

                this.gameObject.SetActive(false);
            }
        }
    }
}
