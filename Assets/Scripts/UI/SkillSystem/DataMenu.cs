using UnityEngine;
using UnityEngine.EventSystems;

public class DataMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public GameObject dataMenu;
    
    public float Ping;
    private bool IsStart = false;
    private float LastTime = 0;

    void Update()
    {
        if (IsStart && Time.time - LastTime > Ping)
        {
            IsStart = false;
            Debug.Log("长按");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        LongPress(true);
        Debug.Log("按下");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsStart)
        {
            LongPress(false);
            Debug.Log("抬起");
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("离开");

        dataMenu.SetActive(false);
    }
    //鼠标进入按键区域
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("进入");

        if (Input.mousePosition.x >= Screen.width / 2 &&
            Input.mousePosition.y >= Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x - 7, worldPosition.y - 3.5f);
        }
        else if (Input.mousePosition.x < Screen.width / 2 &&
            Input.mousePosition.y >= Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x + 7, worldPosition.y - 3.5f);
        }
        else if (Input.mousePosition.x < Screen.width / 2 &&
            Input.mousePosition.y < Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x + 7, worldPosition.y + 3.5f);
        }
        else if (Input.mousePosition.x >= Screen.width / 2 &&
            Input.mousePosition.y < Screen.height / 2 &&
            dataMenu.activeSelf == false)
        {
            //激活信息窗口
            dataMenu.SetActive(true);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            dataMenu.transform.position = new Vector2(worldPosition.x - 7, worldPosition.y + 3.5f);
        }
    }
    public void LongPress(bool bStart)
    {
        IsStart = bStart;
        LastTime = Time.time;
    }
}