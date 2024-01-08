using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedParticle : MonoBehaviour
{
    private float timer;
    void Awake()
    {
        timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            //Destroy(gameObject);
            ObjectPool.Instance.PushObject(gameObject);
            timer = 0;
        }
    }
}
