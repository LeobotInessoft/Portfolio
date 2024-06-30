using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
            if (Session["email"] == null || Session["pass"] == null)
            {
                try
                {
                    LiteralLogin.Text = "<li><a  href=\"Login.aspx\">Login</a></li>";
                }
                catch
                {
                }
            }
            else
            {
                ClassThreadDB db = new ClassThreadDB();
                Player pl = db.Login(cc, (string)Session["email"], (string)Session["pass"]);
                LiteralLogin.Text = "<li><a  href=\"Profile.aspx?id="+pl.ID+"\">"+pl.DisplayName+"</a></li>";

            }
            LiteralBottomMenu.Text = Clear.GenerateBottomHTML(cc);

        }
    }
}