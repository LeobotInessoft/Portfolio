using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlane : MonoBehaviour {
    public List<Transform> SpawnList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public Transform GetSpawnPosition(int pos, int total)
    {
        Transform ret= null;
        if (total == 2)
        {
            switch (pos)
            {
                case 0:
                    {
                        ret=SpawnList[0];
                        break;
                    }
                case 1:
                    {
                        ret = SpawnList[2];
                        break;
                    }
            }
        }
        else
        {
            if (total == 4)
            {
                switch (pos)
                {
                    case 0:
                        {
                            ret = SpawnList[0];
                            break;
                        }
                    case 1:
                        {
                            ret = SpawnList[1];
                            break;
                        }
                    case 2:
                        {
                            ret = SpawnList[2];
                            break;
                        }
                    case 3:
                        {
                            ret = SpawnList[3];
                            break;
                        }
                }
            }
            else
            {
                if (total == 5)
                {
                    switch (pos)
                    {
                        case 0:
                            {
                                ret = SpawnList[0];
                                break;
                            }
                        case 1:
                            {
                                ret = SpawnList[1];
                                break;
                            }
                        case 2:
                            {
                                ret = SpawnList[2];
                                break;
                            }
                        case 3:
                            {
                                ret = SpawnList[3];
                                break;
                            }
                        case 4:
                            {
                                ret = SpawnList[4];
                                break;
                            }
                    }
                }
                else
                {
                    if (total == 6)
                    {
                        switch (pos)
                        {
                            case 0:
                                {
                                    ret = SpawnList[0];
                                    break;
                                }
                            case 1:
                                {
                                    ret = SpawnList[1];
                                    break;
                                }
                            case 2:
                                {
                                    ret = SpawnList[2];
                                    break;
                                }
                            case 3:
                                {
                                    ret = SpawnList[3];
                                    break;
                                }
                            case 4:
                                {
                                    ret = SpawnList[5];
                                    break;
                                }
                            case 5:
                                {
                                    ret = SpawnList[6];
                                    break;
                                }
                        }
                    }
                    else
                    {
                        if (total == 7)
                        {
                            switch (pos)
                            {
                                case 0:
                                    {
                                        ret = SpawnList[0];
                                        break;
                                    }
                                case 1:
                                    {
                                        ret = SpawnList[1];
                                        break;
                                    }
                                case 2:
                                    {
                                        ret = SpawnList[2];
                                        break;
                                    }
                                case 3:
                                    {
                                        ret = SpawnList[3];
                                        break;
                                    }
                                case 4:
                                    {
                                        ret = SpawnList[4];
                                        break;
                                    }
                                case 5:
                                    {
                                        ret = SpawnList[5];
                                        break;
                                    }
                                case 6:
                                    {
                                        ret = SpawnList[6];
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            switch (pos)
                            {
                                case 0:
                                    {
                                        ret = SpawnList[0];
                                        break;
                                    }
                                case 1:
                                    {
                                        ret = SpawnList[1];
                                        break;
                                    }
                                case 2:
                                    {
                                        ret = SpawnList[2];
                                        break;
                                    }
                                case 3:
                                    {
                                        ret = SpawnList[3];
                                        break;
                                    }
                                case 4:
                                    {
                                        ret = SpawnList[4];
                                        break;
                                    }
                                case 5:
                                    {
                                        ret = SpawnList[5];
                                        break;
                                    }
                                case 6:
                                    {
                                        ret = SpawnList[6];
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
        return ret;
    }
}
