using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraSwitcherPanel : MonoBehaviour
{
    public Dropdown DropdownRobots;
    public static CameraSwitcherPanel PublicAccess;
    List<RobotMeta> viewedRobots;
    RobotMeta currentSelected = null;
    // Use this for initialization
    void Start()
    {
        PublicAccess = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObjectFollower.PublicAccess.GameObjectToFollow != null)
        {
            if (currentSelected == null)
            {
                SetSelect(GameObjectFollower.PublicAccess.GameObjectToFollow);
            }
            else
            {
                if (currentSelected.transform != GameObjectFollower.PublicAccess.GameObjectToFollow.transform)
                {
                    SetSelect(GameObjectFollower.PublicAccess.GameObjectToFollow);
                }

            }
        }

    }
    private void SetSelect(GameObject anObject)
    {
        for (int c = 0; c < viewedRobots.Count; c++)
        {
            if (viewedRobots[c].transform == anObject.transform)
            {
                currentSelected = viewedRobots[c];
                DropdownRobots.value = c;
                break;
            }
        }

    }
    public void SetInitToggle()
    {

    }
    public void ToggleFollowCam(bool val)
    {
        if (val == true)
        {
            GameObjectFollower.PublicAccess.SwitchToCam(GameObjectFollower.EnumFollowType.Behind);
        }
    }
    public void ToggleOrbitCam(bool val)
    {
        if (val == true)
        {
            GameObjectFollower.PublicAccess.SwitchToCam(GameObjectFollower.EnumFollowType.Orbit);
        }
    }
    public void ToggleFreeCam(bool val)
    {
        if (val == true)
        {
            GameObjectFollower.PublicAccess.SwitchToCam(GameObjectFollower.EnumFollowType.FreeCam);
        }
    }
    public void SetUpDropdownList(List<RobotMeta> allMetas)
    {
        

        DropdownRobots.ClearOptions();
        List<Dropdown.OptionData> lst = new List<Dropdown.OptionData>();
        int sel = 0;
        for (int c = 0; c < allMetas.Count; c++)
        {
            Dropdown.OptionData anOp = new Dropdown.OptionData(allMetas[c].Template.RobotName);

            lst.Add(anOp);
            if (allMetas[c].RuntimeRobotID ==Match.PlayAsRobotID || allMetas[c].RuntimeRobotLeagueID== Match.PlayAsRobotID)
            {
                sel = c;
                currentSelected = allMetas[c];
            }
        }

        DropdownRobots.AddOptions(lst);
        viewedRobots = allMetas;
        print("Sel : " + sel);
        DropdownRobots.value = sel;
    }
    public void DropDownChange()
    {
        GameObjectFollower.PublicAccess .AutoFollowKillerOnDeath = false;
        print("Drop Change");
        RobotMeta selRobot = viewedRobots[DropdownRobots.value];
        GameObjectFollower.PublicAccess.SwitchToRoboto(selRobot);
       
    }

}

