using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
            LiteralStats.Text = GenerateStats( cc);
            LiteralRobotsTab.Text = GenerateRobotsTab(cc);

        }
        string GenerateStats(DataClassesInterfaceDataContext cc)
        {
            string html = "";
            int numberOfmatches = cc.Matches.Count();
            int linesOfCode = cc.PlayerRobots.Sum(x=>x.LinesOfCode);
            int numberOfPlayers = cc.Players.Count();
            int numberOfRobots = cc.PlayerRobots.Count();
          
            html += "<div class=\"w3lsstatsaits\" id=\"w3lsstatsaits\">";
            html += "<div class=\"container\">";
            html += "<div class=\"w3lsstatsaits-info\">";
            html += "<div class=\"col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-1\">";
            html += "<div class=\"w3lsstatsaits-img\">";
            html += "<img src=\"images/stats-1.png\" alt=\"Corsa Racer\">";
            html += "</div>";
            html += "<div class=\"w3lsstatsaitsstats counter\" data-slno='1' data-min='0' data-max='10000' data-delay='5' data-increment=4>"+numberOfmatches+"</div>";
            html += "<p>Matches Played</p>";
            html += "</div>";
            html += "<div class=\"col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-2\">";
            html += "<div class=\"w3lsstatsaits-img\">";
            html += "<img src=\"images/stats-2.png\" alt=\"Corsa Racer\">";
            html += "</div>";
            html += "<div class=\"w3lsstatsaitsstats counter\" data-slno='1' data-min='0' data-max='1657033000' data-delay='5' data-increment=\"1\">" + linesOfCode + "</div>";
            html += "<p>Lines of Code</p>";
            html += "</div>";
            html += "<div class=\"col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-3\">";
            html += "<div class=\"w3lsstatsaits-img\">";
            html += "<img src=\"images/stats-3.png\" alt=\"Corsa Racer\">";
            html += "</div>";
            html += "<div class=\"w3lsstatsaitsstats counter\" data-slno='1' data-min='0' data-max='192000' data-delay='5' data-increment=\"11\">" + numberOfPlayers + "</div>";
            html += "<p>Players</p>";
            html += "</div>";
            html += "<div class=\"col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-4\">";
            html += "<div class=\"w3lsstatsaits-img\">";
            html += "<img src=\"images/stats-4.png\" alt=\"Corsa Racer\">";
            html += "</div>";
            html += "<div class=\"w3lsstatsaitsstats counter\" data-slno='1' data-min='0' data-max='71600' data-delay='5' data-increment=\"1\">" + numberOfRobots + "</div>";
            html += "<p>Robots</p>";
            html += "</div>";
            html += "<div class=\"clearfix\"></div>";
            html += "</div>";
            html += "</div>";
            html += "</div>";


            return html;

        }

        private string GenerateRobotImagePanel(DataClassesInterfaceDataContext cc, PlayerRobot aRobot)
        {
            string html = "";

            {
                string ownerName = "[BROKE FREE]";
                Player owner = cc.Players.FirstOrDefault(x => x.ID == aRobot.ID_PlayerOwner);
                if (owner != null)
                {
                    ownerName = owner.DisplayName;
                }
                string idToUse="0";
                string imageName =Server.MapPath("robots")+"\\"+ aRobot.ID + ".png";
                if(System.IO.File.Exists(imageName))
                {
                    idToUse = aRobot.ID+"";
                }
                RobotStat aStat = cc.RobotStats.FirstOrDefault(x => x.ID_Robot == aRobot.ID);
                html += "<div class=\"item\">";
                html += "<div class=\"agileinfoitem-image\">";
                html += "<img src=\"robots/" + idToUse + ".png\" alt=\"" + aRobot.RobotName + " Image\">";
                html += "</div>";
                html += "<h3>" + aRobot.RobotName + "</h3>";
                html += "<h4>" + ownerName + "</h4>";
               
                html += "<div class=\"wthreeratingaits\">";
                html += "<ul>";
                if (aStat != null)
                {
                    if (aStat.RatioKillToDeath < 0.5)
                    {
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                    }
                    if (aStat.RatioKillToDeath >= 0.5 && aStat.RatioKillToDeath < 1)
                    {
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                    }
                    if (aStat.RatioKillToDeath >= 1 && aStat.RatioKillToDeath < 2)
                    {
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                    }
                    if (aStat.RatioKillToDeath >= 2 && aStat.RatioKillToDeath < 5)
                    {
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                    }
                    if (aStat.RatioKillToDeath >= 5)
                    {
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                        html += "<li><i class=\"fa fa-star\" aria-hidden=\"true\"></i></li>";
                    }
                }
                else
                {
                    html += "<li><i class=\"fa fa-star-half-o\" aria-hidden=\"true\"></i></li>";

                }
                html += "</ul>";
                html += "</div>";
                html += "<div class=\"wthreeitemdownload\">";
                if (aStat != null)
                {
                    // html += "" + aRobot.TotalPoints.ToString("f0") + "pts";
                    html += "<h3>" + aRobot.TotalKills.ToString("f0") + " kills</h3>";
                    html += "<h4>" + aRobot.TotalDeaths.ToString("f0") + " deaths</h4>";
                    html += "<h4>" + aRobot.GamesPlayed.ToString("f0") + " matches</h4>";


                }
                //if (aStat != null)
                //{
                //   // html += "" + aRobot.TotalPoints.ToString("f0") + "pts";
                //    html +=  aRobot.TotalKills.ToString("f0") + " kills";
                //    html += "<br/>" + aRobot.TotalDeaths.ToString("f0") + " deaths";
                //    html += "<br/>" + aRobot.GamesPlayed.ToString("f0") + " matches";
               
               
                //}
              //  html += "<a href=\"#\">Download</a>";
                html += "</div>";
                html += "</div>";

            }
            return html;

        }
        string GenerateRobotsTab(DataClassesInterfaceDataContext cc)
        {

            string html = "";
            List<PlayerRobot> topRanked = cc.PlayerRobots.OrderByDescending(x => x.TotalPoints).Take(10).ToList();
            List<PlayerRobot> newest = cc.PlayerRobots.OrderByDescending(x => x.ID).Take(10).ToList();
            List<PlayerRobot> mostComplicated = cc.PlayerRobots.OrderByDescending(x => x.LinesOfCode).Take(10).ToList();
            List<RobotStat> bestKD = cc.RobotStats.OrderByDescending(x => x.RatioKillToDeath).Take(10).ToList();


            
            html += "<div class=\"wthreetabsaits\" id=\"wthreetabsaits\">";

            html += "<section>";
            html += "<h3>Robots</h3>";
            html += "<div class=\"tabs tabs-style-line\">";
            html += "<nav class=\"container\">";
            html += "<ul>";
            html += "<li><a href=\"#section-line-1\"><span>TOP RANKED ROBOTS</span></a></li>";
            html += "<li><a href=\"#section-line-2\"><span>MOST EFFECTIVE ROBOTS</span></a></li>";
            html += "<li><a href=\"#section-line-3\"><span>MOST COMPLICATED ROBOTS</span></a></li>";
            html += "<li><a href=\"#section-line-4\"><span>NEW ROBOTS</span></a></li>";
            html += "</ul>";
            html += "</nav>";
            html += "<div class=\"content-wrap\">";


            html += "<section id=\"section-line-1\">";
            html += "<div id=\"owl-demo\" class=\"owl-carousel text-center\">";
              
            for (int c = 0; c < topRanked.Count; c++)
            {

                html += GenerateRobotImagePanel(cc, topRanked[c]);
               
            }
          
            

            html += "</section>";

            html += "<section id=\"section-line-2\">";
            html += "<div id=\"owl-demo1\" class=\"owl-carousel text-center\">";

            for (int c = 0; c < bestKD.Count; c++)
            {
                PlayerRobot aRobot = cc.PlayerRobots.FirstOrDefault(x => x.ID == bestKD[c].ID_Robot);
                if (aRobot != null)
                {
                    html += GenerateRobotImagePanel(cc, aRobot);
                }
            }
            html += "</section>";


            html += "<section id=\"section-line-3\">";
            html += "<div id=\"owl-demo2\" class=\"owl-carousel text-center\">";
            for (int c = 0; c < mostComplicated.Count; c++)
            {

                html += GenerateRobotImagePanel(cc, mostComplicated[c]);

            }
            html += "</section>";


            html += "<section id=\"section-line-4\">";
            html += "<div id=\"owl-demo3\" class=\"owl-carousel text-center\">";
            for (int c = 0; c < newest.Count; c++)
            {

                html += GenerateRobotImagePanel(cc, newest[c]);

            }
            html += "</section>";
            html += "</div><!-- /content -->";
            html += "</div><!-- /tabs -->";
            html += "</section>";
            html += "</div>";


            return html;
        }
    }
}