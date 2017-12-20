<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Calendars.aspx.vb" Inherits="Comunita_OnLine.ProjectCalendars" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/ProjectManagement/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProjectManagement/Css/ProjectManagement.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBnoResources" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:Button ID="BTNsaveCalendarsTop" runat="server" Text="*Save" Visible="false" />
                                <asp:HyperLink ID="HYPgoToProjectMapTop" CssClass="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardTop" CssClass="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardTop" CssClass="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsTop" CssClass="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>                                
                            </div>
                            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content clearfix">
                                <h2>PAGINA IN FASE DI REALIZZAZIONE</h2>


                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:Button ID="BTNsaveCalendarsBottom" runat="server" Text="*Save" Visible="false" />
                                <asp:HyperLink ID="HYPgoToProjectMapBottom" CssClass="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardBottom" CssClass="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardBottom" CssClass="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsBottom" CssClass="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>