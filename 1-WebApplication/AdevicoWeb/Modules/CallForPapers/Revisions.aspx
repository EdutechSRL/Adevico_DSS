<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Revisions.aspx.vb" Inherits="Comunita_OnLine.Revisions" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
     <CTRL:Header ID="CTRLheader" runat="server" EnableScripts="false" />
    <style type="text/css">
        .icons .icon
        {
            background-color:Gray;
            border-color:Black;
            border:2px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="viewbuttons clearfix">
        <asp:HyperLink ID="HYPlist" runat="server" Text="List calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPmanage" runat="server" Text="Manage calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
    </div>
    <div class="tabs clearfix">
         <telerik:radtabstrip id="TBScall" runat="server" align="Justify" width="100%"
            height="20px" causesvalidation="false" autopostback="false" skin="Outlook" enableembeddedskins="true">
            <tabs>
                <telerik:RadTab text="Revisioni" value="Revisions" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Valutati" value="Evaluated" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Bozze" value="Draft" visible="false"></telerik:RadTab>
                <telerik:RadTab text="In corso" value="SubmissionOpened" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Sottomessi" value="Submitted" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Conclusi" value="SubmissionClosed" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Da valutare" value="ToEvaluate" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Valutati" value="Evaluated" visible="false"></telerik:RadTab>
            </tabs>
        </telerik:radtabstrip>
    </div>
    <div class="contentwrapper edit clearfix" id="DVfilter" runat="server" visible="false">
        <div class="left">
            <asp:Label ID="LBsearchRevisionFor_t" runat="server" AssociatedControlID="TXBusername" CssClass="fieldlabel"></asp:Label>
            <asp:TextBox ID="TXBusername" runat="server" CssClass="inputtext"></asp:TextBox>
            <br />
            <asp:Label ID="LBrevisionStatusFilter_t" runat="server" AssociatedControlID="RBLstatus" CssClass="fieldlabel"></asp:Label>
            <asp:RadioButtonList ID="RBLstatus" runat="server" AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputradiobuttonlist"></asp:RadioButtonList>
        </div>
        <div class="right">
            <asp:Button id="BTNfindRevisions" runat="server" />
        </div>
    </div>
    <asp:MultiView ID="MLVresults" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWlist" runat="server">
            <div class="pager" runat="server" id="DVpagerTop"  visible="false">
                <asp:literal ID="LTpageTop" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
            <div class="contentwrapper edit clearfix">
                <asp:Repeater id="RPTrevisions" runat="server">
                    <HeaderTemplate>
                        <table class="revisions table light fullwidth">
                            <thead>
                                <tr>
                                    <th class="submitternumber">
                                        <asp:literal ID="LTrevType_t" runat="server">Tipo</asp:literal>
                                        <asp:LinkButton ID="LNBorderByTypeUp" runat="server" cssclass="icon orderUp" CommandArgument="ByType.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByTypeDown" runat="server" cssclass="icon orderDown" CommandArgument="ByType.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="cfp" id="THcall" runat="server">
                                        <asp:literal ID="LTrevCallName_t" runat="server">Call For Paper</asp:literal>
                                        <asp:LinkButton ID="LNBorderByCallUp" runat="server" cssclass="icon orderUp" CommandArgument="ByCall.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByCallDown" runat="server" cssclass="icon orderDown" CommandArgument="ByCall.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="submittername" id="THsubmitter" runat="server">
                                        <asp:literal ID="LTrevRequiredBy_t" runat="server">Submitter</asp:literal>
                                        <asp:LinkButton ID="LNBorderByUserUp" runat="server" cssclass="icon orderUp" CommandArgument="ByUser.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByUserDown" runat="server" cssclass="icon orderDown" CommandArgument="ByUser.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="submittedon">
                                        <asp:literal ID="LTrevDate_t" runat="server">Date</asp:literal>
                                        <asp:LinkButton ID="LNBorderByDateUp" runat="server" cssclass="icon orderUp" CommandArgument="ByDate.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByDateDown" runat="server" cssclass="icon orderDown" CommandArgument="ByDate.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="submittedon">
                                        <asp:literal ID="LTrevDeadline_t" runat="server">Deadline</asp:literal>
                                        <asp:LinkButton ID="LNBorderByDeadlineUp" runat="server" cssclass="icon orderUp" CommandArgument="ByDeadline.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByDeadlineDown" runat="server" cssclass="icon orderDown" CommandArgument="ByDeadline.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="status">
                                        <asp:literal ID="LTrevStatus_t" runat="server">Status</asp:literal>
                                        <asp:LinkButton ID="LNBorderByStatusUp" runat="server" cssclass="icon orderUp" CommandArgument="ByStatus.True" CommandName="orderby"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBorderByStatusDown" runat="server" cssclass="icon orderDown" CommandArgument="ByStatus.False" CommandName="orderby"></asp:LinkButton>
                                    </th>
                                    <th class="actions"><asp:literal ID="LTrevActions_t" runat="server">Actions</asp:literal></th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                                <tr>
                                    <td class="submitternumber">
                                        <asp:Label ID="LBrevisionType" runat="server"></asp:Label>
                                    </td>
                                    <td class="cfp"><asp:literal ID="LTrevCallName" runat="server"/></td>
                                    <td class="submittername" id="THsubmitter" runat="server"><asp:literal ID="LTrevRequiredBy" runat="server"/></td>
                                    <td class="submittedon"><asp:literal ID="LTrevDate" runat="server"/></td>
                                    <td class="submittedon"><asp:literal ID="LTrevDeadline" runat="server"/></td>
                                    <td class="status warning">
                                        <span class="icons">
                                            <asp:Label ID="LBstatus" runat="server" CssClass="icon status revision"></asp:Label>
                                        </span>
                                        <asp:literal ID="LTrevStatus" runat="server"/>
                                    </td>
                                    <td class="actions">
                                        <asp:literal ID="LTemptyActions" runat="server" Text=" "/>
                                        <span class="icons">
                                            <asp:Button ID="BTNlistCancelRequest" runat="server" CssClass="icon delete needconfirm" Visible="false" CommandName="cancelrequest"/>
                                            <asp:HyperLink ID="HYPviewRevision" runat="server" CssClass="icon view" Visible="false"></asp:HyperLink>
                                        </span>
                                    </td>
                                </tr>
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
            <asp:Label id="LBnoRevisions" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>