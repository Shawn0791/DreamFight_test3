using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem2 : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;
    public Image speakerImage;
    [Header("文本文件")]
    public TextAsset textFile;
    public static int index;
    [Header("头像")]
    public Sprite[] heads;

    private bool textFinished;
    private bool cancelTyping;
    private GameObject player;

    List<string> textList = new List<string>();
    void Awake()
    {
        GetTextFromFile(textFile);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }
    void Update()
    {
        //超过页数关闭UI
        if (Input.GetKeyDown(KeyCode.E) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            //恢复人物控制
            GameManager.instance.gameMode = GameManager.GameMode.Normal;

            return;
        }
        //if (Input.GetKeyDown(KeyCode.E)&&textFinished)
        //{
        //    StartCoroutine(SetTextUI());
        //}
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished)
            {
                cancelTyping = !cancelTyping;
            }
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var LineData = file.text.Split('\n');

        foreach (var line in LineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";
        //更改名字
        switch (textList[index])
        {
            case "A\r":
                speakerImage.sprite = heads[0];
                index++;
                break;
            case "B\r":
                speakerImage.sprite = heads[1];
                index++;
                break;
            case "C\r":
                speakerImage.sprite = heads[2];
                index++;
                break;
            case "D\r":
                speakerImage.sprite = heads[3];
                index++;
                break;
        }

        //for (int i = 0; i < textList[index].Length; i++)
        //{
        //    textLabel.text += textList[index][i];

        //    yield return new WaitForSeconds(0.1f);
        //}
        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            //逐字出现速度
            yield return new WaitForSeconds(0.05f);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }
}
