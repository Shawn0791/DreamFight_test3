using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_5EnterHugeCity : MonoBehaviour
{
    public GameObject HugeCity;
    public GameObject Forest;
    public Animator blackAnim;
    public GameObject talkUI;
    public GameObject oldTalkUI;
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
            //黑屏
            blackAnim.SetTrigger("black");
            //关闭上一个UI
            oldTalkUI.SetActive(false);
            //打开背景
            Forest.SetActive(false);
            HugeCity.SetActive(true);
            //人物躺平动画
            player.GetComponent<Animator>().Play("LayDown");

            talkUI.SetActive(true);

            //切断人物控制
            GameManager.instance.gameMode = GameManager.GameMode.DialogueMoment;
            //立刻静止为idle状态
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("isMoving", false);

            //枪不在手
            player.GetComponent<NormalMode>().enabled = true;
            player.GetComponent<GunMode>().enabled = false;
            player.GetComponent<Animator>().SetBool("Normal", true);
            player.GetComponent<Animator>().SetBool("Gun", false);
            revolver.SetActive(true);

            this.gameObject.SetActive(false);
        }
    }
}
