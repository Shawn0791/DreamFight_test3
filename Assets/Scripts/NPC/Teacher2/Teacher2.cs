using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher2 : MonoBehaviour,InterFaces
{
    public bool attack;
    public GameObject[] things;
    public Transform throwPoint;

    private int rand;
    private Transform player;
    private Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowThings()
    {
        rand = Random.Range(0, 6);
        for (int i = 0; i < things.Length; i++)
        {
            if (rand == i)
            {
                GameObject thing = ObjectPool.Instance.GetObject(things[i]);
                thing.transform.position = throwPoint.position;
            }
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
            interfaces.GetHitBack(100, dir, 500);
        }
    }

    void InterFaces.GetHit(float damage)
    {

    }

    void InterFaces.GetHitBack(float damage, Vector3 dir, float force)
    {
        anim.SetTrigger("isHurt");
    }
}
