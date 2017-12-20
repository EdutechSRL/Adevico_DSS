<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="PathManagement.aspx.vb" Inherits="Comunita_OnLine.PathManagement"
    ValidateRequest="false" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ Register TagPrefix="CTRL" TagName="SearchComm" Src="~/UC/UC_SearchCommunityByService.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="~/UC/UC_SearchUserByCommunities.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_HelpEpRole.ascx" TagName="CTRLhelpRole" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
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
            $(".slidingDiv.open").show();

            $(".show_hide").show();

            $('.show_hide').click(function () {
                $(".slidingDiv").slideToggle();
            });

            $(".clearIt").click(function () {
                // $(".clearMe input").val("");
                var datepicker = $find("<%= RDPendDate.ClientID %>");
                datepicker.clear();

                var x = $find("<%= TXBhEnd.ClientID %>");
                x.clear();
                // x._textBoxElement.value = "";
                // x.set_value("");
                //  x.set_textBoxValue("");

                var y = $find("<%= TXBmEnd.ClientID %>");
                y.clear();
            });

            $(".rolepartecipant .selectall").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".rolepartecipant input[type='checkbox']").prop("checked", true);
            });
            $(".rolepartecipant .selectnone").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".rolepartecipant input[type='checkbox']").prop("checked", false);
            });
            $(".rolestatviewer .selectall").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".rolestatviewer input[type='checkbox']").prop("checked", true);
            });
            $(".rolestatviewer .selectnone").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".rolestatviewer input[type='checkbox']").prop("checked", false);
            });
            $(".roleevaluator .selectall").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".roleevaluator input[type='checkbox']").prop("checked", true);
            });
            $(".roleevaluator .selectnone").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".roleevaluator input[type='checkbox']").prop("checked", false);
            });
            $(".rolemanager .selectall").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".rolemanager input[type='checkbox']").prop("checked", true);
            });
            $(".rolemanager .selectnone").click(function () {
                var $table = $(this).parents("table").first();
                $table.find(".rolemanager input[type='checkbox']").prop("checked", false);
            });
            $(".collapsable .expander").click(function () {
                $(this).parents(".collapsable").first().toggleClass("collapsed");
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="Width940">    
        <asp:MultiView ID="MLVpathCreate" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWcreatePath" runat="server">
                <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
                <asp:Wizard ID="WZRpathCreate" runat="server" ActiveStepIndex="0" Width="100%" BackColor="#EFF3FB"
                    BorderColor="#B5C7DE" BorderWidth="1px" DisplaySideBar="False">
                    <WizardSteps>
                        <asp:WizardStep ID="WSTdata" runat="server" Title="*Path Detail" StepType="Step">
                            <asp:Label runat="server" ID="LBdetailTitle" Font-Bold="true" ForeColor="#00008B"
                                Text="PathDetail**"></asp:Label><br />
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
                            <div id="DIVtype" class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall">Type:*</asp:Label>
                                </div>
                                <div class="DetailRightItem">
                                    <asp:RadioButtonList ID="RBLtype" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                        <asp:ListItem runat="server" Value="0" Selected="true">e-learning</asp:ListItem>
                                        <asp:ListItem runat="server" Value="1">webinar</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div id="DIVdescriptionEdit" class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBdescriptionTitle" runat="server" CssClass="Titolo_campoSmall">Description:**</asp:Label>
                                </div>
                                <div class="DetailEditor">
                                    <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass" 
                                    LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass"  EditorHeight="360px" >
                                    </CTRL:CTRLeditor>
                                </div>
                            </div>
                            <div class="DetailItem">
                                <asp:CheckBox ID="CKBplayMode" runat="server" CssClass="Titolo_campoSmall" />
                            </div>
                            <div class="DetailItem" runat="server" id="DIVsingleAction">
                                <asp:CheckBox ID="CKBsingleAction" runat="server" CssClass="Titolo_campoSmall" />
                            </div>
                            <div class="DetailItem" runat="server" id="DIVfloatingDeadlines">
                                <asp:CheckBox ID="CKBfloatingDeadlines" runat="server" CssClass="Titolo_campoSmall" />
                            </div>
                            <div id="DIVcompletion" class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBminCompletionTitle" runat="server" CssClass="Titolo_campoSmall">% Min complet:**</asp:Label>
                                    <asp:TextBox ID="TXBminCompletion" runat="server" Width="30px" MaxLength="3" TextMode="SingleLine"></asp:TextBox>
                                    %
                                    <asp:RangeValidator ID="RNVminCompletion" runat="server" ErrorMessage="Il valore deve essere compreso tra 0 e 100"
                                        Text="" ControlToValidate="TXBminCompletion" MinimumValue="0" MaximumValue="100"
                                        Type="Integer" Display="Dynamic"></asp:RangeValidator>
                                    <asp:CompareValidator ID="COVminCompletion" runat="server" ErrorMessage="Il valore deve essere un intero"
                                        Text="" ControlToValidate="TXBminCompletion" Type="Integer" Operator="DataTypeCheck"
                                        Display="Dynamic"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RFVminCompl" runat="server" ErrorMessage="Required!"
                                        ControlToValidate="TXBminCompletion" Display="Dynamic" />
                                    <span class="minCompletion openHelp img_span ico_help_s"></span>
                                </div>
                            </div>
                            <div runat="server" id="DIVtime">
                                <div class="DetailItem ">
                                    <telerik:RadAjaxPanel runat="server" ID="AjaxPanel1">
                                        <asp:Label runat="server" ID="LBhours" Text="Ore riconosciute*" CssClass="Titolo_campoSmall"></asp:Label>
                                        <telerik:RadNumericTextBox DecimalDigits="0" Type="number" ShowSpinButtons="true"
                                            IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                            Label="" runat="server" ID="TXBhours" Width="50px" MaxValue="100" MinValue="0">
                                        </telerik:RadNumericTextBox>:<telerik:RadNumericTextBox DecimalDigits="0" Type="number"
                                            ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                            Label="" runat="server" ID="TXBmins" Width="50px" MaxValue="59" MinValue="0">
                                        </telerik:RadNumericTextBox>
                                        <span style="vertical-align: middle;" class="weightMan openHelp img_span ico_help_s">
                                            &nbsp</span>
                                    </telerik:RadAjaxPanel>
                                </div>
                                <asp:Literal runat="server" ID="LITalert"></asp:Literal>
                            </div>
                            <div class="DetailItem" runat="server" id="DIVweightAuto">
                                <asp:Label runat="server" ID="LBweightAuto" CssClass="Titolo_campoSmall"> </asp:Label>
                                <span class="weightAuto openHelp img_span ico_help_s"></span>
                            </div>
                            <div class="DetailItem" runat="server" id="DIVmark">
                                <asp:Label ID="LBminMark" runat="server" CssClass="Titolo_campoSmall">% Min mark:**</asp:Label>
                                <asp:TextBox ID="TXBminMark" runat="server" Width="30px" MaxLength="3" TextMode="SingleLine"></asp:TextBox>
                                % <span class="helpMark openHelp img_span ico_help_s"></span>
                                <asp:RangeValidator ID="RNVminMark" runat="server" ErrorMessage="Il valore deve essere compreso tra 0 e 100"
                                    Text="" ControlToValidate="TXBminMark" MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                                <asp:CompareValidator ID="COVminMark" runat="server" ErrorMessage="Il valore deve essere un intero"
                                    Text="" ControlToValidate="TXBminMark" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                                <asp:RequiredFieldValidator ID="RFVminMark" runat="server" ErrorMessage="Required!"
                                    ControlToValidate="TXBminMark" />
                            </div>
                            <div class="btnAdvanced DetailItem ">
                                <asp:Label ID="LBsetEndDate" runat="server" CssClass="show_hide Link_Menu" Text="*advanced" />
                            </div>
                            <div class="slidingDiv" runat="server" id="DIVsliding">
                                <div class="DetailItem">
                                    <div class="DetailLeftItem" style="padding: 0px;">
                                        <asp:Label runat="server" ID="LBendDate" Text="AAAAA*" CssClass="Titolo_campoSmall"></asp:Label>
                                    </div>
                                    <div class="DetailRightItem inline clearMe">
                                        <telerik:RadDatePicker ID="RDPendDate" CssClass="" runat="server" >
                                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" 
                                                ViewSelectorText="x" runat="server">
                                            </Calendar>
                                            <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" runat="server"></DateInput>
                                        </telerik:RadDatePicker>
                                    </div>
                                    <div class="DetailRightItem inline">
                                         <telerik:RadAjaxPanel runat="server" ID="Radajaxpanel2">
                                            <telerik:RadNumericTextBox DecimalDigits="0" Type="number" ShowSpinButtons="true"
                                                IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                                Label="" runat="server" ID="TXBhEnd" Width="50px" MaxValue="100" MinValue="0">
                                            </telerik:RadNumericTextBox>:<telerik:RadNumericTextBox DecimalDigits="0" Type="number"
                                                ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                                Label="" runat="server" ID="TXBmEnd" Width="50px" MaxValue="59" MinValue="0">
                                            </telerik:RadNumericTextBox>
                                        </telerik:RadAjaxPanel>

                                        <asp:Label ID="LBerrorEndDate" runat="server" CssClass="errorMSG" Visible="false" />
                                    </div>
                                    <%--<span class="icons inline">
                                    <span class="icon delete clearIt">X</span>
                                    </span>--%>
                                    <a runat="server" id="Aclearit" class="Link_Menu clearIt">**cancella</a>
                                </div>
                                <div class="DetailItem hidden">
                                    <div class="floatLeft inline">
                                        <asp:Label ID="LBendDateOverflow" runat="server" Text="*end over" CssClass="Titolo_campoSmall" />
                                    </div>
                                    <div class="floatLeft inline">
                                        <telerik:RadAjaxPanel runat="server" ID="Radajaxpanel1">
                                            <telerik:RadNumericTextBox DecimalDigits="0" Type="number" ShowSpinButtons="true"
                                                IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                                Label="" runat="server" ID="TXBhEndOver" Width="50px" MaxValue="100" MinValue="0">
                                            </telerik:RadNumericTextBox>:<telerik:RadNumericTextBox DecimalDigits="0" Type="number"
                                                ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                                Label="" runat="server" ID="TXBmEndOver" Width="50px" MaxValue="59" MinValue="0">
                                            </telerik:RadNumericTextBox>
                                        </telerik:RadAjaxPanel>
                                    </div>
                                    <div class="floatLeft">
                                        <span class="endDateOverflow openHelp inline img_link ico_help_s"></span>
                                    </div>
                                </div>
                                <div class="DetailItem">
                                    <asp:CheckBox ID="CKBcontinueExecution" runat="server" CssClass="Titolo_campoSmall "
                                        Text="*continue execution" />
                                </div>
                            </div>
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="DropDownList1">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="TXBhours" />
                                            <telerik:AjaxUpdatedControl ControlID="TXBmins" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="DropDownList3">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="TXBhEndOver" />
                                            <telerik:AjaxUpdatedControl ControlID="TXBmEndOver" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="DropDownList4">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="TXBhEnd" />
                                            <telerik:AjaxUpdatedControl ControlID="TXBmEnd" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>                   
                            </telerik:RadAjaxManager>

                             <div class="clearfix collapsable collapsed" runat="server" visible="false" id="DVadvancedSettings">
                                <div class="sectionheader clearfix">
                                    <div class="left">
                                        <h3 class="sectiontitle clearifx"><asp:Literal ID="LTadvancedSettingsTitle" runat="server"></asp:Literal><span class="extrainfo expander" id="SPNexpand" runat="server"><asp:Label ID="LBspanExpandList" runat="server" CssClass="on">*click to expand</asp:Label><asp:Label ID="LBspanCollapseList" runat="server" CssClass="off">*click to collapse</asp:Label></span></h3>
                                    </div>
                                    <div class="right hideme">
                                    </div>
                                </div>
                                <div class="hideme">
                                    <div class="fieldobject fieldoptions">
                                        <div class="fieldrow fieldlongtext">
                                            <label class="fieldlabel">  
                                                <asp:Label ID="LBcompletionPolicy_t" runat="server">*Completion settings</asp:Label>
                                            </label>
                                            <div class="inlinewrapper"> 
                                                <asp:RadioButtonList ID="RBLcompletionPolicy" CssClass="radiobuttonlist" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server"/>
                                            </div>
                                        </div>
                                        <div class="fieldrow fieldlongtext">
                                            <label class="fieldlabel">  
                                                <asp:Label ID="LBdisplayPolicy_t" runat="server">*Display subactivities</asp:Label>
                                            </label>
                                            <div class="inlinewrapper">
                                                <asp:RadioButtonList ID="RBLdisplayPolicy" CssClass="radiobuttonlist" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server"/>
                                            </div>
                                        </div>
                                        <div class="fieldrow fieldlongtext">
                                            <label class="fieldlabel">  
                                                <asp:Label ID="LBscormSettingsPolicy_t" runat="server">*SCORM subactivities</asp:Label>
                                            </label>
                                            <div class="inlinewrapper"> 
                                                <asp:RadioButtonList ID="RBLscormSettings" CssClass="radiobuttonlist" RepeatLayout="Flow" RepeatDirection="Horizontal" runat="server"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTselectPermission" runat="server" Title="*Select Permission"
                            StepType="Step">
                            <asp:Label runat="server" ID="LBpermissionTitle" Font-Bold="true" ForeColor="#00008B"
                                Text="Select Permission**"></asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="DIVButton">
                                <asp:Button ID="BTNselectPerson" runat="server" Text="**AddUser" CssClass="Link_Menu" />
                                <span class="selectPermission openHelp img_span ico_help_s"></span>
                            </div>
                            <div id="DIVcrolePermission" runat="server" class="DIVrepeater">
                                <asp:Label ID="LBcrolePermission" runat="server" Text="*Permis" CssClass="Titolo_campo"></asp:Label>
                                <span class="permission openHelp img_span ico_help_s"></span>
                                <asp:Repeater ID="RPcrolePermission" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table id="tableMap" border="1" width="100%" cellspacing="0">
                                            <tr class="ROW_header_Small_Center">
                                                <td rowspan="2">
                                                    <asp:Label ID="LBcommRoleTitle" runat="server" Text="*Ruolo"></asp:Label>
                                                </td>
                                                <td colspan="<%#Me.RoleEpCount %>">
                                                    <asp:Label ID="LBroleEp" runat="server"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>
                                            </tr>
                                            <tr class="ROW_header_Small_Center">
                                                <td class="rolepartecipant">
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
                                                </td>
                                                <td class="rolestatviewer">
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
                                                </td>
                                                <td class="roleevaluator" <%#Me.NotIsAutoTimePath %>>
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
                                                </td>
                                                <td class="rolemanager">
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBcommRole" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem rolepartecipant">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolestatviewer">
                                                <asp:CheckBox ID="CKBstatviewer" runat="server" Text="*statviewer" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem roleevaluator" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolemanager">
                                                <asp:CheckBox ID="CKBmanager" runat="server" Text="managee" CssClass="dettagli_CampoSmall" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ROW_Alternate_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBcommRole" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem rolepartecipant">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolestatviewer">
                                                <asp:CheckBox ID="CKBstatviewer" runat="server" Text="*statviewer" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem roleevaluator" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolemanager">
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
                                <asp:Repeater ID="RPuserPermission" runat="server" EnableViewState="false">
                                    <HeaderTemplate>
                                        <table id="tableMap" border="1" width="880px" cellspacing="0">
                                            <tr class="ROW_header_Small_Center">
                                                <td rowspan="2">
                                                    <asp:Label ID="LBdeleteTitle" runat="server" Text="D"></asp:Label>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:Label ID="LBpersonTitle" runat="server" Text="*Ruolo"></asp:Label>
                                                </td>
                                                <td colspan="<%#Me.RoleEpCount %>">
                                                    <asp:Label ID="LBroleEp" runat="server"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>
                                            </tr>
                                            <tr class="ROW_header_Small_Center">
                                                <td class="rolepartecipant">
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
                                                </td>
                                                <td class="rolestatviewer">
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
                                                </td>
                                                <td class="roleevaluator" <%#Me.NotIsAutoTimePath %>>
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
                                                </td>
                                                <td class="rolemanager">
                                                    <span class="icons">
														<span class="icon selectall" title="">&nbsp;</span><span class="icon selectnone" title="">&nbsp;</span>
													</span>
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
                                            <td class="TableItem rolepartecipant">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolestatviewer">
                                                <asp:CheckBox ID="CKBstatviewer" runat="server" Text="*statviewer" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem roleevaluator" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolemanager">
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
                                            <td class="TableItem rolepartecipant">
                                                <asp:CheckBox ID="CKBpartecipant" runat="server" Text="*partecipant" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolestatviewer">
                                                <asp:CheckBox ID="CKBstatviewer" runat="server" Text="*statviewer" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem roleevaluator" <%#Me.NotIsAutoTimePath %>>
                                                <asp:CheckBox ID="CKBevaluator" runat="server" Text="*evaluator" CssClass="dettagli_CampoSmall" />
                                            </td>
                                            <td class="TableItem rolemanager">
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
                            <asp:Label runat="server" ID="LBuserTitle" Font-Bold="true" ForeColor="#00008B" Text="Path User**"></asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div style="padding: 5px;">
                                <CTRL:USERlist ID="CTRLselectUser" runat="server"></CTRL:USERlist>
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTsummary" runat="server" Title="*Summary" StepType="Step">
                            <asp:Label runat="server" ID="LBsummaryTitle" Font-Bold="true" ForeColor="#00008B"
                                Text="Summary**"></asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBnameTitleRes" runat="server" Text="titleww res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBnameRes" runat="server" Text="titleww res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBdescriptionTitleRes" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label>
                                </div>
                                <div class="DetailRightItem">
                                    <div class="renderedtext inlinewrapper">
                                        <asp:Label ID="LBdescriptionRes" runat="server" Text="title res"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBminCompletionTitleRes" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBminCompletionRes" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem" id="DIVminMark" runat="server">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBminMarkTitleRes" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBminMarkRes" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBstatusTitleRes" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBstatusRes" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                                <%--  <div class="DetailMulti">
                                <asp:Label ID="LBmandatoryTitleRes" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                            <div class="DetailMulti">
                                <asp:Image ID="IMGmandatory" runat="server" /></div>--%>
                            </div>
                            <div class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBplayModeTitle" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBplayMode" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBsingleActionTitle" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBsingleAction" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem" style="display:none;">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBfloatingDeadlinesTitle" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBfloatingDeadlines" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem" style="display:none;">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBcompletionPolicyTitle" runat="server" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBcompletionPolicy" runat="server"  CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem" style="display:none;">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBdisplayPolicyTitle" runat="server" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBdisplayPolicy" runat="server"  CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div class="DetailItem">
                                <div class="DetailLeftItem">
                                    <asp:Label ID="LBscormSettingsPolicyTitle" runat="server" CssClass="Titolo_campoSmall"></asp:Label></div>
                                <div class="DetailRightItem">
                                    <asp:Label ID="LBscormSettingsPolicy" runat="server"  CssClass="dettagli_CampoSmall"></asp:Label></div>
                            </div>
                            <div id="DIVendTimeRes" runat="server">
                                <div class="DetailItem">
                                    <div class="DetailLeftItem">
                                        <asp:Label ID="LBendDateTitle" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                    <div class="DetailRightItem">
                                        <asp:Label ID="LBendDateRes" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                                </div>
                                <div class="DetailItem  hidden" id="DIVendDateOver" runat="server">
                                    <div class="DetailLeftItem">
                                        <asp:Label ID="LBendDateOverflowTitle" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                    <div class="DetailRightItem">
                                        <asp:Label ID="LBendDateOverflowRes" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                                </div>
                                <div class="DetailItem">
                                    <div class="DetailLeftItem">
                                        <asp:Label ID="LBcontinueExecutionTitle" runat="server" Text="title res" CssClass="Titolo_campoSmall"></asp:Label></div>
                                    <div class="DetailRightItem">
                                        <asp:Label ID="LBcontinueExecution" runat="server" Text="title res" CssClass="dettagli_CampoSmall"></asp:Label></div>
                                </div>
                            </div>
                            <div class="DIVrepeater">
                                <asp:Label ID="LBcroleSummary" runat="server" Text="*resu" CssClass="Titolo_campo"></asp:Label>
                                <asp:Repeater ID="RPcroleSummary" runat="server" EnableViewState="false" OnItemDataBound="RPsummary_ItemDataBound">
                                    <HeaderTemplate>
                                        <table id="tableMap" border="1" width="100%" cellspacing="0">
                                            <tr class="ROW_header_Small_Center">
                                                <td>
                                                    <asp:Label ID="LBnameCroleTitle" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBpartecipantTitle" runat="server" Text="*PArtecipante"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>

                                                <td>
                                                    <asp:Label ID="LBstatviewerTitle" runat="server" Text="*StatViewer"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>

                                                <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                    <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBname" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGpartecipant" runat="server" />&nbsp
                                            </td>

                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGstatviewer" runat="server" />&nbsp
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
                                                &nbsp<asp:Image ID="IMGpartecipant" runat="server" />&nbsp
                                            </td>

                                            <td class="TableItem">
                                                &nbsp<asp:Image ID="IMGstatviewer" runat="server" />&nbsp
                                            </td>

                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                &nbsp<asp:Image ID="IMGevaluator" runat="server" />
                                                &nbsp
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
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>

                                                <td>
                                                    <asp:Label ID="LBstatviewerTitle" runat="server" Text="*Statviewer"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>

                                                <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                    <asp:Label ID="LBevaluatorTitle" runat="server" Text="*"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LBmanagerTitle" runat="server"></asp:Label>
                                                    <span class="helpRole img_span ico_help_s"></span>
                                                </td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBname" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGpartecipant" runat="server" />
                                                &nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGstatviewer" runat="server" />
                                                &nbsp
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                &nbsp
                                                <asp:Image ID="IMGevaluator" runat="server" />
                                                &nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGmanager" runat="server" />
                                                &nbsp
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ROW_Alternate_Small">
                                            <td class="TableItem">
                                                <asp:Label ID="LBname" runat="server" Text="*Ruolo" CssClass="Titolo_campoSmall"></asp:Label>
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGpartecipant" runat="server" />
                                                &nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGstatviewer" runat="server" />
                                                &nbsp
                                            </td>
                                            <td class="TableItem" <%#Me.NotIsAutoTimePath %>>
                                                &nbsp
                                                <asp:Image ID="IMGevaluator" runat="server" />
                                                &nbsp
                                            </td>
                                            <td class="TableItem">
                                                &nbsp
                                                <asp:Image ID="IMGmanager" runat="server" />
                                                &nbsp
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WSTselectCommunity" runat="server" Title="*community " StepType="Step">
                            <asp:Label runat="server" ID="LBselectCommunityTitle" Font-Bold="true" ForeColor="#00008B"
                                Text="PathDetail**"></asp:Label><br />
                            <hr style="color: #00008B;" />
                            <div class="DetailItem">
                                <CTRL:SearchComm ID="CTRLcommunity" runat="server" SelectionMode="single" AllowMultipleOrganizationSelection="false"
                                    AllowCommunityChangedEvent="true" />
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
                <CTRL:Messages runat="server" ID="CTRLmessages" />
            </asp:View>
        </asp:MultiView>
    </div>
    <CTRL:CTRLhelpRole ID="CTRLhelpRole" runat="server" />
    <div id="helpDialog1" class="dialog">
        <asp:Label ID="LbMincompletionHelp" runat="server" Text="*A" CssClass="minCompletion hideMe hidden"></asp:Label>
        <asp:Label ID="LbWeightAutoHelp" runat="server" Text="*b" CssClass="weightAuto hideMe hidden"></asp:Label>
        <asp:Label ID="LbWeightManHelp" runat="server" Text="*c" CssClass="weightMan hideMe hidden"></asp:Label>
        <asp:Label ID="LBendDateOverflowHelp" runat="server" Text="*c" CssClass="endDateOverflow hideMe hidden"></asp:Label>
        <asp:Label ID="LBpermissionHelp" runat="server" Text="*c" CssClass="permission hideMe hidden"></asp:Label>
        <asp:Label ID="LBselectPermHelp" runat="server" Text="*c" CssClass="selectPermission hideMe hidden"></asp:Label>
        <asp:Label ID="LBmarkHelpPath" runat="server" CssClass="helpMark hideMe hidden" />
    </div><asp:Literal ID="LToptionCssClass" runat="server" visible="false">inlinewrapper</asp:Literal><asp:Literal ID="LToptionTemplate" runat="server" visible="false">#label#<div class="description">#description#</div></asp:Literal>
</asp:Content>
