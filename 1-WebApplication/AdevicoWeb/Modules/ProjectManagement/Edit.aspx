<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Edit.aspx.vb" Inherits="Comunita_OnLine.EditProject" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/ProjectManagement/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ConfirmSettings" Src="~/Modules/ProjectManagement/UC/UC_ConfirmProjectSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="BaseSettings" Src="~/Modules/ProjectManagement/UC/UC_ProjectBaseSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="BaseSettingsHeader" Src="~/Modules/ProjectManagement/UC/UC_ProjectBaseSettingsHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link href="../../Graphics/Modules/ProjectManagement/Css/ProjectManagement.css" rel="Stylesheet" />
     <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" />

    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
       <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>

     <script language="javascript" type="text/javascript">
         $(document).ready(function () {
             $(".view-modal.view-confirmsettings").dialog({
                 appendTo: "form",
                 closeOnEscape: false,
                 modal: true,
                 width: 890,
                 height: 350,
                 minHeight: 300,
                 minWidth: 700,
                 title: '<%=ConfirmSettingsDialogTitleTranslation() %>',
                 open: function (type, data) {
                     //$(this).parent().appendTo("form");
                     $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                 }
             });
         });
    </script>
     <CTRL:BaseSettingsHeader ID="CTRLbaseSettingsHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBemptyMessage" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div style="height: auto;" class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:Button ID="BTNsaveProjectSettingsTop" runat="server" Text="*Save" Visible="false" />
                                <asp:HyperLink ID="HYPgoToProjectMapTop" class="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                            </div>
                            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content clearfix">
                                <div class="fieldobject">
                                    <CTRL:BaseSettings ID="CTRLsettings" runat="server" DisplayActivitiesToAdd="false" AllowEditDateCalculationByCpm="false"  />
                                </div>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:Button ID="BTNsaveProjectSettingsBottom" runat="server" Text="*Save" Visible="false" />
                                <asp:HyperLink ID="HYPgoToProjectMapBottom" class="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsBottom" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="view-modal view-confirmsettings" id="DVconfirmSettings" runat="server" visible="false">
        <CTRL:ConfirmSettings id="CTRLconfirmSettings" runat="server" RaiseCommandEvents="True"></CTRL:ConfirmSettings>
    </div>
</asp:Content>