using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMove_down : MonoBehaviour
{
    void Destory()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
