<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Switch.ascx.vb" Inherits="Comunita_OnLine.UC_Switch" %>

	<asp:literal id="LTmainContainer" runat="server"><span class="btnswitchgroup" data-name="resources" data-rel="" data-table=""></asp:literal><!--
		--><asp:LinkButton ID="LKBon" runat="server" CssClass="btnswitch on first active">On</asp:LinkButton><!--
		--><asp:LinkButton ID="LKBoff" runat="server" CssClass="btnswitch off last">Off</asp:LinkButton>
	</span>
	<asp:PlaceHolder ID="PLHdescription" runat="server">
	<span class="desc">
		<asp:Literal ID="LTdescriptionPre" runat="server">Value is </asp:Literal><!--
		--><span class="value"><asp:Literal ID="LTdescriptionValue" runat="server">on</asp:Literal></span><!--
		--><asp:Literal ID="LTdescriptionPost" runat="server"> (on/off)</asp:Literal><!--
	--></span>
	</asp:PlaceHolder>


	<asp:literal id="LTmainContainer_template" runat="server" Visible="false"><span class="{css} {cssdisabled}" data-name="{dataname}" data-rel="{datarel}" data-table="{datatable}"></asp:literal>