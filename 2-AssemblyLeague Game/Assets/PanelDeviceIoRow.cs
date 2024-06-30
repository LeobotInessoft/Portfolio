using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelDeviceIoRow : MonoBehaviour {
    public RobotConstructor.DataComponentType TheRobotComponent;
    public Text TextDeviceName;
    public InputField InputIoNumber;
    public IDECanvasManager TheCanvas;
    public GameObject PrefebRepresenting;
    public PanelIoMap PanelIoMap;
    public ComponentType HelpManual;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
       
        
    }
    public void SetRow()
    {
        if (TheRobotComponent != null)
        {
            if (PrefebRepresenting != null)
            {
                ComponentType aType = PrefebRepresenting.GetComponent<ComponentType>();
                TextDeviceName.text = aType.DeviceName;
                if (TheRobotComponent.IoNumberCustomMap != null )
                {
                    InputIoNumber.text = TheRobotComponent.IoNumberCustomMap;
                }
                else
                {
                    if (aType.ModuleType == ComponentType.EnumComponentType.Legs) InputIoNumber.text = 1+"";
                    if (aType.ModuleType == ComponentType.EnumComponentType.Cockpit) InputIoNumber.text = 2 + "";
                    if (aType.ModuleType == ComponentType.EnumComponentType.Shoulders) InputIoNumber.text = 2 + "";
                    if (aType.ModuleType == ComponentType.EnumComponentType.WeaponAnySlot) InputIoNumber.text = 3 + "";
                    if (aType.ModuleType == ComponentType.EnumComponentType.Backpack) InputIoNumber.text = 4 + "";
                    if (aType.ModuleType == ComponentType.EnumComponentType.Antenna) InputIoNumber.text = 5 + "";
           

                }
            }
        }
    }

    public void ButtonClick()
    {
        if (TheCanvas != null)
        {
            HelpManual.InitAllFunctionsIO(InputIoNumber.text);
      
            PanelIoMap.ShowManual(HelpManual, TheRobotComponent, InputIoNumber.text);
        }
    }
}
