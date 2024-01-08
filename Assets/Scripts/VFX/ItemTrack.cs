using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrack : MonoBehaviour
{
    public void DestroyThis()
    {
        //Destroy(gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }
}
