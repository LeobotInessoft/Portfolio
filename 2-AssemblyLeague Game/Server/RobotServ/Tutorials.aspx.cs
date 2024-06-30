using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class Tutorials : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
            LiteralContent.Text = GetHtmlForum(cc);
        }

        string GetHtmlForum(DataClassesInterfaceDataContext cc)
        {
            string html = "";
            List<Tutorial> thr = cc.Tutorials.OrderByDescending(x => x.CreateDate).ToList();
            html += " <table style=\"width: 100%;\">";
            html += "        <tr>";
            html += "            <td width=\"10%\">";
            html += "                ";
            html += "            </td>";
            html += "            <td>";
            html += "             <h3>Topic</h3>  ";
            html += "            </td>";
            html += "            <td width=\"10%\">";
            html += "               Replies/Views";
            html += "            </td>";
            html += "        </tr>";
            for (int c = 0; c < thr.Count; c++)
            {
                Player pl = cc.Players.FirstOrDefault(x => x.ID == thr[c].ID_Player);
                int totalReplies = cc.TutorialReplies.Count(x => x.ID_Thread == thr[c].ID);
                html += "        <tr>";
                html += "            <td>";
                html += "                " + thr[c].CreateDate.ToString("dd MMM") + "";
                html += "            </td>";
                html += "            <td>";
                html += "               <a href=\"Tutorial.aspx?id=" + thr[c].ID + "\"> <b>" + thr[c].Heading + "</b></a>";
              //  html += "                <br/> by <b>" + pl.DisplayName + "</b>";
                html += "            </td>";
                html += "            <td>";
                html += "               " + totalReplies + "/" + thr[c].TotalViews + "";
                html += "            </td>";
                html += "        </tr>";

            }
            html += "      ";
            html += "    </table>";

            return html;

        }

      
    }
}