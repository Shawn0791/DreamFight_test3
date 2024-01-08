using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCreater : MonoBehaviour
{
    private Transform player;
    private Vector2 target;
    private float createCD;
    private float timer;

    public GameObject fireTrack;
    public GameObject fireCast;
    public float speed;
    public float interval;
    private float lifetimer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void OnEnable()
    {
        timer = 3;
        lifetimer = 5;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;


        if (timer <= 0) 
        {
            CreatFireTrack();

            if (createCD > 0)
                createCD -= Time.deltaTime;

            if (lifetimer > 0)
                lifetimer -= Time.deltaTime;

            if (lifetimer <= 0 || new Vector2(transform.position.x, transform.position.y) == target)
                //收回对象池
                ObjectPool.Instance.PushObject(gameObject);

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed);
        }
    }

    private void CreatFireTrack()
    {
        //每隔一段时间生成一团火
        if (createCD <= 0)
        {
            GameObject fire = ObjectPool.Instance.GetObject(fireTrack);
            fire.transform.position = transform.position;
            fire.transform.rotation = transform.rotation;

            createCD = interval;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject cast = ObjectPool.Instance.GetObject(fireCast);
            cast.transform.position = transform.position;
            cast.transform.rotation = transform.rotation;
            //收回对象池
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
