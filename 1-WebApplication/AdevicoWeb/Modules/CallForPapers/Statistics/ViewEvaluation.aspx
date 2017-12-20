<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master"
    CodeBehind="ViewEvaluation.aspx.vb" Inherits="Comunita_OnLine.ViewEvaluation" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyNumber.ascx" TagName="CTRLfuzzyNumber" TagPrefix="CTRL" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"  EnableTreeTableScript="true" EnableSemiFixedScript="true"  />
<%--    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
  
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
        <link rel="stylesheet" href="../../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.checkboxList.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.textVal.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery-semifixed.js"></script>
     <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>--%>
    <script language="javascript" type="text/javascript">
        $(function(){
            $(".table.light.evaluations").treeTable({clickableNodeNames:true, initialState:"expanded"});

            $(".view-commissions").click(function(){
                $(".table.light.evaluations tr").each(function(){ $(this).collapse(); }); //.treeTable("collapseAll");
            });

            $(".view-evaluators").click(function(){
                $(".table.light.evaluations tr").each(function(){ $(this).collapse(); });
                $("tr.evaluator").each(function(){
                    $(this).reveal();
                });
            });

            $(".view-criterias").click(function(){
                $(".table.light.evaluations tr").each(function(){ $(this).collapse(); });
                $("tr.criteria").each(function(){
                    $(this).reveal();
                });
            });

            $(".view-comments").click(function(){
                $(".table.light.evaluations tr").each(function(){ $(this).collapse(); });
                $("tr.comment").each(function(){
                    $(this).reveal();
                });
            });
        });


    </script>
    <style>
        dt.general span.criteriavote span.boolean,
        dt.general span.criteriavote span.score
        {
            font-size: 0.7em;
            padding-left: 1em;
        }
        span.statusitem span.blue
        {
            font-size: 1em;
            font-weight: normal;
            font-style: normal;
            padding: 1px 3px;
            min-width: 20px;
            display: inline-block;
            text-align: center;
            box-shadow: inset 1px 1px 3px rgba(0,0,0,0.3);
            text-shadow: 0 1px 0 rgba(255,255,255,0.5);
            background-color: #4a9ac7;
            color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="contentwrapper edit clearfix">
        <div class="view evaluation">
            <asp:MultiView ID="MLVdata" runat="server" ActiveViewIndex="1">
                <asp:View ID="VIWdataEmpty" runat="server">
                   <CTRL:Messages ID="CTRLemptyMessage"  runat="server"/>
                </asp:View>
                <asp:View ID="VIWdata" runat="server">
                    <div class="infobar">
                        <div class="fieldobject">
                            <div class="fieldrow">
                                <asp:Label ID="LBcallName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcallName">*Call:</asp:Label>
                                <asp:Label ID="LBcallName" runat="server" CssClass="fieldtext"></asp:Label>
                            </div>
                            <div class="fieldrow">
                                <asp:Label ID="LBowner_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBowner_t">*Submitter:</asp:Label>
                                <asp:Label ID="LBowner" runat="server" CssClass="fieldtext"></asp:Label>
                            </div>
                            <div class="fieldrow">
                                <asp:Label ID="LBsubmittedOn_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsubmittedOn">*Submitted on:</asp:Label>
                                <asp:Label ID="LBsubmittedOn" runat="server" CssClass="fieldtext"></asp:Label>
                                <asp:Label ID="LBsubmittedBy_t" runat="server" CssClass="fieldlabel" Visible="false"
                                    AssociatedControlID="LBsubmittedBy"></asp:Label>
                                <asp:Label ID="LBsubmittedBy" runat="server" CssClass="fieldtext" Visible="false"></asp:Label>
                            </div>
                            <div class="fieldrow" id="DVevaluatorInfo" runat="server" visible="false">
                                <asp:Label ID="LBevaluatedBy_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LTevaluatedBy">*Evaluated by:</asp:Label>
                                <span class="fieldtext">
                                    <asp:Literal ID="LTevaluatedBy" runat="server"></asp:Literal>
                                    <asp:Label ID="LBcommitteesInfo" runat="server" CssClass="extrainfo" Visible="false"></asp:Label>
                                </span>
                            </div>
                            <div class="fieldrow clearfix">
                                <span class="fieldlabel">
                                    <asp:Literal ID="LTdisplayEvaluationsStatus_t" runat="server">*Evaluation:</asp:Literal>
                                </span>
                                <span class="status completion">
                                    <span class="statusitem" id="SPNsingleCommittee" runat="server">
                                        <asp:Label ID="LBcommitteeEvaluation" runat="server"></asp:Label>
                                    </span>
                                    <asp:Repeater ID="RPTevaluationStatus" runat="server">
                                        <ItemTemplate>
                                            <span class="statusitem"><span class="label">
                                                <asp:Literal ID="LTinCommittee" runat="server">*in</asp:Literal>
                                                <asp:Label ID="LBcommitteeName" runat="server"></asp:Label>
                                            </span>
                                                <asp:Label ID="LBcommitteeEvaluation" runat="server"></asp:Label>
                                            </span>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </span>
                            </div>
                        </div>
                        <div class="fieldrow buttons clearfix">
                            <asp:HyperLink ID="HYPviewSubmission" runat="server" Visible="false" CssClass="Link_Menu"></asp:HyperLink>
                            <asp:HyperLink ID="HYPviewTableEvaluation" runat="server" CssClass="Link_Menu" Visible="false">*View evaluation</asp:HyperLink>
                            <asp:HyperLink ID="HYPprintEvaluation" runat="server" Visible="false" CssClass="Link_Menu" href="javascript:window.print();"></asp:HyperLink>
                        </div>
                    </div>
                     <CTRL:Messages ID="CTRLdssMessage"  runat="server" visble="false"/>
                    <asp:MultiView ID="MLVevaluation" runat="server" ActiveViewIndex="1">
                        <asp:View ID="VIWempty" runat="server">
                            <br /><br /><br /><br /><br />
                            <asp:Label ID="LBempyMessage" runat="server"></asp:Label>
                        </asp:View>
                        <asp:View ID="VIWevaluator" runat="server">
                            <div class="evaluation">
                                <asp:Repeater ID="RPTevaluatorCommittee" runat="server">
                                    <ItemTemplate>
                                        <dl class="criterias">
                                            <div class="commissionname">
                                                <h3>
                                                    <asp:Literal ID="LTcommitteeName" runat="server"></asp:Literal></h3>
                                            </div>
                                            <dt class="criteriatitle general">
                                                <asp:Label ID="LBevaluationVote_t" runat="server" class="criterianame">*General evaluation:</asp:Label>
                                                <asp:Label ID="LBevaluationVote" runat="server" class="criteriavote"></asp:Label>
                                                <span id="SPNfuzzyNumber" runat="server" visible="false" class="criteriavote"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
                                            </dt>
                                            <dd class="criteriacomment general" id="DDgeneralComment" runat="server">
                                                <asp:Label ID="LBevaluationComment_t" runat="server" class="fieldlabel" AssociatedControlID="LBevaluationComment">*Comment:</asp:Label>
                                                <asp:Label ID="LBevaluationComment" runat="server" class="description">*Comment:</asp:Label>
                                            </dd>
                                            <dd class="criteriacomment general" id="DDgeneralEmptyComment" runat="server" visible="false">
                                                &nbsp;
                                            </dd>
                                            <asp:Repeater ID="RPTevaluationCriteria" runat="server" DataSource="<%#Container.DataItem.Evaluation.Criteria %>"
                                                OnItemDataBound="RPTevaluationCriteria_ItemDataBound">
                                                <ItemTemplate>
                                                    <dt class="criteriatitle">
                                                        <asp:Label ID="LBevaluationCriterion_t" runat="server" class="criterianame">*Criterion:</asp:Label>
                                                        <asp:Label ID="LBevaluationCriterion" runat="server" class="criteriavote"></asp:Label>
                                                        <span id="SPNfuzzyNumber" runat="server" visible="false" class="criteriavote"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
                                                    </dt>
                                                    <dd class="criteriacomment" id="DDcriterionComment" runat="server">
                                                        <asp:Label ID="LBevaluationCriterionComment_t" runat="server" class="fieldlabel" AssociatedControlID="LBevaluationCriterionComment">Comment:</asp:Label>
                                                        <asp:Label ID="LBevaluationCriterionComment" runat="server" class="criteriavote"></asp:Label>
                                                    </dd>
                                                    <dd class="criteriacomment general" id="DDcriterionEmptyComment" runat="server" visible="false">
                                                        &nbsp;
                                                    </dd>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </dl>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWevaluators" runat="server">
                            <div class="visibilityNav">
                                <a class="view-commissions"><asp:Literal ID="LTnavigateToComittees" runat="server">*Committees</asp:Literal></a>
                                <span class="icon treenavseparator">></span>
                                <a class="view-evaluators"><asp:Literal ID="LTnavigateToEvaluators" runat="server">*Evaluators</asp:Literal></a>
                                <span class="icon treenavseparator">></span>
                                <a class="view-criterias"><asp:Literal ID="LTnavigateToCriteria" runat="server">*Criteria</asp:Literal></a>
                                <span class="icon treenavseparator" runat="server" id="SPNnavigateToComments">></span>
                                <asp:HyperLink ID="HYPnavigateToComments" runat="server" CssClass="view-comments">*Comments</asp:HyperLink>
	                        </div>                
                            <table class="table light evaluations">
                                <thead>
                                    <tr>
                                        <th class="title">
                                            <asp:Literal ID="LTcommitteeName_t" runat="server">*Name</asp:Literal>
                                            <span class="right"><asp:Literal ID="LTcommitteeEvaluationStatus_t" runat="server">*Status</asp:Literal></span>
                                        </th>
                                        <th class="vote">
                                            <asp:Literal ID="LTcommitteeVote_t" runat="server">*Vote</asp:Literal>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RPTcomittees" runat="server">
                                        <ItemTemplate>
                                            <tr id="com-<%#Container.DataItem.IdCommittee %>" class="commission <%#CssRowItem(Container.DataItem.Display) %>">
                                                <td class="title">
                                                    <span class="left"><%#Container.DataItem.CommitteeName%></span>
                                                    <span class="right">
                                                        <span class="status " runat="server" id="SPNstatus">
                                                            <asp:Label ID="LBcommitteeEvaluation" runat="server"></asp:Label>
                                                        </span>
                                                    </span>
                                                </td>
                                                <td class="vote">
                                                    <asp:Label ID="LBcommitteeVote" runat="server" class="criteriavote"></asp:Label>
                                                    <span id="SPNfuzzyNumber" runat="server" visible="false" class="criteriavote"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
                                                </td>
                                            </tr>
                                            <asp:Repeater ID="RPTevaluators" runat="server" DataSource="<%#Container.DataItem.Evaluations %>" OnItemDataBound="RPTevaluators_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr id="com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>" class="evaluator <%#CssRowItem(Container.DataItem.Display) %> child-of-com-<%#Container.DataItem.IdCommittee %>">
                                                        <td class="title">
                                                            <span class="left"><%#Container.DataItem.EvaluatorName%></span>
                                                            <span class="right">
                                                                <span class="status " runat="server" id="SPNstatus">
                                                                    <asp:Label ID="LBevaluatorEvaluation" runat="server"></asp:Label>
                                                                </span>
                                                            </span>
                                                        </td>
                                                        <td class="vote">
                                                            <asp:Label ID="LBevaluatorVote" runat="server" class="criteriavote"></asp:Label>
                                                            <span id="SPNfuzzyNumber" runat="server" visible="false" class="criteriavote"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
                                                        </td>
                                                    </tr>
                                                    <asp:MultiView ID="MLVevaluationComment" runat="server">
                                                        <asp:View ID="VIWnoComment" runat="server">
                                                        </asp:View>
                                                        <asp:View ID="VIWcomment" runat="server">
                                                            <tr id="com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>-crt-0" class="criteria general first child-of-com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>">
                                                                <td class="title">
                                                                   <asp:Literal ID="LTevaluationComment_t" runat="server"></asp:Literal>
                                                                </td>
                                                                <td class="vote">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr id="com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>-crt-0-comment" class="comment general child-of-com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>-crt-0">
                                                                <td class="comment" colspan="4">
                                                                   <span class="wrapper"><%#Container.DataItem.Comment%></span>
                                                                </td>
                                                            </tr>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                    <asp:Repeater ID="RPTcriteria" runat="server" DataSource="<%#Container.DataItem.Criteria %>" OnItemDataBound="RPTcriteria_ItemDataBound">
                                                        <ItemTemplate>
                                                             <tr id="com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>-crt-<%#Container.DataItem.IdCriterion %>" class="criteria <%#CssRowItem(Container.DataItem.Display) %>  child-of-com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>">
                                                                <td class="title">
                                                                    <%#Container.DataItem.Criterion.Name%> 
                                                                    <span class="right">
                                                                        <span class="status " runat="server" id="SPNstatus">
                                                                            <asp:Label ID="LBcriterionEvaluation" runat="server"></asp:Label>
                                                                        </span>
                                                                     </span>
                                                                </td>
                                                                <td class="vote">
                                                                    <asp:Label ID="LBcriterionVote" CssClass="criteriavote " runat="server"></asp:Label>
                                                                    <span id="SPNfuzzyNumber" runat="server" visible="false" class="criteriavote"><CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" runat="server"></CTRL:CTRLfuzzyNumber></span>
                                                                </td>
                                                            </tr>
                                                            <asp:MultiView ID="MLVcriterionComment" runat="server">
                                                                <asp:View ID="VIWnoComment" runat="server">
                                                                </asp:View>
                                                                <asp:View ID="VIWcomment" runat="server">
                                                                    <tr id="com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>-crt-<%#Container.DataItem.IdCriterion %>-comment" class="comment child-of-com-<%#Container.DataItem.IdCommittee %>-eva-<%#Container.DataItem.IdEvaluator %>-crt-<%#Container.DataItem.IdCriterion %>">
                                                                        <td class="comment" colspan="4">
                                                                            <span class="wrapper"><%#Container.DataItem.Comment%></span>
                                                                        </td>
                                                                    </tr>
                                                                </asp:View>
                                                            </asp:MultiView>
                                                            </ItemTemplate>
                                                    </asp:Repeater>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                                <tfoot>
                                </tfoot>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>
