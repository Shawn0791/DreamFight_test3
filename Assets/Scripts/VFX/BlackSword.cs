using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSword : MonoBehaviour
{
    private Transform player;
    private float ang;
    private bool attack;
    protected float lifeTime;

    public float damage;
    public float rotateSpeed;
    public float attackSpeed;
    public GameObject shadowPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lifeTime = 5;
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        attack = false;
        ang = 1;
        lifeTime = 5;
    }

    void Update()
    {
        if (ang > 0.5f && attack == false) 
        {
            //计算角度
            Vector2 dir = player.position - new Vector3(transform.position.x, transform.position.y - 1, 0);
            float rad = Mathf.Acos(Vector2.Dot(transform.up, dir.normalized));
            ang = rad * Mathf.Rad2Deg;
            //转动
            //transform.rotation *= Quaternion.Euler(0, 0, rotationAngle);
            transform.up = Vector2.Lerp(transform.up, dir.normalized, rotateSpeed*Time.deltaTime);
        }
        else if (ang < 0.5f)
        {
            if(lifeTime>0)
                lifeTime -= Time.deltaTime;
            else
                ObjectPool.Instance.PushObject(gameObject);

            attack = true;
            //攻击
            transform.Translate(transform.up * attackSpeed * Time.deltaTime, Space.World);
            //出现残影
            GameObject shadow = ObjectPool.Instance.GetObject(shadowPrefab);
            shadow.transform.position = transform.position;
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
            interfaces.GetHitBack(damage, dir, 300);
        }
    }
}
