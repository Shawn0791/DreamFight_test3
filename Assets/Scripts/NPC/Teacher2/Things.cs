using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Things : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private InterFaces interfaces;

    public Vector2 startSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        if (player.transform.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(-startSpeed.x, startSpeed.y);
        }
        else
        {
            rb.velocity = new Vector2(startSpeed.x, startSpeed.y);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //获取interface
            InterFaces interfaces = collision.gameObject.GetComponent<InterFaces>();
            //计算方向
            Vector3 dir = transform.position - collision.transform.position;
            //传递伤害
            interfaces.GetHitBack(100, dir, 300);
        }
        else
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
