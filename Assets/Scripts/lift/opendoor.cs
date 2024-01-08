using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opendoor : MonoBehaviour
{
    public float speed;
    public GameObject leftdoor, rightdoor;
    public Transform leftpoint, rightpoint;
    public Transform leftback, rightback;
    private bool moveleft, moveright;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            moveleft = true;
            moveright = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            moveleft = false;
            moveright = false;
        }
    }
    private void Update()
    {
        if(moveleft==true)
            leftdoor.transform.position = Vector3.MoveTowards(leftdoor.transform.position, leftpoint.position, speed*Time.deltaTime);
        if (moveright == true)
            rightdoor.transform.position = Vector3.MoveTowards(rightdoor.transform.position, rightpoint.position, speed * Time.deltaTime);
        if (moveleft == false)
            leftdoor.transform.position = Vector3.MoveTowards(leftdoor.transform.position, leftback.position, speed * Time.deltaTime);
        if (moveright == false)
            rightdoor.transform.position = Vector3.MoveTowards(rightdoor.transform.position, rightback.position, speed * Time.deltaTime);
    }
}