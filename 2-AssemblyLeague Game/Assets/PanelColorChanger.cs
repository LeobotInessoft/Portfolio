using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelColorChanger : MonoBehaviour
{
    public InputField InputR;
    public InputField InputG;
    public InputField InputB;

    public RobotConstructor TheConstructor;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ColorChanged()
    {
        try
        {
            float r = float.Parse(InputR.text);
            float g = float.Parse(InputG.text);
            float b = float.Parse(InputB.text);
            float min = 0.25f;
            float max = 10f;
            if (r <= min)
            {
                r = min;
            }
            if (g <= min)
            {
                g = min;
            }
            if (b <= min)
            {
               b  = min;
            }
            if (r >=max)
            {
                r = max;
            }
            if (g >=max)
            {
                g =max;
            }
            if (b >= max)
            {
                b = max; 
            }
            TheConstructor.ApplyColour(new Color(r, g, b, 1f));
        }
        catch
        {

        }
    }

}
