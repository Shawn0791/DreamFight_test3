using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Inventory myBag;
    public Item activeItemData;
    public GameObject slotGrid;
    //public Slot slotPrefab;
    public Text itemName;
    public Text itemInfo;

    public List<GameObject> slots = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
    private void OnEnable()
    {
        RefreshItem();
        instance.itemInfo.text = "";
    }
    //刷新物品描述
    public void UpdateItemInfo()
    {
        itemInfo.text = activeItemData.itemInfo;
        itemName.text = activeItemData.itemName;
    }


    //创建一个新物品
    /*public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, 
            Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform,false);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHold.ToString();
    }*/
    //刷新物品背包
    public static void RefreshItem()
    {
        /*for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }*/

        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.myBag.itemList[i]);
            //传输item数据
            instance.slots[i].GetComponent<Slot>().slotItem = instance.myBag.itemList[i];
            //记录格子ID
            instance.slots[i].GetComponent<Slot>().slotID = i;
            //显示格子内物品
            instance.slots[i].GetComponent<Slot>().SetUpSlot(instance.myBag.itemList[i]);
        }
    }
}
