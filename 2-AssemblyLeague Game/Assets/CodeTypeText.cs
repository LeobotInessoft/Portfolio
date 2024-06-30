using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CodeTypeText : MonoBehaviour {
    public Text TextToReadFrom;
    public Text TextToWriteTo;
    // Use this for initialization
	void Start () {
        TextToWriteTo.supportRichText = true;

	}
	
	// Update is called once per frame
	void Update () {
        TextToWriteTo.text = ConvertText(TextToReadFrom.text);
	}
    private string ConvertText(string inp)
    {
        string outp = "";

        outp = inp;

      

        return outp;
    }
}
