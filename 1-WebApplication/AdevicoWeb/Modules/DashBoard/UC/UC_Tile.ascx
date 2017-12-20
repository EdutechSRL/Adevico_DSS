<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Tile.ascx.vb" Inherits="Comunita_OnLine.UC_Tile" %>
<div class="tile <%=GetTileCssClass %>">
    <asp:Literal ID="LTlinkOpen" runat="server"><a href="{0}" title="{1}"></asp:Literal>
    <div class="innerwrapper">
		<div class="tileheader clearfix">
			<div class="left">
				<h3><asp:Literal ID="LTtileTitle" runat="server"></asp:Literal></h3>
			</div>
			<div class="right"></div>
		</div>
		<div class="tilecontent clearfix">
            <div class="icon" id="DVicon" runat="server"><asp:Image ID="IMGtileIcon" runat="server" Visible="false" /></div>
		</div>      		 		  
        <div class="tilefooter">
			<div class="left"></div>
			<div class="right"></div>
        </div>
	</div>
    <asp:Literal ID="LTlinkClose" runat="server"></a></asp:Literal>
</div>	        		 
<asp:Literal ID="LTcssClassTileIcon" runat="server" Visible="false">comtype_64</asp:Literal>
<asp:Literal ID="LTcssClassCustomTile" runat="server" Visible="false">custom</asp:Literal>
<asp:Literal ID="LTgetTileCssClass" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTcssClassDefaultItemClass" runat="server" Visible="false">community</asp:Literal>
<asp:Literal ID="LTlinkPreview" runat="server" Visible="false"><a href="{0}" title="{1}" target="_blank"></asp:Literal>