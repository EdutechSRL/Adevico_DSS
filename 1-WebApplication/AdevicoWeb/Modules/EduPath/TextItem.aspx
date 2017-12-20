<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" 
    CodeBehind="TextItem.aspx.vb" Inherits="Comunita_OnLine.TextItem" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="~/UC/UC_SearchUserByCommunities.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/PfStyle.css" rel="Stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
<div class="Width940">
    <asp:MultiView ID="MLVtextItem" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWitemText" runat="server">
            <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
            <asp:Wizard ID="WZRnoteCreate" runat="server" ActiveStepIndex="0" Width="100%" BackColor="#EFF3FB"
                BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="False">
                <WizardSteps>
                 <asp:WizardStep ID="WSTdata" runat="server" Title="*Unit Detail" StepType="Step">
            <asp:Label runat="server" ID="LBdetailTitle" Font-Bold="true" ForeColor="#00008B"
                            Text="Select Permission**"></asp:Label><br />
                        <hr style="color: #00008B;" />
            <div class="DetailItem">
                <div class="DetailLeftItem">
                    <asp:Label ID="LBdescriptionTitle" runat="server" CssClass="Titolo_campoSmall">Description:**</asp:Label>
                </div>
                <div class="DetailEditor">
                    <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass" 
                        LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass"  EditorHeight="360px" >
                    </CTRL:CTRLeditor>
                    <asp:Label ID="LBerrorEditor" runat="server" CssClass="erroreSmall" Visible="false" />
                </div>
            </div>
           <%-- <br />
            <div class="DIVrepeater">
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
                                            </td>
                                            <td  "<%#Me.NotIsAutoTimePath %>">
                                                <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
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
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                        </div><br />--%>
                        <div class="DIVrepeater">
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
                                            </td>
                                            <td  "<%#Me.NotIsAutoTimePath %>">
                                                <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="ROW_Normal_Small">
                                        <td class="TableItem">
                                            &nbsp<asp:Label ID="LBname" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                        </td>
                                        <td class="TableItem">
                                           &nbsp <asp:Image ID="IMGpartecipant" runat="server" />&nbsp
                                        </td>
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                        </div>
                        <div id="DIVcrolePermission" runat="server" class="DIVrepeater">
                            <asp:Label ID="LBcrolePermission" runat="server" Text="*Permis" CssClass="Titolo_campo"></asp:Label>
                            <asp:Label ID="LBcrolePermissionNoAss" runat="server" Visible="false"></asp:Label>
                            <asp:Repeater ID="RPcrolePermission" runat="server" EnableViewState="true">
                                <HeaderTemplate>
                                    <table id="tableMap" border="1" width="880px" cellspacing="0">
                                        <tr class="ROW_header_Small_Center">
                                            <td>
                                                <asp:Label ID="LBcommRoleTitle" runat="server" Text="*Ruolo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBpartecipantTitle" runat="server" Text="*PArtecipante"></asp:Label>
                                            </td>
                                            <td  "<%#Me.NotIsAutoTimePath %>">
                                                <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="ROW_Normal_Small">
                                        <td class="TableItem">
                                            <asp:Label ID="LBcommRole" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                        </td>
                                        <td class="TableItem">
                                            <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" Enabled="false"/>
                                        </td>
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                        <div id="DIVpersonPermission" runat="server" class="DIVrepeater" >
                            <asp:Label ID="LBpersonPermission" runat="server" Text="*Permis" CssClass="Titolo_campo"></asp:Label>
                            <asp:Label ID="LBpersonPermissionNoAss" runat="server" Visible="false"></asp:Label>
                            <asp:Repeater ID="RPuserPermission" runat="server" EnableViewState="true">
                                <HeaderTemplate>
                                    <table id="tableMap" border="1" width="880px" cellspacing="0">
                                        <tr class="ROW_header_Small_Center">
                                            <td>
                                                <asp:Label ID="LBdeleteTitle" runat="server" Text="D" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBpersonTitle" runat="server" Text="*Ruolo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBpartecipantTitle" runat="server" Text="*PArtecipante"></asp:Label>
                                            </td>
                                            <td  "<%#Me.NotIsAutoTimePath %>">
                                                <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
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
                                            <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" Enabled="false"  />
                                        </td>
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                                        <td class="TableItem"  "<%#Me.NotIsAutoTimePath %>">
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
                   <%-- <asp:Button ID="BTNedit" runat="server" CausesValidation="false" Text="**Edit" CssClass="Link_Menu"
                        OnClick="BTNedit_Click" />--%>
                    <asp:Button ID="BTNnext" runat="server" CausesValidation="true" Text="**NextProva"
                        CssClass="Link_Menu" OnClick="BTNnext_Click" />
                </StepNavigationTemplate>
            </asp:Wizard>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
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
</asp:Content>
