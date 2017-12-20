<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WorkBookAdd.aspx.vb" Inherits="Comunita_OnLine.WorkBookAdd"
    Theme="Materiale" MaintainScrollPositionOnPostback="true" 
    MasterPageFile="~/AjaxPortal.Master"%>


<%@ Register TagPrefix="CTRL" TagName="CTRLtype" Src="UC_DiarioPersonale/UC_WorkBookType.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WorkBook" Src="UC_DiarioPersonale/UC_WorkBookData.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="../UC/UC_SearchUserByCommunities.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="../UC/UC_SearchCommunityByService.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
        <div id="DVmenu" style="width: 900px; text-align: right;" align="center">
            <asp:Button ID="BTNreturnToList" runat="server" Text="Torna all'elenco" />
        </div>
        <div align="left" style="width: 900px; padding-top: 5px;">
            <asp:MultiView ID="MLVworkBook" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWcreate" runat="server">
                    <asp:Wizard ID="WZRworkBook" runat="server" ActiveStepIndex="0" Width="100%" BackColor="#EFF3FB"
                        BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="false">
                        <WizardSteps>
                            <asp:WizardStep ID="WSTcommunity" runat="server" Title="Scelta comunità" StepType="Step">
                                <asp:Label runat="server" ID="LBworkbookCommunity" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                                <hr style="color: #00008B;" />
                                <div style="width: 900px; min-height: 400px; padding: 20px, auto; margin: 0, auto;
                                    background-color: White;">
                                    <CTRL:CTRLsearch ID="CTRLcommunity" runat="server" SelectionMode="single" AllowMultipleOrganizationSelection="false"
                                        AllowCommunityChangedEvent="true" />
                                    <br />
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WSTtype" runat="server" Title="Scelta comunità" StepType="Step">
                                <asp:Label runat="server" ID="LBworkbookType" Font-Bold="true" ForeColor="#00008B"></asp:Label><br />
                                <hr style="color: #00008B;" />
                                <div style="width: 100%; padding: 20px, auto; min-height: 400px; margin: auto, auto;
                                    background-color: White;">
                                    <CTRL:CTRLtype ID="CTRLtype" runat="server" />
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WSTdata" runat="server" Title="Dati quaderno" StepType="Step">
                                <div class="titolo">
                                    <asp:Label runat="server" ID="LBworkBookData" CssClass="titolo">Dati WorkBook</asp:Label>
                                </div>
                                <hr style="color: #00008B;" />
                                <div style="padding: 20px, auto; margin: 0, auto; background-color: White;">
                                    <CTRL:WorkBook ID="CTRLworkBook" runat="server" Mode="Creating"></CTRL:WorkBook>
                                    <br />
                                    <br />
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WSTauthors" runat="server" Title="Autori" StepType="Step">
                                <div class="titolo">
                                    <asp:Label runat="server" ID="LBauthors" CssClass="titolo">Autori</asp:Label>
                                </div>
                                <hr style="color: #00008B;" />
                                <div style="padding: 20px,auto; min-height: 400px; background-color: White;">
                                    <div style="padding: 20px,auto; background-color: White;">
                                        <asp:CheckBox ID="CBXselfAuthor" runat="server" />
                                    </div>
                                    <div style="padding: 20px,auto; background-color: White;">
                                        <CTRL:USERlist ID="CTRLuserList" runat="server"></CTRL:USERlist>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WSTowner" runat="server" Title="Proprietario" StepType="Finish">
                                <div class="titolo">
                                    <asp:Label runat="server" ID="LBownerTitle" CssClass="titolo"></asp:Label>
                                </div>
                                <hr style="color: #00008B;" />
                                <div style="padding: 40px,auto; margin: 0, auto; background-color: White; min-height: 400px;">
                                    <asp:RadioButtonList ID="RBLowner" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow">
                                    </asp:RadioButtonList>
                                    <br />
                                    <br />
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WSTcomplete" runat="server" StepType="Complete">
                                <div class="titolo">
                                    <asp:Label runat="server" ID="LBcomplete" CssClass="titolo"></asp:Label>
                                </div>
                                <hr style="color: #00008B;" />
                                <div style="padding: 40px,auto; background-color: White; min-height: 400px;">
                                    <asp:Label runat="server" ID="LBcompleteMessage"></asp:Label>
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
                            <asp:Button ID="BTNgoToWorkBookList" runat="server" CommandName="Cancel" Text="Torna all'elenco" />
                            <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                        </StartNavigationTemplate>
                        <StepNavigationTemplate>
                            <asp:Button ID="BTNgoToWorkBookList" runat="server" CommandName="Cancel" Text="Torna all'elenco" />
                            <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                Text="Precedente" />
                            <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                        </StepNavigationTemplate>
                        <FinishNavigationTemplate>
                            <asp:Button ID="BTNgoToWorkBookList" runat="server" CommandName="Cancel" Text="Torna all'elenco" />
                            <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                Text="Precedente" />
                            <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Fine" />
                        </FinishNavigationTemplate>
                    </asp:Wizard>
                </asp:View>
                <asp:View ID="VIWnoPermission" runat="server">
                    <div id="DVpermessi" align="center">
                        <div align="right" style="text-align: right; clear: right">
                            <asp:Button ID="BTNreturnToWorkBookList" runat="server" Text="Torna all'elenco" />
                        </div>
                        <div style="height: 50px;">
                        </div>
                        <div align="center">
                            <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                        </div>
                        <div style="height: 50px;">
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
</asp:Content>
