<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Calls.aspx.vb" Inherits="Comunita_OnLine.ListCalls" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="COL" TagName="Dialog" Src="~/Modules/EduPath/UC/UCDialog.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" EnableScripts="false" />
    <script type="text/javascript">
        $(function () {
            $(".descriptionswitch").click(function () {
                $(this).parent(".description").toggleClass("expanded");
            })
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="viewbuttons clearfix">
        <asp:HyperLink ID="HYPlist" runat="server" Text="List calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPmanage" runat="server" Text="Manage calls" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPcreateCall" runat="server" Text="Add call" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
    </div>
    <div class="tabs clearfix">
         <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
         <telerik:radtabstrip id="TBScall" runat="server" align="Justify" width="100%"
            height="20px" causesvalidation="false" autopostback="false" skin="Outlook" enableembeddedskins="true">
            <tabs>
                <telerik:RadTab text="Revisioni" value="Revisions" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Bozze" value="Draft" visible="false"></telerik:RadTab>
                <telerik:RadTab text="In corso" value="SubmissionOpened" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Sottomessi" value="Submitted" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Conclusi" value="SubmissionClosed" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Da valutare" value="ToEvaluate" visible="false"></telerik:RadTab>
                <telerik:RadTab text="Valutati" value="Evaluated" visible="false"></telerik:RadTab>
            </tabs>
        </telerik:radtabstrip>
    </div>

    <div class="viewoptions clearfix" runat="server" id="DVfilters" visible="false">
        <br /><br /><br />
        <asp:Label ID="LBitemsVisibility_t" runat="server" CssClass="Titolo_campo">Visualizza:</asp:Label>
        <asp:RadioButtonList ID="RBLvisibility" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="Testo_Campo"
            AutoPostBack="true">
        </asp:RadioButtonList>
    </div>
   
    <asp:MultiView ID="MLVresults" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWlist" runat="server">
            <div class="pager" runat="server" id="DVpagerTop"  visible="false">
                <asp:literal ID="LTpageTop" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
            <div class="list">
                <asp:Repeater id="RPTcalls" runat="server">
                    <HeaderTemplate>
                        <ul class="cfps playmode">    
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="cfp clearfix" id="">
                            <div class="externalleft icons">
                               <%-- <span class="icon movecfp" title="Move CFP">&nbsp;</span>--%>&nbsp;
                            </div>
                            <div class="cfpcontainer">
                                <div class="cfpheader clearfix">
                                    <span class="left">
                                        <a name="<%#Container.DataItem.Id %>"></a>
                                        <span class="titlecont">
                                            <span class="icons">
                                                <asp:Label ID="LBlocked" CssClass="icon locked" runat="server" Visible="false">&nbsp;</asp:Label>
                                            </span>
                                            <span class="title">
                                                <asp:Literal ID="LTname" runat="server"></asp:Literal>
                                                <asp:Literal ID="LTedition" runat="server" Visible="false"></asp:Literal>
                                            </span>
                                            <span class="icons">
                                                <!--<span class="icon default">&nbsp;</span>-->
                                                <asp:Label ID="LBnewItem" CssClass="icon hasnew" runat="server" Visible="false">&nbsp;</asp:Label>
                                                <asp:Label ID="LBisAdvance" CssClass="icon special" runat="server" Visible="false">&nbsp;</asp:Label>
                                                <asp:Label ID="LBexpiringItem" CssClass="icon expiring" runat="server" Visible="false">&nbsp;</asp:Label>
                                            </span>
                                        </span>
                                    </span>
                                    <span class="right">
                                        <span class="icons">
                                            <col:dialoglinkbutton runat="server" id="LNBdelete" visible="false" CssClass="icon delete" causesvalidation="false" commandname="confirmDelete"  CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</col:dialoglinkbutton>
                                            <asp:LinkButton ID="LNBvirtualDelete" runat="server" Visible="false" CssClass="icon virtualdelete" CommandName="virtualdelete" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBrecover" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                            <asp:HyperLink ID="HYPedit" runat="server" CssClass="icon edit" Visible="false">&nbsp;</asp:HyperLink>
                                            <asp:HyperLink ID="HYPpreview" runat="server" CssClass="icon view" Visible="false" Target="_blank">&nbsp;</asp:HyperLink>
                                            <asp:LinkButton ID="LNBcloneCall" runat="server" Visible="false" CssClass="icon copy" CommandName="copy" CommandArgument='<%#Container.DataItem.Id %>'>&nbsp;</asp:LinkButton>
                                            <asp:HyperLink ID="HYPsubmissions" runat="server" CssClass="icon submission" Visible="false">&nbsp;</asp:HyperLink>
                                            <asp:HyperLink ID="HYPnotificationMail" runat="server" CssClass="icon mail" Visible="false">&nbsp;</asp:HyperLink>
                                            <asp:HyperLink ID="HYPcommitteeSettings" runat="server" CssClass="icon committeesettings" Visible="false">&nbsp;</asp:HyperLink>
                                            <asp:HyperLink ID="HYPevaluationsStatistics" runat="server" CssClass="icon stats" Visible="false">&nbsp;</asp:HyperLink>
                                            <asp:HyperLink ID="HYPevaluate" runat="server" CssClass="icon evaluate" Visible="false">&nbsp;</asp:HyperLink>
                                            
                                        </span>
                                        <!--<span class="resetstats">reset stats!</span>-->
                                    </span>
                                   <div class="clearfix">&nbsp;</div>
                                </div>
                                <div class="cfpdesc clearfix">
                                    <div class="description left">
                                        <div class="intro" id="DVsummary" runat="server">
                                            <div class="renderedtext">
                                                <asp:literal ID="LTintroCall" runat="server" ></asp:literal>
                                            </div>
                                        </div>
                                        <div class="body" id="DVbody" runat="server">
                                            <div class="renderedtext">
                                                <asp:literal ID="LTdescriptionCall" runat="server"></asp:literal>
                                            </div>
                                        </div>
                                        <asp:Label ID="LBdescriptionswitch" class="descriptionswitch" runat="server">&nbsp;</asp:Label>
                                    </div>
                                   <div class="right">
                                       &nbsp;
                                   </div>
                                </div>
                                <div class="cfpfooter clearfix">
                                    <div class="left">
                                        <div class="expiredate">
                                            <asp:Label ID="LBendDateTime_t" class="Label" runat="server">Scadenza:</asp:Label>
                                            <asp:Label ID="LBendDate" runat="server" class="cfpdetails"></asp:Label>
                                        </div>
                                        <div class="cfpstatus" runat="server" id="DVstatus">
                                            <asp:Label ID="LBstatus_t" class="Label" runat="server">Status:</asp:Label>
                                            <asp:Literal ID="LTstatus" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="right">
                                        <div class="evalsubmissions" runat="server" id="DVevaluation">
                                            <asp:Label ID="LBevaluation_t" class="Label" runat="server"></asp:Label>
                                            <asp:HyperLink ID="HYPevaluation" runat="server" CssClass="cfpdetails"></asp:HyperLink>
                                        </div>
                                        <div class="item">
                                        </div>
                                    </div>
                                </div>
                                <div class="clearer"></div>
                           </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                    </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="pager" runat="server" id="DVpagerBottom" visible="false">
                <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
        </asp:View>
        <asp:View ID="VIWnoItems" runat="server">
            <br /><br /><br /><br /><br /><br />
            <asp:Label id="LBnoCalls" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
    <col:dialog runat="server" id="DLGremoveItem" dialogclass="mandatoryDial" serversidecancel="false"></col:dialog>
</asp:Content>