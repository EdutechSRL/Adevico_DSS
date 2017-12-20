<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditHeaderFooter.ascx.vb" Inherits="Comunita_OnLine.UC_EditHeaderFooter" %>

<%@ Register TagPrefix="CTRL" TagName="Element" Src="./UC_EditPageElement.ascx" %>
<%@ Register Src="~/Modules/DocTemplate/Uc/UC_EditVersions.ascx" TagName="CTRLprevVersion" TagPrefix="CTRL" %>


<fieldset class="light">
	<legend>
		<asp:Literal ID="LITlegendLeft" runat="server">#Left element</asp:Literal>
	</legend>
	<div class="fieldobject">
		<CTRL:Element id="UCleft" runat="server" />
	</div>

	<%--...revision...--%>
    <CTRL:CTRLprevVersion ID="UCprevVersionLeft" runat="server" />
    
</fieldset>

<fieldset class="light">
	<legend>
		<asp:Literal ID="LITlegendCenter" runat="server">#Center element</asp:Literal>
	</legend>
	<div class="fieldobject">
		<CTRL:Element id="UCcenter" runat="server" />
	</div>

	<%--...revision...--%>
    <CTRL:CTRLprevVersion ID="UCprevVersionCenter" runat="server" />

</fieldset>

<fieldset class="light">
	<legend>
		<asp:Literal ID="LITlegendRight" runat="server">#Right element</asp:Literal>
	</legend>
	<div class="fieldobject">
		<CTRL:Element id="UCright" runat="server" />
	</div>

	<%--...revision...--%>

    <CTRL:CTRLprevVersion ID="UCprevVersionRight" runat="server" />

</fieldset>