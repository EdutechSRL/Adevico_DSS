<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleAttachmentInlineCommands.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleAttachmentInlineCommands" %>
<asp:Literal ID="LTddbuttonlist" runat="server" Visible="true"></asp:Literal>
<asp:MultiView id="MLVcommands" runat="server">
    <asp:View ID="VIWpostbackCommands" runat="server"><!--
    --><asp:LinkButton ID="LNBuploadtomoduleitem" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Upload File</asp:LinkButton><!--
    --><asp:LinkButton ID="LNBuploadtomoduleitemandcommunity" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Upload File & Add to Community</asp:LinkButton><!--
    --><asp:LinkButton ID="LNBlinkfromcommunity" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Select Community File</asp:LinkButton><!--
    --><asp:LinkButton ID="LNBaddurltomoduleitem" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Add URL</asp:LinkButton><!--
    --><asp:LinkButton ID="LNBaddurltomoduleitemandcommunity" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Add URL & Add to Community</asp:LinkButton><!--
--></asp:View>
    <asp:View ID="VIWjqueryCommands" runat="server"><!--
    --><asp:HyperLink ID="HYPuploadtomoduleitem" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Upload File</asp:HyperLink><!--
    --><asp:HyperLink ID="HYPuploadtomoduleitemandcommunity" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Upload File & Add to Community</asp:HyperLink><!--
    --><asp:HyperLink ID="HYPlinkfromcommunity" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Select Community File</asp:HyperLink><!--
    --><asp:HyperLink ID="HYPaddurltomoduleitem" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Add URL</asp:HyperLink><!--
    --><asp:HyperLink ID="HYPaddurltomoduleitemandcommunity" runat="server" CssClass="linkMenu ddbutton" Visible="false">*Add URL & Add to Community</asp:HyperLink><!--
    --></asp:View>
</asp:MultiView></div>
<asp:Literal ID="LTddbutton" runat="server" Visible="false">linkMenu ddbutton</asp:Literal>
<asp:Literal ID="LTddbuttonActive" runat="server" Visible="false">active</asp:Literal>
<asp:Literal ID="LTddbuttonlistDisabled" runat="server" Visible="false"><div class="ddbuttonlist"></asp:Literal>
<asp:Literal ID="LTddbuttonlistEnabled" runat="server" Visible="false"><div class="ddbuttonlist enabled"></asp:Literal>
<asp:Literal ID="LTopendialogcssclassprefix" runat="server" Visible="false">opendlg</asp:Literal>