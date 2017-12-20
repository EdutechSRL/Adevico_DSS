<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="PublishIntoRepositoryFromModuleItem.aspx.vb" Inherits="Comunita_OnLine.PublishIntoRepositoryFromModuleItem" %>

<%@ Register TagPrefix="CTRL" TagName="ModuleFileSelector" Src="~/Modules/Common/UC/UC_ModuleInternalFileSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="~/UC/UC_SearchCommunityByService.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .DivEpButton
        {
            width: 900px;
            text-align: right; 
            padding-top: 5px;
            margin: 0px auto;
            clear: both;
        }
        .DivContainer
        {
            width: 100%;
            }
        .WizardContaner
        {
            width: 100%;
            background-color:#EFF3FB;
            border-width:1px;
            border-color: #B5C7DE;
            }
        .StepStyle
        {
            font-size:0.8em;
       }
        .StepStyleContainer
        {
            height: 400px;
            padding: 10px 10px 10px 10px;
            background-color: White;
       }
       .PublishRow
       {
           clear:both;
           width: 100%;
           padding-top: 10px;
           }
       .RenameRow
       {
           padding-left: 4%;
           }
       .SummaryRow
       {
           clear:both;
           width: 100%;
           }
        table.itemsTable
        {
            margin-top:3em;
            width:100%;            
            border-width: 2px;
	        border-spacing: 2px;
	        border-style: outset;
	        border-color: #9C9C9C;
	        border-collapse: collapse;
	        /*background-color: white;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="DivEpButton" runat="server" id="DVmenu">
        <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back to file management" Height="18px"></asp:HyperLink>
    </div>
    <asp:MultiView ID="MLVpublish" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWerror" runat="server">
            <div style="padding-top: 100px; padding-bottom: 100px;">
                <asp:Label runat="server" ID="LBerror"></asp:Label>
            </div>
        </asp:View>
        <asp:View ID="VIWselectFolder" runat="server">
            <div class="DivContainer">
                <asp:Wizard ID="WZRcommunityDiary" runat="server" ActiveStepIndex="0" cssclass="WizardContaner" DisplaySideBar="false">
                    <WizardSteps>
                        <asp:WizardStep ID="WSTfile" runat="server" Title="Select file to publish" StepType="Start">
                            <asp:Label runat="server" ID="LBfileToExport" Font-Bold="true" ForeColor="#00008B">Select file/s to publish into community repository</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="StepStyleContainer">
                                <CTRL:ModuleFileSelector ID="CTRLmoduleFileSelector" runat="server" />
                                <br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTcommunity" runat="server" Title="Select community" StepType="Step">
                            <asp:Label runat="server" ID="LBselectCommunity" Font-Bold="true" ForeColor="#00008B">Select community</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="StepStyleContainer">
                                <CTRL:CTRLsearch ID="CTRLcommunity" runat="server" SelectionMode="single" AllowMultipleOrganizationSelection="false"
                                    AllowCommunityChangedEvent="true" />
                            </div>
                            <br />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTfolder" runat="server" Title="Select community folder" StepType="Step">
                            <asp:Label runat="server" ID="LBselectFolder" Font-Bold="true" ForeColor="#00008B">Select community folder</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="StepStyleContainer">
                                <asp:Label runat="server" ID="LBinfoFolder"></asp:Label>
                                <br />
                                <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="900px" AjaxEnabled="false" />
                                <br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTmultipleFileError" runat="server" Title="Rename file to publish into community folder"
                            StepType="Step">
                            <asp:Label runat="server" ID="LBmultipleFileError" Font-Bold="true" ForeColor="#00008B">Select name for</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="StepStyleContainer">
                                <asp:Repeater ID="RPTfileName" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div class="PublishRow">
                                                    <asp:Label ID="LBfileNameToReplace" runat="server" AssociatedControlID="TXBfileName"></asp:Label>
                                                </div>
                                                <div class="PublishRow RenameRow">
                                                    <asp:Literal ID="LTfileID" runat="server" Visible="false"></asp:Literal>
                                                    <asp:Literal ID="LTimageFile" runat="server"></asp:Literal>
                                                    <asp:TextBox ID="TXBfileName" runat="server" Columns="60" MaxLength="150"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVfileName" runat="server" ControlToValidate="TXBfileName"
                                                        Text="*"></asp:RequiredFieldValidator>
                                                    <asp:Literal ID="LTfileType" runat="server"></asp:Literal>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <br />
                                <br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTcomplete" runat="server" StepType="Finish">
                            <asp:Label runat="server" ID="LBsummary" Font-Bold="true" ForeColor="#00008B">Select name for</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="StepStyleContainer">
                                <asp:Label runat="server" ID="LBsummaryDescription"></asp:Label>
                                <br />
                                <asp:Repeater ID="RPTfileToPublish" runat="server">
                                    <HeaderTemplate>
                                        <table class="itemsTable">
                                             <tr class="ROW_header_Small_Center">
                                                <th>
                                                </th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# me.BackGroundItem(Container.ItemType)%>'>
                                            <td>
                                                <div class="SummaryRow">
                                                    <asp:Literal ID="LTfileID" runat="server" Visible="false"></asp:Literal>
                                                    <asp:Literal ID="LTfileNameIcon" runat="server"></asp:Literal>
                                                    <asp:Literal ID="LTfileName" runat="server" Visible="false"></asp:Literal>
                                                    <asp:Literal ID="LTfileExtension" runat="server" Visible="false"></asp:Literal>
                                                </div>    
                                                <div class="SummaryRow">
                                                    <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="Titolo_campoSmall">Visible To:</asp:Label>&nbsp;
                                                    <asp:RadioButtonList ID="RBLvisibleTo" runat="server" CssClass="Testo_campoSmall"
                                                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                        <asp:ListItem Selected="true" Value="True">A tutti</asp:ListItem>
                                                        <asp:ListItem Value="False">Al proprietario</asp:ListItem>
                                                    </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <br />
                                <br />
                            </div>
                        </asp:WizardStep>
                    </WizardSteps>
                    <StepStyle  CssClass="StepStyle" />
                    <SideBarButtonStyle BackColor="#507CD1" ForeColor="White" />
                    <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                        BorderWidth="1px"  Font-Size="0.8em" ForeColor="#284E98" />
                    <SideBarStyle BackColor="#507CD1" Font-Size="0.9em" VerticalAlign="Top" />
                    <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                        Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
                    <StartNavigationTemplate>
                        <asp:Button ID="BTNgoToBackUrl" runat="server" CommandName="Cancel" Text="Torna all'elenco"
                            CausesValidation="false" />
                        <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                    </StartNavigationTemplate>
                    <StepNavigationTemplate>
                        <asp:Button ID="BTNgoToBackUrl" runat="server" CommandName="Cancel" Text="Torna all'elenco"
                            CausesValidation="false" />
                        <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                            Text="Precedente" />
                        <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                    </StepNavigationTemplate>
                    <FinishNavigationTemplate>
                        <asp:Button ID="BTNgoToBackUrl" runat="server" CommandName="Cancel" Text="Torna all'elenco" />
                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                            Text="Precedente" />
                        <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Fine" />
                    </FinishNavigationTemplate>
                </asp:Wizard>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
