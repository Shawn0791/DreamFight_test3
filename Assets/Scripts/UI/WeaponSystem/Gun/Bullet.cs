using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject explosionPrefab;

    public float bulletLife;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bulletLife -= Time.deltaTime;

        DestroyBullet();
    }

    public void SetSpeed(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            //获取interface
            InterFaces interfaces = collision.gameObject.GetComponent<InterFaces>();
            //传递伤害
            interfaces.GetHit(damage);

            //生成爆炸特效
            //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = transform.position;
            //销毁子弹
            //Destroy(gameObject);
            ObjectPool.Instance.PushObject(gameObject);
        }
        else
        {
            //生成爆炸特效
            //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = transform.position;
            //销毁子弹
            //Destroy(gameObject);
            ObjectPool.Instance.PushObject(gameObject);
        }
    }

    private void DestroyBullet()
    {
        if (bulletLife < 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
