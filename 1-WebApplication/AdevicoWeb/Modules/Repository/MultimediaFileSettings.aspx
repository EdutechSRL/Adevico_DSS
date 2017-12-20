<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="MultimediaFileSettings.aspx.vb" Inherits="Comunita_OnLine.MultimediaFileSettings" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Repository.Common.css" type="text/css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        padding-bottom: 5px;">
        <span style="vertical-align: text-bottom;">
            <asp:HyperLink ID="HYPback" runat="server" CssClass="Link_Menu" Visible="false"
                Text="Back to management" Height="18px"></asp:HyperLink>
            <asp:Button ID="BTNsetDefault" runat="server" Visible="false"/>
        </span>
    </div>
    <div style="width: 900px; text-align: left; padding-top: 5px; margin: 0px auto;
        padding-bottom: 5px;">
        <div style="float:left; width:602px">
            <asp:Literal ID="LTmultimediaInfo" runat="server"></asp:Literal>
            <telerik:RadTreeView ID="RDTmultimediaFiles" runat="server" Width="600px" AutoPostBack="false"
             CssFile="~/RadControls/TreeView/Skins/Materiale/StyleImport.css" MultipleSelect="false"  />
        </div>
        <div style="float:left; width:296px;">
            <br /><br /><br /><br /><br />
            <asp:Literal ID="LTinfo" runat="server">ATTENZIONE: una voce dell'albero a sinistra per selezionare il file da caricare di default all'apertura del pacchetto multimediale.</asp:Literal>
        </div>
    </div>
</asp:Content>