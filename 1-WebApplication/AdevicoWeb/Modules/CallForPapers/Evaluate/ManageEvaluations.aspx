<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ManageEvaluations.aspx.vb" Inherits="Comunita_OnLine.ManageEvaluations" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/Evaluate/UC/UC_WizardEvaluationCommitteesSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"  EnableTreeTableScript="true" />
    <!-- OPEN SCRIPT SELECT BY EVALUATORS -->
    <script type="text/javascript" language="javascript">
        $(function () {
            $(".table.managesubmission.byevaluator").treeTable({ clickableNodeNames: true, treeColumn:1, initialState: "expanded" });

            $(".table.managesubmission.byevaluator th input[type='checkbox']").change(function () {
                var $this = $(this);
                $(this).parents("table").first().find("td input[type='checkbox']").prop("checked", $this.is(":checked"));
            });

            function checkbyevaluator($table) {
                $table.find("tr.commission").each(function () {
                    var $com = $(this);
                    var id = $com.attr("id");
                    var nt = $table.find("tr.child-of-" + id + " input[type='checkbox']").size();
                    var nc = $table.find("tr.child-of-" + id + " input[type='checkbox']:checked").size();
                    $com.find("input[type='checkbox']").prop("checked", nt == nc);
                });

                $table.find("tr.submission").each(function () {
                    var $com = $(this);
                    var id = $com.attr("id");
                    var nt = $table.find("tr.child-of-" + id + " input[type='checkbox']").size();
                    var nc = $table.find("tr.child-of-" + id + " input[type='checkbox']:checked").size();
                    $com.find("input[type='checkbox']").prop("checked", nt == nc);
                });

                var nt = $table.find("tbody input[type='checkbox']").size();
                var nc = $table.find("tbody input[type='checkbox']:checked").size();

                $table.find("thead input[type='checkbox']").prop("checked", nt == nc);
            }

            $(".table.managesubmission.byevaluator tr td input[type='checkbox']").change(function () {
                var $this = $(this);
                var $tr = $(this).parents("tr").first();
                var $table = $(this).parents("table");
                var id = $tr.attr("id");
                $("tr[id^='" + id + "-'] td input[type='checkbox']").prop("checked", $this.is(":checked"));
                checkbyevaluator($table);
            });

            $(".byevaluator .view-commissions").click(function () {
                $(".table.managesubmission.byevaluator tr").each(function () { $(this).collapse(); }); //.treeTable("collapseAll");
            });

            $(".byevaluator .view-evaluators").click(function () {
                $(".table.managesubmission.byevaluator tr").each(function () { $(this).collapse(); });
                $("tr.evaluator").each(function () {
                    $(this).reveal();
                });
            });
            checkbyevaluator($("table.table.managesubmission.byevaluator"));
        })
    </script>
    <!-- CLOSE SCRIPT SELECT BY EVALUATORS -->
    <!-- OPEN SCRIPT SELECT BY EVALUATIONS -->
    <script type="text/javascript" language="javascript">
        $(function () {
            $(".table.managesubmission.bysubmission").treeTable({ clickableNodeNames: true, treeColumn: 1, initialState: "expanded" });

            $(".table.managesubmission.bysubmission th input[type='checkbox']").change(function () {
                var $this = $(this);
                $(this).parents("table").first().find("td input[type='checkbox']").prop("checked", $this.is(":checked"));

                var $el;
                var $elout;

                var ultrafast = 1;
                var fast = 200;
                var slow = 3000;

                if ($this.is(":checked")) {
                    $el = $this.siblings(".selectorpopup.checkall");
                    $elout = $this.siblings(".selectorpopup.checknone");
                } else {
                    $el = $this.siblings(".selectorpopup.checknone");
                    $elout = $this.siblings(".selectorpopup.checkall");
                }

                $el.fadeIn(fast).addClass("open");
                var ovt = setTimeout(function () { $el.fadeOut(fast, function () { $el.removeClass("open"); }); clearTimeout(ovt); }, slow);
                $elout.fadeOut(ultrafast, function () { $elout.removeClass("open"); });
            });

            function checkbysubmission($table) {
                $table.find("tr.commission").each(function () {
                    var $com = $(this);
                    var id = $com.attr("id");
                    var nt = $table.find("tr.child-of-" + id + " input[type='checkbox']").size();
                    var nc = $table.find("tr.child-of-" + id + " input[type='checkbox']:checked").size();
                    $com.find("input[type='checkbox']").prop("checked", nt == nc);
                });

                $table.find("tr.submission").each(function () {
                    var $com = $(this);
                    var id = $com.attr("id");
                    var nt = $table.find("tr.child-of-" + id + " input[type='checkbox']").size();
                    var nc = $table.find("tr.child-of-" + id + " input[type='checkbox']:checked").size();
                    $com.find("input[type='checkbox']").prop("checked", nt == nc);
                });

                var nt = $table.find("tbody input[type='checkbox']").size();
                var nc = $table.find("tbody input[type='checkbox']:checked").size();

                $table.find("thead input[type='checkbox']").prop("checked", nt == nc);
            }

            $(".table.managesubmission.bysubmission tr td input[type='checkbox']").change(function () {
                var $this = $(this);
                var $tr = $(this).parents("tr").first();
                var $table = $(this).parents("table");
                var id = $tr.attr("id");
                $("tr[id^='" + id + "-'] td input[type='checkbox']").prop("checked", $this.is(":checked"));
                checkbysubmission($table);
            });


            $(".bysubmission .view-submissions").click(function () {
                $(".table.managesubmission.bysubmission tr").each(function () { $(this).collapse(); }); //.treeTable("collapseAll");
            });

            $(".bysubmission .view-commissions").click(function () {
                $(".table.managesubmission.bysubmission tr").each(function () { $(this).collapse(); }); //.treeTable("collapseAll");
                $("tr.commission").each(function () {
                    $(this).reveal();
                });
            });

            $(".bysubmission .view-evaluators").click(function () {
                $(".table.managesubmission.bysubmission tr").each(function () { $(this).collapse(); });
                $("tr.evaluator").each(function () {
                    $(this).reveal();
                });
            });
            checkbysubmission($("table.table.managesubmission.bysubmission"));
        })
    </script>
    <!-- CLOSE SCRIPT SELECT BY EVALUATIONS -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
 <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
           <CTRL:Messages ID="CTRLemptyMessage"  runat="server"/>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
    <div class="contentwrapper edit clearfix persist-area">
        <div class="column left persist-header copyThis">
            <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
        </div>
        <div class="column right resizeThis" style="height: auto;">
            <div class="rightcontent managesubmission">
                <div class="header">
                    <div class="DivEpButton">
                        <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                        <asp:Button ID="BTNopenCloseEvaluationsTop" runat="server" CssClass="Link_Menu" Text="Save" />
                    </div>
                    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                </div>
                <div class="contentouter">
                    <div class="content">
                        <!-- @Start MANAGE evaluations -->
                        <div class="fieldobject">
                            <div class="fieldrow inline">
                                <asp:Label ID="LBendEvaluationOn_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RDPendEvaluationOn">*Valutazioni concluse entro il:</asp:Label>
                                <telerik:RadDateTimePicker ID="RDPendEvaluationOn" runat="server">
                                </telerik:RadDateTimePicker>
                                <asp:Button ID="BTNupdateEvaluationEndDate" runat="server" CssClass="Link_Menu" Text="*Aggiorna data" />
                            </div>
                        </div>
                        <div class="fieldobject switchs">
                            <div class="fieldrow operationswitch inline">
                                <asp:Label ID="LBopenCloseEvaluations_t" runat="server" CssClass="fieldlabel">*Operazione</asp:Label>
                                <span class="btnswitchgroup"><!--                                            
                                    --><asp:LinkButton ID="LNBcloseEvaluations" runat="server" CssClass="btnswitch active first" Text="*Blocca"></asp:LinkButton><!--
                                    --><asp:LinkButton ID="LNBopenEvaluations" runat="server" CssClass="btnswitch normal last" Text="*Sblocca"></asp:LinkButton><!--
                                    --></span>
                            </div>
                            <div class="fieldrow typeswitch inline" id="DVdisplaySelector" runat="server" visible="true">
                                <asp:Label ID="LBopenCloseEvaluationByType_t" runat="server" CssClass="fieldlabel">*Tipo</asp:Label>
                                <span class="btnswitchgroup"><!--
                                    --><asp:LinkButton ID="LNBdisplayByEvaluations" runat="server" CssClass="btnswitch active first" Text="*Sottomissioni"></asp:LinkButton><!--
                                    --><asp:LinkButton ID="LNBdisplayByEvaluators" runat="server" CssClass="btnswitch normal last" Text="*Valutatori"></asp:LinkButton><!--
                                --></span>
                            </div>
                        </div>
                        <div class="fieldobject filters" id="DVsubmissionFilters" runat="server" visible="false">
                            <div class="fieldrow filtername inline">
                                <asp:Label ID="LBsubmissionName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBname">*Nome</asp:Label>
                                <asp:TextBox ID="TXBname" runat="server"></asp:TextBox>
                            </div>
                            <div class="fieldrow filtersubmission inline" id="DVsubmitterType" runat="server">
                                <asp:Label ID="LBsubmissionType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLsubmittersType">*Tipo</asp:Label>
                                <asp:DropDownList ID="DDLsubmittersType" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="fieldrow filterupdate inline">
                                <asp:Button ID="BTNupdateManageEvaluationsTable" runat="server" class="Link_Menu"
                                    Text="*Aggiorna" />
                            </div>
                        </div>
                        <div class="visibilityNav byevaluator" id="DVnavigation" runat="server" visible="false">
                            <asp:HyperLink ID="HYPviewSubmissions" runat="server" CssClass="view-submissions">Domande</asp:HyperLink>
                            <asp:Label ID="LBnavigateToCommission" runat="server" CssClass="icon treenavseparator">&gt;</asp:Label>
                            <asp:HyperLink ID="HYPviewCommittees" runat="server" CssClass="view-commissions">*Commissioni</asp:HyperLink>
                            <span class="icon treenavseparator">&gt;</span>
                            <asp:HyperLink ID="HYPviewEvaluators" runat="server" CssClass="view-evaluators">*Valutatori</asp:HyperLink>
                        </div>
                        <div class="tablewrapper">
                            <asp:MultiView ID="MLVdata" runat="server">
                                <asp:View ID="VIWemptyData" runat="server">
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Label ID="LBviewEmptyData" runat="server"></asp:Label>
                                </asp:View>
                                <asp:View ID="VIWbyEvaluators" runat="server">
                                    <table class="table managesubmission byevaluator fullwidth light">
                                        <thead>
                                            <tr>
                                                <th class="select">
                                                    <input type="checkbox" class="checkall" />
                                                </th>
                                                <th class="name">
                                                    <asp:Literal ID="LTnameTableHeader" runat="server">*name</asp:Literal>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="RPTselectByComittees" runat="server">
                                                <ItemTemplate>
                                                    <tr class="commission" id="com-<%#Container.DataItem.Id %>">
                                                        <td class="select">
                                                            <input type="checkbox" class="checkcommission"></input><asp:Literal ID="LTidCommittee"
                                                                runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                                                        </td>
                                                        <td class="name">
                                                            <asp:Literal ID="LTcommitteNameHeader" runat="server"></asp:Literal> - 
                                                            <%#Container.DataItem.Name%>
                                                            <span class="info"><span class="separator">&nbsp;(</span><span class="number"><%#Container.DataItem.ItemsCount(Me.CurrentAction)%></span><span class="separator">)</span></span>
                                                        </td>
                                                    </tr>
                                                    <asp:Repeater ID="RPTselectByEvaluators" runat="server" DataSource="<%#Container.DataItem.Evaluators %>">
                                                        <ItemTemplate>
                                                            <tr class="evaluator child-of-com-<%#Container.DataItem.IdCommittee %>" id="com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>">
                                                                <td class="select">
                                                                    <input type="checkbox" class="checkevaluator" runat="server" id="CBXevaluator" /><asp:Literal
                                                                        ID="LTidEvaluator" runat="server" Text='<%#Container.DataItem.IdEvaluator %>'
                                                                        Visible="false" />
                                                                </td>
                                                                <td class="name">
                                                                    <%#Container.DataItem.Name %>
                                                                    <span class="info"><span class="separator">&nbsp;(</span><span class="number"><%#Container.DataItem.ItemsCount(Me.CurrentAction)%></span><span class="separator">)</span></span>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </body> </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </asp:View>
                                <asp:View ID="VIWbySubmissions" runat="server">
                                    <table class="table managesubmission bysubmission fullwidth light">
                                        <thead>
                                            <tr>
                                                <th class="select">
                                                    <span class="headercheckbox leftside">
                                                        <input type="checkbox" class="checkall"/>
                                                        <div class="selectorpopup checkall">
                                                            <div class="inner">
                                                                &nbsp;<asp:LinkButton ID="LNBselectItemsIntoAllPages" runat="server" Text="*Seleziona su tutte le pagine"></asp:LinkButton>
                                                            </div>
                                                            <div class="bottomarrow">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="selectorpopup checknone">
                                                            <div class="inner">
                                                                &nbsp;<asp:LinkButton ID="LNBunselectItemsIntoAllPages" runat="server" Text="*Deseleziona su tutte le pagine"></asp:LinkButton>
                                                            </div>
                                                            <div class="bottomarrow">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                    </span>
                                                </th>
                                                <th class="name"><asp:Literal ID="LTnameTableHeaderbySubmission" runat="server">*name</asp:Literal></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        <asp:Repeater ID="RPTselectBySubmissions" runat="server">
                                            <ItemTemplate>
                                                <tr class="submission" id="sub-<%#Container.DataItem.Id %>">
                                                    <td class="select">
                                                        <input type="checkbox" class="checksubmission"></input><asp:Literal ID="LTidCommittee" runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                                                    </td>
                                                    <td class="name">
                                                         <asp:Literal ID="LTsubmissionNameHeader" runat="server"></asp:Literal> - 
                                                         <%#Container.DataItem.DisplayName%>
                                                         <span class="info"><span class="separator">&nbsp;(</span><span class="number"><%#Container.DataItem.ItemsCount(Me.CurrentAction)%></span><span class="separator">)</span></span>
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="RPTselectByComittees" runat="server" DataSource="<%#Container.DataItem.Committees %>" OnItemDataBound="RPTselectByComitteesInternal_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr class="commission child-of-sub-<%#Container.DataItem.IdSubmission%>" id="sub-<%#Container.DataItem.IdSubmission%>-com-<%#Container.DataItem.Id %>">
                                                            <td class="select">
                                                                <input type="checkbox" class="checkcommission"></input><asp:Literal ID="LTidCommittee"
                                                                    runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                                                            </td>
                                                            <td class="name">
                                                                <asp:Literal ID="LTcommitteNameHeader" runat="server"></asp:Literal> - 
                                                                <%#Container.DataItem.Name%>
                                                                <span class="info"><span class="separator">&nbsp;(</span><span class="number"><%#Container.DataItem.ItemsCount(Me.CurrentAction)%></span><span class="separator">)</span></span>
                                                            </td>
                                                           
                                                        </tr>
                                                        <asp:Repeater ID="RPTselectByEvaluators" runat="server" DataSource="<%#Container.DataItem.Evaluators %>" OnItemDataBound="RPTselectByEvaluators_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr class="evaluator child-of-sub-<%#Container.DataItem.IdSubmission%>-com-<%#Container.DataItem.IdCommittee %>" id="sub-<%#Container.DataItem.IdSubmission%>-com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>">
                                                                <td class="select">
                                                                    <input type="checkbox" class="checkevaluator" runat="server" id="CBXevaluator" />
                                                                    <asp:Literal ID="LTidEvaluator" runat="server" Text='<%#Container.DataItem.IdEvaluator %>' Visible="false" />
                                                                    <asp:Literal ID="LTidEvaluation" runat="server" Text='<%#Container.DataItem.IdEvaluation %>' Visible="false" />
                                                                </td>
                                                                <td class="name">
                                                                    <%#Container.DataItem.Name %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </tbody>
                                    </table>
                                    <div class="pager">
                                        <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false" Visible="false"></CTRL:GridPager>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                        <!-- @CLOSE MANAGE evaluations -->
                    </div>
                </div>
                <div class="footer">
                    <div class="DivEpButton">
                        <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                        <asp:Button ID="BTNopenCloseEvaluationsBottom" runat="server" Text="Save" />
                    </div>
                </div>
            </div>
        </div>
    </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>