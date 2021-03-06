﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CommitteesSummary.aspx.vb" Inherits="Comunita_OnLine.CommitteesSummary" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/UC/UC_StackedBar.ascx" TagName="StackedBar" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyNumber.ascx" TagName="CTRLfuzzyNumber" TagPrefix="CTRL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"  EnableDropDownButtonsScript="true" EnableTreeTableScript="true" />
    <link href="../../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.progressbar.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.slider.extend.js"></script>
  <%--  <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../../Graphics/Modules/CallForPapers/css/cfp-evaluation.css" rel="Stylesheet" />
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.progressbar.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.slider.extend.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>--%>

    <!-- EXPORT -->
    <script type="text/javascript">
        var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieName = "<% = Me.CookieName %>";
        var DisplayMessage = "<% = Me.DisplayMessageToken %>";
        var DisplayTitle = "<% = Me.DisplayTitleToken %>";
    </script>
    <script type="text/javascript">
        var fileDownloadCheckTimer;
        function blockUIForDownload() {
            var token = new Date().getTime(); //use the current timestamp as the token value
            $("input[id='" + TokenHiddenFieldId + "']").val(token);
            $.blockUI({ message: DisplayMessage, title: DisplayTitle, draggable: false, theme: true });
            fileDownloadCheckTimer = window.setInterval(function () {
                var cookieValue = $.cookie(CookieName);
                if (cookieValue == token)
                    finishDownload();
            }, 1000);
        }

        function finishDownload() {
            window.clearInterval(fileDownloadCheckTimer);
            $.cookie(CookieName, null); //clears this cookie value
            $.unblockUI();
        }
    </script>

    <!-- TEMPORARY - TO REMOVE-->
     <script type="text/javascript" language="javascript">
         /*var page=0;
         var lastpage= 5;
         var pagesize = 5;*/

         $(function () {
             $(".ddbuttonlist.enabled").dropdownButtonList();
                          $(".tree_table").treeTable({
                              clickableNodeNames: true,
                              initialState: "collapsed",
                              persist: false
                          });


             $(".tree_table").data("page", 0);
             //$(".tree_table").data("lastpage",5);     //  copy all of those from table attributes/class
             $(".tree_table").data("pagesize", 5);     // egual for each? maybe yes


             $(".tree_table").each(function () {
                 var n = $(this).find("th.evaluator").size() - $(this).data("pagesize");
                 if (n < 0) { n = 0; }
                 $(this).data("lastpage", n);
             })



             $(".progressbar").myProgressBar();

             $(".tree_table").each(function () {

                 var page = $(this).data("page");
                 var pagesize = $(this).data("pagesize");
                 var lastpage = $(this).data("lastpage");

                 $(this).find(".slider.min").html(1 + page);
                 $(this).find(".slider.max").html(pagesize + page);
                 $(this).find(".slider.all").html(lastpage + pagesize);
             });



             $(".colslider").slider({
                 value: 0,
                 min: 0,
                 max: 10,
                 step: 1,
                 slide: function (event, ui) {
                     var $table = $(this).parents("table.tree_table");

                     var page = ui.value;
                     var pagesize = $table.data("pagesize");
                     var lastpage = $table.data("lastpage");



                     $table.data("page", page);

                     $table.find(".colslider").not($(this)).slider("value", page);

                     //$(".colslider").not($(this)).slider("value",page);
                     $table.find(".slider.min").html(1 + page);
                     $table.find(".slider.max").html(pagesize + page);
                     $table.find(".slider.all").html(lastpage + pagesize);


                     $table.find("tr").each(function () {
                         var $tr = $(this);
                         var $ths = $tr.children("th.evaluator");
                         var $tds = $tr.children("td.evaluator");

                         $ths.hide();
                         $tds.hide();
                         for (x = 0; x < pagesize; x++) {
                             $ths.eq(x + page).show();
                             $tds.eq(x + page).show();
                         }

                     });
                 }
                 //});
             }).sliderAccess({
                 touchonly: false,
                 upIcon: 'ui-icon-triangle-1-e',
                 downIcon: 'ui-icon-triangle-1-w',
                 upText: "",
                 downText: ""
             });

             $(".ui-slider-access button").click(function () {
                 return false;
             });

             $(".colslider").each(function () {
                 $(this).slider("option", "max", $(this).parents("table.tree_table").data("lastpage"));
             })


             $(".tree_table tr").each(function () {
                 var $tr = $(this);
                 var $ths = $tr.children("th.evaluator");
                 var $tds = $tr.children("td.evaluator");

                 var $table = $(this).parents("table.tree_table");

                 var page = $table.data("page");
                 var pagesize = $table.data("pagesize");
                 var lastpage = $table.data("lastpage");

                 $ths.hide();
                 $tds.hide();

                 for (x = 0; x < pagesize; x++) {
                     $ths.eq(x + page).show();
                     $tds.eq(x + page).show();
                 }

             });

             $(".comment").click(function () {

                 var x = $(this).parents("td").find(".commentdialog");

                 if (x.data("dialog") != "open") {
                     x.data("dialog", "open");
                     var clone = x.clone();

                     clone.dialog({
                         close: function () {
                             x.removeData("dialog");
                         }
                     });
                 }
             });

             $(".closecomments").click(function () {

                 $(".commentdialog").dialog("close");
             });

             $(".dialog.legend").dialog("option", "width", 600);

             $(".openlegend").click(function () {
                 var href = $(this).attr("href");
                 $(href).dialog("open");
                 return false;
             });

         });



    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
     <div class="viewbuttons clearfix">
        <div class="ddbuttonlist enabled" id="DVexport" runat="server"><!--
    --><asp:LinkButton ID="LNBexportAllCommitteesSummaryToCsv" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportAllCommitteesSummaryToXLS" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportFilteredCommitteesSummaryToCsv" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportFilteredCommitteesSummaryToXLS" runat="server" Text="Esporta" CssClass="linkMenu" OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportAllEvaluationsSummaryDataToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportAllEvaluationsSummaryData" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportFullEvaluationsSummaryDataToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportFullEvaluationsSummaryDataToXml" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
    --></div>
        <asp:HyperLink ID="HYPlist" runat="server" Text="List calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPmanage" runat="server" Text="Manage calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToSummaryIndex" runat="server" Text="Back to summary" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
    </div>
    <CTRL:Messages ID="CTRLdssMessage"  runat="server" visble="false"/>
    <div class="contentwrapper edit clearfix" id="DVfilter" runat="server" >
        <div class="left">
            <asp:Label ID="LBsearchEvaluationsFor_t" runat="server" AssociatedControlID="TXBusername" CssClass="fieldlabel"></asp:Label>
            <asp:TextBox ID="TXBusername" runat="server" CssClass="inputtext"></asp:TextBox>
            <asp:Button id="BTNfindEvaluations" runat="server" />
            <br />
            <div class="evaluationfilter" id="DVsubmitterType" runat="server">
                <asp:Label ID="LBsubmitterType_t" runat="server" AssociatedControlID="DDLsubmitterTypes" CssClass="fieldlabel"></asp:Label>
                <asp:DropDownList ID="DDLsubmitterTypes" runat="server" CssClass="inputtext" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="evaluationfilter" id="DVstatusfilter" runat="server">
                <asp:Label ID="LBevaluationStatusFilter_t" runat="server" AssociatedControlID="RBLevaluationStatus" CssClass="fieldlabel"></asp:Label>
                <asp:RadioButtonList ID="RBLevaluationStatus" runat="server" AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputradiobuttonlist"></asp:RadioButtonList>
            </div>

        </div>
        <div class="right">            
        </div>
    </div>
     <asp:MultiView id="MLVstatistics" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnocalls" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWnoItems" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnoEvaluations" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        
        <asp:View ID="VIWstatistics" runat="server">
            <div class="view">
                <div class="pager" runat="server" id="DVpagerTop"  visible="false">
                    <asp:literal ID="LTpageTop" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
                </div>
                <table id="tree_table" class="tree_table evaluation detail <%=CssTableCommitteeCount() %>">
                    <thead>
                        <tr class="sliderrow">
                            <th class="submitternumber empty">&nbsp;</th>
                            <th class="submittername empty">&nbsp;</th>
                            <th class="submittertype empty">&nbsp;</th>
                            <th class="points empty">&nbsp;</th>
                            <th class="status empty">&nbsp;</th>
                            <th colspan="5" class="slidercell commissions">
                                <span >
                                    <asp:Literal ID="LTcommitteesSliderTop" runat="server"></asp:Literal>
                                    <div class="colslider" id="slider"></div>
                                    <span class="slider min"></span>-<span class="slider max"></span>/<span class="slider all"></span>
                                </span>
                            </th>
                        </tr>
                        <tr>
                            <th class="submitternumber" rowspan="2">
                                <asp:HyperLink ID="HYPorderByEvaluationIndexUp" runat="server" cssclass="icon orderUp" Visible="false">#</asp:HyperLink>
                                <asp:HyperLink ID="HYPorderByEvaluationIndexDown" runat="server" cssclass="icon orderDown" Visible="false">#</asp:HyperLink>
                            </th>
                            <th class="submittername" rowspan="2">
                                <asp:Literal id="LTsubmitterName_t" runat="server"></asp:Literal>
                                <asp:HyperLink ID="HYPorderByUserUp" runat="server" cssclass="icon orderUp" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPorderByUserDown" runat="server" cssclass="icon orderDown" Visible="false"></asp:HyperLink>
                            </th>
                            <th class="submittertype" rowspan="2">
                                <asp:Literal id="LTsubmitterType_t" runat="server"></asp:Literal>
                                <asp:HyperLink ID="HYPorderByTypeUp" runat="server" cssclass="icon orderUp" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPorderByTypeDown" runat="server" cssclass="icon orderDown" Visible="false"></asp:HyperLink>
                            </th>
                            <th class="points" rowspan="2">
                                <asp:Literal id="LTsubmissionPoints_t" runat="server"></asp:Literal>
                                <asp:HyperLink ID="HYPorderByEvaluationPointsUp" runat="server" cssclass="icon orderUp" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPorderByEvaluationPointsDown" runat="server" cssclass="icon orderDown" Visible="false"></asp:HyperLink>
                            </th>
                            <th class="status" rowspan="2">
                                <asp:Literal id="LTevaluationStatus_t" runat="server"></asp:Literal>
                                <asp:HyperLink ID="HYPorderByEvaluationStatusUp" runat="server" cssclass="icon orderUp" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPorderByEvaluationStatusDown" runat="server" cssclass="icon orderDown" Visible="false"></asp:HyperLink>
                            </th>
                            <th class="commission" colspan="5">
                                <asp:DropDownList ID="DDLcommittees" runat="server" cssclass="commissionsselect" AutoPostBack="true"></asp:DropDownList>
                            </th>
                        </tr>
                        <tr>
                            <asp:Repeater ID="RPTcommittees" runat="server">
                                <ItemTemplate>
                                    <th class="evaluator"><asp:label ID="LBcommitteeName" runat="server"></asp:label></th>    
                                </ItemTemplate>
                            </asp:Repeater>
                            <th class="evaluator" runat="server" visible="false" id="THcommitteePlaceHolder"></th>
                        </tr>
                    </thead>
                <tbody>
                    <asp:Repeater ID="RPTevaluations" runat="server">
                        <ItemTemplate>
                            <tr id="subm-<%#Container.DataItem.IdSubmission %>" class="submitter">
                                <td class="submitternumber"><%#Container.DataItem.Position%></td>
                                <td class="submittername"><%#Container.DataItem.DisplayName%></td>
                                <td class="submittertype"><%#Container.DataItem.SubmitterType%></td>
                                <td class="points">
                                    <asp:Label ID="LBvote" runat="server"></asp:Label>
                                    <span class="text" runat="server" visible="false" id="SPNfuzzy"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
                                </td>
                                <td class="status">
                                    <CTRL:StackedBar id="CTRLevaluationStackedBar" runat="server" ContainerCssClass="evaluationstats"></CTRL:StackedBar>
                                </td>
                                <asp:Repeater ID="RPTcommitteeEvaluations" runat="server" DataSource="<%#Container.DataItem.Committees %>"  OnItemDataBound="RPTcommitteeEvaluations_ItemDataBound">
                                    <ItemTemplate>
                                        <td class="evaluator" id="TDcommitteeEvaluation" runat="server"><asp:Label ID="LBcommitteeEvaluation" runat="server"></asp:Label><span class="text" runat="server" visible="false" id="SPNfuzzy"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span></td>    
                                    </ItemTemplate>
                                </asp:Repeater>
                                <td class="evaluator" runat="server" visible="false" id="TDcommitteePlaceHolder">&nbsp;</td>
                            </tr>
                            <tr id="stat-progressbar-<%#Container.DataItem.IdSubmission %>" class="stats child-of-subm-<%#Container.DataItem.IdSubmission %>">
                                <td class="commissionstats" colspan="5">
                                    <asp:Label ID="LBcommitteeEvaluation" runat="server"></asp:Label>
                                    <span class="text" runat="server" visible="false" id="Span1"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber1" runat="server"></CTRL:CTRLfuzzyNumber></span>
                                </td>
                                <asp:Repeater ID="RPTcommitteesStatus" runat="server" DataSource="<%#Container.DataItem.Committees %>" OnItemDataBound="RPTcommitteesStatus_ItemDataBound">
                                    <ItemTemplate>
                                        <td class="evaluator">
                                            <CTRL:StackedBar id="CTRLevaluationStackedBar" ContainerCssClass="commissionstats" runat="server"></CTRL:StackedBar>
                                        </td>    
                                    </ItemTemplate>
                                </asp:Repeater>
                                <td class="evaluator" runat="server" visible="false" id="TDcommissionstatsPlaceHolder">&nbsp;</td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr class="sliderrow">
                            <td colspan="5" class="empty">&nbsp;</td>
                            <td colspan="5" class="slidercell">
                                <span >
                                    <asp:Literal ID="LTcommitteesSliderBottom" runat="server"></asp:Literal>
                                    <div class="colslider"></div>
                                    <span class="slider min">1</span>-<span class="slider max">5</span>/<span class="slider all">10</span>
                                </span>
                            </td>
                        </tr>
                    </tfoot>
                </table>
                <div class="pager" runat="server" id="DVpagerBottom" visible="false">
                    <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>