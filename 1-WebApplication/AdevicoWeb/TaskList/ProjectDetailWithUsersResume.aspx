<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ProjectDetailWithUsersResume.aspx.vb" 
Inherits="Comunita_OnLine.ProjectDetailWithUsersResume" %>

<%@ Register TagPrefix="CTRL" TagName="involvedUsersDetail" Src="UC/UC_involvedUsersDetail.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
     <div id="DVtitle" style="width: 900px; text-align: left;" class="RigaTitolo" align="center">
        <asp:Label ID="LBtitoloSuperiore" runat="server">**Nome Progetto</asp:Label>
    </div>
    <div>
         <asp:HyperLink ID="HYPreturn" runat="server" CssClass="Link_Menu" Text="**return"></asp:HyperLink>
        <asp:HyperLink ID="HYPdetail" runat="server" CssClass="Link_Menu" Text="**return"></asp:HyperLink>
    </div>

    <asp:MultiView ID="MLVprojectUsersTable" runat="server" ActiveViewIndex ="0" >
        <asp:View ID="VIWusersTablePerProject" runat="server" >

            <div id="DIVusersPerProjectsTable" style="padding: 5px;">                
                <CTRL:involvedUsersDetail ID="CTRLinvolvedUsersDetail" runat="server" />
            </div>
         </asp:View>

         <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div align="right" style="text-align: right; clear: right">
                    <asp:HyperLink ID="HYPreturnError" runat="server" Text="**Return" CssClass="Link_Menu" />
                </div>
                <div align="center" style="padding: 5px;">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
         </asp:View>

    </asp:MultiView>

</asp:Content>
