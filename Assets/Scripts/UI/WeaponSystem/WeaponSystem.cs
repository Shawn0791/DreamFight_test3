using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem : MonoBehaviour
{
    public Image image;
    public Text reload;
    public Text magazine;
    public Text left;

    public SkillData[] allSkill;

    public float bulletNum;
    public float bulletLeft;

    void Start()
    {
        //默认为拳头
        image.sprite = allSkill[0].weaponImage;
        reload.text = allSkill[0].reloadMp.ToString();
        magazine.text = allSkill[0].magazineNum.ToString();
        bulletNum = allSkill[0].magazineNum;
    }

    void Update()
    {
        left.text = bulletLeft.ToString();
    }

    public void ChangeWeapon(int id)
    {
        image.sprite = allSkill[id].weaponImage;
        reload.text = allSkill[id].reloadMp.ToString();
        magazine.text = allSkill[id].magazineNum.ToString() + "/";
        bulletNum = bulletLeft = allSkill[id].magazineNum;
    }
}
