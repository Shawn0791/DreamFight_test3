using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public Item slotItem;
    public Image slotImage;
    public Text slotNum;
    public int slotID;//格子ID = 背包物品序列ID

    public GameObject dataMenu;
    public GameObject itemInSlot;
    public Inventory MyBag;
    public GameObject ResolveItem;

    private Camera gunCamera;

    private void Start()
    {
        gunCamera = GameObject.FindGameObjectWithTag("GunCam").GetComponent<Camera>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("按下");
        if (Input.GetMouseButtonDown(1) && slotItem != null) 
        {
            if (transform.parent.parent.parent.GetComponent<BagSystem>().resolveIsOpen == true)  
            {
                //分解系统slot图片等于此图片
                ResolveItem.transform.GetChild(0).GetComponent<Image>().sprite = slotImage.sprite;
                //分解系统item激活
                ResolveItem.SetActive(true);
                return;
            }
            //使用此物品
            if (slotItem.canUsed == true)
            {
                //如果数量大于1
                if (slotItem.itemHold > 1)
                {
                    //数字减少
                    slotItem.itemHold--;
                    slotNum.text = slotItem.itemHold.ToString();
                    //调用相关函数

                    return;
                }
                //如果数量等于1
                if (slotItem.itemHold == 1)
                {
                    //删除数据
                    MyBag.itemList[slotID] = null;
                    //item取消激活
                    transform.GetChild(0).gameObject.SetActive(false);
                    //删除图片
                    slotImage.sprite = null;
                }
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("抬起");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("离开");
        dataMenu.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("进入");
        //更新面板信息
        if (slotItem == null)
        {
            //Debug.Log("nothing here");
            return;
        }
        InventoryManager.instance.activeItemData = slotItem;
        InventoryManager.instance.UpdateItemInfo();
        //更新面板位置
        dataMenu.SetActive(true);
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,0);
        Vector3 worldPosition = gunCamera.ScreenToWorldPoint(screenPosition);
        dataMenu.transform.position = new Vector3(worldPosition.x, worldPosition.y,0);

    }

    public void SetUpSlot(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        itemInSlot.SetActive(true);
        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemHold.ToString();
    }
}

