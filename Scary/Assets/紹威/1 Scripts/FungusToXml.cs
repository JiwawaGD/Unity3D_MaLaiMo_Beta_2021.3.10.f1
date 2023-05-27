using UnityEngine;
using Fungus;
using System.Xml;

public class FungusToXml : MonoBehaviour
{
    public Flowchart fungusFlowchart; // ??Fungus流程的Flowchart?件
    public string xmlFilePath; // XML文件保存路?

    private void Start()
    {
        // ?用Fungus流程的Execute方法?始?行流程
        fungusFlowchart.ExecuteBlock("Start");
    }

    private void Update()
    {
        // ?查Fungus流程是否完成
        if (fungusFlowchart.HasExecutingBlocks() == false)
        {
            // ?取Fungus中的?出文本
            string outputText = fungusFlowchart.GetStringVariable("OutputText");

            // ?文本保存到XML文件中
            SaveTextToXml(outputText);

            // 可?：在完成后???用程序或?行其他操作
            // ...
        }
    }

    private void SaveTextToXml(string text)
    {
        // ?建XML文??象
        XmlDocument xmlDoc = new XmlDocument();

        // ?建根??
        XmlNode rootNode = xmlDoc.CreateElement("Data");
        xmlDoc.AppendChild(rootNode);

        // ?建文本??
        XmlNode textNode = xmlDoc.CreateElement("Text");
        textNode.InnerText = text;
        rootNode.AppendChild(textNode);

        // 保存XML文件
        xmlDoc.Save(xmlFilePath);
    }
}
