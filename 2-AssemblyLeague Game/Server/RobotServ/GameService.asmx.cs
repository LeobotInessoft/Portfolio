using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace RobotServ
{
    /// <summary>
    /// Summary description for GameService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GameService : System.Web.Services.WebService
    {

        [WebMethod]
        public MatchVideoDetail GetMatchVideoDetail(int matchID, string p1, string p2, string p3)
        {
            MatchVideoDetail ret = new MatchVideoDetail();
            ret.Detail = "Assembly League Robots in Arena:";
            if (p1 == "423hj23" && p2 == "fdshdsfdsf" && p3 == "dfhjsfdhjsfewy")
            {
                DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
                Match aMatch = cc.Matches.FirstOrDefault(x => x.ID == matchID);
                ret.Heading = "Assembly League Match "+ aMatch.ID;
                List<RobotMatchResult> entries = cc.RobotMatchResults.Where(x => x.ID_Match == aMatch.ID).ToList();
                for (int c = 0; c < entries.Count; c++)
                {
                    string desc = "";
                    PlayerRobot aRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == entries[c].ID_Robot);
                    Player aPlayer = cc.Players.FirstOrDefault(x => x.ID == aRobot.ID_PlayerOwner);
                    desc = aRobot.RobotName + " created by " + aPlayer.DisplayName+"\n";
                    ret.Detail += desc;
            
                }
              
            }
            return ret; ;
        }
        [WebMethod]
        public bool SetMatchVideoDetail(int matchID, string p1, string p2, string p3, string YoutTubeID)
        {
            bool ret = false;
            if (p1 == "423hj23" && p2 == "fdshdsfdsf" && p3 == "dfhjsfdhjsfewy")
            {
                DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
                Match aMatch = cc.Matches.FirstOrDefault(x => x.ID == matchID);
                aMatch.YouTubeID = YoutTubeID;
                cc.SubmitChanges();
                ret = true;
            }
            return ret; ;
        }
    }
    [Serializable]
    public class MatchVideoDetail
    {
        public string Heading;
        public string Detail;
    }
}
