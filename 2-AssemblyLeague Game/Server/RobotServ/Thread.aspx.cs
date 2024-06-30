using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class Thread : System.Web.UI.Page
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
                LiteralContent.Text = GetHtml(cc, currentID);
            }

            if (Session["email"] != null && Session["pass"] != null)
            {
                TextBoxReply.Enabled = true;
                ButtonReply.Enabled = true;

            }
            else
            {
                TextBoxReply.Text = "[LOGIN TO REPLY]";
                TextBoxReply.Enabled = false;
                ButtonReply.Enabled = false;

            }

        }

        string GetHtml(DataClassesInterfaceDataContext cc, int threadID)
        {
            ThreadForum thread = cc.ThreadForums.FirstOrDefault(x => x.ID == threadID);

            string html = "";
            if (thread != null)
            {
                thread.TotalViews++;
                cc.SubmitChanges();
                Player op = cc.Players.FirstOrDefault(x => x.ID == thread.ID_Player);
                List<ThreadReply> replies = cc.ThreadReplies.Where(x => x.ID_Thread == thread.ID).OrderByDescending(x => x.CreateDate).ToList();
                html += " <h1>";
                html += "" + thread.Heading + "</h1>";
                if (op != null)
                {
                    html += "                    <b> " + op.DisplayName + " said</b>";
                }
                html += "                <p>";
                html += "" + thread.Detail + "</p>";

                html += "             ";
                html += "              <hr />";
                for (int c = 0; c < replies.Count; c++)
                {
                    Player poser = cc.Players.FirstOrDefault(x => x.ID == replies[c].ID_Player);
                    if (poser != null)
                    {
                        html += "                    <b> " + poser.DisplayName + " said</b>";
                    }
                    html += "                <p>";
                    html += "" + replies[c].ReplyText + ".</p>";
                }

            }
            return html;
        }

        protected void ButtonReply_Click(object sender, EventArgs e)
        {
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
             int currentID = 0;
            if (Request.QueryString["id"] != null)
            {
                currentID = Convert.ToInt32(Request.QueryString["id"]);
            }
            if (currentID > 0)
            {
                string email = (string)Session["email"];
                string pass = (string)Session["pass"];
                if (email != null && pass != null)
                {
                    ClassThreadDB thd = new ClassThreadDB();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(
                       HttpUtility.HtmlEncode(TextBoxReply.Text));
                    sb.Replace("&lt;b&gt;", "<b>");
                    sb.Replace("&lt;/b&gt;", "");
                    sb.Replace("&lt;i&gt;", "<i>");
                    sb.Replace("&lt;/i&gt;", "");
                    ThreadReply aReply = thd.CreateThreadReply(cc, email, pass, currentID, sb.ToString());
                    if (aReply != null)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }

        }

    }
}