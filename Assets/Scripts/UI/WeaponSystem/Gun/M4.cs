using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4 : Gun
{
    protected override void Shoot()
    {
        //枪械指向鼠标
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;

        //开枪间隔冷却
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                timer = 0;
        }
        //按住开枪
        if (Input.GetButton("Fire1"))
        {
            if (timer == 0 && bulletLeft != 0)
            {
                Fire();
                timer = interval;
            }
            //子弹为0点击换弹
            else if (bulletLeft == 0 && player.GetComponent<PlayerData>().mp >= gunData.reloadMp)
            {
                //换弹动画
                anim.SetTrigger("Reload");
                //消耗蓝
                player.GetComponent<PlayerData>().mp -= gunData.reloadMp;
            }
        }
    }
}
