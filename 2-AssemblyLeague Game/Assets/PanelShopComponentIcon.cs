using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelShopComponentIcon : MonoBehaviour {
    public Text TextPartName;
    public Image ImagePartPic;
    public Text TextPartPrice;
    public GameObject PanelCheckMark;
    ComponentType viewedComponent;
    public PanelShopViewComponent PanelDetailView;
    // Use this for initialization
	void Start () {

    
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetComponent(ComponentType aType, RobotOwnerLookup.PlayerPurchases currentPurchases)
    {
       
        viewedComponent = aType;
        TextPartName.text = aType.DeviceName;
        TextPartPrice.text = "$"+aType.OfflinePurchaseCost.ToString("f0");
        ImagePartPic.sprite = Resources.Load<Sprite>("Component Images/" + aType.UniqueDeviceID + "");
        if (currentPurchases.ComponentsPurchasedIDs.Contains(aType.UniqueDeviceID))
        {
             PanelCheckMark.gameObject.SetActive(true);
        }
        else
        {
            PanelCheckMark.gameObject.SetActive(false);
       
        }
    }
   

    public void ButtonClick()
    {
        if (PanelDetailView != null)
        {
            AuxAudio.PublicAccess.PlayComponentName(viewedComponent);
            AuxAudio.PublicAccess.PlayComponentDescription(viewedComponent);
            PanelDetailView.SetDetail(viewedComponent);
            PanelDetailView.gameObject.SetActive(true);
         
        }

    }
}
