using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChewerHP : MonsterHP
{
    protected override void Update()
    {
        HideBar();

        hp = monster.GetComponent<Chewer>().hp;
        maxHp = monster.GetComponent<Chewer>().maxHp;
        //血条随血量变动
        hp_fill.fillAmount = hp / maxHp;
        //不随主体翻转
        transform.localScale = monster.transform.localScale;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
}
