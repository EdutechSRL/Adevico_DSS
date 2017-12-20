<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Add.aspx.vb" Inherits="Comunita_OnLine.AddDashboardSettings" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Common/UC/UC_GenericWizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Content" Src="~/Modules/Dashboard/Settings/UC/UC_DashboardBaseSettings.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <link href="<%=GetBaseUrl()%>Graphics/Modules/TileTag/css/TileTag.css?v=201604071200lm" rel="Stylesheet" />
    <script src="<%=GetBaseUrl()%>Jscript/Modules/TileTag/TileTag.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="contentwrapper edit clearfix persist-area">	        
    <div class="vwizard viewsetsettings step1 hasheader">
        <div class="column left persist-header copyThis">
            <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
        </div>
        <div class="column right resizeThis">
            <div class="rightcontent">
                <div class="header">
                    <div class="DivEpButton">
                         <asp:Button ID="BTNaddDashboardSettingsTop" runat="server" Text="*Save" Visible="false" class="linkMenu" />
                         <asp:HyperLink ID="HYPbackToDashboardListTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                    </div>
                    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                </div>
                <div class="content">
					<CTRL:Content ID="CTRLsettings" runat="server" Visible="false" />
                </div>
				<div class="footer">
					<div class="DivEpButton">
						<asp:Button ID="BTNaddDashboardSettingsBottom" runat="server" Text="*Save" Visible="false" />
                        <asp:HyperLink ID="HYPbackToDashboardListBottom" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
					</div>
				</div>
            </div>
        </div>
    </div>
</div>
</asp:Content>