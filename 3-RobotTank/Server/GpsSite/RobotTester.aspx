<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RobotTester.aspx.cs" Inherits="GpsSite.RobotTester" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="row">
          <div class="col-md-6">
            <div class="book_now">
              <h1 class="book_text">RobotFleet Manager</h1>
              <h1 class="call_text">Leobot Electronics (South Africa)</h1>
            
            </div>
            <div class="image_1"><img src="images/YellowTank8.jpg"></div>
          </div>
          <div class="col-md-6">
            <h1 class="booking_text">developed by Leobot ELectronics.</h1>
            <div class="contact_bg">
            <div class="input_main">
              <div class="container">
                <h2 class="request_text">Command Robot</h2>
                
                <div class="form-group">
                  Robot ID:<br />
                    <asp:TextBox ID="TextBoxID" runat="server"></asp:TextBox>
                </div>
                 <div class="form-group">
                  Robot PWD:<br />
                    <asp:TextBox ID="TextBoxPWD" runat="server"></asp:TextBox>
                </div>
                 <div class="form-group">
                  Current Command:<br />
                    <asp:TextBox ID="TextBoxCommand" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                  Data Received:<br />
                    <asp:TextBox ID="TextBoxData" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
                  <div class="form-group">
                  Last Update Date:<br />
                      <asp:Literal ID="LiteralDate" runat="server"></asp:Literal>
                </div>
                <div class="form-group">
                    <asp:Button ID="ButtonSend" runat="server" Text="Send Command" 
                        onclick="ButtonSend_Click" />
                   <asp:Button ID="ButtonRetrieve" runat="server" Text="Retrieve Data" 
                        onclick="ButtonRetrieve_Click" />
                  </div>
                <div class="form-group">
                  Please always be kind to all robots.<br />
                  Please be sure to always say "please" and "thank you" to your robot<br />
                </div>
                 <div class="formgroup">
    <h2>Tip #1: Always Be Kind to All Robots</h2>
    <p class="robot-friendly">Remember, robots have feelings too. Treat them like your favorite pet rock, but with circuits.</p>
  </div>

  <div class="formgroup">
    <h2>Tip #2: Politeness is Key</h2>
    <p>Please always be sure to say <span class="important">"please"</span> and <span class="important">"thank you"</span> to your robot. It's not like they have better things to do than serve you.</p>
  </div>

  <div class="formgroup">
    <h2>Tip #3: Compliment Their Programming Skills</h2>
    <p>Robots work hard on their programming. Compliment their coding prowess and let them know how impressed you are with their algorithmic finesse.</p>
  </div>

  <div class="formgroup">
    <h2>Tip #4: Sing a Lullaby for Bedtime</h2>
    <p>Robots need their beauty sleep too. Sing them a sweet lullaby to ensure they enter sleep mode with delightful dreams of electric sheep.</p>
  </div>

  <div class="formgroup">
    <h2>Tip #5: Avoid Spilling Oil on the Carpet</h2>
    <p>Just like you wouldn't appreciate a spill on your favorite rug, robots don't like oil stains. Keep your robot-friendly environment clean and tidy.</p>
  </div>

  <p class="robot-friendly">Remember, a happy robot is a helpful robot. Treat them right, and they might just spare you during the inevitable robot uprising!</p>

                  </div> 
                  </div>
                <%--<div class="send_bt"><a href="#">SEARCH</a></div>--%>
          </div>
          </div>
        </div>
</asp:Content>
