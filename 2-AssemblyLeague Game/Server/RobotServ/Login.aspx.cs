using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
            int id = 0;
            string email = TextBoxEmail.Text;
            string pass = TextBoxPassword.Text;
            ClassThreadDB thd = new ClassThreadDB();
            Player aplyer= thd.Login(cc, email, pass);
          if(aplyer!= null)
          {
              Session["email"] = aplyer.Email;
              Session["pass"] = aplyer.Password;
              Response.Redirect("Forum.aspx");
          }
        }
    }
}