using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    public float speed;
    public float stopTime = 0.5f;
    public float fadeSpeed = 0.01f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        //随机偏移
        float angle = Random.Range(-45f, 45f);
        rb.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * speed;

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        rb.gravityScale = 3;
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopTime);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;

        while (sprite.color.a>0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - fadeSpeed);
            yield return new WaitForFixedUpdate();
        }
        //销毁弹壳
        //Destroy(gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }
}
