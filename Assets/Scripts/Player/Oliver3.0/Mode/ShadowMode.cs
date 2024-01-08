using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMode : MonoBehaviour
{
    private float inputX, inputY;
    private Rigidbody2D rb;
    private Animator anim;
    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        movement();
    }
    //2.5D移动
    void movement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(inputX, inputY).normalized;
        rb.velocity = input * speed;

        //移动动画
        if (inputX != 0 || inputY != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        //转向
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }
}
