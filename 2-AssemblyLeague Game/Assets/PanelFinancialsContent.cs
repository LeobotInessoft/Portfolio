using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PanelFinancialsContent : MonoBehaviour
{
    public Text TextPageNumber;
    public RobotConstructor TheConstructor;
    public RobotOwnerLookup TheLookUp;
    int currentPage = 0;
    public List<PanelFiancialRow> AllIconPanels;

    // Use this for initialization
    void Start()
    {
        AllIconPanels = new List<PanelFiancialRow>();
        AllIconPanels.AddRange(gameObject.transform.GetComponentsInChildren<PanelFiancialRow>());
        for (int c = 0; c < AllIconPanels.Count; c++)
        {
            AllIconPanels[c].MyPanelFinancialsContent = this;



        }
        ShowComponents();
    }


    List<xFinanceRow> currentFullSetOfComponentsInCategory;
    public void SetCurrentDataSet(List<xFinanceRow> rows)
    {
        currentFullSetOfComponentsInCategory = rows;
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
            if (currentFullSetOfComponentsInCategory == null)
            {
                AllIconPanels[c].gameObject.SetActive(false);
            }
            else
            {
                if (startIndex + c < currentFullSetOfComponentsInCategory.Count)
                {

                    xFinanceRow toSho = currentFullSetOfComponentsInCategory[startIndex + c];
                    if (toSho != null)
                    {
                        AllIconPanels[c].SetRow(toSho);
                        AllIconPanels[c].gameObject.SetActive(true);
                    }
                    else
                    {

                        AllIconPanels[c].gameObject.SetActive(false);
                    }
                }
                else
                {
                    AllIconPanels[c].gameObject.SetActive(false);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentFullSetOfComponentsInCategory != null)
        {
            if (currentPage * AllIconPanels.Count > currentFullSetOfComponentsInCategory.Count)
            {
                currentPage--;

                Refresh = true;
            }
            if (currentPage < 0)
            {
                currentPage = 0;
                Refresh = true;
            }
        }
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


}
