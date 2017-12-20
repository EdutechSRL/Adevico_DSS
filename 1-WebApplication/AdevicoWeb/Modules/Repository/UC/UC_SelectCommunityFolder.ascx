<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectCommunityFolder.ascx.vb"
    Inherits="Comunita_OnLine.UC_SelectCommunityFolder" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style type="text/css">
    .rtChk
    {
        font-size: large;
    }
</style>
<asp:Literal ID="LTnoFolder" runat="server">No community folder found !</asp:Literal>
<telerik:radtreeview id="RDTcommunityRepository" runat="server" autopostback="false"
     cssfile="~/RadControls/TreeView/Skins/Materiale/StyleImport.css"
    checkchildnodes="true" multipleselect="false"  />
