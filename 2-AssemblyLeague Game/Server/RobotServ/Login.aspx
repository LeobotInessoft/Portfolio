<%@ Page Title="" Language="C#" MasterPageFile="~/Clear.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RobotServ.Login" %>
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
										<h1>Login</h1>
                                        <p>Don't have an account? - <a href="Register.aspx"">Register Here</a> </p>
								      <asp:TextBox ID="TextBoxEmail" runat="server" Name="Email" placeholder="Email" required=""  ForeColor="Black" ></asp:TextBox>
								      <asp:TextBox ID="TextBoxPassword" runat="server" Name="Password" placeholder="Password" required=""  ForeColor="Black" TextMode="Password"></asp:TextBox>
										<div class="send-button wthree agileits">
										      <asp:Button ID="ButtonLogin" runat="server" Text="Login" CssClass="btn" 
                                                  onclick="ButtonLogin_Click" />
										</div>
										<div class="clearfix"></div>
									</div>
								
                                
							
						
</asp:Content>
