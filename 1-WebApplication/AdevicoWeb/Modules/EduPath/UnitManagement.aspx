<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="UnitManagement.aspx.vb" Inherits="Comunita_OnLine.UnitManagement" %>

<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="~/UC/UC_SearchUserByCommunities.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ Register Src="./UC/UC_HelpEpRole.ascx" TagName="CTRLhelpRole" TagPrefix="CTRL" %>
<%@ Register Src="./UC/UC_HelpVisibility.ascx" TagName="CTRLhelpVisibility" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/PfStyle.css" rel="Stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#helpDialog1").dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: false,
                title: "",
                width: 150,
                height: 400,
                minHeight: 150,
                minWidth: 400,
                zIndex: 99999,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });

            $(".openHelp").live("click", function () {
                var cssClasses = $(this).attr("class");
                var cssClass = cssClasses.split(" ")[0];
                $("#helpDialog1").dialog("open");
                $("#helpDialog1").find(".hideMe").addClass("hidden");
                $("#helpDialog1").find("." + cssClass).removeClass("hidden");
            });

            $(".slidingDiv").hide();
            $(".show_hide").show();

            $('.show_hide').click(function () {
                $(".slidingDiv").slideToggle();
            });

        });


 

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="Width940">
        <asp:MultiView ID="MLVunitCreate" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWcreateUnit" runat="server">
                <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
                <asp:Wizard ID="WZRunitCreate" runat="server" ActiveStepIndex="0" Width="100%" BackColor="#EFF3FB"
                    BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="False">
                    <WizardSteps>
                        <asp:WizardStep ID="WSTdata" runat="server" Title="*Unit Detail" StepType="Step">
                            <asp:Label runat="server" ID="LBdetailTitle" Font-Bold="true" ForeColor="#00008B"
                                Text="unitDetail**"></asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div id="DIV" class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBnameTitle" runat="server" CssClass="Titolo_campoSmall">Title:**</asp:Label>
                                </div>
                                <div class="DetailRightItem">
                                    <asp:TextBox ID="TXBname" runat="server" MaxLength="250" Width="70%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVname" runat="server" ControlToValidate="TXBname"
                                        Display="Static" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div id="DIVdescriptionEdit" class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBdescriptionTitle" runat="server" CssClass="Titolo_campoSmall">Description:**</asp:Label>
                                </div>
                                <div class="DetailEditor">
                                    <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass" 
                                    LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" >
                                    </CTRL:CTRLeditor>
