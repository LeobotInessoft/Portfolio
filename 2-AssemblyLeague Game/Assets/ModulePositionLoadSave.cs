using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
public class ModulePositionLoadSave : MonoBehaviour
{
    public RobotTemplate TheTemplate;
    public string TemplateFileName = "tmp.txt";
    public bool MustSave = false;
    public bool MustLoad = false;
    public List<GameObject> ObjectTypes;
    // Use this for initialization
    void Start()
    {
        TheTemplate = new RobotTemplate();
    }

    // Update is called once per frame
    void Update()
    {
        if (MustSave)
        {
            MustSave = false;
            SaveTemplate();
        }
        if (MustLoad)
        {
            MustLoad = false;
            LoadTemplate();
        }
    }
    private void SaveTemplate()
    {
        List<PositionOnParent> positions = new List<PositionOnParent>();
        positions.AddRange(gameObject.GetComponentsInChildren<PositionOnParent>());
        RobotTemplate temp = new RobotTemplate();
        temp.ModuleList = new List<Module>();
        for (int c = 0; c < positions.Count; c++)
        {
            Module aMod = new Module();
            aMod.DeviceUniqueID = positions[c].ParentDeviceID;
            aMod.LocalPosX = positions[c].Position.x;
            aMod.LocalPosY = positions[c].Position.y;
            aMod.LocalPosZ = positions[c].Position.z;

            aMod.LocalRotX = positions[c].Rotation.x;
            aMod.LocalRotY = positions[c].Rotation.y;
            aMod.LocalRotZ = positions[c].Rotation.z;
            temp.ModuleList.Add(aMod);
        }
        RobotTemplate.SaveToFile(temp, TemplateFileName);

    }
    private void LoadTemplate()
    {
        RobotTemplate temp = RobotTemplate.LoadFromFile(TemplateFileName);

        for (int c = 0; c < temp.ModuleList.Count; c++)
        {
            
            print("Found " + c);
            //instatiate each
        }

    }
    private GameObject GetObjectOfType()
    {
        GameObject ret = null;
        for (int c = 0; c < ObjectTypes.Count; c++)
        {
            IOHandler handler = ObjectTypes[c].GetComponent<IOHandler>();

        }
        return ret;

    }
    [Serializable]
    public class RobotTemplate
    {
        public int ChasisID;
        public List<Module> ModuleList;
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
    }
    public class Module
    {
        public int DeviceUniqueID;
        public float LocalPosX;
        public float LocalPosY;
        public float LocalPosZ;
        public float LocalRotX;
        public float LocalRotY;
        public float LocalRotZ;

    }
}
