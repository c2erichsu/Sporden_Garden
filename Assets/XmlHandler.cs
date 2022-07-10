using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Linq;

public class XmlHandler : MonoBehaviour {

    public static int LanController = 1;

    void Start()
    {
    }

    public static string GrabXMLAssest(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset t = Resources.Load("TextAsset") as TextAsset;
        xmlDoc.LoadXml(t.text);
        XmlNodeList nodelist = xmlDoc.SelectNodes("//root/plant/" + path);

        return nodelist[0].ChildNodes[0].Value;
    }
}
