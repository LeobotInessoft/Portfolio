using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class WWWInterface : MonoBehaviour
{

    public bool IsBusy = false;
    System.DateTime lastTicketDate;
    public WWW www;
    public static WWWInterface PublicAccess;
    public delegate void DataDone(string result);
    public event DataDone OnDataDoneReceiving;
    public event DataDone OnErrorDoneReceiving;
    // Start is called before the first frame update
    void Start()
    {
        PublicAccess = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UploadData(string server, string datax)
    {
        xActionRequest act = new xActionRequest();
        act.DataTest = datax;
        StartCoroutine(GetAllData(server, act));

    }
    public IEnumerator GetAllData(string BaseUrl, xActionRequest action)
    {
        if (IsBusy == false)
        {
            IsBusy = true;

            action.TicketDateUTC = System.DateTime.UtcNow;
            lastTicketDate = action.TicketDateUTC;

            Application.backgroundLoadingPriority = ThreadPriority.High;
            string url = BaseUrl + "/";
            WWWForm aForm = new WWWForm();
            aForm.AddField("id", action.DataTest);

            www = new WWW(url, aForm);
            www.threadPriority = ThreadPriority.High;


            yield return www;
            IsBusy = false;

            if (www.text.Length > 0)
                if (OnDataDoneReceiving != null) OnDataDoneReceiving(www.text);
            if (www.error != null && www.error.Length > 0)
            {
                if (OnErrorDoneReceiving != null)
                {
                    OnDataDoneReceiving(www.error);
                }
            }

            print(www.text);
            print(www.error);


        }
        else
        {
            yield return 0;

        }
    }

    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
    public static string ReplaceTextFormat(string input)
    {
        string ret = "";
        print("Text to Fix: " + input);

        bool IsInTag = false;
        string tempInput = input.Replace("<br>", "\n").Replace("</br>", "\n").Replace("<br/>", "\n");
        for (int c = 0; c < tempInput.Length; c++)
        {
            if (tempInput[c].ToString().ToLower() == "<")
            {
                IsInTag = true;
            }


            if (IsInTag == false)
            {
                ret += tempInput[c].ToString();
            }
            if (tempInput[c].ToString().ToLower() == ">")
            {
                IsInTag = false;
            }

        }


        print("Result Text: " + input);
        return ret;
    }

}
public class xActionRequest
{
    public System.DateTime TicketDateUTC;
    public int UserId;
    public string DataTest;


    public static string SerializeToText(xActionRequest aProject)
    {
        string FileContent;
        XmlSerializer xsSubmit = new XmlSerializer(typeof(xActionRequest));
        StringWriter sww = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sww);
        xsSubmit.Serialize(writer, aProject);
        var xml = sww.ToString();
        return sww.ToString();
    }
    public static xActionRequest DeserializeText(string text)
    {
        try
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(xActionRequest));
            StringReader sww = new StringReader(text);
            XmlReader writer = XmlReader.Create(sww);
            xActionRequest theProject = (xActionRequest)xsSubmit.Deserialize(writer);
            return theProject;
        }
        catch
        {
            return null;
        }
    }

}

