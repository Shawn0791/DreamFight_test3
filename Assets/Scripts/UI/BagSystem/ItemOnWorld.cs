using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerInventory;
    public GameObject SCBSystem;
    public GameObject ItemTrackPoint;

    private void Update()
    {
        StartCoroutine(ItemTrack());
    }

    //碰撞拾取
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    //添加新物品
    public void AddNewItem()
    {
        if (!playerInventory.itemList.Contains(thisItem))
        {
            //playerInventory.itemList.Add(thisItem);
            //InventoryManager.CreateNewItem(thisItem);
            for (int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisItem;
                    InventoryManager.instance.slots[i].GetComponent<Slot>().itemInSlot.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHold++;
        }
        //刷新背包数据
        InventoryManager.RefreshItem();
        //刷新SCB背包数据
        SCBSystem.transform.GetComponentInParent<SCBSystem>().RefreshData();
    }

    //轨迹
    IEnumerator ItemTrack()
    {
        Vector3 lastPos = transform.position;
        yield return new WaitForSeconds(0.1f);
        if (lastPos!=transform.position)
        {
            //Instantiate(ItemTrackPoint, transform.position, Quaternion.identity);
            GameObject point = ObjectPool.Instance.GetObject(ItemTrackPoint);
            point.transform.position = transform.position;
            point.transform.rotation = transform.rotation;
        }
        yield return new WaitForSeconds(0.5f);
    }
}
