using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_4_4Freeze : MonoBehaviour
{
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
            //切断人物控制
            GameManager.instance.gameMode = GameManager.GameMode.DialogueMoment;
            //立刻静止为idle状态
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("isMoving", false);

            //外力击飞
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 100));

            talkUI.SetActive(true);

            this.gameObject.SetActive(false);
        }
    }
}
