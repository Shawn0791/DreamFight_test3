using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_1Start : MonoBehaviour
{
    public GameObject talkUI;

    private GameObject player;
    private Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //人物躺平
            anim.Play("LayDown");

            talkUI.SetActive(true);

            //切断人物控制
            GameManager.instance.gameMode = GameManager.GameMode.DialogueMoment;
            //立刻静止为idle状态
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            anim.SetBool("isMoving", false);

            this.gameObject.SetActive(false);
        }
    }
}
