using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Story_7MeetHugeMan : MonoBehaviour
{
    public GameObject talkUI;
    public PlayableDirector playableDir;
    public GameObject steamMan;

    private GameObject player;
    private bool enter;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            talkUI.SetActive(true);

            //切断人物控制
            GameManager.instance.gameMode = GameManager.GameMode.DialogueMoment;
            //立刻静止为idle状态
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("isMoving", false);

            //蒸汽人转向
            steamMan.transform.localScale = new Vector3(-3, 3, 1);

            enter = true;
        }
    }

    private void Update()
    {
        if (enter)
        {
            if (talkUI.activeSelf == false)
            {
                //播放过场动画
                playableDir.Play();
                //切断人物控制
                player.GetComponent<NormalMode>().enabled = false;

                this.gameObject.SetActive(false);
            }
        }
    }
}
