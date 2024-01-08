using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public int bulletNum = 3;
    public float bulletAngle = 5;

    protected override void Fire()
    {
        //开枪动画
        anim.SetTrigger("Shoot");
        //计算子弹数中值
        int median = bulletNum / 2;
        for (int i = 0; i < bulletNum; i++)
        {
            //生成子弹
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = muzzlePos.position;
            //设置子弹寿命
            bullet.GetComponent<Bullet>().bulletLife = bulletLife;
            //根据奇偶数调整角度
            if (bulletNum % 2 == 1)
            {
                bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median),
                    Vector3.forward) * direction);
            }
            else
            {
                bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median) 
                    + bulletAngle / 2, Vector3.forward) * direction);
            }
        }
        //子弹数量减少
        if (bulletLeft > 0)
        {
            bulletLeft--;
        }
        else if (bulletLeft == 0)
        {
            bulletLeft = weaponSystem.GetComponent<WeaponSystem>().bulletNum;
        }
        //生成弹壳
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
