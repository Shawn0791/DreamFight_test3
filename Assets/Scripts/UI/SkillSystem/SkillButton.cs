using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public SkillData skillData;
    public GameObject dataMenu;

    public GameObject skillManager;
    public GameObject mpUI;
    public GameObject particle1,particle2;
    public GameObject mpCell;
    public Camera menucamara;
    public GameObject head;
    public GameObject talkBG;
    public Image circle;

    public float Ping;
    public float timer;

    private GameObject player;
    private bool IsStart = false;
    private float LastTime = 0;
    private float rate1, rate2;
    private float scaleXY = 1;
    private bool condition = true;
    private bool circleBegin;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        LongPress();
        Shaking();
        ChangeSzie();
        Circle();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //传递skilldata
        SkillManager.instance.activeSkill = skillData;
        SkillManager.instance.DisplaySkillInfo();

        //Debug.Log("进入");
        //显示DataMenu
        if (Input.mousePosition.x >= Screen.width / 2 &&
            Input.mousePosition.y >= Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = menucamara.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x - 7, worldPosition.y - 3.5f);
        }
        else if (Input.mousePosition.x < Screen.width / 2 &&
            Input.mousePosition.y >= Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = menucamara.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x + 7, worldPosition.y - 3.5f);
        }
        else if (Input.mousePosition.x < Screen.width / 2 &&
            Input.mousePosition.y < Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = menucamara.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x + 7, worldPosition.y + 3.5f);
        }
        else if (Input.mousePosition.x >= Screen.width / 2 &&
            Input.mousePosition.y < Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = menucamara.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x - 7, worldPosition.y + 3.5f);
        }
    }
    public void LongPress()
    {
        if (IsStart && Time.time - LastTime > Ping)
        {
            IsStart = false;
            Debug.Log("长按");
            //如果未解锁
            if (skillData.isUnlocked == false)
            {
                skillManager.GetComponent<SkillManager>().unlockStart = true;
                //button开始颤抖
                GetComponent<Shakeable>().shaking = true;
                //粒子2出现
                particle2.SetActive(true);
                //细胞纹路出现
                mpCell.SetActive(true);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        LongPress(true);
        Debug.Log("按下");
        //如果未解锁
        if (skillData.isUnlocked == false)
        {
            //MP条开始颤抖
            mpUI.GetComponent<Shakeable>().shaking = true;
            //粒子1出现(朝鼠标移动)
            particle1.SetActive(true);
        }
        //右键点击技能
        if (Input.GetMouseButton(1))
        {
            //头像对话框出现
            talkBG.SetActive(true);
            //头像晃动
            head.GetComponent<Animator>().SetBool("Talking", true);
            //逐字输出
            if(skillManager.GetComponent<SkillManager>().typeFinished&&
                skillManager.GetComponent<SkillManager>().cancelType == false)
            {
                StartCoroutine(skillManager.GetComponent<SkillManager>().TypeText());
            }
        }
        //圆圈启动
        circleBegin = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        LongPress(false);
        Debug.Log("抬起");
        skillManager.GetComponent<SkillManager>().unlockStart = false;
        //按钮和MP条颤抖取消
        GetComponent<Shakeable>().shaking = false;
        mpUI.GetComponent<Shakeable>().shaking = false;
        //粒子1和2消失
        particle1.SetActive(false);
        particle2.SetActive(false);
        //细胞纹路消失
        mpCell.SetActive(false);
        //圆圈回转
        circleBegin = false;
        StartCoroutine(circleBack());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("离开");

        //去除new标识
        transform.Find("New").gameObject.SetActive(false);
        skillData.isNew = false;
        //清空技能exp
        skillData.skillExp = 0;
        //关闭菜单
        dataMenu.SetActive(false);
    }
    public void Shaking()
    {
        //按键震动逐渐加强
        rate1 = (skillData.skillExp / skillData.unlockExp) * 0.08f;
        GetComponent<Shakeable>().shakeRate = new Vector3(rate1, rate1, 0);

        //MP条震动逐渐加强
        rate2 = 0.05f - (player.GetComponent<PlayerData>().mp /
            player.GetComponent<PlayerData>().maxMp * 0.05f);
        mpUI.GetComponent<Shakeable>().shakeRate = new Vector3(rate2, rate2, 0);
    }
    //长按
    public void LongPress(bool bStart)
    {
        IsStart = bStart;
        LastTime = Time.time;
    }
    //技能按钮变大
    public void ChangeSzie()
    {
        //持续更新大小
        transform.localScale = new Vector3(scaleXY, scaleXY, 1);
        //解锁时变大
        if (skillData.isUnlocked && condition) 
        {
            scaleXY = 1.5f;
            condition = false;
        }
        //恢复大小
        if (scaleXY > 1)
            scaleXY -= 0.005f;
        else if (scaleXY < 1)
            scaleXY = 1;
    }
    //圆圈动态解锁
    public void Circle()
    {
        if (circleBegin)
        {
            timer += Time.deltaTime;
            circle.fillAmount = timer / Ping;
        }
        else
        {
            timer = 0;
        }
    }
    //圆圈动态返回
    IEnumerator circleBack()
    {
        while (circle.fillAmount > 0)
        {
            circle.fillAmount -= 0.03f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
