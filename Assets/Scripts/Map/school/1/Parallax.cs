using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float XmoveRate;
    public float YmoveRate;
    public bool lockY;
    private float xStartPoint;
    private float yStartPoint;

    void Start()
    {
        xStartPoint = transform.position.x;
        yStartPoint = transform.position.y;
    }

    void Update()
    {
        if (!lockY)
        {
            transform.position = new Vector3(xStartPoint + cam.position.x * XmoveRate,
                yStartPoint + cam.position.y * YmoveRate, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(xStartPoint + cam.position.x * XmoveRate,
                cam.position.y, transform.position.z);
        }
    }
}
