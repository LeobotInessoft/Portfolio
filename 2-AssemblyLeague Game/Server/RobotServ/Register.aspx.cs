using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotServ
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            string error = "";
            DataClassesInterfaceDataContext cc = new DataClassesInterfaceDataContext();
            int id = 0;
            string displayName = TextBoxDisplayName.Text;
            string email = TextBoxEmail.Text;
            string pass = TextBoxPassword.Text;
            string passRe = TextBoxPassword2.Text;
            if (CheckBoxTerms.Checked == true)
            {

            }
            else
            {
                error = "Please agree to the Terms & Conditions.";
       
            }
            if (email.Length > 0 && pass.Length > 0)
            {

            }
            else
            {
                error = "Please fill in all the fields.";
            }
            if (pass == passRe)
            {

            }
            else
            {
                error = "Passwords do not match";
            }
            if (email.Contains("@") == false)
            {

            }
            else
            {
                error = "Invalid email address";
            }
            if (error.Length == 0)
            {
                ClassThreadDB thd = new ClassThreadDB();

                Player aplyer = thd.Register(cc, displayName, email, pass);
                if (aplyer != null)
                {
                    //Session["email"] = aplyer.Email;
                    //Session["pass"] = aplyer.Password;
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    error = "Unable to register with the email address.";

                }
            }
            LiteralError.Text = error;
        }
    }
}