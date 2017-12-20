<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ExternalTopBar.ascx.vb" Inherits="Comunita_OnLine.UC_ExternalTopBar" %>
<%--Nomi Standard: OK--%>
<div id="toolbar">
	<div class="page-width">
		<div id="notifications">
			<ul>
				<li>&nbsp;</li>
			</ul>
		</div>
		<div id="tools">
			<ul>
				<li id="greetings">
					<span>
						<asp:literal ID="LTwelcome_t" runat="server">*Benvenuto, </asp:literal>
					</span>
					<strong>
						<asp:HyperLink ID="HYPuser" runat="server">#User Name</asp:HyperLink>
					</strong>
				</li>
				<li id="Top_Tools">
					<a class="menu" href="#">
						<asp:Literal ID="LTmanage_m" runat="server">*Gestione</asp:Literal>
					</a>
					<ul class="sub" style="opacity: 0;">
						<li>
							<asp:HyperLink ID="HYPuserSettings" runat="server">*Profilo</asp:HyperLink>
						</li>
						<li>
							<asp:HyperLink ID="HYPlist" runat="server">*Lista Ticket</asp:HyperLink>
						</li>
						<li>
							<asp:HyperLink ID="HYPadd" runat="server">*Nuovo Ticket</asp:HyperLink>                  
						</li>
					</ul>
				</li>
				<li id="Top_Lang">
					<asp:HyperLink ID="HYPlanguageCurrent" runat="server" cssclass="menu" ToolTip="*Italiano">#Italiano</asp:HyperLink>
					<ul class="sub">
						<asp:Repeater ID="RPTlanguages" runat="server">
							<ItemTemplate>
								<li>
									<asp:LinkButton ID="LKBlanguage" runat="server">#Language</asp:LinkButton>
								</li>    
							</ItemTemplate>
						</asp:Repeater>
						
					</ul>
				</li>
				<li id="Top_Logout">
					<asp:LinkButton ID="LKBlogout" runat="server">*Esci</asp:LinkButton>
				</li>
			</ul>
		</div>
	</div>
</div>