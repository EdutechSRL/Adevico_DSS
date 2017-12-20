<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="UsersPortalList.aspx.vb" Inherits="Comunita_OnLine.UsersPortalList"  Theme="Materiale" EnableTheming="true" %>
<%@ Register TagPrefix="CTRL" TagName="UsersList" Src="./UC/UC_UsersAccessRegister.ascx"%>
<%@ Register TagPrefix="CTRL" TagName="AccessTab" Src="./UC/UC_AccessTab.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.UsageResults.DomainModel" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
<div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        padding-bottom: 5px;">
        <asp:HyperLink id="HYPbackHistory" runat="server" CssClass="Link_Menu" Visible="false" Text="Back"></asp:HyperLink>
    </div> 
  <div style="width: 900px; text-align:center; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <CTRL:AccessTab id="CTRLaccessTab" runat="server" CurrentView="UsersPortalPresence"></CTRL:AccessTab>
    </div>
    <div>
        <CTRL:UsersList id="CTRLusersList" runat="server" isPortal="true"></CTRL:UsersList>
    </div>
</asp:Content>