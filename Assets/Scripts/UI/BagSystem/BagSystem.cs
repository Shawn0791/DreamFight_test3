using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagSystem : MonoBehaviour
{
    private bool bagIsOpen;
    public GameObject MyBag;
    public bool resolveIsOpen;
    public GameObject ResolveSystem;
    public Image ResImage;
    public GameObject SCBSystem;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenMyBag();
    }

    //开关背包
    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bagIsOpen = !bagIsOpen;
            //刷新SCB背包数据
            SCBSystem.transform.GetComponentInParent<SCBSystem>().RefreshData();
            //背包激活
            MyBag.SetActive(bagIsOpen);
            SCBSystem.SetActive(!bagIsOpen);
        }
    }
    //开关分解系统
    public void OpenResolveSystem()
    {
        resolveIsOpen = !resolveIsOpen;
        ResolveSystem.SetActive(resolveIsOpen);
        //清空分解槽
        ResImage.sprite = null;
        ResImage.transform.parent.gameObject.SetActive(false);
    }
}
