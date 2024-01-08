using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlParticle1 : MonoBehaviour
{
    public SkillManager skillManager;
    public Camera menuCamera;
    void Update()
    {
        //指针旋转
        Vector2 mouseWorldPosition = menuCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - new Vector2(transform.position.x,
            transform.position.y)).normalized;
        //指向z轴
        transform.up = direction;
    }
}
