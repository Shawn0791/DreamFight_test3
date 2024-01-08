using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCBSystem : MonoBehaviour
{
    public Inventory MyBag;
    public Inventory SCBBag;

    public Image scbImage;


    public int selectedNum;

    public List<GameObject> Bagslots = new List<GameObject>();
    public List<GameObject> SCBslots = new List<GameObject>();

    void Start()
    {
        RefreshData();

        //给定序号
        for (int i = 0; i < SCBslots.Count; i++)
        {
            SCBslots[i].GetComponent<SCBslot>().slotNum = i;
        }
    }

    private void Update()
    {
        SwitchSelectedNum();
        UsedSelectedItem();
    }

    //滚轮切换选中项代号
    void SwitchSelectedNum()
    {
        //切换选中项
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedNum > 0)
        {
            selectedNum--;
            SwitchSelectedSlot();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedNum == 0)
        {
            selectedNum = 5;
            SwitchSelectedSlot();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedNum < 5) 
        {
            selectedNum++;
            SwitchSelectedSlot();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && selectedNum == 5)
        {
            selectedNum = 0;
            SwitchSelectedSlot();
        }
    }

    //更改选中项
    public void SwitchSelectedSlot()
    {
        //还原六个窗格大小
        for (int i = 0; i < SCBslots.Count; i++)
        {
            SCBslots[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        SCBslots[selectedNum].GetComponent<SCBslot>().SelectThisSlot();
    }

    //使用当前选中道具
    public void UsedSelectedItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && SCBBag.itemList[selectedNum] != null) 
        {
            //ID
            int id = Bagslots[selectedNum].GetComponent<Slot>().slotID;
            //如果数量大于1
            if (SCBBag.itemList[selectedNum].itemHold > 1)
            {
                //数字减少
                MyBag.itemList[id].itemHold--;
                SCBslots[selectedNum].transform.GetChild(0).GetChild(1).GetComponent<Text>().text =
                    MyBag.itemList[id].itemHold.ToString();
                //刷新背包数据
                InventoryManager.RefreshItem();
                //调用相关函数

                return;
            }
            //如果数量等于1
            if (SCBBag.itemList[selectedNum].itemHold == 1)
            {
                //删除数据
                MyBag.itemList[id] = null;
                //item取消激活
                SCBslots[selectedNum].transform.GetChild(0).gameObject.SetActive(false);
                //删除图片
                SCBslots[selectedNum].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                //刷新背包数据
                InventoryManager.RefreshItem();
            }
        }
    }

    //更新SCB背包信息
    public void RefreshData()
    {
        for (int i = 0; i < SCBBag.itemList.Count; i++)
        {
            //数据对应
            SCBBag.itemList[i] = MyBag.itemList[i + 114];
            SCBslots[i].GetComponent<SCBslot>().slotItem = SCBBag.itemList[i];
            //激活状态对应
            SCBslots[i].transform.GetChild(0).gameObject.SetActive(Bagslots[i].transform.GetChild(0).gameObject.activeSelf);
            //图片显示对应
            SCBslots[i].transform.GetChild(0).Find("Image").GetComponent<Image>().sprite =
                Bagslots[i].transform.GetChild(0).Find("Image").GetComponent<Image>().sprite;
            //数量显示对应
            SCBslots[i].transform.GetChild(0).Find("Number").GetComponent<Text>().text =
                Bagslots[i].transform.GetChild(0).Find("Number").GetComponent<Text>().text;
        }
    }
}
