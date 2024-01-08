using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo info;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1)
        {
            //Destroy(gameObject);
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
