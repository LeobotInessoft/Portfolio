using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System;
using System.Xml;
using System.Linq;
public class RobotConstructor : MonoBehaviour
{
    public RobotOwnerLookup TheOwnerLookupFilter;
    public bool IsInputOn = true;
    public Transform BuildPlatformTarget;
    public List<GameObject> AllParts;

    public GameObject PartInHand;
    public ModuleAcceptor CurrentAttachTarget;
    int indexInList = 0;
    // Use this for initialization
    RigidbodyConstraints originalConstraints;
    public static RobotConstructor PublicAccess;

    public GameObject MuzzleFire_Bullet;
    public GameObject ExpendCasing_Bullet;

    public GameObject GetMuzzleFirePrefab()
    {
        GameObject ret = MuzzleFire_Bullet;

        return ret;
    }
    public GameObject GetExpendCasingPrefab()
    {
        GameObject ret = ExpendCasing_Bullet;

        return ret;
    }
    void Start()
    {
        PublicAccess = this;
        CurrentAttachTarget = BuildPlatformTarget.GetComponent<ModuleAcceptor>();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsInputOn)
        {
            

        }
        if (PartInHand != null && CurrentAttachTarget != null)
        {
            PartInHand.gameObject.transform.rotation = CurrentAttachTarget.gameObject.transform.rotation;
            PartInHand.gameObject.transform.localScale = CurrentAttachTarget.gameObject.transform.localScale;
        }

