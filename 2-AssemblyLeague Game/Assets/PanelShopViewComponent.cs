using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PanelShopViewComponent : MonoBehaviour
{
    public Text TextHeading;
    public Text TextQty;
    public Button ButtonBuy;
    public PanelComponentDescription PanelDescription;
    public PanelComponentStats PanelStats;
    public RobotConstructor TheRobotConstructor;
    public PanelShopComponentIcon SmallIcon;
    ComponentType viewedComp;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonCloseClick()
    {
        gameObject.SetActive(false);
    }
    public void ButtonBuyClick()
    {
        RobotOwnerLookup.PublicAccess.DoPurchase(viewedComp);
        PanelShopContent.Refresh = true;
        gameObject.SetActive(false);
    }
    public void SetDetail(ComponentType aType)
    {
        viewedComp = aType;
        TextHeading.text = aType.DeviceName;
        RobotOwnerLookup.PlayerPurchases aPurch = RobotOwnerLookup.PublicAccess.GetPlayerPurchases(); 
        PanelDescription.ShowComponent(aType);
        PanelStats.SetStats(aType, TheRobotConstructor);
        SmallIcon.SetComponent(aType,aPurch );
        bool alreadyHas = false;
        int qty = aPurch.ComponentsPurchasedIDs.Count(x=>x== aType.UniqueDeviceID);
        TextQty.text = qty+"";
       
        if (alreadyHas == false)
        {
           
           
        }
    }
}
