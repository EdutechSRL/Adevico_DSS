<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="NoticeboardSimple.aspx.vb" Inherits="Comunita_OnLine.NoticeboardSimple" ValidateRequest="false" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLeditorHTML" Src="~/Generici/UC/UC_EditorHTML.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 900px; text-align: right; padding-top: 5px;">
        <asp:HyperLink ID="HYPback" runat="server" Text="Back" CssClass="linkMenu"></asp:HyperLink>
        <asp:Button ID="BTNsaveHTML" runat="server" Text="Save" />
    </div>
    <div style="width: 900px; margin-top: 5px;">
        <div style="text-align: left; font-weight: bold; margin: 0px 0px 0px 70px; padding: 0px 0px 0px 0px;
            float: left;">
            <CTRL:CTRLeditorHTML id="CTRLeditorHTML" runat="server" EditorAlign="center" AllowScrolling="false">
            </CTRL:CTRLeditorHTML>
        </div>
    </div>
    <br />
</asp:Content>