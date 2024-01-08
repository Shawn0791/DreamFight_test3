using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    protected private Transform target;
    protected private SpriteRenderer sr;
    protected private float alpha;

    public string tagName;

    protected virtual void Awake()
    {
        target = GameObject.FindGameObjectWithTag(tagName).transform;
        sr = GetComponent<SpriteRenderer>();
        alpha = 0.8f;
    }

    protected virtual private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag(tagName).transform;
        sr.sprite = target.GetComponent<SpriteRenderer>().sprite;
        alpha = 0.8f;
        //旋转角度与主体一致
        transform.rotation = target.rotation;
        //左右翻转与主体一致
        transform.localScale = target.localScale;
    }

    protected virtual void Update()
    {
        //逐渐增加透明度
        sr.color = new Color(0.3f, 0.3f, 0.3f, alpha);
        if (alpha > 0)
            alpha -= 0.03f;


        //收回对象池
        if (alpha <= 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
