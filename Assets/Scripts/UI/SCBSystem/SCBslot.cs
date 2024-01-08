using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SCBslot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public Item slotItem;
    public Text itemName;
    public int slotNum;
    public GameObject dataMenu;

    private Camera gunCamera;
    private bool update;

    private void Start()
    {
        gunCamera = GameObject.FindGameObjectWithTag("GunCam").GetComponent<Camera>();
    }

    private void Update()
    {
        UpdateMenuPos();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("按下");
        //左键更改选中
        if (Input.GetMouseButton(0))  
        {
            transform.parent.parent.parent.GetComponent<SCBSystem>().selectedNum = slotNum;
            transform.parent.parent.parent.GetComponent<SCBSystem>().SwitchSelectedSlot();
        }
        //右键使用道具
        /*if (Input.GetMouseButton(1) && slotItem != null)
        {
            //如果数量大于1
            if (slotItem.itemHold > 1)
            {
                //数量减1

                //调用物品使用函数
            }
            //如果数量等于1
            else if (slotItem.itemHold == 1)
            {
                //删除mybag数据

                //删除bagsystem的图片和数字

                //刷新快捷背包

                //调用物品使用函数
            }
        }*/
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("抬起");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("离开");
        dataMenu.SetActive(false);
        update = false;
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
        //传输信息
        itemName.text = slotItem.itemName;
        //更新面板位置
        update = true;
    }
    //选中当前窗格
    public void SelectThisSlot()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }
    //更新面板位置
    void UpdateMenuPos()
    {
        if (update)
        {
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = gunCamera.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x, worldPosition.y);
        }
    }
}
