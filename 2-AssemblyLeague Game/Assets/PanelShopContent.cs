using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PanelShopContent : MonoBehaviour
{
    public Text TextPageNumber;
    public RobotConstructor TheConstructor;
    public RobotOwnerLookup TheLookUp;
    int currentPage = 0;
    public PanelShopViewComponent TheCompDetail;
    public List<PanelShopComponentIcon> AllIconPanels;
    public enum EnumCategory
    {
        All,
        Locomotion,
        CockpitsShoulders,
        Weaponry,
        Misc

    }
    EnumCategory currentCategor = EnumCategory.All;
    // Use this for initialization
    void Start()
    {
        AllIconPanels = new List<PanelShopComponentIcon>();
        AllIconPanels.AddRange(gameObject.transform.GetComponentsInChildren<PanelShopComponentIcon>());
        SwitchToCategory(EnumCategory.All);
    }
    public void OnEnable()
    {
        SwitchToCategory(currentCategor);
  
    }
    List<ComponentType> GetComponentsOfType(EnumCategory aCat)
    {
        List<ComponentType> ret = new List<ComponentType>();
        List<ComponentType> tmp = TheConstructor.GetAllComponent(true);
        ret.AddRange(tmp);
        ret = ret.OrderBy(x => x.OfflinePurchaseCost).ToList();
        return ret;
    }
    List<ComponentType> currentFullSetOfComponentsInCategory;
    private void SwitchToCategory(EnumCategory aCat)
    {
        currentCategor = aCat;
        currentFullSetOfComponentsInCategory = GetComponentsOfType(aCat);
        currentFullSetOfComponentsInCategory = currentFullSetOfComponentsInCategory.Where(x => x != null).ToList();
        switch (aCat)
        {

            case EnumCategory.Locomotion:
                {
                    List<ComponentType> tmp = new List<ComponentType>();
                    tmp.AddRange(currentFullSetOfComponentsInCategory.Where(x => x.ModuleType == ComponentType.EnumComponentType.Legs).ToList());
                    currentFullSetOfComponentsInCategory = tmp;
                    break;
                }
            case EnumCategory.CockpitsShoulders:
                {
                    List<ComponentType> tmp = new List<ComponentType>();
                    tmp.AddRange(currentFullSetOfComponentsInCategory.Where(x => x.ModuleType == ComponentType.EnumComponentType.Cockpit).ToList());
                    tmp.AddRange(currentFullSetOfComponentsInCategory.Where(x => x.ModuleType == ComponentType.EnumComponentType.Shoulders).ToList());
                    currentFullSetOfComponentsInCategory = tmp;
                    break;
                }
            case EnumCategory.Weaponry:
                {
                    List<ComponentType> tmp = new List<ComponentType>();
                    tmp.AddRange(currentFullSetOfComponentsInCategory.Where(x => x.ModuleType == ComponentType.EnumComponentType.WeaponAnySlot).ToList());
                    tmp.AddRange(currentFullSetOfComponentsInCategory.Where(x => x.ModuleType == ComponentType.EnumComponentType.WeaponLeft).ToList());
                    tmp.AddRange(currentFullSetOfComponentsInCategory.Where(x => x.ModuleType == ComponentType.EnumComponentType.WeaponRight).ToList());
                    currentFullSetOfComponentsInCategory = tmp;
                    break;
                }
            case EnumCategory.Misc:
                {
                    List<ComponentType> tmp = new List<ComponentType>();
                    tmp.AddRange(currentFullSetOfComponentsInCategory);
                    tmp = tmp.Where(x => x.ModuleType != ComponentType.EnumComponentType.Legs).ToList();
                    tmp = tmp.Where(x => x.ModuleType != ComponentType.EnumComponentType.Cockpit).ToList();
                    tmp = tmp.Where(x => x.ModuleType != ComponentType.EnumComponentType.Shoulders).ToList();
                    tmp = tmp.Where(x => x.ModuleType != ComponentType.EnumComponentType.WeaponAnySlot).ToList();
                    tmp = tmp.Where(x => x.ModuleType != ComponentType.EnumComponentType.WeaponLeft).ToList();
                    tmp = tmp.Where(x => x.ModuleType != ComponentType.EnumComponentType.WeaponRight).ToList();


                    currentFullSetOfComponentsInCategory = tmp;
                    break;
                }
        }
        currentFullSetOfComponentsInCategory = currentFullSetOfComponentsInCategory.OrderBy(x => x.OfflinePurchaseCost).ToList();
        currentPage = 0;
        TheCompDetail.gameObject.SetActive(false);
        ShowComponents();
    }
    public static bool Refresh = false;
    private void ShowComponents()
    {
        Refresh = false;
        RobotOwnerLookup.PlayerPurchases purchasesExisting = TheLookUp.GetPlayerPurchases();
        int startIndex = currentPage * AllIconPanels.Count;
        for (int c = 0; c < AllIconPanels.Count; c++)
        {
            if (startIndex + c < currentFullSetOfComponentsInCategory.Count)
            {

                ComponentType toSho = currentFullSetOfComponentsInCategory[startIndex + c];
                if (toSho != null)
                {
                    AllIconPanels[c].SetComponent(toSho, purchasesExisting);
                    AllIconPanels[c].gameObject.SetActive(true);
                }
                else
                {
                    print("Component Error: " + currentFullSetOfComponentsInCategory[startIndex + c]);
                    AllIconPanels[c].gameObject.SetActive(false);
                }
            }
            else
            {
                AllIconPanels[c].gameObject.SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Refresh)
        {
            ShowComponents();
        }
        TextPageNumber.text = currentPage + "";
    }

    public void ButtonPreviousPage()
    {
        currentPage--;
        if (currentPage < 0) currentPage = 0;
        ShowComponents();
    }
    public void ButtonNextPage()
    {
        currentPage++;
        if (currentPage * AllIconPanels.Count > currentFullSetOfComponentsInCategory.Count) currentPage--;
        ShowComponents();
    }

    public void ButtonLocomotionSystems()
    {
        SwitchToCategory(EnumCategory.Locomotion);
    }
    public void ButtonCockpits()
    {
        SwitchToCategory(EnumCategory.CockpitsShoulders);
    }
    public void ButtonWeaponry()
    {
        SwitchToCategory(EnumCategory.Weaponry);
    }
    public void ButtonMisc()
    {
        SwitchToCategory(EnumCategory.Misc);
    }
}
