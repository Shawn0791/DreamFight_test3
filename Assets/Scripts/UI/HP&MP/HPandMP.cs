using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPandMP : MonoBehaviour
{
    public Image HP_1;
    public Image HP_2;
    public Image MP_1;
    public Image MP_2;

    private Transform player;
    public float recoverSpeed;
    public float consumeSpeed;
    private float hp;
    private float maxHp;
    private float mp;
    private float maxMp;
    
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //获取hp和mp信息
        hp = player.GetComponent<PlayerData>().hp;
        maxHp = player.GetComponent<PlayerData>().maxHp;
        mp = player.GetComponent<PlayerData>().mp;
        maxMp = player.GetComponent<PlayerData>().maxMp;

        //掉血
        HP_1.fillAmount = hp / maxHp;
        //红血缓慢回复
        if (HP_2.fillAmount > HP_1.fillAmount)
        {
            player.GetComponent<PlayerData>().hp += maxHp * recoverSpeed;
            HP_1.fillAmount += recoverSpeed;
        }

        //掉蓝
        MP_1.fillAmount = mp / maxMp;
        //掉蓝效果
        if (MP_2.fillAmount > MP_1.fillAmount)
        {
            MP_2.fillAmount -= consumeSpeed;
        }
        else
        {
            MP_2.fillAmount = MP_1.fillAmount;
        }
    }
}
