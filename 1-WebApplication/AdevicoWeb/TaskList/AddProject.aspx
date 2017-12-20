<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddProject.aspx.vb" Inherits="Comunita_OnLine.AddProject"
    MasterPageFile="~/AjaxPortal.Master" EnableTheming="true" ValidateRequest="false" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="../UC/UC_SearchCommunityByService.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLprojectType" Src="UC/UC_TaskListType.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Detail" Src="UC/UC_TaskDetail.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVtitle" style="width: 900px; text-align: left;" class="RigaTitolo" align="center">
        <asp:Label ID="LBtitoloSuperiore" runat="server">Add Project</asp:Label>
    </div>
    <asp:MultiView ID="MLVaddProject" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWaddProject" runat="server">
            <asp:Wizard ID="WZRtaskProject" runat="server" ActiveStepIndex="0" Width="100%" BackColor="#EFF3FB"
                BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="false">
                <WizardSteps>
                    <asp:WizardStep ID="WSTselectType" runat="server" Title="Select Project's Type" StepType="Start">
                        <asp:Label runat="server" ID="LBprojectType" Font-Bold="true" ForeColor="#00008B"
                            Text="Select Project's Type**"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div style="padding: 5px;">
                            <CTRL:CTRLprojectType ID="CTRLprojectType" runat="server" />
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WSTselectCommunity" runat="server" Title="Select Community" StepType="Step">
                        <asp:Label runat="server" ID="LBselectCommunity" Font-Bold="true" ForeColor="#00008B"
                            Text="Select Community**"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div style="padding: 5px;">
                            <CTRL:CTRLsearch ID="CTRLcommunity" runat="server" SelectionMode="single" AllowMultipleOrganizationSelection="false"
                                AllowCommunityChangedEvent="true" />
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WSTcomplete" runat="server" StepType="Finish">
                        <asp:Label runat="server" ID="LBprojectProperties" Font-Bold="true" ForeColor="#00008B"
                            Text="Set Project Properties**"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div id="DIVtaskDetail" runat="server" style="padding: 5px;">
                            <CTRL:Detail ID="CTRLdetail" runat="server" />
                        </div>
                    </asp:WizardStep>
                </WizardSteps>
                <StepStyle Font-Size="0.8em" ForeColor="#333333" />
                <SideBarButtonStyle BackColor="#507CD1" ForeColor="White" />
                <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                    BorderWidth="1px" Font-Size="0.8em" ForeColor="#284E98" />
                <SideBarStyle BackColor="#507CD1" Font-Size="0.9em" VerticalAlign="Top" />
                <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                    Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
                <StartNavigationTemplate>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="**Return" CssClass="Link_Menu" />
                </StartNavigationTemplate>
                <StepNavigationTemplate>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="**Return" CssClass="Link_Menu" />
                    <asp:Button ID="PreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                        Text="Previus*" CssClass="Link_Menu" />
                    <asp:Button ID="BTNnext" runat="server" CausesValidation="false" CommandName="Next"
                        Text="**NextProva" CssClass="Link_Menu" OnClick="BTNnextClick" />
                </StepNavigationTemplate>
                <FinishNavigationTemplate>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="**Return" CssClass="Link_Menu" />
                    <asp:Button ID="PreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                        Text="Previus*" CssClass="Link_Menu" />
                    <asp:Button ID="FinishButton" runat="server" CausesValidation="True" CommandName="MoveComplete"
                        Text="**Complete" CssClass="Link_Menu" />
                </FinishNavigationTemplate>
            </asp:Wizard>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div align="right" style="text-align: right; clear: right">
                    <asp:HyperLink ID="HYPreturnToTaskList" runat="server" Text="Return To TaskList"
                        CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBNerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
