using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_3ChangeToForest : MonoBehaviour
{
    public GameObject School;
    public GameObject Forest;
    public GameObject talkUI;
    public GameObject revolver;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            School.SetActive(false);
            Forest.SetActive(true);
            talkUI.SetActive(true);

            //切断人物控制
            GameManager.instance.gameMode = GameManager.GameMode.DialogueMoment;
            //立刻静止为idle状态
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("isMoving", false);

            //枪在手
            player.GetComponent<NormalMode>().enabled = false;
            player.GetComponent<GunMode>().enabled = true;
            player.GetComponent<Animator>().SetBool("Normal", false);
            player.GetComponent<Animator>().SetBool("Gun", true);
            revolver.SetActive(true);


            this.gameObject.SetActive(false);
        }
    }
}
