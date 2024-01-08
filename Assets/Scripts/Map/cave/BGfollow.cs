using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGfollow : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x, -46,35); 
    }
}
