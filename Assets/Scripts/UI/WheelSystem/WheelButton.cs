using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WheelButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public int buttonNum;//当前是第几个按钮
    public SkillInventory WheelSkill;
    public SkillInventory UnlockSkill;
    public Image image;

    private bool canChange;
    private int selectNum;//选中的技能是解锁背包的几号

    void Start()
    {
        //if (WheelSkill.skillList[buttonNum] == null)
        //{
        //    image.gameObject.SetActive(false);
        //}
        //else
        //{
        //    image.sprite = WheelSkill.skillList[buttonNum].skillSprite;
        //}
    }

    void Update()
    {
        //滚轮更换轮盘模块显示的技能
        if (canChange ) 
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectNum > 0)
            {
                //更换轮盘背包技能
                selectNum--;
                WheelSkill.skillList[buttonNum] = UnlockSkill.skillList[selectNum];
                //更换图标
                image.sprite = WheelSkill.skillList[buttonNum].skillSprite;
                //刷新轮盘
                WheelManager.instance.RefreshWheel();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectNum == 0)
            {
                //更换轮盘背包技能
                selectNum = UnlockSkill.skillList.Count - 1;
                WheelSkill.skillList[buttonNum] = UnlockSkill.skillList[selectNum];
                //更换图标
                image.sprite = WheelSkill.skillList[buttonNum].skillSprite;
                //刷新轮盘
                WheelManager.instance.RefreshWheel();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectNum < UnlockSkill.skillList.Count - 1) 
            {
                //更换轮盘背包技能
                selectNum++;
                WheelSkill.skillList[buttonNum] = UnlockSkill.skillList[selectNum];
                //更换图标
                image.sprite = WheelSkill.skillList[buttonNum].skillSprite;
                //刷新轮盘
                WheelManager.instance.RefreshWheel();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectNum == UnlockSkill.skillList.Count - 1)
            {
                //更换轮盘背包技能
                selectNum = 0;
                WheelSkill.skillList[buttonNum] = UnlockSkill.skillList[selectNum];
                //更换图标
                image.sprite = WheelSkill.skillList[buttonNum].skillSprite;
                //刷新轮盘
                WheelManager.instance.RefreshWheel();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("进入");

        canChange = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("离开");

        canChange = false;
    }
}
