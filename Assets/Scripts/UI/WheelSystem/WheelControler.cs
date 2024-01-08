using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelControler : MonoBehaviour
{
    public float changeUsedMp;
    
    public GameObject wheel;

    public GameObject[] cross;
    public GameObject[] bg_2;
    public bool[] mode;
    public SkillInventory unlockSkills;
 
    public int selectNum;

    private Transform player;
    private Camera gunCamera;
    private Animator anim;
    private float stopAngle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gunCamera = GameObject.FindGameObjectWithTag("GunCam").GetComponent<Camera>();
        anim = player.transform.GetComponent<Animator>();
        //刷新轮盘
        WheelManager.instance.RefreshWheel();
    }
    private void Update()
    {
        transform.position = gunCamera.transform.position;

        HoldQ();
        ReleaseQ();
        PressQ();
    }
    void HoldQ()
    {
        //按住Q
        if (Input.GetKey(KeyCode.Q))
        {
            //显示轮盘
            wheel.SetActive(true);
            //游戏时间缩放0.1倍
            Time.timeScale = 0.1f;
            //指针旋转
            Vector2 mouseWorldPosition = gunCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPosition - new Vector2(transform.position.x,
                transform.position.y)).normalized;
            //指向上方
            transform.up = direction;

            //Debug.Log(transform.eulerAngles.z);

            //选中模块效果
            for (int i = 0; i < bg_2.Length; i++)
            {
                if(i*60< transform.eulerAngles.z&& transform.eulerAngles.z < (i + 1) * 60)
                {
                    bg_2[i].SetActive(true);
                }
                else
                {
                    bg_2[i].SetActive(false);
                }
            }
            //记录停下时的角度
            stopAngle = transform.eulerAngles.z;
        }
    }

    void ReleaseQ()
    {
        //松开Q
        if (Input.GetKeyUp(KeyCode.Q) && 
            player.GetComponent<PlayerData>().mp > changeUsedMp)  
        {
            ////消耗蓝
            //player.GetComponent<PlayerData>().mp -= changeUsedMp;
            ////生成枪械动画
            //anim.SetTrigger("CreateGun");
            //获取选中模块序号
            for (int i = 0; i < bg_2.Length; i++)
            {
                if (i * 60 < stopAngle && stopAngle < (i + 1) * 60)
                {
                    selectNum = i;
                }
            }
            //选中模块效果
            transform.parent.GetComponent<WheelManager>().SelectMode(selectNum);
            //取消轮盘
            wheel.SetActive(false);
            //游戏时间恢复
            Time.timeScale = 1;
        }
    }

    void PressQ()
    {
        //按下Q时判断选项是否锁定
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < cross.Length; i++)
            {
                cross[i].SetActive(!mode[i]);
            }
        }

    }
}
