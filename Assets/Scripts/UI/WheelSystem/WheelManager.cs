using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelManager : MonoBehaviour
{
    public static WheelManager instance;
    

    public SkillInventory WheelSkill;
    public GameObject[] arms;
    public Image[] images;

    public GameObject weaponSystem;

    private GameObject player;
    private GameObject pointer;
    private int selectedID;
    private int selectNum;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    void Start()
    {
        pointer = transform.Find("Pointer").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

    }

    public void SelectMode(int selectNum)
    {
        //如果选中项有图片
        if(images[selectNum].gameObject.activeSelf == true)
        {
            //如果该项目已经选中
            if (arms[selectNum].activeSelf == true)
            {
                //取消选中
                arms[selectNum].SetActive(false);
                //切换为normal模式
                player.GetComponent<ChangeMode>().ChangeToNormal();
                //传递技能ID给武器显示系统
                weaponSystem.GetComponent<WeaponSystem>().ChangeWeapon(0);
            }
            else//否则即为未被选中，选中该项目
            {
                this.selectNum = selectNum;
                StartCoroutine(PlayAnimation());
            }
        }
    }

    IEnumerator PlayAnimation()
    {
        //播放动画
        player.GetComponent<Animator>().SetTrigger("CreateGun");
        yield return new WaitForSeconds(0.75f);
        //遍历选中该项目
        for (int i = 0; i < arms.Length; i++)
        {
            if (i == selectNum)
            {
                //消耗蓝量

                //获取技能ID
                if (WheelSkill.skillList[i] != null)
                {
                    selectedID = WheelSkill.skillList[i].skillID;
                }
                //执行changemode中函数
                player.GetComponent<ChangeMode>().ChangeByID(selectedID);
                //选中模块特效
                arms[i].SetActive(true);
                //传递技能ID给武器显示系统
                weaponSystem.GetComponent<WeaponSystem>().ChangeWeapon(selectedID);
            }
            else
                arms[i].SetActive(false);
        }
    }

    public void RefreshWheel()
    {
        for (int i = 0; i < WheelSkill.skillList.Count; i++)
        {
            if (WheelSkill.skillList[i] == null)
            {
                images[i].gameObject.SetActive(false);
            }
            else
            {
                images[i].sprite = WheelSkill.skillList[i].skillSprite;
                images[i].gameObject.SetActive(true);
            }
        }
    }
}
