using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEngine.Windows;
using static System.Net.Mime.MediaTypeNames;

public class DialogSystem : MonoBehaviour
{
    [Header("UI本身")] public TMP_Text textLable;

    [Header("對話腳本")] public TextAsset textAsset;

    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private bool isInside;
    private bool isFading;

    public int index;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        isInside = true;
        if (!isFading)
        {
            StartCoroutine(FabeIn());
            GetTextFormFile(textAsset);
            textLable.text = textList[index];
            StartCoroutine(DestroyTextAfterDelay(2f));
            StartCoroutine(AddTextAfterDelay(18f));
        }

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
        isInside = false;
        if (!isFading)
        {
            //StartCoroutine(FadeOut());
            textLable.text = "";
        }
    }
    private IEnumerator AddTextAfterDelay(float Textdelay)
    {
        yield return new WaitForSeconds(Textdelay);
        isInside = true;
        if (!isFading)
        {
            //StartCoroutine(FabeIn());
            index++;
            textLable.text = textList[index];
            StartCoroutine(DestroyTextAfterDelay(2f));
        }
    }
    IEnumerator FabeIn()
    {
        isFading = true;


        Color startColorText = textLable.color;

        startColorText.a = 0f;

        textLable.color = startColorText;

        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInTime);
            Color newColor = textLable.color;
            newColor.a = alpha;
            textLable.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFading = false;
    }

    IEnumerator FadeOut()
    {
        isFading = true;


        Color startColorText = textLable.color;

        startColorText.a = 1f;

        textLable.color = startColorText;

        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime);
            Color newColor = textLable.color;
            newColor.a = alpha;
            textLable.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFading = false;
        //Destroy(gameObject, 1.5f);

    }
}
