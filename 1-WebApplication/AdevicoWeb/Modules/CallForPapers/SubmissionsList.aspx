
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="SubmissionsList.aspx.vb" Inherits="Comunita_OnLine.SubmissionsList" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLreport" Src="~/Modules/CallForPapers/UC/UC_SubmissionExport.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="DisplayFile" Src="~/Modules/Repository/UC/UC_ModuleRepositoryAction.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="PrintDraft" src="~/Modules/CallForPapers/UC/UC_PrintDraft.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" EnableScripts="true"  EnableTreeTableScript="true" EnableDropDownButtonsScript="true"  />
    <script type="text/javascript">
        <% = me.CTRLreport.GetControlScript(HDNdownloadTokenValue.ClientID) %>
    </script>
    <!-- Inizio Script importanti -->

    <script type="text/javascript">
        $(function () {
            $(".ddbuttonlist.enabled").dropdownButtonList();
            $("table.submissions").treeTable({
                clickableNodeNames: false,
                initialState: "collapsed",
                persist: false
            });

            $("fieldset.section.collapsed").each(function () {
                var $fieldset = $(this);
                var $legend = $fieldset.children().filter("legend");
                var $children = $fieldset.children().not("legend");
                $children.toggle();
            });

            $("fieldset.section.collapsable legend").click(function () {
                var $legend = $(this);
                var $fieldset = $legend.parent();
                var $children = $fieldset.children().not("legend");
                $children.toggle();
                $fieldset.toggleClass("collapsed");
            });

            //$(".fieldrow.fieldinput .checkboxlist")
        });
    </script>

    <!-- Fine Script importanti-->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="viewbuttons clearfix">
        <div runat="server" id="DVexport" class="ddbuttonlist enabled" visible="true"><!--   
            --><asp:LinkButton ID="LNBexportDataToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="data" OnClientClick="blockUIForDownload(6);return true;" ></asp:LinkButton><!--
            --><asp:LinkButton ID="LNBexportDataToXML" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="data" OnClientClick="blockUIForDownload(5);return true;" ></asp:LinkButton><!--
            --><asp:LinkButton ID="LNBexportToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="xls" OnClientClick="blockUIForDownload(6);return true;" Visible="false"></asp:LinkButton><!--
            --><asp:LinkButton ID="LNBexportToXML" runat="server" Text="Esporta" CssClass="linkMenu"  CommandName="xls" OnClientClick="blockUIForDownload(5);return true;" Visible="false"></asp:LinkButton><!--
        --></div>  

        <asp:HyperLink ID="HYPlist" runat="server" Text="List calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPmanage" runat="server" Text="Manage calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <CTRL:CTRLreport ID="CTRLreport" runat="server" WebOnlyRender="True" isContainer="false" Visible="false" />
    </div>
    <div class="contentwrapper edit clearfix" id="DVfilter" runat="server" visible="true" >
        <div class="left">
            <asp:Label ID="LBsearchSubmissionFor_t" runat="server" AssociatedControlID="TXBusername" CssClass="fieldlabel"></asp:Label>
            <asp:TextBox ID="TXBusername" runat="server" CssClass="inputtext"></asp:TextBox>
            <asp:Button id="BTNfindSubmissions" runat="server" />
            <br />
            <div class="evaluationfilter" id="DVstatusfilter" runat="server">
                <asp:Label ID="LBsubmissionStatusFilter_t" runat="server" AssociatedControlID="RBLstatus" CssClass="fieldlabel"></asp:Label>
                <asp:RadioButtonList ID="RBLstatus" runat="server" AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputradiobuttonlist"></asp:RadioButtonList>
            </div>
        </div>
        <div class="right">            
        </div>
    </div>
    <asp:MultiView ID="MLVsubmissions" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWlist" runat="server">
            <div class="pager" runat="server" id="DVpagerTop"  visible="false">
                <asp:literal ID="LTpageTop" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
            <div class="contentwrapper edit clearfix">
                <asp:Repeater id="RPTsubmissions" runat="server">
                    <HeaderTemplate>
                        <table class="cfp light submissions treeTable">
                            <thead>
                                <tr>
                                    <th class="partecipant">
                                        <asp:literal ID="LTsubPartecipant_t" runat="server">Sottomittore</asp:literal>
                                        <asp:LinkButton ID="LNBorderByUserUp" runat="server" cssclass="icon orderUp" CommandArgument="ByUser.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByUserDown" runat="server" cssclass="icon orderDown" CommandArgument="ByUser.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="partecipanttype">
                                        <asp:literal ID="LTsubPartecipantType_t" runat="server">Tipo</asp:literal>
                                        <asp:LinkButton ID="LNBorderByTypeUp" runat="server" cssclass="icon orderUp" CommandArgument="ByType.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByTypeDown" runat="server" cssclass="icon orderDown" CommandArgument="ByType.False" CommandName="orderby"></asp:LinkButton>
                                        
                                    </th>
                                    <th class="submittedon" id="THsubmittedOn" runat="server" visible="false">
                                        <asp:literal ID="LTsubSubmittedOn_t" runat="server">Date</asp:literal>
                                        <asp:LinkButton ID="LNBorderBySubmittedOnUp" runat="server" cssclass="icon orderUp" CommandArgument="BySubmittedOn.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderBySubmittedOnDown" runat="server" cssclass="icon orderDown" CommandArgument="BySubmittedOn.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="submittedon" id="THmodifedOn" runat="server" visible="false">
                                        <asp:literal ID="LTsubModifedOn_t" runat="server">Date</asp:literal>
                                        <asp:LinkButton ID="LNBorderByDateUp" runat="server" cssclass="icon orderUp" CommandArgument="ByDate.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByDateDown" runat="server" cssclass="icon orderDown" CommandArgument="ByDate.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="status">
                                        <asp:literal ID="LTsubStatus_t" runat="server">Status</asp:literal>
                                        <asp:LinkButton ID="LNBorderByStatusUp" runat="server" cssclass="icon orderUp" CommandArgument="ByStatus.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByStatusDown" runat="server" cssclass="icon orderDown" CommandArgument="ByStatus.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="<%=GetSignCss%>">
                                        <asp:literal ID="LTsign" runat="server">Controfirme</asp:literal>
                                    </th>
                                    <th class="actions"><asp:literal ID="LTsubActions_t" runat="server">Actions</asp:literal></th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                                <tr id="subm-<%#Container.DataItem.Submission.Id %>" class="submission <%#GetRevisionCssClass(Container.DataItem.Submission) %>">
                                    <td class="partecipant">
                                         <asp:literal ID="LTintegration" runat="server"/>
                                        <asp:literal ID="LTpartecipant" runat="server"/>
                                        <asp:Label ID="LBrevisionField" runat="server" CssClass="revisionfield revisioned right"></asp:Label>
                                        
                                       <%-- <span class="revisionfield revisioned right">
                                            <label>Revisioned</label>
                                        </span>--%>
                                    </td>
                                    <td class="partecipanttype">
                                        <asp:literal ID="LTsubPartecipantType" runat="server"></asp:literal>
                                    </td>
                                    <td class="submittedon" id="TDsubmittedOn" runat="server" visible="false">
                                        <asp:literal ID="LTsubSubmittedOn" runat="server">Date</asp:literal>
                                    </td>
                                    <td class="submittedon" id="TDmodifedOn" runat="server" visible="false">
                                        <asp:literal ID="LTsubModifedOn" runat="server">Date</asp:literal>
                                    </td>
                                    <td class="status">
                                        <span class="icons">
                                            <asp:Label ID="LBstatus" runat="server" CssClass="icon status"></asp:Label>
                                        </span>
                                        <asp:literal ID="LTsubStatus" runat="server"/>
                                        <asp:placeholder ID="PHsign" runat="server">
                                            <CTRL:DisplayFile ID="CTRLdisplayFile" runat="server"/>
                                        </asp:placeholder>
                                    </td>
                                    <td class="<%=GetSignCss%>">
                                        <CTRL:DisplayFile ID="CTRLdisplaySignNoRev" runat="server"/>
                                        <asp:Label runat="server" id="LBLNoSign" runat="server">Non caricata</asp:Label>
                                    </td>
                                    <td class="actions">
                                        <asp:literal ID="LTemptyActions" runat="server" Text=" "/>
                                        <span class="icons">                                            
                                            <asp:LinkButton ID="LNBsubDelete" runat="server" Visible="false" CssClass="icon delete needconfirm" CommandName="delete" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBsubVirtualDelete" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBsubRecover" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                            <CTRL:CTRLreport ID="CTRLreport" runat="server" isContainer="false" OnGetConfigTemplate="CTRLreport_GetConfigTemplate" OnGetContainerTemplate="CTRLreport_GetContainerTemplate" OnGetHiddenIdentifierValueEvent="CTRLreport_GetHiddenIdentifierValueEvent"/>
                                            <CTRL:PrintDraft ID="CTRLprintDraf" runat="server" ButtonCssClass="icon export pdf"/>
                                            <asp:HyperLink ID="HYPviewSubmission" runat="server" CssClass="icon view" Visible="false"></asp:HyperLink>
                                        </span>
                                    </td>
                                </tr>
                                <asp:Repeater ID="RPTrevisions" runat="server" DataSource="<%#Container.DataItem.Submission.Revisions%>" OnItemDataBound="RPTrevisions_ItemDataBound">
                                    <ItemTemplate>
                                        <tr id="subm-<%#Container.DataItem.Submission.Id %>-revision-<%#Container.DataItem.Id %>" class="revision child-of-subm-<%#Container.DataItem.Submission.Id %> <%#GetRevisionCssClass(Container.DataItem.Display) %>">
                                            <td class="partecipant" colspan="2">
                                               <asp:Label ID="LBrevisionName" runat="server" CssClass="revisionname"></asp:Label>
                                               <asp:Label ID="LBrevisionby" runat="server" CssClass="revisionby"></asp:Label>
                                            </td>
                                            <td class="submittedon">
                                                <asp:literal ID="LTsubSubmittedOn" runat="server">Date</asp:literal>
                                            </td>
                                            <td class="status warning">
                                                <span class="icons">
                                                    <asp:Label ID="LBstatus" runat="server" CssClass="icon status"></asp:Label>
                                                </span>
                                                <asp:literal ID="LTsubStatus" runat="server"/>
                                            </td>
                                            <td class="actions">
                                                <asp:literal ID="LTemptyActions" runat="server" Text=" "/>
                                                <span class="icons">
                                                    <CTRL:CTRLreport ID="CTRLreport" runat="server" isContainer="false" OnGetConfigTemplate="CTRLreport_GetConfigTemplate" OnGetContainerTemplate="CTRLreport_GetContainerTemplate" OnGetHiddenIdentifierValueEvent="CTRLreport_GetHiddenIdentifierValueEvent"/>
                                                    
                                                    <asp:HyperLink ID="HYPviewSubmission" runat="server" CssClass="icon view" Visible="false"></asp:HyperLink>
                                                </span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>

                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="pager" runat="server" id="DVpagerBottom" visible="false">
                <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
        </asp:View>
        <asp:View ID="VIWnoItems" runat="server">
            <br /><br /><br /><br /><br /><br />
            <asp:Label id="LBnoSubmissions" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />

     <asp:literal ID="LTintegrationContent" runat="server" Visible="false">
        <span class="integrations icons">
            <span class="icon revision" title="Integrazioni: {1}/{2}"></span>
        </span>
    </asp:literal>
</asp:Content>