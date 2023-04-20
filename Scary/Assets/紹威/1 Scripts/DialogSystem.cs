using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEngine.Windows;

public class DialogSystem : MonoBehaviour
{
    [Header("UI本身")] public TMP_Text textLable;

    [Header("對話腳本")] public TextAsset textAsset;

    public int index;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        GetTextFormFile(textAsset);
        textLable.text = textList[index];
        StartCoroutine(AddTextAfterDelay(10f));

        StartCoroutine(DestroyTextAfterDelay(20f));
    }

    // Update is called once per frame
    void Update()
    {

        //textLable.text = textList[index];
        //stroy(textLable, 5);

    }
    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }
    private IEnumerator DestroyTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 5 秒後刪除文字      
        textLable.text = "";
    }
    private IEnumerator AddTextAfterDelay(float Textdelay)
    {
        yield return new WaitForSeconds(Textdelay);
        index++;
        textLable.text = textList[index];
    }
}
