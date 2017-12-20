<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReallocateUsers.aspx.vb"
    Inherits="Comunita_OnLine.ReallocateUser" MasterPageFile="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="AddVirtualAssignment" Src="UC/UC_AddVirtualTaskAssignments.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Resource" Src="UC/UC_ReallocateResourcesOnNodes.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVtitle" style="width: 900px; text-align: left;" class="RigaTitolo" align="center">
        <asp:Label ID="LBtitoloSuperiore" runat="server">Reallocate Resource**</asp:Label>
    </div>
    <asp:MultiView ID="MLVreallocateUsers" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstepWizard" runat="server">
            <asp:Wizard ID="WZRreallocateUsers" runat="server" Width="100%" BackColor="#EFF3FB" BorderColor="#B5C7DE"
                BorderWidth="1px" DisplaySideBar="false">
                <WizardSteps>
                    <asp:WizardStep ID="WSTselectResource" runat="server" Title="Select Resource" StepType="Start">
                        <asp:Label runat="server" ID="LBselectResource" Font-Bold="true" ForeColor="#00008B"
                            Text="**Select Users"></asp:Label>
                            <asp:Label runat="server" ID="LBtaskNameSelect" Font-Bold="true" ForeColor="#00008B"
                            Text="**Select Users"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div>
                            <CTRL:Resource ID="CTRLresourceEditable" runat="server" />
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WSTconfirmResource" runat="server" Title="Resume Users" StepType="Finish">
                        <asp:Label runat="server" ID="LBresumeUsers" Font-Bold="true" ForeColor="#00008B" Text="**Confirm Resource"></asp:Label>
                        <asp:Label runat="server" ID="LBtaskNameResume" Font-Bold="true" ForeColor="#00008B"
                            Text="**Select Users"></asp:Label><br />
                        <hr style="color: #00008B;" />                        
                        <div>
                            <CTRL:Resource ID="CTRLresourceResume" runat="server" />
                        </div>
                    </asp:WizardStep>
<%--                    <asp:WizardStep ID="WSTaddUser" runat="server" StepType="Step" Title="Add User">
                        <asp:Label runat="server" ID="LBaddUsersTitle" Font-Bold="true" ForeColor="#00008B"
                            Text="**Add Users"></asp:Label>
                        <asp:Label runat="server" ID="LBaddUserTaskName" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                        <hr style="color: #00008B;" />
                        <div style="padding: 5px;">
                            <CTRL:AddVirtualAssignment ID="CTRLaddVirtualAssignment" runat="server" />
                        </div>
                    </asp:WizardStep>--%>
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
                    <asp:Button ID="NextButton" runat="server" CausesValidation="true" CommandName="MoveNext"
                        Text="**Next" CssClass="Link_Menu" />
                </StartNavigationTemplate>
<%--                <StepNavigationTemplate>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="**Return" CssClass="Link_Menu" />
                    <asp:Button ID="PreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                        Text="**Previus" CssClass="Link_Menu" />
                    <asp:Button ID="NextButton" runat="server" CausesValidation="false" CommandName="MoveNext"
                        Text="**Next" CssClass="Link_Menu" />
                </StepNavigationTemplate>--%>
                <FinishNavigationTemplate>
                    <asp:Button ID="CancelButton" runat="server" CausesValidation="false" CommandName="Cancel"
                        Text="**Return" CssClass="Link_Menu" />
                    <asp:Button ID="PreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
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
