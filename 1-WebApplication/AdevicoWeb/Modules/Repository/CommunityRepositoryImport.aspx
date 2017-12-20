<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryImport.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryImport" %>

<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="~/UC/UC_SearchCommunityByService.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_SelectCommunityFiles.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <style type="text/css">
        td
        {
            font-size: 11px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            $('#selectFolder').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 600,
                height: 400,
                minHeight: 400,
                minWidth: 600,
                zIndex: 99999,
                open: function(type, data) {
                   // $(this).parent().appendTo("form");
                }
            });
        });

        function showDialog(id) {
            $('#' + id).dialog("open");
            return false;
        }
        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
    </script>

    <asp:MultiView ID="MLVimport" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWerror" runat="server">
            <div style="padding-top: 100px; padding-bottom: 100px;">
                <asp:Label runat="server" ID="LBerror"></asp:Label>
            </div>
        </asp:View>
        <asp:View ID="VIWselectFolder" runat="server">
            <div id="Div2" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
                clear: both;" runat="server">
                <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false"
                    Text="Back to file management" Height="18px"></asp:HyperLink>
            </div>
            <div style="width: 900px;">
                <asp:Wizard ID="WZRimportCommunityItem" runat="server" ActiveStepIndex="0" Width="900px"
                    BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="false">
                    <WizardSteps>
                        <asp:WizardStep ID="WSTcommunity" runat="server" Title="Select community" StepType="Start">
                            <asp:Label runat="server" ID="LBselectCommunity" Font-Bold="true" ForeColor="#00008B">Select community</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="background-color: White;">
                                <CTRL:CTRLsearch ID="CTRLcommunity" runat="server" SelectionMode="single" AllowMultipleOrganizationSelection="false"
                                    AllowCommunityChangedEvent="true" />
                            </div>
                            <br />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTfile" runat="server" Title="Select file to publish" StepType="Step">
                            <asp:Label runat="server" ID="LBfileToExport" Font-Bold="true" ForeColor="#00008B">Select file/s to publish into community repository</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="height: 400px; padding: 20px, auto; margin: 0, auto; background-color: White;
                                overflow: auto;">
                                <CTRL:CommunityFile ID="CTRLsourceItems" runat="server" width="900px" TriStateSelection="False" FolderSelectable="true" EnableClientScript="true" />
                                <br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTfolder" runat="server" Title="Select community folder" StepType="Finish">
                            <asp:Label runat="server" ID="LBselectFolder" Font-Bold="true" ForeColor="#00008B">Select community folder</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="height: 400px; padding: 20px, auto; margin: 0, auto; background-color: White;">
                                <div style="clear: both;">
                                    <div style="float: left; width: 130px; text-align: left;">
                                        <asp:Label ID="LBcommunity_t" runat="server" CssClass="Titolo_campoSmall">Community:</asp:Label>
                                    </div>
                                    <div style="float: left;">
                                        <asp:Label ID="LBcommunity" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                    </div>
                                </div>
                                <div style="clear: both;">
                                    <div style="float: left; width: 130px; text-align: left; padding-top: 5px; padding-bottom: 5px;">
                                        <span style="vertical-align: middle;">
                                            <asp:Label ID="LBpath_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label></span>
                                    </div>
                                    <div style="float: left;">
                                        <asp:UpdatePanel ID="UDPdata" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                            runat="server">
                                            <ContentTemplate>
                                                <span style="vertical-align: middle;">
                                                    <asp:Label ID="LBpath" runat="server" CssClass="Testo_campoSmall"></asp:Label>&nbsp;
                                                    <asp:Button ID="BTNeditPath" runat="server" Text="Change" OnClientClick="showDialog('selectFolder');"
                                                        CausesValidation="false" /></span>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div style="clear: both;">
                                    <div style="float: left; width: 130px; text-align: left;">
                                        <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="Titolo_campoSmall">Visible To:</asp:Label>
                                    </div>
                                    <div style="float: left;">
                                        <asp:RadioButtonList ID="RBLvisibleTo" runat="server" CssClass="Testo_campoSmall"
                                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Selected="True" Value="True">A tutti</asp:ListItem>
                                            <asp:ListItem Value="False">Al proprietario</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <br />
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTmultipleFileError" runat="server" Title="Rename file to publish into community folder"
                            StepType="Finish">
                            <asp:Label runat="server" ID="LBmultipleFileError" Font-Bold="true" ForeColor="#00008B">Select name for</asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="height: 400px; padding: 20px, auto; margin: 0, auto; background-color: White;">
                                <asp:Repeater ID="RPTfileName" runat="server">
                                    <HeaderTemplate>
                                        <table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div>
                                                    <asp:Label ID="LBfileNameToReplace" runat="server" AssociatedControlID="TXBfileName"></asp:Label>
                                                </div>
                                                <div style="padding-left: 50px;">
                                                    <asp:Literal ID="LTfileID" runat="server" Visible="false"></asp:Literal>
                                                    <asp:Literal ID="LTimageFile" runat="server"></asp:Literal>
                                                    <asp:TextBox ID="TXBfileName" runat="server" Columns="60" MaxLength="150"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVfileName" runat="server" ControlToValidate="TXBfileName"
                                                        Text="*"></asp:RequiredFieldValidator>
                                                    <asp:Literal ID="LTfileType" runat="server"></asp:Literal>
                                                    <asp:Literal ID="LTfileOldName" runat="server" Visible="false"></asp:Literal>
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
                    </WizardSteps>
                    <StepStyle Font-Size="0.8em" ForeColor="#333333" />
                    <SideBarButtonStyle BackColor="#507CD1" ForeColor="White" />
                    <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
                        BorderWidth="1px" Font-Size="0.8em" ForeColor="#284E98" />
                    <SideBarStyle BackColor="#507CD1" Font-Size="0.9em" VerticalAlign="Top" />
                    <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                        Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
                    <StartNavigationTemplate>
                      <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Back to file management" Height="18px"></asp:HyperLink>
                        <asp:Button ID="BTNgoToManagement" runat="server" CommandName="Cancel" Text="Torna all'elenco" Visible="false"
                            CausesValidation="false" />
                        <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                    </StartNavigationTemplate>
                    <StepNavigationTemplate>
                      <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Back to file management" Height="18px"></asp:HyperLink>
                        <asp:Button ID="BTNgoToManagement" runat="server" CommandName="Cancel" Text="Torna all'elenco" Visible="false"
                            CausesValidation="false" />
                        <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" 
                            Text="Precedente" />
                        <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Avanti" />
                    </StepNavigationTemplate>
                    <FinishNavigationTemplate>
                        <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Back to file management" Height="18px"></asp:HyperLink>
                        <asp:Button ID="BTNgoToManagement" runat="server" CommandName="Cancel" Text="Torna all'elenco" CausesValidation="false" Visible="false"/>
                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" 
                            Text="Precedente" />
                        <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Fine" />
                    </FinishNavigationTemplate>
                </asp:Wizard>
            </div>
        </asp:View>
    </asp:MultiView>
    <div id="selectFolder">
        <asp:UpdatePanel ID="UDPselectFolder" UpdateMode="Conditional" ChildrenAsTriggers="true"
            runat="server">
            <ContentTemplate>
                <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="420px" SelectionMode="Single"
                    AjaxEnabled="true" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
