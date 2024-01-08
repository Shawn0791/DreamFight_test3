using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public bool unlockStart;
    public bool typeFinished = true;
    public bool cancelType;
    public SkillData activeSkill;
    public SkillData[] allSkills;

    public GameObject dataMenu;
    public GameObject title;
    public SkillButton[] skillButtons;
    public GameObject mpUI, particle1,particle2;
    public GameObject mpCell;
    public GameObject particle3;
    public SkillInventory unlockSkills;


    [Header("UI")]
    public Image skillImage;
    public Text skillNameText, mpConsText, headTalkText, skillAttribute;

    private GameObject player;
    private float mp;
    private bool condition;


    //单例
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        mp = player.GetComponent<PlayerData>().mp;

        Unlocking();
    }

    public void DisplaySkillInfo()//信息传递到显示面板
    {
        //信息显示面板
        skillImage.sprite = activeSkill.skillSprite;
        skillNameText.text = activeSkill.skillName;
        mpConsText.text = "Control consumption:\n" + activeSkill.mpCons;
        skillAttribute.text = activeSkill.skillAttribute;
        //头像自言自语
        //headTaking.text = activeSkill.headTaking;
    }

    public void Unlocking()
    {
        if (dataMenu.activeSelf == true) 
        {
            //技能解锁
            if (unlockStart == true && mp > 0 && activeSkill.isUnlocked == false)  
            {
                //mp兑换exp
                if (mp > 0 && activeSkill.skillExp < activeSkill.unlockExp)
                {
                    player.GetComponent<PlayerData>().mp -= 0.2f;
                    activeSkill.skillExp += 0.2f;
                    //粒子3实例化条件回调
                    condition = false;
                }
                else
                {
                    unlockStart = false;
                }
            }
            //停止时缓慢减少exp
            else if (unlockStart == false && activeSkill.isUnlocked == false &&
                activeSkill.skillExp < activeSkill.unlockExp && activeSkill.skillExp > 0)   
            {
                activeSkill.skillExp -= Time.deltaTime;
            }
            //解锁成功
            else if (activeSkill.skillExp >= activeSkill.unlockExp)
            {
                //按键亮起
                //原色(0.3396226f,0.3396226f,0.3396226f,1f)
                //红色(0.75f, 0f, 0f, 1f)
                //金色(1f,0.7568f,0.0274f,1f)
                skillButtons[activeSkill.skillID - 1].GetComponent<Image>().color
                    = new Color(1f, 0.7568f, 0.0274f, 1f);
                //解锁技能
                activeSkill.isUnlocked = true;
                
                //************************************
                //如果技能为可装备则加入已解锁技能背包
                if (activeSkill.canEquip == true && !unlockSkills.skillList.Contains(activeSkill)) 
                {
                    unlockSkills.skillList.Add(activeSkill);

                    for (int i = 0; i < 6; i++)
                    {
                        //遍历轮盘列表没有该技能并且存在一个空位
                        if (WheelManager.instance.WheelSkill.skillList[i] == null &&
                           !WheelManager.instance.WheelSkill.skillList.Contains(activeSkill))
                        {
                            //空位填上该技能
                            WheelManager.instance.WheelSkill.skillList[i] = activeSkill;
                            //刷新轮盘
                            WheelManager.instance.RefreshWheel();
                        }
                    }
                }
                //加入轮盘背包
                
                //************************************

                //按钮和MP条颤抖取消
                skillButtons[activeSkill.skillID - 1].GetComponent<Shakeable>().shaking = false;
                mpUI.GetComponent<Shakeable>().shaking = false;
                //粒子1和2消失
                particle1.SetActive(false);
                particle2.SetActive(false);
                //细胞纹路消失
                mpCell.SetActive(false);
                //粒子3出现
                if (!condition)
                {
                    Instantiate(particle3, skillButtons[activeSkill.skillID - 1].transform.position,
                        Quaternion.identity);
                    condition = true;
                }
            }
        }
    }

    //逐字输出协程
    public IEnumerator TypeText()
    {
        typeFinished = false;
        //暂存文本
        string talkText = activeSkill.headTaking;
        //清空文本框
        headTalkText.text = "";
        //foreach (var letter in talkText.ToCharArray())
        //{
        //    headTalkText.text += letter;
        //    yield return new WaitForSeconds(0.1f);
        //}
        int i = 0;
        while (!cancelType && !typeFinished && i < talkText.Length) 
        {
            headTalkText.text += talkText[i];
            i++;
            yield return new WaitForSeconds(0.05f);
        }
        headTalkText.text = talkText;
        typeFinished = true;
        cancelType = false;
    }
}
