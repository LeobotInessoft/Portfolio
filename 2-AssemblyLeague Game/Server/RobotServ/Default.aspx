<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RobotServ.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!-- About -->
	<div class="w3lsaboutaits" id="w3lsaboutaits">
		<div class="container">
			<div class="w3lsaboutaits-grids">
				<div class="col-md-6 w3lsaboutaits-grid w3lsaboutaits-grid-1">
					<h3>ASSEMBLY LEAGUE</h3>
					<p>Construct a killer robot from scratch then challenge your enemy to battle in the arena. Choose from over 150 components to include in your killer robot, program them to do accomplish your strategy then see your robot rise in the ranks.</p>
				</div>
				<div class="col-md-6 w3lsaboutaits-grid w3lsaboutaits-grid-2">
					<img src="images/about.jpg" alt="Game Robo">
				</div>
				<div class="clearfix"></div>
			</div>
		</div>
	</div>
	<!-- //About -->



	<!-- Platforms -->
	<%--<div class="agileinfoplatforms" id="agileinfoplatforms">
		<div class="agileinfoplatformsgrids">
			<div class="col-md-3 w3agile_gallery_grid w3agile_gallery_grid1">
				<div class="w3agile_gallery_image">
					<figure>
						<img src="images/platform-1.jpg" alt="Game Robo" class="img-responsive">
						<figcaption>
							<h4>PC</h4>
							<p><span><img src="images/platform-1-icon.png" alt="Game Robo"></span></p>
						</figcaption>
					</figure>
				</div>
			</div>
			<div class="col-md-3 w3agile_gallery_grid w3agile_gallery_grid2">
				<div class="w3agile_gallery_image">
					<figure>
						<img src="images/platform-2.jpg" alt="Robo Craft" class="img-responsive">
						<figcaption>
							<h4>Playstation</h4>
							<p><span><img src="images/platform-2-icon.png" alt="Game Robo"></span></p>
						</figcaption>
					</figure>
				</div>
			</div>
			<div class="col-md-3 w3agile_gallery_grid w3agile_gallery_grid3">
				<div class="w3agile_gallery_image">
					<figure>
						<img src="images/platform-3.jpg" alt="Game Robo" class="img-responsive">
						<figcaption>
							<h4>XBOX</h4>
							<p><span><img src="images/platform-3-icon.png" alt="Game Robo"></span></p>
						</figcaption>
					</figure>
				</div>
			</div>
			<div class="col-md-3 w3agile_gallery_grid w3agile_gallery_grid4">
				<div class="w3agile_gallery_image">
					<figure>
						<img src="images/platform-4.jpg" alt="Game Robo" class="img-responsive">
						<figcaption>
							<h4>WII</h4>
							<p><span><img src="images/platform-4-icon.png" alt="Game Robo"></span></p>
						</figcaption>
					</figure>
				</div>
			</div>
		</div>
		<div class="clearfix"></div>
	</div>--%>
	<!-- //Platforms -->

    <asp:Literal ID="LiteralRobotsTab" runat="server"></asp:Literal>

	<!-- Tabs -->
	<%--<div class="wthreetabsaits" id="wthreetabsaits">
		<section>
			<h3>Robots</h3>
			<div class="tabs tabs-style-line">
				<nav class="container">
					<ul>
						<li><a href="#section-line-1"><span>TOP RANKED ROBOTS</span></a></li>
						<li><a href="#section-line-2"><span>MOST EFFECTIVE ROBOTS</span></a></li>
						<li><a href="#section-line-3"><span>MOST COMPLICATED ROBOTS</span></a></li>
						<li><a href="#section-line-4"><span>NEW ROBOTS</span></a></li>
					</ul>
				</nav>
				<div class="content-wrap">
					<section id="section-line-1">
						<div id="owl-demo" class="owl-carousel text-center">
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="robots/55.png" alt="Game Robo">
								</div>
								<h3>Forza Horizon 3</h3>
								<h4>2 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="robots/54.png" alt="Game Robo">
								</div>
								<h3>Fallout 4</h3>
								<h4>1 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="robots/68.png" alt="Game Robo">
								</div>
								<h3>Mass Effect 4</h3>
								<h4>900000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
								<img src="robots/67.png" alt="Game Robo">
								</div>
								<h3>The Division</h3>
								<h4>874000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
								<img src="robots/51.png" alt="Game Robo">
								</div>
								<h3>Assassin's Creed III</h3>
								<h4>997000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/popular-6.jpg" alt="Game Robo">
								</div>
								<h3>The Elder Scrolls</h3>
								<h4>688000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/popular-7.jpg" alt="Game Robo">
								</div>
								<h3>Watch Dogs</h3>
								<h4>589000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/popular-8.jpg" alt="Game Robo">
								</div>
								<h3>Titanfall</h3>
								<h4>800000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/popular-9.jpg" alt="Game Robo">
								</div>
								<h3>The Witcher 3</h3>
								<h4>950000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/popular-10.jpg" alt="Game Robo">
								</div>
								<h3>Battlefield 4</h3>
								<h4>1 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
						</div>
					</section>
					<section id="section-line-2">
						<div id="owl-demo1" class="owl-carousel text-center">
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-1.jpg" alt="Game Robo">
								</div>
								<h3>Call of Duty: Ghosts</h3>
								<h4>3 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-2.jpg" alt="Game Robo">
								</div>
								<h3>Mortal Kombat X</h3>
								<h4>1.5 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-3.jpg" alt="Game Robo">
								</div>
								<h3>Guild Wars 2</h3>
								<h4>530000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-4.jpg" alt="Game Robo">
								</div>
								<h3>League of Legends</h3>
								<h4>974000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-5.jpg" alt="Game Robo">
								</div>
								<h3>Child of Eden</h3>
								<h4>457000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-6.jpg" alt="Game Robo">
								</div>
								<h3>Dark Souls III</h3>
								<h4>788000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-7.jpg" alt="Game Robo">
								</div>
								<h3>World of Warships</h3>
								<h4>489000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-8.jpg" alt="Game Robo">
								</div>
								<h3>Metro 2033</h3>
								<h4>907000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-9.jpg" alt="Game Robo">
								</div>
								<h3>DOTA 2</h3>
								<h4>750000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/new-10.jpg" alt="Game Robo">
								</div>
								<h3>World of warcraft</h3>
								<h4>2 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
						</div>
					</section>
					<section id="section-line-3">
						<div id="owl-demo2" class="owl-carousel text-center">
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-1.jpg" alt="Game Robo">
								</div>
								<h3>Batman: Arkham</h3>
								<h4>2 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-2.jpg" alt="Game Robo">
								</div>
								<h3>Tomb Raider: Rise</h3>
								<h4>1.5 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-3.jpg" alt="Game Robo">
								</div>
								<h3>S.T.A.L.K.E.R. 2</h3>
								<h4>530000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-4.jpg" alt="Game Robo">
								</div>
								<h3>Minecraft</h3>
								<h4>974000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-5.jpg" alt="Game Robo">
								</div>
								<h3>Deadpool</h3>
								<h4>457000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-6.jpg" alt="Game Robo">
								</div>
								<h3>World of Tanks</h3>
								<h4>788000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-7.jpg" alt="Game Robo">
								</div>
								<h3>Crysis 3</h3>
								<h4>489000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-8.jpg" alt="Game Robo">
								</div>
								<h3>Hitman</h3>
								<h4>907000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-9.jpg" alt="Game Robo">
								</div>
								<h3>Call Of Duty 11</h3>
								<h4>750000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/best-10.jpg" alt="Game Robo">
								</div>
								<h3>DOOM</h3>
								<h4>2 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
						</div>
					</section>
					<section id="section-line-4">
						<div id="owl-demo3" class="owl-carousel text-center">
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-1.jpg" alt="Game Robo">
								</div>
								<h3>Gran Turismo 6</h3>
								<h4>3 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-2.jpg" alt="Game Robo">
								</div>
								<h3>The Crew</h3>
								<h4>1 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-3.jpg" alt="Game Robo">
								</div>
								<h3>Assetto Corsa</h3>
								<h4>900000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-4.jpg" alt="Game Robo">
								</div>
								<h3>Project CARS</h3>
								<h4>874000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-5.jpg" alt="Game Robo">
								</div>
								<h3>Diablo III</h3>
								<h4>997000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-6.jpg" alt="Game Robo">
								</div>
								<h3>BioShock Infinite</h3>
								<h4>688000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-7.jpg" alt="Game Robo">
								</div>
								<h3>Shadow of Mordor</h3>
								<h4>589000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-8.jpg" alt="Game Robo">
								</div>
								<h3>The Last of Us</h3>
								<h4>800000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-half-o" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-9.jpg" alt="Game Robo">
								</div>
								<h3>Splinter Cell</h3>
								<h4>950000+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star-o" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
							<div class="item">
								<div class="agileinfoitem-image">
									<img src="images/editor-10.jpg" alt="Game Robo">
								</div>
								<h3>Rainbow Six</h3>
								<h4>1 Million+ Downloads</h4>
								<div class="wthreeratingaits">
									<ul>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
										<li><i class="fa fa-star" aria-hidden="true"></i></li>
									</ul>
								</div>
								<div class="wthreeitemdownload">
									<a href="#">Download</a>
								</div>
							</div>
						</div>
					</section>
				</div><!-- /content -->
			</div><!-- /tabs -->
		</section>
	</div>--%>
	<!-- //Tabs -->



	<!-- Blogs -->
	<%--<div class="wthreeblogsaits" id="wthreeblogsaits">
		<div class="wthreeblogsaits-grids">

			<h3>RoboCraft League Blogs & News</h3>
			<div class="col-md-3 wthreeblogsaits-grid wthreeblogsaits-grid-1">
				<a href="#" data-toggle="modal" data-target="#myModal1"><img src="images/blog-1.jpg" alt="Game Robo"></a>
				<span class="w3date">10-10-2016</span>
				<h4><a href="#" data-toggle="modal" data-target="#myModal1">NVIDIA TITAN X</a></h4>
				<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.</p>
				<button class="btn btn-primary" data-toggle="modal" data-target="#myModal1">READ MORE<i class="fa fa-arrow-right" aria-hidden="true"></i></button>
			</div>
			<div class="col-md-3 wthreeblogsaits-grid wthreeblogsaits-grid-2">
				<a href="#" data-toggle="modal" data-target="#myModal2"><img src="images/blog-2.jpg" alt="Game Robo"></a>
				<span class="w3date">15-10-2016</span>
				<h4><a href="#" data-toggle="modal" data-target="#myModal2">PS4 VR HEADSET</a></h4>
				<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.</p>
				<button class="btn btn-primary" data-toggle="modal" data-target="#myModal2">READ MORE<i class="fa fa-arrow-right" aria-hidden="true"></i></button>
			</div>
			<div class="col-md-3 wthreeblogsaits-grid wthreeblogsaits-grid-3">
				<a href="#" data-toggle="modal" data-target="#myModal3"><img src="images/blog-3.jpg" alt="Game Robo"></a>
				<span class="w3date">20-10-2016</span>
				<h4><a href="#" data-toggle="modal" data-target="#myModal3">ACCESSORIES FOR XBOX1</a></h4>
				<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.</p>
				<button class="btn btn-primary" data-toggle="modal" data-target="#myModal3">READ MORE<i class="fa fa-arrow-right" aria-hidden="true"></i></button>
			</div>
			<div class="col-md-3 wthreeblogsaits-grid wthreeblogsaits-grid-4">
				<a href="#" data-toggle="modal" data-target="#myModal4"><img src="images/blog-4.jpg" alt="Game Robo"></a>
				<span class="w3date">25-10-2016</span>
				<h4><a href="#" data-toggle="modal" data-target="#myModal4">NEW SUPER MARIO BROS!</a></h4>
				<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.</p>
				<button class="btn btn-primary" data-toggle="modal" data-target="#myModal4">READ MORE<i class="fa fa-arrow-right" aria-hidden="true"></i></button>
			</div>
			<div class="clearfix"></div>
		</div>

		<!-- Tooltip-Content -->
		<div class="tooltip-content">

			<div class="modal fade details-modal" id="myModal1" tabindex="-1" role="dialog" aria-hidden="true">
				<div class="modal-dialog modal-lg">
					<div class="modal-content">
						<div class="modal-header">
							<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
							<h4 class="modal-title">NVIDIA TITAN X</h4>
						</div>
						<div class="modal-body">
							<img src="images/blog-1.jpg" alt="Game Robo">
							<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						</div>
					</div>
				</div>
			</div>

			<div class="modal fade details-modal" id="myModal2" tabindex="-1" role="dialog" aria-hidden="true">
				<div class="modal-dialog modal-lg">
					<div class="modal-content">
						<div class="modal-header">
							<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
							<h4 class="modal-title">PS4 VR HEADSET</h4>
						</div>
						<div class="modal-body">
							<img src="images/blog-2.jpg" alt="Game Robo">
							<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						</div>
					</div>
				</div>
			</div>

			<div class="modal fade details-modal" id="myModal3" tabindex="-1" role="dialog" aria-hidden="true">
				<div class="modal-dialog modal-lg">
					<div class="modal-content">
						<div class="modal-header">
							<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
							<h4 class="modal-title">ACCESSORIES FOR XBOX2</h4>
						</div>
						<div class="modal-body">
							<img src="images/blog-3.jpg" alt="Game Robo">
							<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						</div>
					</div>
				</div>
			</div>

			<div class="modal fade details-modal" id="myModal4" tabindex="-1" role="dialog" aria-hidden="true">
				<div class="modal-dialog modal-lg">
					<div class="modal-content">
						<div class="modal-header">
							<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
							<h4 class="modal-title">NEW SUPER MARIO BROS!</h4>
						</div>
						<div class="modal-body">
							<img src="images/blog-3.jpg" alt="Game Robo">
							<p>Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						</div>
					</div>
				</div>
			</div>

		</div>
		<!-- //Tooltip-Content -->

	</div>--%>
	<!-- //Blogs -->



	<!-- Portfolio -->
	<%--<div class="w3portfolioaits" id="w3portfolioaits">
	<h3>Portfolio</h3>
		<div class="w3portfolioaits-items">
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-1">
				<a class="example-image-link" href="images/popular-1.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/popular-1.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-2">
				<a class="example-image-link" href="images/editor-1.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/editor-1.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-3">
				<a class="example-image-link" href="images/editor-2.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/editor-2.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-4">
				<a class="example-image-link" href="images/editor-3.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/editor-3.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-5">
				<a class="example-image-link" href="images/editor-4.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/editor-4.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-6">
				<a class="example-image-link" href="images/editor-9.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/editor-9.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-7">
				<a class="example-image-link" href="images/best-8.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/best-8.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-8">
				<a class="example-image-link" href="images/best-6.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/best-6.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-9">
				<a class="example-image-link" href="images/new-1.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/new-1.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-9">
				<a class="example-image-link" href="images/new-7.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/new-7.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-9">
				<a class="example-image-link" href="images/new-8.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/new-8.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="col-md-3 w3portfolioaits-item w3portfolioaits-item-9">
				<a class="example-image-link" href="images/popular-10.jpg" data-lightbox="example-set" data-title="">
					<div class="grid">
						<figure class="effect-apollo">
							<img src="images/popular-10.jpg" alt="Game Robo">
								<figcaption></figcaption>
						</figure>
					</div>
				</a>
			</div>
			<div class="clearfix"></div>
		</div>

	</div>--%>
	<!-- //Portfolio -->



	<!-- Stats -->
    <asp:Literal ID="LiteralStats" runat="server"></asp:Literal>

	<%--<div class="w3lsstatsaits" id="w3lsstatsaits">
		<div class="container">

			<div class="w3lsstatsaits-info">
				<div class="col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-1">
					<div class="w3lsstatsaits-img">
						<img src="images/stats-1.png" alt="Corsa Racer">
					</div>
					<div class="w3lsstatsaitsstats counter" data-slno='1' data-min='0' data-max='10000' data-delay='5' data-increment=4>0</div>
					<p>Matches Played</p>
				</div>
				<div class="col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-2">
					<div class="w3lsstatsaits-img">
						<img src="images/stats-2.png" alt="Corsa Racer">
					</div>
					<div class="w3lsstatsaitsstats counter" data-slno='1' data-min='0' data-max='1657033000' data-delay='5' data-increment="1">0</div>
					<p>Lines of Code</p>
				</div>
				<div class="col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-3">
					<div class="w3lsstatsaits-img">
						<img src="images/stats-3.png" alt="Corsa Racer">
					</div>
					<div class="w3lsstatsaitsstats counter" data-slno='1' data-min='0' data-max='192000' data-delay='5' data-increment="11">0</div>
					<p>Players</p>
				</div>
				<div class="col-md-3 w3lsstatsaits-grid w3lsstatsaits-grid-4">
					<div class="w3lsstatsaits-img">
						<img src="images/stats-4.png" alt="Corsa Racer">
					</div>
					<div class="w3lsstatsaitsstats counter" data-slno='1' data-min='0' data-max='71600' data-delay='5' data-increment="1">0</div>
					<p>Robots</p>
				</div>
				<div class="clearfix"></div>
			</div>

		</div>
	</div>--%>
	<!-- //Stats -->



	<!-- Partners -->
	<%--<div class="aitspartnersw3l">
		<div id="owl-demo4" class="owl-carousel text-center">
			<div class="item">
				<img src="images/partner-1.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-2.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-3.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-4.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-5.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-6.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-7.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-8.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-9.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-10.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-11.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-12.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-13.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-14.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-15.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-16.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-17.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-18.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-19.jpg" alt="Game Robo">
			</div>
			<div class="item">
				<img src="images/partner-20.jpg" alt="Game Robo">
			</div>
		</div>
	</div>--%>
	<!-- //Partners -->

</asp:Content>
