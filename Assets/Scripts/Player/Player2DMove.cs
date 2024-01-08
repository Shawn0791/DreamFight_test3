using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float horizontalSpeed;
    //public float verticalSpeed;
    public float jumpF;

    private bool isHurt;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement();
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("walk", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f&& Mathf.Abs(rb.velocity.y)<0.1f)
            {
                anim.SetBool("hurt", false);
            }
        }
        
    }

    //角色走动 player walk
    void Movement()
    {
        //左右走动 left and right
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * horizontalSpeed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
        //跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpF);
        }

    }
}