<%--
                                    <CTRL:CTRLvisualEditor ID="CTRLeditorDescription" runat="server" FontNames="Verdana"
                                        FontSizes="2,3,4" ToolsFile="~/RadControls/Editor/Localization/it-IT/ToolsAdvancedEvents.xml"
                                        ShowScrollingSpeed="false" ShowAddDocument="false" ShowAddImage="true" ShowAddSmartTag="true"
                                        EditorEnabled="true" AllowPreview="false"  />--%>
                                </div>
                            </div>
                            <div id="DIVcompletion" class="DetailItem" runat="server">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBminCompletionTitle" runat="server" CssClass="Titolo_campoSmall">% Min complet:**</asp:Label>
                                </div>
                                <div class="DetailRightItem">
                                    <asp:TextBox ID="TXBminCompletion" runat="server" Width="30px" MaxLength="3" TextMode="SingleLine"></asp:TextBox>
                                    % <span class="minCompletion openHelp img_link ico_help_s"></span>
                                    <asp:RangeValidator ID="RNVminCompletion" runat="server" ErrorMessage="Il valore deve essere compreso tra 0 e 100"
                                        Text="" ControlToValidate="TXBminCompletion" MinimumValue="0" MaximumValue="100"
                                        Type="Integer"></asp:RangeValidator>
                                    <asp:CompareValidator ID="COVminCompletion" runat="server" ErrorMessage="Il valore deve essere un intero"
                                        Text="" ControlToValidate="TXBminCompletion" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RFVminCompl" runat="server" ErrorMessage="Required!"
                                        ControlToValidate="TXBminCompletion" />
                                </div>
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBminMark" runat="server" CssClass="Titolo_campoSmall">% Min mark:**</asp:Label>
                                </div>
                                <div class="DetailRightItem">
                                    <asp:TextBox ID="TXBminMark" runat="server" Width="30px" MaxLength="3" TextMode="SingleLine"></asp:TextBox>
                                    % <span class="helpMark openHelp img_link ico_help_s"></span>
                                    <asp:RangeValidator ID="RNVminMark" runat="server" ErrorMessage="Il valore deve essere compreso tra 0 e 100"
                                        Text="" ControlToValidate="TXBminMark" MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                                    <asp:CompareValidator ID="COVminMark" runat="server" ErrorMessage="Il valore deve essere un intero"
                                        Text="" ControlToValidate="TXBminMark" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RFVminMark" runat="server" ErrorMessage="Required!"
                                        ControlToValidate="TXBminMark" />
                                </div>
                                <br />
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBweight" runat="server" CssClass="Titolo_campoSmall">Weight:**</asp:Label>
                                </div>
                                <div class="DetailRightItem">
                                    <asp:TextBox ID="TXBweight" runat="server" Width="30px" MaxLength="3" TextMode="SingleLine"></asp:TextBox>
                                    <asp:Label ID="LBpoints" runat="server">*points</asp:Label>
                                    <span class="helpWeight openHelp img_link ico_help_s"></span>
                                    <asp:CompareValidator ID="COVweight" runat="server" ErrorMessage="Il valore deve essere un intero"
                                        Text="" ControlToValidate="TXBweight" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RFVweight" runat="server" ErrorMessage="Required!"
                                        ControlToValidate="TXBweight" />
                                </div>
                            </div>
                            <div id="DIVvisibility" class="DetailItem">
                                <asp:CheckBox ID="CKBvisibilityUnit" runat="server" CssClass="Titolo_campoSmall" />
                                <span class="openVisibility img_link ico_help_s"></span>
                            </div>
                            <div class="DetailItem" id="DIVmandatory" runat="server">
                                <asp:CheckBox ID="CKBmandatoryUnit" runat="server" CssClass="Titolo_campoSmall" />
                            </div>
                            <div class="btnAdvanced DetailItem">
                                <asp:Label ID="LBadvanced" runat="server" CssClass="show_hide Link_Menu" Text="*advanced" />
                            </div>
                            <div class="DIVrepeater slidingDiv">
                                <asp:Label ID="LBcroleSummary" runat="server" Text="*resu" CssClass="Titolo_campo"></asp:Label>
                                <asp:Repeater ID="RPcRoleSummary" runat="server" EnableViewState="false" OnItemDataBound="RPsummary_ItemDataBound">
                                    <HeaderTemplate>
                                        <table id="tableMap" border="1" width="100%" cellspacing="0">
                                            <tr class="ROW_header_Small_Center">
                                                <td>
                                                    <asp:Label ID="LBnameCroleTitle" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBpartecipantTitle" runat="server" Text="*PArtecipante"></asp:Label>
                                                    <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                                <td <%#Me.NotIsAutoTimePath %>>
                                                    <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                                    <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
                                                    <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBname" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGpartecipant" runat="server" />
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                &nbsp<asp:Image ID="IMGevaluator" runat="server" />&nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGmanager" runat="server" />&nbsp
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ROW_Alternate_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBname" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGpartecipant" runat="server" />
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                &nbsp<asp:Image ID="IMGevaluator" runat="server" />&nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGmanager" runat="server" />&nbsp
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="DIVrepeater slidingDiv">
                                <asp:Label ID="LBpersonSummary" runat="server" Text="*resum" CssClass="Titolo_campo"></asp:Label>
                                <asp:Repeater ID="RPpersonSummary" runat="server" EnableViewState="false" OnItemDataBound="RPsummary_ItemDataBound">
                                    <HeaderTemplate>
                                        <table id="tableMap" border="1" cellspacing="0" width="100%">
                                            <tr class="ROW_header_Small_Center">
                                                <td>
                                                    <asp:Label ID="LBnamePersonTitle" runat="server" Text="*Ruolo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBpartecipantTitle" runat="server" Text="*PArtecipante"></asp:Label>
                                                     <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                                <td <%#Me.NotIsAutoTimePath %>>
                                                    <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                                     <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
                                                     <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="TableItem">
                                                &nbsp<asp:Label ID="LBname" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGpartecipant" runat="server" />&nbsp
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                &nbsp
                                                <asp:Image ID="IMGevaluator" runat="server" />&nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGmanager" runat="server" />&nbsp
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ROW_Alternate_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBname" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGpartecipant" runat="server" />&nbsp
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                &nbsp
                                                <asp:Image ID="IMGevaluator" runat="server" />&nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGmanager" runat="server" />&nbsp
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTselectPermission" runat="server" Title="*Select Permission"
                            StepType="Step">
                            <asp:Label runat="server" ID="LBpermissionTitle" Font-Bold="true" ForeColor="#00008B"
                                Text="Select Permission**"></asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="text-align: right; padding: 0px 10px 0px  10px">
                                <asp:Button ID="BTNselectPerson" runat="server" Text="**AddUser" CssClass="Link_Menu" />
                                <span class="selectPermission openHelp img_link ico_help_s" ></span>
                            </div>
                            <div id="DIVcrolePermission" runat="server" class="DIVrepeater">
                                <asp:Label ID="LBcrolePermission" runat="server" CssClass="Titolo_campo"></asp:Label>
                                <span class="permission openHelp img_link ico_help_s" ></span>
                                <asp:Label ID="LBcrolePermissionNoAss" runat="server" Visible="false"></asp:Label>
                                <asp:Repeater ID="RPcrolePermission" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table id="tableMap" border="1" width="880px" cellspacing="0">
                                            <tr class="ROW_header_Small_Center">
                                                <td>
                                                    <asp:Label ID="LBcommRoleTitle" runat="server" Text="*Ruolo"></asp:Label>
                                                </td>
                                                <td colspan="<%#Me.RoleEpCount %>">
                                                    <asp:Label ID="LBroleEp" runat="server"></asp:Label>
                                                     <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                            </tr>
                                        >
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBcommRole" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" Enabled="false" />
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBmanager" runat="server" Text="managee" CssClass="dettagli_CampoSmall" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ROW_Alternate_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBcommRole" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" Enabled="false" />
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBmanager" runat="server" Text="managee" CssClass="dettagli_CampoSmall" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <br />
                            <div id="DIVpersonPermission" runat="server" class="DIVrepeater">
                                <asp:Label ID="LBpersonPermission" runat="server" Text="*Permis" CssClass="Titolo_campo"></asp:Label>
                                <asp:Label ID="LBpersonPermissionNoAss" runat="server" Visible="false"></asp:Label>
                                <asp:Repeater ID="RPuserPermission" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table id="tableMap" border="1" width="880px" cellspacing="0">
                                            <tr class="ROW_header_Small_Center">
                                                <td>
                                                    <asp:Label ID="LBdeleteTitle" runat="server" Text="D"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBpersonTitle" runat="server" Text="*Ruolo"></asp:Label>
                                                </td>
                                                <td colspan="<%#Me.RoleEpCount %>">
                                                    <asp:Label ID="LBroleEp" runat="server"></asp:Label>
                                                     <span class="helpRole img_link ico_help_s"></span>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBactivePerson" runat="server" Text="*Select" />
                                            </td>
                                            <td class="TableItem">
                                                <asp:Label ID="LBperson" runat="server" Text="*Nomeutente" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" Enabled="false" />
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBmanager" runat="server" Text="managee" CssClass="dettagli_CampoSmall" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ROW_Alternate_Small">
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBactivePerson" runat="server" Text="*Select" />
                                            </td>
                                            <td class="TableItem">
                                                <asp:Label ID="LBperson" runat="server" Text="*Nomeutente" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" Enabled="false" />
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem">
                                                <asp:CheckBox ID="CKBmanager" runat="server" Text="managee" CssClass="dettagli_CampoSmall" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTselectUser" runat="server" Title="*Select Person" StepType="Step">
                            <asp:Label runat="server" ID="LBuserTitle" Font-Bold="true" ForeColor="#00008B" Text="unit User**"></asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="padding: 5px;">
                                <CTRL:USERlist ID="CTRLselectUser" runat="server"></CTRL:USERlist>
                            </div>
                        </asp:WizardStep>
                    </WizardSteps>
                    <StepStyle Font-Size="0.8em" ForeColor="#333333" />
                    <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                        Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
                    <StepNavigationTemplate>
                        <asp:Button ID="BTNcancel" runat="server" CausesValidation="false" Text="**Return"
                            CssClass="Link_Menu" OnClick="BTNcancel_Click" />
                        <asp:Button ID="BTNprevious" runat="server" CausesValidation="False" Text="Previus*"
                            CssClass="Link_Menu" OnClick="BTNprevious_Click" />
                        <asp:Button ID="BTNedit" runat="server" CausesValidation="false" Text="**Edit" CssClass="Link_Menu"
                            OnClick="BTNedit_Click" />
                        <asp:Button ID="BTNnext" runat="server" CausesValidation="true" Text="**NextProva"
                            CssClass="Link_Menu" OnClick="BTNnext_Click" />
                    </StepNavigationTemplate>
                </asp:Wizard>
            </asp:View>
            <asp:View ID="VIWerror" runat="server">
                <div id="DVerror" align="center">
                    <div align="right" style="text-align: right; clear: right;">
                        <asp:Button ID="BTNerror" runat="server" CssClass="Link_Menu" />
                    </div>
                    <div align="center">
                        <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWmessages" runat="server">
                <CTRL:Messages runat="server" ID="CTRLmessages"/>
            </asp:View>
        </asp:MultiView>
    </div>
    <CTRL:CTRLhelpRole ID="CTRLhelpRole" runat="server" />
    <CTRL:CTRLhelpVisibility ID="CTRLhelpVisibility" runat="server" />
    <div id="helpDialog1" class="dialog">
        <asp:Label ID="LbMincompletionHelp" runat="server" Text="*A" CssClass="minCompletion hideMe hidden"></asp:Label>
        <asp:Label ID="LBpermissionHelpUnit" runat="server" Text="*c" CssClass="permission hideMe hidden"></asp:Label>
        <asp:Label ID="LBselectPermHelpUnit" runat="server" Text="*c" CssClass="selectPermission hideMe hidden"></asp:Label>
        <asp:Label ID="LBmarkHelpUnit" runat="server" CssClass="helpMark hideMe hidden" />
        <asp:Label ID="LBweightHelpUnit" runat="server" CssClass="helpWeight hideMe hidden" />
    </div>
</asp:Content>