        if (PartInHand != null)
        {
           
        }
    }
    private void SaveRobot()
    {
        string fileName = "temp.txt";
        ComponentType aCompType = BuildPlatformTarget.GetComponentInChildren<ComponentType>();

      
        List<ComponentType> allUsedComponents = new List<ComponentType>();


        allUsedComponents.AddRange(BuildPlatformTarget.GetComponentsInChildren<ComponentType>());
        RobotTemplate aTemplate = new RobotTemplate();
        aTemplate.ModuleList = new List<DataComponentType>();

        aTemplate.ColorB = lastColor.b;
        aTemplate.ColorG = lastColor.g;
        aTemplate.ColorR = lastColor.r;
        for (int c = 0; c < allUsedComponents.Count; c++)
        {
            DataComponentType adata = GetComponetTypeData(allUsedComponents[c]);
            aTemplate.ModuleList.Add(adata);
        }
        RobotTemplate.SaveToFile(aTemplate, fileName);


    }
    public RobotTemplate GetRobotTemplate()
    {
        ComponentType aCompType = BuildPlatformTarget.GetComponentInChildren<ComponentType>();
        List<ComponentType> allUsedComponents = new List<ComponentType>();


        allUsedComponents.AddRange(BuildPlatformTarget.GetComponentsInChildren<ComponentType>());
        RobotTemplate aTemplate = new RobotTemplate();
        aTemplate.ModuleList = new List<DataComponentType>();
        for (int c = 0; c < allUsedComponents.Count; c++)
        {
            DataComponentType adata = GetComponetTypeData(allUsedComponents[c]);
            aTemplate.ModuleList.Add(adata);
        }
        aTemplate.ColorB = lastColor.b;
        aTemplate.ColorG = lastColor.g;
        aTemplate.ColorR = lastColor.r;

        return aTemplate;

    }

    public void TurnPartOnOff(GameObject anObject, bool isOn)
    {


        ComponentType aComp = anObject.GetComponent<ComponentType>();
        aComp.TurnPartOnOff(isOn);
        
    }
    public GameObject LoadRobot(Transform ParentToAttachTo, RobotTemplate aRobot)
    {
        GameObject retObject = null;
        ComponentType aCompType = BuildPlatformTarget.GetComponentInChildren<ComponentType>();
        for (int c = 0; c < ParentToAttachTo.childCount; c++)
        {
            Transform anObj = ParentToAttachTo.GetChild(c);
            anObj.parent = null;
            Destroy(anObj.gameObject);
        }
        

        if (aRobot != null)
        {

            List<DataComponentType> ComponentsLeft = new List<DataComponentType>();
            ComponentsLeft.AddRange(aRobot.ModuleList);

            bool isDone = false;
            int loopDown = 150;
            while (isDone == false && loopDown > 0)
            {
                loopDown--;
                if (ComponentsLeft.Count == 0)
                {
                    isDone = true;
                }

                if (isDone == false)
                {
                    int lowestLength = FindLowestLengthOfParentAcceptors(ComponentsLeft);
                    List<int> indexesToRemove = new List<int>();
                    for (int c = 0; c < ComponentsLeft.Count; c++)
                    {
                        if (ComponentsLeft[c].ParentAcceptorIDs.Count == lowestLength)
                        {
                            //instantiate it
                            GameObject anObject = GetPrefabByID(ComponentsLeft[c].ComponentID);

                            ModuleAcceptor anAcceptor = GetAcceptorFromRobotByID(ParentToAttachTo.gameObject, ComponentsLeft[c].ParentAcceptorIDs);
                            if (anObject == null) print("NOT FOUND GAMEOBJECT: " + ComponentsLeft[c].ComponentID);
                            if (anAcceptor == null) print("NOT FOUND ACCEPTOR for : " + ComponentsLeft[c].ComponentID);

                            if (anObject != null)
                            {
                                if (anAcceptor != null)
                                {
                                    PartInHand = (GameObject)Instantiate(anObject, anAcceptor.transform.position, anAcceptor.transform.rotation);
                                    PartInHand.gameObject.transform.parent = anAcceptor.transform;
                                }
                                else
                                {
                                    PartInHand = (GameObject)Instantiate(anObject, ParentToAttachTo.position, ParentToAttachTo.rotation);
                                    PartInHand.transform.parent = ParentToAttachTo;

                                }
                                if (retObject == null) retObject = PartInHand;
                                TurnPartOnOff(PartInHand, false);
                                Animator anAnimator = PartInHand.GetComponent<Animator>();
                                if (anAnimator != null)
                                {
                                    anAnimator.enabled = false;
                                }
                                List<Animator> anims = new List<Animator>();
                                anims.AddRange(PartInHand.GetComponentsInChildren<Animator>());
                                for (int x = 0; x < anims.Count; x++)
                                {
                                    anims[x].enabled = false;
                                }

                                ComponentType ct = (ComponentType)PartInHand.GetComponent<ComponentType>();

                                IOHandler io = (IOHandler)PartInHand.GetComponent<IOHandler>();
                                if (io != null)
                                {
                                    try
                                    {
                                        io.IONumber = Convert.ToInt32(ComponentsLeft[c].IoNumberCustomMap.Trim());
                                    }
                                    catch
                                    {
                                        io.IONumber = 0;
                                        ComponentsLeft[c].IoNumberCustomMap = "0";
                                    }
                                }
                                Computer pc = (Computer)PartInHand.GetComponent<Computer>();
                                if (pc != null)
                                {
                                    pc.CodeText = aRobot.MainFunctionCode;
                                }
                                {
                                    PartInHand = null;
                                }
                                indexesToRemove.Add(c);

                            }                         


                        }

                    }
                    if (indexesToRemove.Count > 0)
                    {
                        for (int c = indexesToRemove.Count - 1; c >= 0; c--)
                        {
                            ComponentsLeft.RemoveAt(indexesToRemove[c]);
                        }
                    }
                }

            }
        }
        if (retObject != null)
        {
            RobotMeta aMeta = retObject.GetComponent<RobotMeta>();
            aMeta.Template = aRobot;
            ApplyColour(new Color(aRobot.ColorR, aRobot.ColorG, aRobot.ColorB, 1));

        }
        return retObject;
    }
 
    public GameObject LoadRobot(Transform ParentToAttachTo)
    {
        string fileName = "temp.txt";
        GameObject retObject = null;
        ComponentType aCompType = BuildPlatformTarget.GetComponentInChildren<ComponentType>();
        for (int c = 0; c < ParentToAttachTo.childCount; c++)
        {
            Transform anObj = ParentToAttachTo.GetChild(c);
            anObj.parent = null;
            Destroy(anObj.gameObject);
        }
        RobotTemplate aRobot = RobotTemplate.LoadFromFile(fileName);


        if (aRobot != null)
        {
            List<DataComponentType> ComponentsLeft = new List<DataComponentType>();
            ComponentsLeft.AddRange(aRobot.ModuleList);

            bool isDone = false;
            int loopDown = 150;
            while (isDone == false && loopDown > 0)
            {
                loopDown--;
                if (ComponentsLeft.Count == 0)
                {
                    isDone = true;
                }

                if (isDone == false)
                {
                    int lowestLength = FindLowestLengthOfParentAcceptors(ComponentsLeft);
                    List<int> indexesToRemove = new List<int>();
                    for (int c = 0; c < ComponentsLeft.Count; c++)
                    {
                        if (ComponentsLeft[c].ParentAcceptorIDs.Count == lowestLength)
                        {
                            //instantiate it
                            GameObject anObject = GetPrefabByID(ComponentsLeft[c].ComponentID);

                            ModuleAcceptor anAcceptor = GetAcceptorFromRobotByID(ParentToAttachTo.gameObject, ComponentsLeft[c].ParentAcceptorIDs);
                            if (anObject == null) print("NOT FOUND GAMEOBJECT: " + ComponentsLeft[c].ComponentID);
                            if (anAcceptor == null) print("NOT FOUND ACCEPTOR for : " + ComponentsLeft[c].ComponentID);

                            if (anObject != null)
                            {
                                if (anAcceptor != null)
                                {
                                    print(ComponentsLeft[c].ComponentID + " " + ComponentsLeft[c].ParentAcceptorIDs[0] + " " + anAcceptor.UniqueDeviceID);
                                    PartInHand = (GameObject)Instantiate(anObject, anAcceptor.transform.position, anAcceptor.transform.rotation);
                                    PartInHand.gameObject.transform.parent = anAcceptor.transform;
                                }
                                else
                                {
                                    PartInHand = (GameObject)Instantiate(anObject, ParentToAttachTo.position, ParentToAttachTo.rotation);
                                    PartInHand.transform.parent = ParentToAttachTo;

                                }
                                if (retObject == null) retObject = PartInHand;
                                TurnPartOnOff(PartInHand, false);
                                Animator anAnimator = PartInHand.GetComponent<Animator>();
                                if (anAnimator != null)
                                {
                                    anAnimator.enabled = false;
                                }
                                List<Animator> anims = new List<Animator>();
                                anims.AddRange(PartInHand.GetComponentsInChildren<Animator>());
                                for (int x = 0; x < anims.Count; x++)
                                {
                                    anims[x].enabled = false;
                                }

                                ComponentType ct = (ComponentType)PartInHand.GetComponent<ComponentType>();
                                {
                                    PartInHand = null;
                                }



                                indexesToRemove.Add(c);

                            }
                           


                        }

                    }
                    if (indexesToRemove.Count > 0)
                    {
                        for (int c = indexesToRemove.Count - 1; c >= 0; c--)
                        {
                            ComponentsLeft.RemoveAt(indexesToRemove[c]);
                        }
                    }
                }

            }
        }
        if (retObject != null)
        {
            RobotMeta aMeta = retObject.GetComponent<RobotMeta>();
            aMeta.Template = aRobot;
            aMeta.FileName = fileName;
        }
        return retObject;
    }

    public GameObject GetPrefabByID(string ID)
    {
        GameObject ret = null;

        for (int c = 0; c < AllParts.Count; c++)
        {
            if (AllParts[c] != null)
            {
                ComponentType aType = (ComponentType)AllParts[c].GetComponent<ComponentType>();
                if (aType != null)
                {
                    if (aType.UniqueDeviceID == ID)
                    {
                        ret = AllParts[c];
                        break;
                    }
                }
            }
        }
        return ret;
    }
    private ModuleAcceptor GetAcceptorFromRobotByID(GameObject baseRobot, List<string> ParentalIDs)
    {
        ModuleAcceptor ret = null;
         GameObject currentZoom = baseRobot;
        if (ParentalIDs.Count > 0)
        {
            {
                List<ModuleAcceptor> possibleAcceptors = new List<ModuleAcceptor>();
                possibleAcceptors.AddRange(currentZoom.GetComponentsInChildren<ModuleAcceptor>());
                ModuleAcceptor theAcceptor = null;
                for (int x = 0; x < possibleAcceptors.Count; x++)
                {
                    if (possibleAcceptors[x].UniqueDeviceID == ParentalIDs[0])
                    {
                        theAcceptor = possibleAcceptors[x];
                        // break;
                    }
                }
                if (theAcceptor != null)
                {
                    currentZoom = theAcceptor.transform.gameObject;
                    ret = theAcceptor;
                }
            }

        }

        return ret;
    }
    private int FindLowestLengthOfParentAcceptors(List<DataComponentType> comps)
    {
        int ret = -1;

        int lowestVal = int.MaxValue;
        for (int c = 0; c < comps.Count; c++)
        {
            if (comps[c].ParentAcceptorIDs.Count < lowestVal)
            {
                lowestVal = comps[c].ParentAcceptorIDs.Count;
            }
        }
        ret = lowestVal;
        return ret;
    }
    private DataComponentType GetComponetTypeData(ComponentType aComponent)
    {
        DataComponentType ret = new DataComponentType();
        ret.ComponentID = aComponent.UniqueDeviceID;
        ret.ParentAcceptorIDs = GetParentAcceptors(aComponent);
        return ret;
    }
    public List<string> GetParentAcceptors(ComponentType aComponent)
    {
        List<string> ret = new List<string>();
        bool mustStop = false;

        Transform currTransform = aComponent.gameObject.transform;
        while (mustStop == false)
        {
            currTransform = currTransform.parent;
            if (currTransform == null)
            {
                mustStop = true;
            }
            if (mustStop == false)
            {
                ModuleAcceptor anAcceptor = currTransform.gameObject.GetComponent<ModuleAcceptor>();
                if (anAcceptor != null)
                {
                    FakeAcceptor aFaked = currTransform.gameObject.GetComponent<FakeAcceptor>();
                    if (aFaked == null)
                    {
                        ret.Add(anAcceptor.UniqueDeviceID);
                    }
                }
            }
        }
        return ret;

    }


    public int currentPartAcceptorIndex = 0;

    public void SwitchToNextPartAcceptor(int dir, bool skipAlreadyUsed)
    {
        List<ModuleAcceptor> allAcceptors = new List<ModuleAcceptor>();
        allAcceptors.AddRange(BuildPlatformTarget.GetComponentsInChildren<ModuleAcceptor>());
        List<int> indexToRemove = new List<int>();
        for (int c = 0; c < allAcceptors.Count; c++)
        {
            List<ComponentType> childComps = new List<ComponentType>();
            childComps.AddRange(allAcceptors[c].gameObject.transform.GetComponentsInChildren<ComponentType>());

           

            if (childComps.Count > 0)
            {
                allAcceptors[c].HasAttachment = true;
                indexToRemove.Add(c);
            }
            else
            {
                allAcceptors[c].HasAttachment = false;

            }

        }
        if (skipAlreadyUsed)
        {
            for (int c = indexToRemove.Count - 1; c >= 0; c--)
            {
                allAcceptors.RemoveAt(indexToRemove[c]);
            }
            if (allAcceptors.Count > 1)
            {
                allAcceptors.RemoveAt(0);
            }
        }
        if (allAcceptors.Count > 0)
        {
            currentPartAcceptorIndex += dir;
            if (currentPartAcceptorIndex < 0)
            {
                currentPartAcceptorIndex = allAcceptors.Count - 1;
            }
            if (currentPartAcceptorIndex >= allAcceptors.Count)
            {
                currentPartAcceptorIndex = 0;
            }
            {
                if (currentPartAcceptorIndex < allAcceptors.Count)
                {
                    if (allAcceptors.Count > 0)
                    {
                        CurrentAttachTarget = allAcceptors[currentPartAcceptorIndex];
                    }
                    else
                    {
                        CurrentAttachTarget = null;
                    }
                }
            }
            
        }
        else
        {
            CurrentAttachTarget = null;
        }
    }

    public void SwitchToNextPartInList(int dir, int stopPos, RobotOwnerLookup filterLookup)
    {
        indexInList += dir;
        if (indexInList < 0)
        {
            indexInList = AllParts.Count - 1;
        }
        if (indexInList >= AllParts.Count)
        {
            indexInList = 0;
        }



        if (indexInList >= AllParts.Count) indexInList = 0;

        if (PartInHand != null)
        {
            Destroy(PartInHand);
            PartInHand = null;
        }

        if (AllParts[indexInList] != null && CurrentAttachTarget != null)
        {

            ComponentType ct = (ComponentType)AllParts[indexInList].GetComponent<ComponentType>();
            if (ct != null)
            {
                if (filterLookup != null)
                {
                    RobotOwnerLookup.PlayerPurchases aPurch = filterLookup.GetPlayerPurchases();
                    if (aPurch.ComponentsPurchasedIDs.Contains(ct.UniqueDeviceID) == false)
                    {
                       ct = null;
                    }
                }
            }
            if (ct != null && ct.IsGameReady && CurrentAttachTarget.AllowedTypes.Contains(ct.ModuleType))
            {
                PartInHand = (GameObject)Instantiate(AllParts[indexInList], CurrentAttachTarget.transform.position, CurrentAttachTarget.transform.rotation);

                Animator anAnimator = PartInHand.GetComponent<Animator>();
                if (anAnimator != null)
                {
                    anAnimator.enabled = false;
                }
                List<Animator> anims = new List<Animator>();
                anims.AddRange(PartInHand.GetComponentsInChildren<Animator>());
                for (int c = 0; c < anims.Count; c++)
                {
                    anims[c].enabled = false;
                }
                TurnPartOnOff(PartInHand, false);
            }
            else
            {
                if (stopPos != indexInList)
                {
                    SwitchToNextPartInList(dir, stopPos, TheOwnerLookupFilter);
                }
            }
        }
        else
        {
            if (stopPos != indexInList)
            {
                SwitchToNextPartInList(dir, stopPos, TheOwnerLookupFilter);
            }
        }


    }
    public bool IsSpaceOpenForComponent(ModuleAcceptor anAcceptor)
    {
        bool ret = true;
        ComponentType parentComponentType = FindParentComponent(anAcceptor);
        if (anAcceptor.AllowedTypes.Contains(ComponentType.EnumComponentType.WeaponLeft) || anAcceptor.AllowedTypes.Contains(ComponentType.EnumComponentType.WeaponRight))
        {
            if (parentComponentType != null && parentComponentType.ModuleType == ComponentType.EnumComponentType.Cockpit)
            {
                if (parentComponentType.gameObject.transform.parent != null)
                {
                    ModuleAcceptor possibleShoulders = parentComponentType.gameObject.transform.parent.GetComponent<ModuleAcceptor>();
                    if (possibleShoulders != null)
                    {
                        ComponentType parentofParentComponentType = FindParentComponent(possibleShoulders);
                        if (parentofParentComponentType.ModuleType == ComponentType.EnumComponentType.Shoulders)
                        {
                            print("FOUND SHOULDERS ATTACHED");
                            ret = false;
                        }
                    }

                }

            }
        }
        return ret;
    }
    private ComponentType FindParentComponent(ModuleAcceptor aChild)
    {
        ComponentType ret = null;
        bool MustEnd = false;
        Transform pt = aChild.gameObject.transform.parent;
        while (MustEnd == false)
        {
            if (pt == null) MustEnd = true;

            if (MustEnd == false)
            {
                ComponentType testType = pt.GetComponent<ComponentType>();
                if (testType != null)
                //if (testType.ModuleType == aType)
                {
                    ret = testType;
                    MustEnd = true;


                }
                pt = pt.gameObject.transform.parent;
            }
        }

        return ret;
    }
    private ComponentType FindParentComponent(ModuleAcceptor aChild, ComponentType.EnumComponentType aType)
    {
        ComponentType ret = null;
        bool MustEnd = false;
        Transform pt = aChild.gameObject.transform.parent;
        while (MustEnd == false)
        {
            if (pt == null) MustEnd = true;

            if (MustEnd == false)
            {
                ComponentType testType = pt.GetComponent<ComponentType>();
                if (testType != null)
                    if (testType.ModuleType == aType)
                    {
                        ret = testType;
                        MustEnd = true;


                    }
                pt = pt.gameObject.transform.parent;
            }
        }

        return ret;
    }
    public void ApplyPart()
    {
        indexInList = 0;
        ComponentType ct = (ComponentType)PartInHand.GetComponent<ComponentType>();
        IOHandler anio = PartInHand.GetComponent<IOHandler>();
        if (anio != null)
        {
            if (ct.ModuleType == ComponentType.EnumComponentType.Legs) anio.IONumber = 1;
            if (ct.ModuleType == ComponentType.EnumComponentType.Cockpit) anio.IONumber = 2;
            if (ct.ModuleType == ComponentType.EnumComponentType.Shoulders) anio.IONumber = 2;
            if (ct.ModuleType == ComponentType.EnumComponentType.WeaponAnySlot) anio.IONumber = 3;

            print("Setting IO TO: " + anio.IONumber);


        }
       
        if (CurrentAttachTarget.AllowedTypes.Contains(ct.ModuleType))
        {
          
            PartInHand.gameObject.transform.parent = CurrentAttachTarget.gameObject.transform;
            // PartInHand.gameObject.transform.rotation = CurrentAttachTarget.gameObject.transform.rotation;
            PartInHand = null;
            RobotOwnerLookup.PublicAccess.DoInstall(ct);
     
        }
        else
        {
            Destroy(PartInHand);
            PartInHand = null;
        }
    }
    Color lastColor;
    public void ApplyColour(Color newColor)
    {
        print("CHANGE COLOR");
        {
            lastColor = newColor;
        }
        {
            List<MeshRenderer> allRenderers = new List<MeshRenderer>();
            allRenderers.AddRange(BuildPlatformTarget.GetComponentsInChildren<MeshRenderer>());

            for (int c = 0; c < allRenderers.Count; c++)
            {
                allRenderers[c].material.color = newColor;
            }
        }
        {
            List<SkinnedMeshRenderer> allRenderers = new List<SkinnedMeshRenderer>();
            allRenderers.AddRange(BuildPlatformTarget.GetComponentsInChildren<SkinnedMeshRenderer>());

            for (int c = 0; c < allRenderers.Count; c++)
            {
                allRenderers[c].material.color = newColor;
            }
        }
    }
    public DataComponentType GetDatComponentTypeFromObject(GameObject anObj)
    {
        DataComponentType ret = null;
        ComponentType aMainType = anObj.GetComponent<ComponentType>();
        if (aMainType != null)
        {
            ret = new DataComponentType();
            ret.ComponentID = aMainType.UniqueDeviceID;
            List<ModuleAcceptor> allAcceptors = new List<ModuleAcceptor>();
            List<ModuleAcceptor> usableAcceptors = new List<ModuleAcceptor>();

            allAcceptors.AddRange(anObj.gameObject.GetComponentsInChildren<ModuleAcceptor>());
            for (int c = 0; c < allAcceptors.Count; c++)
            {
                ComponentType aParent = FindParentComponent(allAcceptors[c]);
                if (aParent.transform == aMainType.transform)
                {
                    usableAcceptors.Add(allAcceptors[c]);
                }
            }
            print("Found " + usableAcceptors.Count + " usable acceptors here");

        }

        return ret;
    }
    public List<ComponentType> GetAllComponent(bool OnlyGameReady)
    {
        List<ComponentType> ret = new List<ComponentType>();
        for (int c = 0; c < AllParts.Count; c++)
        {
            if (AllParts[c] != null)
            {
                ComponentType aType = AllParts[c].GetComponent<ComponentType>();
                if (aType != null)
                {
                    if (aType.IsGameReady || OnlyGameReady == false)
                    {
                        ret.Add(aType);
                    }
                }
            }
        }
        return ret;
    }
    public ComponentType GetComponentByID(string id)
    {
        ComponentType ret = null;
        for (int c = 0; c < AllParts.Count; c++)
        {
            if (AllParts[c] != null)
            {
                ComponentType aType = AllParts[c].GetComponent<ComponentType>();
                if (aType != null)
                {
                    if (aType.UniqueDeviceID == id)
                    {
                        ret = aType;
                        break;
                    }
                }
            }
        }
        return ret;
    }
    public List<ComponentType> GetAllComponentsOfType(ComponentType.EnumComponentType compType)
    {
        List<ComponentType> ret = new List<ComponentType>();
        for (int c = 0; c < AllParts.Count; c++)
        {
            if (AllParts[c] != null)
            {
                ComponentType aType = AllParts[c].GetComponent<ComponentType>();
                if (aType != null)
                {
                    if (aType.ModuleType == compType)
                    {
                        ret.Add(aType);
                    }
                }
            }
        }
        return ret;
    }


    [Serializable]
    public class RobotTemplate
    {
     
        public List<DataComponentType> ModuleList;
        public int LeagueID;
        public string LeagueOwnerName;
        public string OwnerID;
        public int LeagueOwnerID;
        public string RobotID;
        public string FileName;
        public string RobotName;
        public string TeamPassword;

        public string MainFunctionCode;
        public float ColorR;
        public float ColorG;
        public float ColorB;

        public float TotalWins;
        public float TotalMatches;
        public float TotalKills;
        public float TotalDeaths;
        public byte[] ScreenShot;

        public float CalculateTotalWeight;
        public float CalculateMaxSpeed;
        public float CalculateSqrMeterSize;
        public float CalculateLinesOfCode;
        public float CalculateCodeProcessedPerSecond;
        public float CalculateCodeTotalLeagueScore;
        public float CalculateTotalArmour;
      
       
        public static void SaveToFile(RobotTemplate aRobot, string fileName)
        {

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName,
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, aRobot);
            stream.Close();
        }
        public static RobotTemplate LoadFromFile(string fileName)
        {
            RobotTemplate ret = null;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
            ret = (RobotTemplate)formatter.Deserialize(stream);
            stream.Close();
            return ret;
        }


        public float CalculateTotalWeightValue(RobotConstructor theConstructor, RobotConstructor.RobotTemplate temp)
        {
            float Total = 0;
            List<RobotConstructor.DataComponentType> comps = temp.ModuleList;
            List<ComponentType> types = new List<ComponentType>();
            for (int c = 0; c < comps.Count; c++)
            {
                types.Add(theConstructor.GetComponentByID(comps[c].ComponentID));
            }
            if (types.Count > 0)
            {
                Total = types.Sum(x => x.WeightInKG);
            }
            return Total;

        }
        public float CalculateMaxSpeedValue(RobotConstructor theConstructor, RobotConstructor.RobotTemplate temp, float actualWeight)
        {
            float Total = 0;
            List<RobotConstructor.DataComponentType> comps = temp.ModuleList;
            List<ComponentType> types = new List<ComponentType>();
            for (int c = 0; c < comps.Count; c++)
            {
                types.Add(theConstructor.GetComponentByID(comps[c].ComponentID));
            }
            if (types.Count > 0)
            {
                Total = types.Sum(x => x.Legs_MaxForwardSpeed);
            }
            return (Total / actualWeight) * 4000;

        }
        public float CalculateSqrMeterSizeValue(GameObject theObject)
        {
            float Total = ObjectSizeFinder.PublicAccess.FindSize(theObject);

            return Total;

        }
        public float CalculateLinesOfCodeValue(RobotConstructor theConstructor, RobotConstructor.RobotTemplate temp)
        {
            float Total = 0;
            if (temp.MainFunctionCode != null)
            {
                Total = temp.MainFunctionCode.Count(x => x == '\n');

            }
            return Total;

        }
        public float CalculateCodeProcessedPerSecondValue(RobotConstructor theConstructor, RobotConstructor.RobotTemplate temp)
        {
            float Total = 0;
            List<RobotConstructor.DataComponentType> comps = temp.ModuleList;
            List<ComponentType> types = new List<ComponentType>();
            for (int c = 0; c < comps.Count; c++)
            {
                types.Add(theConstructor.GetComponentByID(comps[c].ComponentID));
            }
            if (types.Count > 0)
            {
                Total = types.Sum(x => x.InstructionsPerSecond);
            }
            return Total;

        }
        public float CalculateCalculateTotalArmourValue(RobotConstructor theConstructor, RobotConstructor.RobotTemplate temp)
        {
            float Total = 0;
            List<RobotConstructor.DataComponentType> comps = temp.ModuleList;
            List<ComponentType> types = new List<ComponentType>();
            for (int c = 0; c < comps.Count; c++)
            {
                types.Add(theConstructor.GetComponentByID(comps[c].ComponentID));
            }
            if (types.Count > 0)
            {
                Total = types.Sum(x => x.Armour);
            }
            return Total;

        }
        
    }
    [Serializable]
    public class DataComponentType
    {
        public string ComponentID;
        public DataIoHandler MyIoMap;
        public List<string> ParentAcceptorIDs;
        public string IoNumberCustomMap;


    }
    [Serializable]
    public class DataModuleAcceptor
    {
        public string AcceptorID;
        public string MapToComponentID;
    }
    [Serializable]
    public class DataIoHandler
    {
        public string IoDeviceID;
        public string MapToInterrupNumber;

    }
}
