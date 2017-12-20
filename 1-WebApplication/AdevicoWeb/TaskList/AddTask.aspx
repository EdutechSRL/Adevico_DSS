<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddTask.aspx.vb" Inherits="Comunita_OnLine.AddTask"
    MasterPageFile="~/AjaxPortal.Master" ValidateRequest="false" %>

<%@ Register TagPrefix="CTRL" TagName="Detail" Src="UC/UC_TaskDetail.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AssignedUser" Src="UC/UC_AssignedUser.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectTask" Src="UC/UC_SelectTask.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AddVirtualAssignment" Src="UC/UC_AddVirtualTaskAssignments.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ChildrenResource" Src="UC/UC_ReallocateResourcesOnNodes.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVtitle" style="width: 900px; text-align: left;" class="RigaTitolo" align="center">
        <asp:Label ID="LBtitoloSuperiore" runat="server">Add Tasks**</asp:Label>
    </div>
    <asp:MultiView ID="MLVaddProject" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWaddProject" runat="server">
            <asp:Wizard ID="WZRaddTask" runat="server" Width="100%" BackColor="#EFF3FB" BorderColor="#B5C7DE"
                BorderWidth="1px" DisplaySideBar="false">
                <WizardSteps>
                    <asp:WizardStep ID="WSTselectParent" runat="server" Title="Select Parent" StepType="Start">
                        <asp:Label runat="server" ID="LBselectParent" Font-Bold="true" ForeColor="#00008B"
                            Text="**Select Task Parent"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div>
                            <CTRL:SelectTask ID="CTRLselectTask" runat="server"></CTRL:SelectTask>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WSTtaskDetail" runat="server" StepType="Step" Title="Set Task Properties">
                        <asp:Label runat="server" ID="LBtaskProperties" Font-Bold="true" ForeColor="#00008B"
                            Text="**Set Task Properties For Child Of "></asp:Label>
                        <asp:Label runat="server" ID="LBtaskPropertiesChild" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div id="DIVtaskDetail" runat="server">
                            <CTRL:Detail ID="CTRLdetail" runat="server" />
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WSTmanageResource" runat="server" StepType="Step" Title="Resume">
                        <asp:Label runat="server" ID="LBmanageResource" Font-Bold="true" ForeColor="#00008B"
                            Text="**User Management For Child Of"></asp:Label>
                        <asp:Label runat="server" ID="LBmangeResourceParentName" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div style="padding: 5px;">
                            <asp:Label ID="LBmanagerError" runat="server" CssClass="erroreSmall" Text="***Ogni task deve avere almeno un manager attivo"></asp:Label>
                        </div>
                        <div align="center" style="padding: 5px;">
                            <CTRL:ChildrenResource ID="CTRLmanageChildrenResource" runat="server" />
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WSTaddUser" runat="server" StepType="Step" Title="Add User">
                        <asp:Label runat="server" ID="LBaddUsersTitle" Font-Bold="true" ForeColor="#00008B"
                            Text="**Add Users"></asp:Label>
                        <asp:Label runat="server" ID="LBaddUserTaskName" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div style="padding: 5px;">
                            <CTRL:AddVirtualAssignment ID="CTRLaddVirtualAssignment" runat="server" />
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WSTresume" runat="server" StepType="Finish" Title="Resume">
                        <asp:Label runat="server" ID="LBresume" Font-Bold="true" ForeColor="#00008B" Text="**Resume Task Data"></asp:Label>
                        <asp:Label runat="server" ID="LBresumeParentName" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div align="center" style="padding: 5px;">
                            <CTRL:ChildrenResource ID="CTRLresumeChildrenResource" runat="server" />
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
                    <asp:Button ID="BTNnext" runat="server" CausesValidation="False" CommandName="ChangeParent"
                        Text="**Next" CssClass="Link_Menu" />
                </StartNavigationTemplate>

                <StepNavigationTemplate>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="**Return" CssClass="Link_Menu" />
                    <asp:Button ID="BTNchangeParent" runat="server" CausesValidation="False" CommandName="ChangeParent"
                        Text="**Change Parent" CssClass="Link_Menu" />
                    <asp:Button ID="BTNpreviousButton" runat="server" CausesValidation="False" CommandName="PreviousStep" Text="**Previous" CssClass="Link_Menu">
                        </asp:Button>
                    <asp:Button ID="BTNnext" runat="server" CausesValidation="True" CommandName="ChangeParent"
                        Text="**Next" CssClass="Link_Menu" />
                    <asp:Button ID="BTNAddAnotherChild" runat="server" CausesValidation="True" CommandName="AddChild"
                        Text="**Add Another Child" CssClass="Link_Menu" />
                </StepNavigationTemplate>

                <FinishNavigationTemplate>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="**Return" CssClass="Link_Menu" />
                    <asp:Button ID="BTNaddAnotherChild" runat="server" CausesValidation="False" CommandName="AddAnotherChild"
                        Text="**Add Another Child" CssClass="Link_Menu" />
                    <asp:Button ID="BTNchangeParent" runat="server" CausesValidation="False" CommandName="ChangeParent"
                        Text="**Change Parent" CssClass="Link_Menu" />
                    <asp:Button ID="BTNpreviousButton" runat="server" CausesValidation="False" CommandName="PreviousStep"
                        Text="**Previus" CssClass="Link_Menu" />
                    <asp:Button ID="FinishButton" runat="server" CausesValidation="False" CommandName="MoveComplete"
                        Text="**Complete" CssClass="Link_Menu" />
                </FinishNavigationTemplate>
            </asp:Wizard>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div align="right" style="text-align: right; clear: right;">
                    <asp:HyperLink ID="HYPreturnToTaskList" runat="server" Text="**Return To TaskList"
                        CssClass="Link_Menu" />
                </div>
                <div align="center" style="padding: 5px;">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
