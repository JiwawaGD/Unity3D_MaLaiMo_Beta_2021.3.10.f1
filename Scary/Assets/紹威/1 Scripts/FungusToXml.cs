using UnityEngine;
using Fungus;
using System.Xml;

public class FungusToXml : MonoBehaviour
{
    public Flowchart fungusFlowchart; // ??Fungus�y�{��Flowchart?��
    public string xmlFilePath; // XML���O�s��?

    private void Start()
    {
        // ?��Fungus�y�{��Execute��k?�l?��y�{
        fungusFlowchart.ExecuteBlock("Start");
    }

    private void Update()
    {
        // ?�dFungus�y�{�O�_����
        if (fungusFlowchart.HasExecutingBlocks() == false)
        {
            // ?��Fungus����?�X�奻
            string outputText = fungusFlowchart.GetStringVariable("OutputText");

            // ?�奻�O�s��XML���
            SaveTextToXml(outputText);

            // �i?�G�b�����Z???�ε{�ǩ�?���L�ާ@
            // ...
        }
    }

    private void SaveTextToXml(string text)
    {
        // ?��XML��??�H
        XmlDocument xmlDoc = new XmlDocument();

        // ?�خ�??
        XmlNode rootNode = xmlDoc.CreateElement("Data");
        xmlDoc.AppendChild(rootNode);

        // ?�ؤ奻??
        XmlNode textNode = xmlDoc.CreateElement("Text");
        textNode.InnerText = text;
        rootNode.AppendChild(textNode);

        // �O�sXML���
        xmlDoc.Save(xmlFilePath);
    }
}
