<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="NoticeboardAdvanced.aspx.vb" Inherits="Comunita_OnLine.NoticeboardAdvanced" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 900px; text-align: right; padding-top: 5px;">
        <asp:HyperLink ID="HYPback" runat="server" Text="Back"  CssClass="linkMenu"></asp:HyperLink>
        <asp:Button ID="BTNsaveADV" runat="server" Text="Save" />
    </div>
    <div style="width: 900px; margin-top: 5px; clear: both; text-align: center;" align="center">
        <CTRL:CTRLeditor id="CTRLeditor" runat="server" ContainerCssClass="containerclass" 
            LoaderCssClass="loadercssclass" EditorHeight="400px" EditorWidth="100%" AllAvailableFontnames="true"
            AutoInitialize="true" ModuleCode="SRVbach" >
        </CTRL:CTRLeditor>
    </div>
    <br />
</asp:Content>