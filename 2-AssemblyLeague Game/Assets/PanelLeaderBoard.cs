using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PanelLeaderBoard : MonoBehaviour
{
    public GameObject BaseIoRow;
    public Transform ParentToAttachIORowTo;

    public List<RobotLeaderboardRow> AllSpawnedIoRows;

    public static PanelLeaderBoard PublicAccess;
    // Use this for initialization
    void Start()
    {

        PublicAccess = this;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClearLeaderboard()
    {
        if (AllSpawnedIoRows == null) AllSpawnedIoRows = new List<RobotLeaderboardRow>();

        for (int c = 0; c < AllSpawnedIoRows.Count; c++)
        {
            Destroy(AllSpawnedIoRows[c].gameObject);
        }
        AllSpawnedIoRows.Clear();
        AllSpawnedIoRows = new List<RobotLeaderboardRow>();
    }
    private void ClearIORows()
    {
        if (AllSpawnedIoRows == null) AllSpawnedIoRows = null;

        for (int c = 0; c < AllSpawnedIoRows.Count; c++)
        {
            Destroy(AllSpawnedIoRows[c].gameObject);
        }
        AllSpawnedIoRows.Clear();
        AllSpawnedIoRows = new List<RobotLeaderboardRow>();
    }
    public void SetLeaderboard(List<GameObject> robots)
    {
      //  ClearIORows();
        for (int c = 0; c < robots.Count; c++)
        {
     SpawnRow(robots[c].GetComponent<RobotMeta>());
        }
    }
    public void UpdateLeaderboard(List<GameObject> robots)
    {

        try
        {
            List<RobotMeta> allMetas = new List<RobotMeta>();
            for (int c = 0; c < robots.Count; c++)
            {
                allMetas.Add(robots[c].GetComponent<RobotMeta>());
            }
            allMetas = allMetas.OrderByDescending(x => x.RuntimeScore).ToList();
            for (int c = 0; c < allMetas.Count; c++)
            {
                RobotLeaderboardRow aRow = AllSpawnedIoRows[c];// GetRow(aMeta);
                aRow.SetRow(allMetas[c]);
                aRow.aMeta.RuntimeRank = c+1;
            }

           

        }
        catch
        {
            print("ERROR");
        }
    }
    public void SetRowPosition2(RobotLeaderboardRow aRow, int pos)
    {
        RectTransform aRect = aRow.gameObject.GetComponent<RectTransform>();

        //aRect.offsetMin = new Vector2(0, (pos + 1) * -1 * 40);
        //aRect.offsetMax = new Vector2(0, (pos + 1) * -1 * 40);
        
    }
    public RobotLeaderboardRow GetRow(RobotMeta aMeta)
    {
        RobotLeaderboardRow ret = null;
        for (int c = 0; c < AllSpawnedIoRows.Count; c++)
        {
            if (AllSpawnedIoRows[c].aMeta.RuntimeRobotID == aMeta.RuntimeRobotID)
            {
                ret = AllSpawnedIoRows[c];
                break;
            }
        }
        return ret;

    }
   
    RobotLeaderboardRow SpawnRow(RobotMeta aRobot)
    {
        
        RobotLeaderboardRow ret = null;
        //  ComponentType comonentType = theIOHandler.gameObject.GetComponent<ComponentType>();

        //FunctionsContainer[] interfaces = comonentType.gameObject.GetInterfaces<FunctionsContainer>();

        //for (int c = 0; c < interfaces.Length; c++)
        //{
        //    interfaces[c].SetComponentFunctions(comonentType, theIOHandler.IONumber.ToString());
        //}
        GameObject aSpawn = (GameObject)Instantiate(BaseIoRow, BaseIoRow.transform.position, BaseIoRow.transform.rotation);
        aSpawn.transform.SetParent(ParentToAttachIORowTo);
        aSpawn.transform.localPosition = Vector3.zero;
        RectTransform aRect = aSpawn.GetComponent<RectTransform>();
        //  aRect.offsetMin = new Vector2(aRect.offsetMin.x,0);
        //aRect.offsetMax = new Vector2(aRect.offsetMax.x, (AllSpawnedIoRows.Count + 2) * -1 * 50);

        aRect.offsetMin = new Vector2(0, (AllSpawnedIoRows.Count*2 + 1) * -1 * 40);
        aRect.offsetMax = new Vector2(0, (AllSpawnedIoRows.Count*2 + 1) * -1 * 40);

        ret = aSpawn.GetComponent<RobotLeaderboardRow>();
        //ret.TheRobotComponent = aTemplate;
        //ret.TheCanvas = this;
        //ret.PrefebRepresenting = TheConstructor.GetPrefabByID(aTemplate.ComponentID);
        //ret.PanelIoMap = TheIoPanel;
        //ret.HelpManual = comonentType;
        ret.SetRow(aRobot);
        aSpawn.SetActive(true);
        AllSpawnedIoRows.Add(ret);
        return ret;
    }

}
