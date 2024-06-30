using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelUpload : MonoBehaviour
{
    public InputField InputServer;
    public InputField InputTestData;
    public Text TextResult;
    // Start is called before the first frame update
    void Start()



    {
        StartCoroutine(LateStart(5));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ShowPanel();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ShowPanel()
    {
        WWWInterface.PublicAccess.OnDataDoneReceiving += PublicAccess_OnDataDoneReceiving;
        WWWInterface.PublicAccess.OnErrorDoneReceiving += PublicAccess_OnDataDoneReceiving;

    }
    public void ClosePanel()
    {

        WWWInterface.PublicAccess.OnDataDoneReceiving -= PublicAccess_OnDataDoneReceiving;
        WWWInterface.PublicAccess.OnErrorDoneReceiving -= PublicAccess_OnDataDoneReceiving;

    }

    private void PublicAccess_OnDataDoneReceiving(string result)
    {
        TextResult.text = result;
    }

    public void ButtonUploadClick()
    {

        WWWInterface.PublicAccess.UploadData(InputServer.text, InputTestData.text);
    }


}
