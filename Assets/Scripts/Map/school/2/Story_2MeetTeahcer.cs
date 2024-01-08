using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_2MeetTeahcer : MonoBehaviour
{
    public GameObject teacher;
    public GameObject talkUI;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.x < 19)
            {
                //触发警告
                teacher.GetComponent<Animator>().SetTrigger("hey");
                //对话框
                talkUI.SetActive(true);
                //切断人物控制
                GameManager.instance.gameMode = GameManager.GameMode.DialogueMoment;
                //立刻静止为idle状态
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<Animator>().SetBool("isMoving", false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.x > 19)
            {
                //开始追击
                teacher.GetComponent<Teacher2>().attack = true;

                this.gameObject.SetActive(false);
            }
        }
    }
}
