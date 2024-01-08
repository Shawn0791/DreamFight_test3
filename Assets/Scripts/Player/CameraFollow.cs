using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerPosition;
    private Vector3 cameraPosition;
    private float speed = 3;

    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        cameraPosition = new Vector3(playerPosition.position.x,
    playerPosition.position.y, playerPosition.position.z -10);
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.
            transform.position, cameraPosition, speed * Time.deltaTime);
    }
}
