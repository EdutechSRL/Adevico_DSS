<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Tree.ascx.vb" Inherits="Comunita_OnLine.UC_MenubarTree" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    .rtChk
    {
        font-size: large;
    }
</style>
    <telerik:radtreeview id="RDTmenu" runat="server" autopostback="false"
        cssfile="~/RadControls/TreeView/Skins/Materiale/StyleImport.css"
        checkchildnodes="true" multipleselect="false" EnableDragAndDrop="True" />
