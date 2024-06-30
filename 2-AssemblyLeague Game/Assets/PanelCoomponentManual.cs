using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelCoomponentManual : MonoBehaviour {
    public Text TextManual;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetComponent(ComponentType aType)
    {
        string disp = "";
        disp += "<size=25>" + aType.DeviceName + "</size> - User Manual\n";
        disp += "<size=20>" + aType.DeviceName + "</size> - Overview\n";
        disp += aType.ShortDescription + "\n";
        if (aType.SpeechDescription.Length > 0)
        {
            disp += aType.SpeechDescription + "\n";
        }

        disp += " This manual will provide you with a guide on how to use the " + aType.DeviceName + " in your code.\n";
        disp += "Same as all other devices, your device can be accessed according to the IO code that you have mapped to the device.\n";
        disp += "For example, if you have this device mapped to IO port 99 then to access the device the following code will interact with the device:\nIO 99";
        disp += "\n";
        disp += "<size=20>" + aType.DeviceName + "</size> - Functions\n";
        
        disp += "The functionality that this device provides can be accessed as follow:\n";
         for (int c = 0; c < aType.Functions2.Count; c++)
        {
            disp +="FUNCTION:"+ aType.Functions2[c]+"\n";
        }
        TextManual.text = disp;
    }
}
