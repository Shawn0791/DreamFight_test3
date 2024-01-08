using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform playerPosition;
    private Vector3 cameraPosition;
    private float speed=3;
    private float positionX=0, positionY=7.5f, positionZ=13.7f;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        cameraPosition = new Vector3(playerPosition.position.x-positionX, 
            playerPosition.position.y-positionY,playerPosition.position.z-positionZ);
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.
            transform.position,cameraPosition, speed * Time.deltaTime);
    }
}
