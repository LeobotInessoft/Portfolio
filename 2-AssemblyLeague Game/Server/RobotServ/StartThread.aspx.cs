using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class StartThread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
            int currentID = 0;
            if (Request.QueryString["id"] != null)
            {
                currentID = Convert.ToInt32(Request.QueryString["id"]);
            }
            if (currentID > 0)
            {
                if (IsPostBack == false)
                {
                    ThreadForum th = cc.ThreadForums.FirstOrDefault(x => x.ID == currentID);
                    //LiteralContent.Text = GetHtml(cc, currentID);
                    TextBoxHeading.Text = th.Heading;
                    TextBoxDetail.Text = th.Detail;
                }
            }
        }

        protected void ButtonReply_Click(object sender, EventArgs e)
        {
            string email = (string)Session["email"];
            string pass = (string)Session["pass"];
            int currentID = 0;
            if (Request.QueryString["id"] != null)
            {
                currentID = Convert.ToInt32(Request.QueryString["id"]);
            }
            if (email != null && pass != null)
            {
                DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
                ClassThreadDB aThread = new ClassThreadDB();
                System.Text.StringBuilder sb = new System.Text.StringBuilder(
                           HttpUtility.HtmlEncode(TextBoxDetail.Text));
                sb.Replace("&lt;b&gt;", "<b>");
                sb.Replace("&lt;/b&gt;", "");
                sb.Replace("&lt;i&gt;", "<i>");
                sb.Replace("&lt;/i&gt;", "");


                ThreadForum result = aThread.CreateThread(cc, email, pass, currentID, TextBoxHeading.Text, sb.ToString());
                if (result != null)
                {
                    Response.Redirect("Thread.aspx?id=" + result.ID);
                }
            }

        }
    }
}