<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinEdit.ascx.vb" Inherits="Comunita_OnLine.UC_SkinEdit" %>

<%@ Register TagPrefix="CTRL" TagName="CtrlSkinCss" Src="UC_SkinCSS.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinImages" Src="UC_SkinImages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinMainLogo" Src="UC_SkinHeadLogo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinFooterLogo" Src="UC_SkinFootLogo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinFooterText" Src="UC_SkinFootText.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinAssociation" Src="UC_SkinAssociation.ascx" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div class="fieldobject clearfix">
    <span class="left">
        <asp:Label ID="LBskinName_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBskinName">#Nome:</asp:Label>
        <asp:TextBox ID="TXBskinName" runat="server" CssClass="Testo_Campo"  Columns="70" MaxLength="255"></asp:TextBox>
        <%--<asp:CheckBox ID="Cbx_IsActive" runat="server" />--%>
        <asp:LinkButton ID="LNBsaveSkinName" runat="server" CssClass="Link_Menu">#Save</asp:LinkButton>

    </span>
    <span class="right">
        <asp:Label runat="server" ID="LBskinId_t"  CssClass="Titolo_Campo" AssociatedControlID="LTskinId">ID:</asp:Label>
        <asp:Literal ID="LTskinId" runat="server">##</asp:Literal>
    </span>
</div>
<div class="tab">
    <telerik:RadTabStrip ID="TBSSkinEdit" runat="server" Align="Justify" Width="100%" Height="20px"
        CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true">
    
        <Tabs>
            <telerik:RadTab Text="#CSS" Value="1" />
            <telerik:RadTab Text="#Immagini" Value="2" />
            <telerik:RadTab Text="#Logo Principale" Value="3" />
            <telerik:RadTab Text="#Loghi Footer" Value="4" />
            <telerik:RadTab Text="#Testo Footer" Value="5" />
            <telerik:RadTab Text="#Associazioni" Value="6" />
        </Tabs>
    
    </telerik:RadTabStrip>
</div>
<div class="edit_content">
    <asp:MultiView ID="MLVskinEdit" runat="server">
        <asp:View ID="VIWcss" runat="server">
            <CTRL:CtrlSkinCss ID="CTRLcss" runat="server" />
        </asp:View>
        <asp:View ID="VIWimages" runat="server">
            <CTRL:CtrlSkinImages ID="CTRLimages" runat="server" />
        </asp:View>
        <asp:View ID="VIWmainLogo" runat="server">
            <CTRL:CtrlSkinMainLogo ID="CTRLmainLogo" runat="server" />
        </asp:View>
        <asp:View ID="VIWfootLogo" runat="server">
            <CTRL:CtrlSkinFooterLogo ID="CTRLfootLogo" runat="server" />
        </asp:View>
        <asp:View ID="VIWfootText" runat="server">
            <CTRL:CtrlSkinFooterText ID="CTRLfootText" runat="server" />
        </asp:View>
        <asp:View ID="VIWassociation" runat="server">
            <CTRL:CtrlSkinAssociation ID="CTRLassociation" runat="server" />
        </asp:View>
    </asp:MultiView>
</div>