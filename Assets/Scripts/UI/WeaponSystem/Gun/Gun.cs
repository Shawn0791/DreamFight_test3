using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject shellPrefab;
    public GameObject weaponSystem;
    [Header("枪械属性")]
    public float interval;
    public float bulletLife;
    public float angle1, angle2;
    public float bulletLeft;
    public SkillData gunData;

    protected private Camera gunCamera;
    protected private Transform muzzlePos;
    protected private Transform shellPos;
    protected private Vector2 mousePos;
    protected private Vector2 direction;
    protected private float timer;
    protected private float flipY;
    protected private Animator anim;
    protected private Transform player;

    protected virtual void Start()
    {
        gunCamera = GameObject.FindGameObjectWithTag("GunCam").GetComponent<Camera>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        muzzlePos = transform.Find("Muzzle");
        shellPos = transform.Find("BulletShell");
        flipY = transform.localScale.y;
        //弹夹数
        bulletLeft = weaponSystem.GetComponent<WeaponSystem>().bulletNum;
    }


    protected virtual void Update()
    {
        //获取鼠标位置
        mousePos = gunCamera.ScreenToWorldPoint(Input.mousePosition);
        //翻转枪械
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);

        //传递子弹数量
        weaponSystem.GetComponent<WeaponSystem>().bulletLeft = bulletLeft;
        
        Shoot();
    }

    //左键开枪
    protected virtual void Shoot()
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
        //单击开枪
        if (Input.GetButtonDown("Fire1"))
        {
            if (timer == 0 && bulletLeft != 0) 
            {
                Fire();
                timer = interval;
            }
            //子弹为0点击换弹
            else if (bulletLeft == 0&&player.GetComponent<PlayerData>().mp>=gunData.reloadMp)
            {
                //换弹动画
                anim.SetTrigger("Reload");
                //消耗蓝
                player.GetComponent<PlayerData>().mp -= gunData.reloadMp;
                ReloadFinish();
            }
        }
    }

    //换弹完成
    public void ReloadFinish()
    {
        bulletLeft = gunData.magazineNum;
    }

    //发射子弹
    protected virtual void Fire()
    {
        //开枪动画
        anim.SetTrigger("Shoot");
        //生成子弹
        //GameObject bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = muzzlePos.position;
        bullet.GetComponent<Bullet>().bulletLife = bulletLife;
        //子弹数量减少
        if (bulletLeft > 0)
        {
            bulletLeft--;
        }
        //随机偏移
        float angle = Random.Range(angle1, angle2);
        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(angle, Vector3.forward) * direction);
        //生成弹壳
        //Instantiate(shellPrefab, shellPos.position, shellPos.rotation);
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
