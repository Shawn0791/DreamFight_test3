using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SkillSystem : MonoBehaviour
{
    public GameObject Weapon, Item, Strong, Ability;
    public GameObject Title;
    //public GameObject menuCamera;
    public GameObject menuCameraObject;
    public GameObject dataMenu;
    public GameObject background;
    public GameObject player;

    private Camera menuCamera;
    void Start()
    {
        menuCamera = menuCameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PressU();
        CameraControl();
    }
    //按U打开界面
    void PressU()
    {
        if (Input.GetKeyDown(KeyCode.U) && Title.activeSelf == false) 
        {
            //取消人物控制

            //UI显示
            Title.SetActive(true);
            Weapon.SetActive(true);
            menuCameraObject.SetActive(true);
            background.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.U) && Title.activeSelf == true)
        {
            //恢复人物控制

            //UI消失
            Title.SetActive(false);
            Weapon.SetActive(false);
            Item.SetActive(false);
            Strong.SetActive(false);
            Ability.SetActive(false);
            menuCameraObject.SetActive(false);
            dataMenu.SetActive(false);
            background.SetActive(false);
        }
    }
 
    void CameraControl()
    {
        if (Title.activeSelf == true)
        {
            //滚轮缩放
            if (Input.GetAxis("Mouse ScrollWheel") > 0 &&
                menuCamera.orthographicSize > 10) 
            {
                menuCamera.orthographicSize--;

            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 &&
                menuCamera.orthographicSize < 20) 
            {
                menuCamera.orthographicSize++;
            }
            //中键拖动
            if (Input.GetMouseButton(2))
            {
                float changeX = Input.GetAxis("Mouse X");
                float changeY = Input.GetAxis("Mouse Y");
                menuCameraObject.transform.Translate(Vector3.left * changeX);
                menuCameraObject.transform.Translate(Vector3.down * changeY);
            }

            //视野边界计算

            //x1 = x + 0.5 * W
            float x1 = menuCameraObject.transform.position.x + (menuCamera.orthographicSize * menuCamera.aspect);
            //y1 = y + size
            float y1 = menuCameraObject.transform.position.y + menuCamera.orthographicSize;

            float x2 = menuCameraObject.transform.position.x - (menuCamera.orthographicSize * menuCamera.aspect);
            float y2 = menuCameraObject.transform.position.y - menuCamera.orthographicSize;

            //限制摄像机移动范围
            if (x1 > 42)
            {
                menuCameraObject.transform.position = new Vector3(42 - (menuCamera.orthographicSize *
                    menuCamera.aspect), menuCameraObject.transform.position.y,
                    menuCameraObject.transform.position.z);
            }
            else if (x2 < -42)
            {
                menuCameraObject.transform.position = new Vector3(-42 + (menuCamera.orthographicSize *
                    menuCamera.aspect), menuCameraObject.transform.position.y,
                    menuCameraObject.transform.position.z);
            }
            if (y1 > 121)
            {
                menuCameraObject.transform.position = new Vector3(menuCameraObject.transform.position.x,
                    121 - menuCamera.orthographicSize, menuCameraObject.transform.position.z);
            }
            else if (y2 < 79)
            {
                menuCameraObject.transform.position = new Vector3(menuCameraObject.transform.position.x,
                    79 + menuCamera.orthographicSize, menuCameraObject.transform.position.z);
            }
        }
    }

    public void SwitchWeapon()
    {
        Weapon.SetActive(true);
        Item.SetActive(false);
        Strong.SetActive(false);
        Ability.SetActive(false);
    }
    public void SwitchItem()
    {
        Weapon.SetActive(false);
        Item.SetActive(true);
        Strong.SetActive(false);
        Ability.SetActive(false);
    }
    public void SwitchStrong()
    {
        Weapon.SetActive(false);
        Item.SetActive(false);
        Strong.SetActive(true);
        Ability.SetActive(false);
    }
    public void SwitchAbility()
    {
        Weapon.SetActive(false);
        Item.SetActive(false);
        Strong.SetActive(false);
        Ability.SetActive(true);
    }
}
