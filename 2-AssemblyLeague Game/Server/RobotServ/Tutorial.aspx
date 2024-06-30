<%@ Page Title="" Language="C#" MasterPageFile="~/Clear.Master" AutoEventWireup="true" CodeBehind="Tutorial.aspx.cs" Inherits="RobotServ.Tutorial"   %>
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
    <!-- Newsletter -->
    <div class="w3lsnewsletter" id="w3lsnewsletter">
        <div class="container">
            <div class="w3lsnewsletter-grids">
                <asp:Literal ID="LiteralContent" runat="server"></asp:Literal>

               
            
             <div class="clearfix">
                </div>
            </div>
            <!-- Contact -->
	
		<div class="container">
			<h3>Respond</h3>
		
          
				<div class="col-md-11 agilecontactw3ls-grid agilecontactw3ls-grid-2">
				    <asp:TextBox ID="TextBoxReply" runat="server" TextMode="MultiLine" ForeColor="Black" name="Message" placeholder="MESSAGE" required=""></asp:TextBox>
					<div class="">
					       <asp:Button ID="ButtonReply" runat="server" Text="Reply" CssClass="btn" 
                               onclick="ButtonReply_Click" />
					</div>
				</div>
			
	</div>
	<!-- //Contact -->
        </div>
    </div>

    <!-- //Newsletter -->
</asp:Content>
