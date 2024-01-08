using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadTalking : MonoBehaviour
{
    public GameObject head;
    public SkillManager skillManager;
    public bool typeFinished;
    void Update()
    {
        //关闭界面
        if (Input.GetMouseButton(0) || Input.GetMouseButton(2) || Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //清空文本框
            transform.Find("TakingText").GetComponent<Text>().text = "";
            //结束逐字输出状态
            skillManager.GetComponent<SkillManager>().typeFinished = true;
            //停止头像动画
            head.GetComponent<Animator>().SetBool("Talking", false);
            //改为未输入完成状态
            typeFinished = false;
            //关闭界面
            this.gameObject.SetActive(false);
        }
        if (gameObject.activeSelf)
        {
            StartCoroutine(RightClick());
        }
    }
    IEnumerator RightClick()
    {
        yield return new WaitForSeconds(0.5f);
        //右键加速显示
        if (Input.GetMouseButton(1) && !typeFinished &&
            skillManager.GetComponent<SkillManager>().typeFinished == false &&
            skillManager.GetComponent<SkillManager>().cancelType == false)
        {
            skillManager.GetComponent<SkillManager>().cancelType = true;
            typeFinished = true;
        }
    }
}
