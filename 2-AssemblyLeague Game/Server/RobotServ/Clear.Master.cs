using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class Clear : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
            if (Session["email"] == null || Session["pass"] == null)
            {
                LiteralLogin.Text = "<li><a  href=\"Login.aspx\">Login</a></li>";

            }
            else
            {
                  ClassThreadDB db = new ClassThreadDB();
                Player pl = db.Login(cc, (string)Session["email"], (string)Session["pass"]);
                LiteralLogin.Text = "<li><a  href=\"Profile.aspx?id=" + pl.ID + "\">" + pl.DisplayName + "</a></li>";

            }
            LiteralBottomMenu.Text = GenerateBottomHTML(cc);

        }

        public static string GenerateBottomHTML(DataClassesInterfaceDataContext cc)
        {
            string html = "";
            html += "<div class=\"col-md-3 agilefooterwthree-grid agilefooterwthree-grid1\">";
            html += "<h4>INFORMATION</h4>";
            html += "<ul>";
            html += "<li><a href=\"Download.aspx\">DOWNLOAD</a></li>";
            html += "<li><a href=\"Tutorials.aspx\">TUTORIALS</a></li>";
            html += "<li><a href=\"Forum.aspx\">FORUM</a></li>";
            html += "<li><a href=\"Login.aspx\">LOGIN</a></li>";
            html += "</ul>";
            html += "</div>";
            html += "<div class=\"col-md-3 agilefooterwthree-grid agilefooterwthree-grid2\">";
            html += "<h4>Tutorials</h4>";
            html += "<ul>";
            List<Tutorial> tutorials = cc.Tutorials.OrderByDescending(x => x.CreateDate).Take(5).ToList();
            for (int c = 0; c < tutorials.Count; c++)
            {
                string txt = tutorials[c].Heading;
                if (txt.Length > 25) txt = txt.Substring(0, 24) + "...";
                html += "<li><a href=\"Tutorial.aspx?id=" + tutorials[c].ID + "\">" + tutorials[c].Heading + "</a></li>";
            }
        
            html += "</ul>";
            html += "</div>";
            html += "<div class=\"col-md-3 agilefooterwthree-grid agilefooterwthree-grid3\">";
            html += "<h4>FORUM</h4>";
            html += "<ul>";
            List<ThreadForum> th = cc.ThreadForums.OrderByDescending(x => x.CreateDate).Take(5).ToList();
            for (int c = 0; c < th.Count; c++)
            {
                string txt = th[c].Heading;
                if (txt.Length > 25) txt = txt.Substring(0, 24) + "...";
                html += "<li><a href=\"Thread.aspx?id=" + th[c].ID + "\">" + th[c].Heading + "</a></li>";
            }
            //html += "<li><a href=\"#\">MY ACCOUNT</a></li>";
            //html += "<li><a href=\"#\">MY ORDERS</a></li>";
            //html += "<li><a href=\"#\">MY DOWNLOADS</a></li>";
            //html += "<li><a href=\"#\">EDIT PROFILE</a></li>";
            //html += "<li><a href=\"#\">CHANGE PASSWORD</a></li>";
            //html += "<li><a href=\"#\">DONATE</a></li>";
            html += "</ul>";
            html += "</div>";

            return html;
        }
    }
}