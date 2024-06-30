using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
public class MatchLoadWaitCanvas : MonoBehaviour
{
    public WwwTTSLoader TheWwwTts;
    public Text TextLevelName;
    public Text TextPrizeMoney;
    public RobotOwnerLookup TheLookup;
    public ArenaLookup ArenaLookup;
    public PanelRobotMatchPreview Prev1;
    public PanelRobotMatchPreview Prev2;
    public PanelRobotMatchPreview Prev3;
    public PanelRobotMatchPreview Prev4;
    public PanelRobotMatchPreview Prev5;
    public PanelRobotMatchPreview Prev6;
    public PanelRobotMatchPreview Prev71;

    float waitStart = 25f;
    // Use this for initialization

    private PanelRobotMatchPreview GetPrevInPos(int totalPlayers, int index)
    {
        PanelRobotMatchPreview ret = null;
        print("Getting pos " + index + "/" + totalPlayers);
        switch (totalPlayers)
        {
            case 1:
                {
                    ret = Prev1;
                    break;
                }
            case 2:
                {
                    switch (index)
                    {
                        case 0:
                            {
                                ret = Prev71;
                                break;
                            }
                        case 1:
                            {
                                ret = Prev6;
                                break;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch (index)
                    {
                        case 0:
                            {
                                ret = Prev71;
                                break;
                            }
                        case 1:
                            {
                                ret = Prev6;
                                break;
                            }
                        case 2:
                            {
                                ret = Prev1;
                                break;
                            }
                    }
                    break;
                }
            case 4:
                {
                    switch (index)
                    {
                        case 0:
                            {
                                ret = Prev2;
                                break;
                            }
                        case 1:
                            {
                                ret = Prev3;
                                break;
                            }
                        case 2:
                            {
                                ret = Prev4;
                                break;
                            }
                        case 3:
                            {
                                ret = Prev5;
                                break;
                            }
                    }
                    break;
                }
            case 5:
                {
                    switch (index)
                    {
                        case 0:
                            {
                                ret = Prev2;
                                break;
                            }
                        case 1:
                            {
                                ret = Prev3;
                                break;
                            }
                        case 2:
                            {
                                ret = Prev4;
                                break;
                            }
                        case 3:
                            {
                                ret = Prev5;
                                break;
                            }
                        case 4:
                            {
                                ret = Prev1;
                                break;
                            }
                    }
                    break;
                }
            case 6:
                {
                    switch (index)
                    {
                        case 0:
                            {
                                ret = Prev2;
                                break;
                            }
                        case 1:
                            {
                                ret = Prev3;
                                break;
                            }
                        case 2:
                            {
                                ret = Prev4;
                                break;
                            }
                        case 3:
                            {
                                ret = Prev5;
                                break;
                            }
                        case 4:
                            {
                                ret = Prev71;
                                break;
                            }
                        case 5:
                            {
                                ret = Prev6;
                                break;
                            }
                    }
                    break;
                }
            case 7:
                {
                    switch (index)
                    {
                        case 0:
                            {
                                ret = Prev2;
                                break;
                            }
                        case 1:
                            {
                                ret = Prev3;
                                break;
                            }
                        case 2:
                            {
                                ret = Prev4;
                                break;
                            }
                        case 3:
                            {
                                ret = Prev5;
                                break;
                            }
                        case 4:
                            {
                                ret = Prev71;
                                break;
                            }
                        case 5:
                            {
                                ret = Prev6;
                                break;
                            }
                        case 6:
                            {
                                ret = Prev1;
                                break;
                            }
                    }
                    break;
                }
        }
        return ret;
    }
    void Start()
    {
        if (Match.CurrentArena == null)
        {
            xArena testArena = new xArena();
            testArena.ArenaName = "Sand Dunes2";
            testArena.BuiltInLevelID = "FreeForAllScene";
            testArena.ExternalURLocation = "";
            testArena.ID = 0;
            testArena.MatchedPlayedHere = 0;

            Match.CurrentArena = testArena;

        }
        if (Match.RobotsInMatch == null)
        {
            Match.PrizeMoneyLeague = 0;
            Match.RobotsInMatch = new List<RobotConstructor.RobotTemplate>();
            TheLookup.LoadFromFile();
            Match.RobotsInMatch = TheLookup.GetAllTemplatesForCurrentPlayer();
            if (Match.RobotsInMatch.Count > 2)
            {
                Match.RobotsInMatch = Match.RobotsInMatch.Take(2).ToList();
            }
        }

        TextLevelName.text = Match.CurrentArena.ArenaName;
        TextPrizeMoney.text = "$" + Match.PrizeMoneyLeague.ToString("f0");
        Prev1.gameObject.SetActive(false);
        Prev2.gameObject.SetActive(false);
        Prev3.gameObject.SetActive(false);
        Prev4.gameObject.SetActive(false);
        Prev5.gameObject.SetActive(false);
        Prev6.gameObject.SetActive(false);
        Prev71.gameObject.SetActive(false);
        List<RobotConstructor.RobotTemplate> allCurrentRobots = new List<RobotConstructor.RobotTemplate>();
        List<PanelRobotMatchPreview> prevsUsed = new List<PanelRobotMatchPreview>();
        for (int c = 0; c < Match.RobotsInMatch.Count; c++)
        {
            PanelRobotMatchPreview prev = GetPrevInPos(Match.RobotsInMatch.Count, c);
            if (prev != null)
            {

                //   prev.SetRobot(Match.RobotsInMatch[c], new Vector3(Random.Range(100, 200), Random.Range(100, 200), 1));
                prev.SetRobot(Match.RobotsInMatch[c], new Vector3(prev.StartPosition.x + Random.Range(-100, 200) * prev.StartPosition.x, prev.StartPosition.y + Random.Range(-100, 200) * prev.StartPosition.y, 1));
                prevsUsed.Add(prev);
                prev.gameObject.SetActive(true);
                allCurrentRobots.Add(Match.RobotsInMatch[c]);
            }
        }
        for (int c = 0; c < prevsUsed.Count; c++)
        {
            prevsUsed[c].SetRobotSliders(allCurrentRobots);
        }
        print("ROBOTS IN MATCH: " + Match.RobotsInMatch.Count);

        {
            TheWwwTts.LoadAudioStart(WwwLeagueInterface.BaseUrl + "TTS/MatchIntro/" + Match.LeagueMatchID + ".mp3");
        }
        

       
    }
    bool startMatch = false;
    // Update is called once per frame
    void Update()
    {

        if (waitStart > 0)
        {
            waitStart -= Time.deltaTime;
            if (waitStart <= 0)
            {
                startMatch = true;
            }
        }
        if (startMatch)
        {
            startMatch = false;
            ChaneToMatchScene();
        }
    }

    public void ChaneToMatchScene()
    {
        ArenaLookup.LoadArena(Match.CurrentArena);
     
    }
}
