using UnityEngine;
using Fungus;
using System.Xml;

public class FungusToXml : MonoBehaviour
{
    public Flowchart fungusFlowchart; // 對應Fungus流程的Flowchart組件
    public string xmlFilePath; // XML文件保存路徑

    private void Start()
    {
        // 調用Fungus流程的Execute方法始直行流程
        fungusFlowchart.ExecuteBlock("Start");
    }

    private void Update()
    {
        // 檢查Fungus流程是否完成
        if (fungusFlowchart.HasExecutingBlocks() == false)
        {
            // 獲取Fungus中的輸出文本
            string outputText = fungusFlowchart.GetStringVariable("OutputText");

            // 將文本保存到XML文件中
            SaveTextToXml(outputText);

            // 可選：在完成後關閉应用程序或执行其他操作
            // ...
        }
    }

    private void SaveTextToXml(string text)
    {
        // 創建XML文檔對象
        XmlDocument xmlDoc = new XmlDocument();

        // 創建根節點
        XmlNode rootNode = xmlDoc.CreateElement("Data");
        xmlDoc.AppendChild(rootNode);

        // 創建文本節點
        XmlNode textNode = xmlDoc.CreateElement("Text");
        textNode.InnerText = text;
        rootNode.AppendChild(textNode);

        // 保存XML文件
        xmlDoc.Save(xmlFilePath);
    }
}
