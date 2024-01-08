using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    private Animator anim;
    //private Transform[] childs;
    public GameObject room;

    private void Start()
    {
        anim = transform.GetComponent<Animator>();
        //childs = new Transform[room.transform.childCount];
        //for (int i = 0; i < room.transform.childCount; i++)
        //{
        //    childs[i] = room.transform.GetChild(i);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("light");
        }
        //items激活
        //for (int i = 0; i < childs.Length; i++)
        //{
        //    childs[i].gameObject.SetActive(true);
        //}
        room.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("dark");
        }
        //items关闭
        //for (int i = 0; i < childs.Length; i++)
        //{
        //    childs[i].gameObject.SetActive(false);
        //}
        room.SetActive(false);
    }

}
