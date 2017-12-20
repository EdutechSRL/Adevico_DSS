<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ManageEvaluators.aspx.vb" Inherits="Comunita_OnLine.ManageEvaluators" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/Evaluate/UC/UC_WizardEvaluationCommitteesSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(".view-modal.view-users").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 890,
                height: 450,
                minHeight: 300,
                minWidth: 700,
                title: '<%=DialogTitleTranslation() %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
         });
    </script>
    <CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
   <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnocalls" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNsaveEvaluatorsTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start EVALUATORS -->
                                <div class="treetop clearfix">
                                    <div class="DivEpButton clearfix">
                                        <asp:Button ID="BTNaddCommitteeMemberTop" runat="server" text="Add evaluator"/>
                                    </div>
                                </div>
                                <div class="fieldobject">
                                    <div class="fieldrow fieldextratime">
                                        <asp:CheckBox ID="CBXallowMultipleCommittees" runat="server" AutoPostBack="true"  CssClass="inputtext"/>
                                        <asp:Label ID="LBallowMultipleCommittees" runat="server" CssClass="inline" AssociatedControlID="">Consenti ad un valutatore di appartenere a pi&ugrave; commissioni.</asp:Label>
			                        </div>
			                    </div>
                                <asp:Repeater ID="RPTassignments" runat="server">
                                    <HeaderTemplate>
                                        <table class="evaluators <%#GetTableCss() %> light">
                                            <thead>
                                                <th class="evaluator"><asp:literal ID="LTevaluator_t" runat="server">Evaluator</asp:literal></th>
                                                <th class="commission" runat="server" id="THcommittee"><asp:literal ID="LTcommittee_t" runat="server">Commission</asp:literal></th>
                                                <th class="actions"><asp:literal ID="LTactions_t" runat="server">Actions</asp:literal></th>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                            <tbody>
                                                <td class="evaluator">
                                                    <a name="evaluator_<%#Container.DataItem.IdCallEvaluator %>"></a>
                                                    <span class="fullname"><%#Container.DataItem.DisplayName%></span>
                                                    <asp:Literal ID="LTidCallEvaluator" runat="server" Visible="false" Text='<%#Container.DataItem.IdCallEvaluator %>'></asp:Literal>
                                                    <asp:Literal ID="LTidPerson" runat="server" Visible="false" Text='<%#Container.DataItem.IdPerson %>'></asp:Literal>
                                                </td>
                                                <td class="commission" runat="server" id="TDcommittee">
                                                    <asp:MultiView ID="MLVselectCommittee" runat="server">
                                                         <asp:View ID="VIWnone" runat="server">
                                                         </asp:View>
                                                        <asp:View ID="VIWmultiple" runat="server">
                                                            <div class="choseselect clearfix">
                                                                <div class="left">
                                                                    <select runat="server" id="SLBcommittees" class="partecipants chzn-select" multiple tabindex="2">            
                                                                    </select>
                                                                </div>
                                                                <div class="right">
											                        <span class="icons">
												                        <span class="icon selectall" title="All" runat="server" id="SPNevaluatorsCommitteeSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNevaluatorsCommitteeSelectNone">&nbsp;</span>
											                        </span>
                                                                </div>
                                                            </div>
                                                        </asp:View>
                                                        <asp:View ID="VIWsingle" runat="server">
                                                            <div class="choseselect clearfix">
                                                                <div class="left">
                                                                    <asp:DropDownList ID="DDLcommittees" runat="server"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </td>
                                                <td class="actions">
                                                    <span class="icons">
                                                        <asp:Button ID="BTNdeleteMember" runat="server" Text="D" CssClass="icon delete needconfirm" CommandName="virtualDelete"/>
                                                    </span>
                                                </td>
                                            </tbody>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:button ID="BTNsaveEvaluatorsBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="view-modal view-users" id="DVselectUsers" runat="server" visible="false">
                 <CTRL:SelectUsers ID="CTRLselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
                  DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="false" DefaultMaxPreviewItems="20" 
                  ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false"/>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>