using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public GameObject wall1, wall2, wall3,wallcollider;
    bool isbroken1, isbroken2, isbroken3;
    bool isHurt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isHurt = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isHurt = false;
    }

    private void Update()
    {
        if (isHurt == true)
        {
            Hurt();
        }
    }
    void Hurt()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("yep");
            //屏幕震动

            //给墙添加rb
            if (isbroken1 == false)
            {
                Rigidbody2D rb = wall1.AddComponent<Rigidbody2D>() as Rigidbody2D;
                isbroken1 = true;
                Destroy(wall1, 2f);
                return;
            }
            else if (isbroken2 == false)
            {
                Rigidbody2D rb = wall2.AddComponent<Rigidbody2D>() as Rigidbody2D;
                isbroken2 = true;
                Destroy(wall2, 2f);
                return;
            }
            else if (isbroken3 == false)
            {
                Rigidbody2D rb = wall3.AddComponent<Rigidbody2D>() as Rigidbody2D;
                isbroken3 = true;
                Destroy(wall3, 2f);
                Destroy(wallcollider, 1f);
                return;
            }
        }
    }
}
