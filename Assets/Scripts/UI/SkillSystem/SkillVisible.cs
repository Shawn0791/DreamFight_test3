using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillVisible : MonoBehaviour
{
    public SkillData[] skillData;
    public GameObject[] skills;

    
    void Start()
    {
        for (int i = 0; i < skillData.Length; i++)
        {
            //是否可见
            if (skillData[i].canUnlocked == true)
            {
                skills[i].SetActive(true);
            }
            else if(skillData[i].canUnlocked == false)
            {
                skills[i].SetActive(false);
            }
            //是否标New
            if (skillData[i].isNew == true)
            {
                skills[i].transform.Find("New").gameObject.SetActive(true);
            }
            else if(skillData[i].isNew == false)
            {
                skills[i].transform.Find("New").gameObject.SetActive(false);
            }
            //是否亮色
            if (skillData[i].isUnlocked == true)
            {
                skills[i].GetComponent<Image>().color = new Color(1f, 0.7568f, 0.0274f, 1f);
            }
            else if(skillData[i].isUnlocked == false)
            {
                skills[i].GetComponent<Image>().color = new Color(0.3396226f, 0.3396226f, 0.3396226f, 1f);
            }
        }
    }
    private void Update()
    {
        Condition1();
        Condition2();
    }
    //一个前提条件的技能可见
    public void Condition1()
    {
        //遍历每一个skilldata
        for (int i = 0; i < skillData.Length; i++)
        {
            //如果唯一的一个前提条件解锁了
            if (skillData[i].preSkills.Length == 1 &&
                skillData[i].preSkills[0].isUnlocked == true &&
                skillData[i].canUnlocked == false)  
            {
                skillData[i].canUnlocked = true;
                skills[i].SetActive(true);
            }
        }
    }
    //两个前提条件的技能可见
    public void Condition2()
    {
        //遍历每一个skilldata
        for (int i = 0; i < skillData.Length; i++)
        {
            //如果两个个前提条件都解锁了
            if (skillData[i].preSkills.Length == 2 &&
                skillData[i].preSkills[0].isUnlocked == true &&
                skillData[i].preSkills[1].isUnlocked == true &&
                skillData[i].canUnlocked == false) 
            {
                skillData[i].canUnlocked = true;
                skills[i].SetActive(true);
            }
        }
    }

}
