<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinCSS.ascx.vb" Inherits="Comunita_OnLine.UC_SkinCSS" %>
<br />
<div class="DVmenu">
    <asp:LinkButton ID="LNBsaveAll" runat="server" CssClass="Link_Menu" CommandName="save">#UploadAll</asp:LinkButton>
</div>
<br />
<div class="Data">
    <asp:Label ID="LBmainCss_t" runat="server" CssClass="Titolo_Campo">#Main CSS:</asp:Label>
    <asp:LinkButton ID="LNBdeleteMainCss" runat="server" CommandName="delete" CommandArgument="Main" CssClass="DeleteSmall">X</asp:LinkButton>
    <asp:HyperLink ID="HYPmainCss" runat="server" Target="_blank">*Main.css</asp:HyperLink>
    <br />
    <asp:FileUpload ID="FUPmainCss" runat="server" />
    <asp:LinkButton ID="LNBuploadMainCss" runat="server" CommandName="upload" CommandArgument="Main"  CssClass="Link_Menu" >*Modify</asp:LinkButton>
</div>

<div class="Data">
    <asp:Label ID="LBadminCss_t" runat="server" CssClass="Titolo_Campo">#Admin CSS:</asp:Label>
    <asp:LinkButton ID="LNBdeleteAdminCss" runat="server" CommandName="delete" CommandArgument="Admin" CssClass="DeleteSmall">X</asp:LinkButton>
    <asp:HyperLink ID="HYPadminCss" runat="server" Target="_blank">*Main.css</asp:HyperLink>
    <br />
    <asp:FileUpload ID="FUPadminCss" runat="server" />
    <asp:LinkButton ID="LNBuploadAdminCss" runat="server" CommandName="upload" CommandArgument="Admin" CssClass="Link_Menu">*Modify</asp:LinkButton>
</div>

<div class="Data">
    <asp:Label ID="LBieCss_t" runat="server" CssClass="Titolo_Campo">#IE CSS:</asp:Label>
    <asp:LinkButton ID="LNBdeleteIeCss" runat="server" CommandName="delete" CommandArgument="IE" CssClass="DeleteSmall">X</asp:LinkButton>
    <asp:HyperLink ID="HYPieCss" runat="server" Target="_blank">*Main.css</asp:HyperLink>
    <br />
    <asp:FileUpload ID="FUPieCss" runat="server" />
    <asp:LinkButton ID="LNBuploadIeCss" runat="server" CommandName="upload" CommandArgument="IE" CssClass="Link_Menu">*Modify</asp:LinkButton>
</div>


<div class="Data">
	<asp:Label ID="LBloginCss_t" runat="server" CssClass="Titolo_Campo">#Login CSS:</asp:Label>
	<asp:LinkButton ID="LNBdeleteLoginCss" runat="server" CommandName="delete" CommandArgument="Login" CssClass="DeleteSmall">X</asp:LinkButton>
	<asp:HyperLink ID="HYPloginCss" runat="server" Target="_blank">*Main.css</asp:HyperLink>
	<br />
    <%--<asp:Label ID="Lbl_void" runat="server" CssClass="Titolo_Campo"></asp:Label>--%>
	<asp:Label ID="LBloginCssInfo_t" runat="server" CssClass="testo_Campo">#IE CSS:</asp:Label>
	<br />
	<asp:FileUpload ID="FUPloginCss" runat="server" />
	<asp:LinkButton ID="LNBuploadLoginCss" runat="server" CommandName="upload" CommandArgument="Login" CssClass="Link_Menu">*Modify</asp:LinkButton>
</div>