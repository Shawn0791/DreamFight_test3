using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private bool isExploded;

    public Vector2 startSpeed;
    public GameObject explodePrefab;

    private void Start()
    {

    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        //初速度
        if (player.localScale.x > 0)
        {
            rb.velocity = new Vector2(startSpeed.x, startSpeed.y);
        }
        else
        {
            rb.velocity = new Vector2(-startSpeed.x, startSpeed.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")||
            collision.CompareTag("Enemy"))
        {
            Explode();
            return;
        }

        if (isExploded == false)
        {
            Invoke("Explode", 1.5f);
            isExploded = true;
        }
    }

    private void Explode()
    {
        //生成爆炸特效
        GameObject exp = ObjectPool.Instance.GetObject(explodePrefab);
        exp.transform.position = transform.position;
        //销毁手雷
        ObjectPool.Instance.PushObject(gameObject);
    }
}
