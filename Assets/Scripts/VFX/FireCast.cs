using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCast : MonoBehaviour
{
    public float damage;
    public float force;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //获取interface
            InterFaces interfaces = collision.gameObject.GetComponent<InterFaces>();
            //计算方向
            Vector3 dir = transform.position - player.position;
            //传递伤害
            interfaces.GetHitBack(damage, 20 * dir, force);
        }
    }

    public void Destroy()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
