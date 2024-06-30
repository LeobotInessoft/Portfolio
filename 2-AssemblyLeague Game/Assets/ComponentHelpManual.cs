using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHelpManual : MonoBehaviour
{
    public UnityEngine.UI.Text TheText;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
   
    public void SetComponent(ComponentType aType, RobotConstructor.DataComponentType TheRobotComponent, string currentIO)
    {
        aType.InitAllFunctionsIO(currentIO);
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
        disp += "For example, if you have this device mapped to IO port " + currentIO + " then to access the device the following code will interact with the device:\nIO " + currentIO;
        disp += "\n";
        disp += "<size=20>" + aType.DeviceName + "</size> - Functions\n";

        disp += "The functionality that this device provides can be accessed as follow:\n\n";
        //  disp += aType.Functions.Count + "FUNCTIONS";
        for (int c = 0; c < aType.Functions2.Count; c++)
        {
            disp += "FUNCTION:" + aType.Functions2[c].Replace("[[IO]]", currentIO) + "\n";
        }
        TheText.text = disp;
    }

}
