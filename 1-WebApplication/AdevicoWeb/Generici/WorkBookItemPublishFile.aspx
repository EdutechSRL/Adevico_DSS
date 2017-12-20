<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookItemPublishFile.aspx.vb" Inherits="Comunita_OnLine.WorkBookItemPublishFile" %>

<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="../UC/UC_SearchCommunityByService.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
 <div id="Div2" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
                clear: both;" runat="server">
                <asp:HyperLink ID="HYPbackToFileManagement" runat="server"
                    CssClass="Link_Menu" Visible="false" Text="Back to file management" Height="18px"></asp:HyperLink>
            </div>
    <asp:MultiView ID="MLVpublish" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWerror" runat="server">
            <div style="padding-top:100px; padding-bottom: 100px;">
            <asp:label runat="server" ID="LBerror"></asp:label>
            </div>
        </asp:View>
        <asp:View ID="VIWselectFolder" runat="server">
           
            <div style="width: 900px;">
                <asp:Wizard ID="WZRworkBook" runat="server" ActiveStepIndex="0" Width="900px" BackColor="#EFF3FB"
                    BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="false">
                    <WizardSteps>
                        <asp:WizardStep ID="WSTfile" runat="server" Title="Select file to publish" StepType="Start">
                            <asp:Label runat="server" ID="LBfileToExport" Font-Bold="true" ForeColor="#00008B">Select file/s to publish into community repository</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="height: 400px; padding: 20px, auto; margin: 0, auto;
                                background-color: White;">
                                 <asp:CheckBoxList ID="CBLworkBookFile" runat="server" CssClass="testo_campoSmall"></asp:CheckBoxList>
                                <br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTcommunity" runat="server" Title="Select community" StepType="Step">
                            <asp:Label runat="server" ID="LBselectCommunity" Font-Bold="true" ForeColor="#00008B">Select community</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <CTRL:CTRLsearch  id="CTRLcommunity" runat="server" SelectionMode="single" AllowMultipleOrganizationSelection="false"  AllowCommunityChangedEvent="true"/>
                             <br />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTfolder" runat="server" Title="Select community folder" StepType="Step">
                            <asp:Label runat="server" ID="LBselectFolder" Font-Bold="true" ForeColor="#00008B">Select community folder</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="height: 400px; padding: 20px, auto; margin: 0, auto;
                                background-color: White;">
                                 <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="900px" AjaxEnabled="false" />
                                <br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTmultipleFileError" runat="server" Title="Rename file to publish into community folder" StepType="Step">
                            <asp:Label runat="server" ID="LBmultipleFileError" Font-Bold="true" ForeColor="#00008B">Select name for</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="height: 400px; padding: 20px, auto; margin: 0, auto;
                                background-color: White;">
                                 <asp:Repeater ID="RPTfileName" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div>
                                                    <asp:Label id="LBfileNameToReplace" runat="server" AssociatedControlID="TXBfileName"></asp:Label>
                                                </div>
                                                <div style=" padding-left:50px;">   
                                                    <asp:Literal ID="LTfileID" runat="server" Visible="false"></asp:Literal>
                                                    <asp:Literal ID="LTimageFile" runat="server"></asp:Literal>
                                                    <asp:TextBox ID="TXBfileName" runat="server" Columns="60" MaxLength="150"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVfileName" runat="server" ControlToValidate="TXBfileName" Text="*"></asp:RequiredFieldValidator>
                                                    <asp:Literal ID="LTfileType" runat="server"></asp:Literal>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                 </asp:Repeater>
                                <br /><br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTcomplete" runat="server" StepType="Finish">
                            <asp:Label runat="server" ID="LBsummary" Font-Bold="true" ForeColor="#00008B">Select name for</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="padding: 40px,auto; background-color: White; height: 400px;">
                                <asp:Label runat="server" ID="LBsummaryDescription"></asp:Label>
                                 <br />
                                 <asp:Repeater ID="RPTfileToPublish" runat="server">
                                    <HeaderTemplate>
                                        <div style=" padding-top:10px; width:900px;">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding-left:5px; float:left; width:300px;">   
                                            <asp:Literal ID="LTfileID" runat="server" Visible="false"></asp:Literal>
                                            <asp:Literal ID="LTfileNameIcon" runat="server"></asp:Literal>
                                            <asp:Literal ID="LTfileName" runat="server" Visible="false"></asp:Literal>
                                            <asp:Literal ID="LTfileExtension" runat="server" Visible="false"></asp:Literal>
                                        </div>
                                         <div style="padding-left:5px; float:left;">
                                            <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="Titolo_campoSmall">Visible To:</asp:Label>&nbsp;
                                           <asp:RadioButtonList ID="RBLvisibleTo" runat="server" CssClass="Testo_campoSmall"
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Selected="true" Value="True">A tutti</asp:ListItem>
                        <asp:ListItem Value="False">Al proprietario</asp:ListItem>
                    </asp:RadioButtonList>
                                        </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </div>
                                    </FooterTemplate>
                                 </asp:Repeater>
                                  <br /><br />
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
                        <asp:Button ID="BTNgoToWorkBookList" runat="server" CommandName="Cancel" Text="Torna all'elenco" CausesValidation="false" />
                        <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                    </StartNavigationTemplate>
                    <StepNavigationTemplate>
                        <asp:Button ID="BTNgoToWorkBookList" runat="server" CommandName="Cancel" Text="Torna all'elenco" CausesValidation="false" />
                        <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                            Text="Precedente"  />
                        <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                    </StepNavigationTemplate>
                    <FinishNavigationTemplate>
                        <asp:Button ID="BTNgoToWorkBookList" runat="server" CommandName="Cancel" Text="Torna all'elenco" />
                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                            Text="Precedente" />
                        <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Fine" />
                    </FinishNavigationTemplate>
                </asp:Wizard>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>