using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GpsSite
{
    public partial class RobotTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonSend_Click(object sender, EventArgs e)
        {
            string deviceId = TextBoxID.Text;
            string devicePwd = TextBoxPWD.Text;
            string deviceSendCommand = TextBoxCommand.Text;
            string deviceReceivedData = TextBoxData.Text;

            using (DataClassesGpsDataContext ct = new DataClassesGpsDataContext())
            {
                RobotCommand rc = ct.RobotCommands.FirstOrDefault(x => x.DeviceID.ToLower() == deviceId.ToLower() && x.AccessPassword == devicePwd);
                if (rc != null)
                {

                    rc.TestCommandToSend = TextBoxCommand.Text.Trim();
                    ct.SubmitChanges();
                    TextBoxCommand.Text = rc.TestCommandToSend;
                    TextBoxData.Text = rc.TestDataReceived;

                }

            }
        }

        protected void ButtonRetrieve_Click(object sender, EventArgs e)
        {
            string deviceId = TextBoxID.Text;
            string devicePwd = TextBoxPWD.Text;
            string deviceSendCommand = TextBoxCommand.Text;
            string deviceReceivedData = TextBoxData.Text;

            using (DataClassesGpsDataContext ct = new DataClassesGpsDataContext())
            {
                RobotCommand rc = ct.RobotCommands.FirstOrDefault(x => x.DeviceID.ToLower() == deviceId.ToLower() && x.AccessPassword == devicePwd);
                if (rc != null)
                {

                    TextBoxCommand.Text = rc.TestCommandToSend;
                    TextBoxData.Text = rc.TestDataReceived;
                    LiteralDate.Text = rc.LastRequestDate.ToString();

                }

            }
        }
    }
}