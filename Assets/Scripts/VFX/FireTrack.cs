using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrack : MonoBehaviour
{
    private float timer;
    public float damage;

    private void OnEnable()
    {
        timer = 5;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            //收回对象池
            ObjectPool.Instance.PushObject(gameObject);
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
            interfaces.GetHit(damage);
        }
    }
}
