<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PortalHeader.ascx.vb" Inherits="Comunita_OnLine.UC_PortalHeader" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLTopBar" Src="./Header/UC_TopBar.ascx" %>
<div id="header">
    <!-- toolbar OK: V3.0b8-->
	<div id="toolbar">
        <div class="page-width">
            <div id="tools">
                <CTRL:CTRLTopBar id="UC_TopBar" runat="server"></CTRL:CTRLTopBar>
			</div>
	    </div>
    </div>
    <!-- end toolbar -->
			
  	<!-- branding -->	
	<div id="branding" class="page-width">
        <asp:Image ID="IMGlogo" runat="server" alt="logo" CssClass="logo"/>
        <asp:Literal ID="LTlogo" runat="server"></asp:Literal>
	    <h1>
            <asp:Label ID="LBcommunityName" Runat="server"></asp:Label>	
            <span></span>
		</h1>
	</div>
	<!-- end branding -->
    <!-- nav main -->
    <asp:Literal ID="LTmenu" runat="server"></asp:Literal>
    <!-- end nav main -->
 </div>