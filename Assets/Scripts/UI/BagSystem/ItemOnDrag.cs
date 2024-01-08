using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private Transform originalParent;
    private int currentItemID;

    private RectTransform rectTransform;
    private Vector3 pos;
    private Vector3 mousePos;

    public Inventory myBag;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始位置
        originalParent = transform.parent;
        //记录ID
        currentItemID = originalParent.GetComponent<Slot>().slotID;
        //移出以防止遮挡
        //transform.SetParent(transform.parent.parent);
        //跟随鼠标
        pos = GetComponent<RectTransform>().position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform,
            eventData.position, eventData.pressEventCamera, out mousePos);
        //关闭射线
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标
        Vector3 newVec;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
            eventData.position, eventData.pressEventCamera, out newVec);
        Vector3 offset = new Vector3(newVec.x - mousePos.x, newVec.y - mousePos.y, 0);
        rectTransform.position = pos + offset;

        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //落点有Image
        if (eventData.pointerCurrentRaycast.gameObject.name == "Image")  
        {
            //itemList物品储存位置改变
            var temp = myBag.itemList[currentItemID];
            myBag.itemList[currentItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
            //目标物品储存位置回调
            myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

            //设置父级
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent,false);
            //设置位置
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
            
            //落点对象更换为原父级
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent, false);
            //落点对象回到原位置
            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;

            //刷新物品背包
            for (int i = 0; i < myBag.itemList.Count; i++)
            {
                InventoryManager.instance.slots[i].GetComponent<Slot>().slotItem = 
                    InventoryManager.instance.myBag.itemList[i];
            }

            //打开射线
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        //落点为slot
        if (eventData.pointerCurrentRaycast.gameObject.tag == "Slot")
        {
            //设置父级
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform, false);
            //设置位置
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            
            if(myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] != myBag.itemList[currentItemID])
            {
                //itemList物品储存位置改变
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myBag.itemList[currentItemID];
                //目标物品储存位置回调
                myBag.itemList[currentItemID] = null;
            }

            //落点对象回到原位置
            eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).position = originalParent.position;
            //落点对象更换为原父级
            eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).SetParent(originalParent, false);

            //刷新物品背包
            for (int i = 0; i < myBag.itemList.Count; i++)
            {
                InventoryManager.instance.slots[i].GetComponent<Slot>().slotItem =
                    InventoryManager.instance.myBag.itemList[i];
            }

            //打开射线
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        //落点为UI其他位置
        if(eventData.pointerCurrentRaycast.gameObject.tag == "Untagged")
        {
            //回到原地
            transform.position = originalParent.position;
            transform.SetParent(originalParent);
            //打开射线
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        //落点为null
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            //销毁背包物品

            //删除mybag存档数据

            //世界位置生成物品

        }
    }

}
