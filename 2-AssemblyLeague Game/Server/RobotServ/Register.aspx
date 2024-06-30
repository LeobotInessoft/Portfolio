<%@ Page Title="" Language="C#" MasterPageFile="~/Clear.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="RobotServ.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="w3lsaboutaits" id="w3lsaboutaits">
        <div class="container">
            <div class="w3lsaboutaits-grids">
                <div class="col-md-6 w3lsaboutaits-grid w3lsaboutaits-grid-1">
                    <h3>
                        ASSEMBLY LEAGUE</h3>
                    <p>
                        Construct a killer robot from scratch then challenge your enemy to battle in the
                        arena. Choose from over 150 components to include in your killer robot, program
                        them to do accomplish your strategy then see your robot rise in the ranks.</p>
                </div>
                <div class="col-md-6 w3lsaboutaits-grid w3lsaboutaits-grid-2">
                    <img src="images/about.jpg" alt="Game Robo">
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>

						   
							
                         		<div id="login" class="animate w3loginagile form">
										<h1>Register</h1>
                                        <p>Register an account to access the forums. </p>
								    <asp:TextBox ID="TextBoxDisplayName" runat="server" Name="DisplayName" placeholder="DisplayName" required=""  ForeColor="Black" ></asp:TextBox>
								       <asp:TextBox ID="TextBoxEmail" runat="server" Name="Email" placeholder="Email" required=""  ForeColor="Black" ></asp:TextBox>
								       <asp:TextBox ID="TextBoxPassword" runat="server" Name="Password" placeholder="Password" required=""  ForeColor="Black" TextMode="Password"></asp:TextBox>
									  <asp:TextBox ID="TextBoxPassword2" runat="server" Name="Password2" placeholder="Re-Enter Password" required=""  ForeColor="Black" TextMode="Password"></asp:TextBox>
										<div class="send-button wthree agileits">
										      <br />
                                            <asp:CheckBox ID="CheckBoxTerms" runat="server" required=""  />I agree to the  <a href="Terms.aspx">Terms & Conditions</a><br />
                                              <br />
&nbsp;<asp:Button ID="ButtonLogin" runat="server" Text="Register" CssClass="btn" 
                                                  onclick="ButtonLogin_Click" />
										</div>
										<div class="clearfix"></div>
                                     <asp:Literal ID="LiteralError" runat="server"></asp:Literal>
									</div>
</asp:Content>
