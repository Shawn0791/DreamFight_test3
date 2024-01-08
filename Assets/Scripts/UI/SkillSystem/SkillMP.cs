using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMP : MonoBehaviour
{
    public Image mp;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        mp.fillAmount = player.GetComponent<PlayerData>().mp / 
            player.GetComponent<PlayerData>().maxMp;
    }
}
