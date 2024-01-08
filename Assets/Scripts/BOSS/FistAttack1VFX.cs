using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistAttack1VFX : MonoBehaviour
{
    public void Destroy()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }

}
