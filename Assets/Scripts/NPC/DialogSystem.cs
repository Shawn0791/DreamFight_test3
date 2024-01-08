using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;
    public Text speakerName;
    [Header("文本文件")]
    public TextAsset textFile;
    public static int index;

    private bool textFinished;
    private bool cancelTyping;

    List<string> textList = new List<string>();
    void Awake()
    {
        GetTextFromFile(textFile);
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
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

        foreach(var line in LineData)
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
                speakerName.text = "Oliver";
                index++;
                break;
            case "B\r":
                speakerName.text = "Nobody";
                index++;
                break;
            case "D\r":
                speakerName.text = "Dad";
                index++;
                break;
            case "M\r":
                speakerName.text = "Mom";
                index++;
                break;
        }

        //for (int i = 0; i < textList[index].Length; i++)
        //{
        //    textLabel.text += textList[index][i];

        //    yield return new WaitForSeconds(0.1f);
        //}
        int letter = 0;
        while (!cancelTyping&&letter<textList[index].Length-1)
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
