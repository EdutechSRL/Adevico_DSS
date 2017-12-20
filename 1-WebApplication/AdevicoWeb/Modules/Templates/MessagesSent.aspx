<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="MessagesSent.aspx.vb" Inherits="Comunita_OnLine.MessagesSent" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessage.ascx" TagName="CTRLmailMessage" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessageHeader.ascx" TagName="CTRLmailMessageHeader" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>

    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.persist-min.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <CTRL:CTRLmailMessageHeader ID="CTRLmailMessageHeader" runat="server" />
    <script language="javascript" type="text/javascript">
        var ddbuttonlist = false;
        $(function () {
            $("table.treetable").treeTable({ clickableNodeNames: true, persist: true  });
            $(".ddbuttonlist.enabled").dropdownButtonList();
            ddbuttonlist = true;
        })

        $(document).ready(function () {
            $(".view-modal.view-previewmail").dialog({
                appendTo: "form",
                closeOnEscape: true,
                modal: true,
                width: 700,
                height: 450,
                minHeight: 200,
                minWidth: 400,
                open: function (type, data) {
                   // $(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWdefault" runat="server">
            <h3><asp:Literal ID="LTdefaultMessage" runat="server"></asp:Literal></h3>
        </asp:View>
        <asp:View ID="VIWcontent" runat="server">
            <div class="">
                <div class="viewbuttons clearfix" id="DVbackUrl" runat="server">
                    <asp:HyperLink ID="HYPbackUrl" runat="server" Text="*Back" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                </div>
                <div class="tabswrapper clearfix" id="DVtab" runat="server">
                    <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                     <telerik:radtabstrip id="TBSmessages" runat="server" align="Justify" width="100%"
                        height="20px" causesvalidation="false" autopostback="false" skin="Outlook" enableembeddedskins="true">
                        <tabs>
                            <telerik:RadTab text="*Templates" value="List" visible="false"></telerik:RadTab>
                            <telerik:RadTab text="*Send message" value="Send" visible="false"></telerik:RadTab>
                            <telerik:RadTab text="*Messages sent" value="Sent" visible="false"></telerik:RadTab>
                        </tabs>
                    </telerik:radtabstrip>
                </div>
                <div class="tabcontent sentmessages">
                    <div class="contentwrapper clearfix">
                        <div class="fieldobject title clearfix" id="DVtitle" runat="server" visible="false">
                            <div class="fieldrow inline left">
                                <label  for=""><asp:Literal ID="LTmessageSent_t" runat="server"></asp:Literal></label>
                            </div>
                            <div class="fieldrow inline right">

                            </div>
                        </div>
                        <div class="fieldobject filters container_12 clearfix">
                            <div class="fieldrow grid_8 alpha namefilter">
                                <asp:Label ID="LBsearchMessageBySubject_t" runat="server" AssociatedControlID="TXBsearchBy" Visible="false">Subject:</asp:Label>
                                <asp:Label ID="LBsearchUserByFilter_t" runat="server" AssociatedControlID="DDLsearchUserBy">Find by:</asp:Label>&nbsp;
                                <span class=""><!--inputgroup-->
                                    <asp:DropDownList ID="DDLsearchUserBy" runat="server" >
                                        <asp:ListItem Value="-1">Tutti</asp:ListItem>
                                        <asp:ListItem Value="1">Nome</asp:ListItem>
                                        <asp:ListItem Value="2" Selected="True">Cognome</asp:ListItem>
                                        <asp:ListItem Value="5">Mail</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TXBsearchBy" runat="server" CssClass="fieldinput"></asp:TextBox>
                                </span>
                            </div>
                            <div class="fieldrow grid_4 omega filtertemplates">
                                &nbsp;
                            </div>
                            <asp:MultiView ID="MLVfilters" runat="server">
                                <asp:View ID="VIWempty" runat="server"></asp:View>
                                <asp:View ID="VIWusers" runat="server">
                                    <div class="clear"></div>
                                    <div class="fieldrow grid_6 alpha rolefilter">
                                        <asp:Label ID="LBcommunityRole_t" runat="server" AssociatedControlID="DDLroles" Visible="false">Role:</asp:Label>&nbsp;
                                        <asp:DropDownList ID="DDLroles" runat="server" Visible="false" AutoPostBack="false">
                                        </asp:DropDownList>
                                        <asp:Label ID="LBprofileType_t" runat="server" AssociatedControlID="DDLprofileTypes" Visible="false">Role:</asp:Label>&nbsp;
                                        <asp:DropDownList ID="DDLprofileTypes" runat="server" Visible="false" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="fieldrow grid_6 omega agencyfilter">
                                        <asp:Label ID="LBcommunityAgencyFilter_t" runat="server" AssociatedControlID="DDLcommunityAgencies" Visible="false">Agency:</asp:Label>&nbsp;
                                        <asp:DropDownList ID="DDLcommunityAgencies" runat="server" Visible="false">
                                        </asp:DropDownList>
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                            <div class="clear"></div>
                            <div class="abcupdate clearfix">
                                <div class="fieldrow left abcfilters" runat="server" id="DVletters" visible="false">
                                    <CTRL:AlphabetSelector ID="CTRLalphabetSelector" runat="server" RaiseSelectionEvent="true">
                                    </CTRL:AlphabetSelector>
                                </div>
                                <div class="fieldrow right updatefilter clearfix">
                                    <asp:Button ID="BTNapplyFilters" runat="server" CssClass="linkMenu" Text="*Update" />
                                </div>
                            </div>
                        </div>
                        <div class="fieldobject clearfix">
                            <div class="fieldrow right">
                                <span class="btnswitchgroup"><!--
                                    --><asp:LinkButton ID="LNBfilterByUser" runat="server" CssClass="btnswitch first active" Text ="*By user"></asp:LinkButton><!--
                                    --><asp:LinkButton ID="LNBfilterByMessage" runat="server" CssClass="btnswitch last normal" Text ="*By message"></asp:LinkButton><!--
                                  --></span>
                            </div>
                        </div>
                        <div class="tablewrapper">
                            <asp:MultiView ID="MLVdata" runat="server">
                                <asp:View ID="VIWrecipients" runat="server">
                                    <asp:Repeater ID="RPTrecipients" runat="server">
                                        <HeaderTemplate>
                                        <table class="table treetable light fullwidth msglistusers">
                                            <thead>
                                                <tr>
                                                    <th class="name">
                                                        <asp:Literal ID="LTrecipientHeaderName_t" runat="server">*Name</asp:Literal>
                                                        <asp:LinkButton ID="LNBorderByUserUp" runat="server" cssclass="icon orderUp" CommandArgument="ByUser.True" CommandName="orderby"></asp:LinkButton>
                                                        <asp:LinkButton ID="LNBorderByUserDown" runat="server" cssclass="icon orderDown" CommandArgument="ByUser.False" CommandName="orderby"></asp:LinkButton>
                                                    </th>
                                                    <th id="THagencyName" class="agencyname" runat="server" visible='<%#me.isColumnVisible(0) %>'>
                                                        <asp:Literal ID="LTuserAgencyNameHeader_t" runat="server">*Agency</asp:Literal>
                                                        <asp:LinkButton ID="LNBorderByAgencyUp" runat="server" cssclass="icon orderUp" CommandArgument="ByAgency.True" CommandName="orderby"></asp:LinkButton>
                                                        <asp:LinkButton ID="LNBorderByAgencyDown" runat="server" cssclass="icon orderDown" CommandArgument="ByAgency.False" CommandName="orderby"></asp:LinkButton>
                                                    </th>
                                                    <th class="numberview">
                                                        <asp:Literal ID="LTmessageNumberHeaderName_t" runat="server">*Messages</asp:Literal>
                                                        <asp:LinkButton ID="LNBorderByMessageNumberUp" runat="server" cssclass="icon orderUp" CommandArgument="ByMessageNumber.True" CommandName="orderby"></asp:LinkButton>
                                                        <asp:LinkButton ID="LNBorderByMessageNumberDown" runat="server" cssclass="icon orderDown" CommandArgument="ByMessageNumber.False" CommandName="orderby"></asp:LinkButton>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="user" id="user-<%#Container.DataItem.Identifier %>" >
                                                <td class="name">
                                                    <asp:literal ID="LTdisplayName" runat="server"></asp:literal>
                                                    <asp:literal ID="LTidentifier" runat="server" Text='<%#Container.DataItem.Identifier %>' Visible="false"></asp:literal>
                                                </td>
                                                <td class="agencyname" id="TDagencyName" runat="server" visible='<%#me.isColumnVisible(0) %>'>
                                                    <%#Container.DataItem.AgencyName %>
                                                </td>   
                                                <td class="numberview">
                                                    <span class="number"><%#Container.DataItem.MessageNumber%></span>
                                                </td>
                                            </tr>
                                            <asp:Repeater ID="RPTrecipientMessages" runat="server" DataSource="<%#Container.DataItem.GetMessagesWithIdentifier() %>" OnItemDataBound="RPTrecipientMessages_ItemDataBound" OnItemCommand="RPTrecipientMessages_ItemCommand">
                                                <ItemTemplate>
                                                        <tr id="msg-<%#Container.DataItem.Message.Id %>-user-<%#Container.DataItem.RecipientIdentifier %>" class="message child-of-user-<%#Container.DataItem.RecipientIdentifier %>">
                                                        <td class="name">
                                                            <span class="message checklenght left">
                                                                <asp:Label ID="LBmessageSentName" runat="server" CssClass="messagename"></asp:Label>
                                                                <asp:Label ID="LBmessageSeparator" runat="server" CssClass="sep" Visible="false"></asp:Label>
                                                                <asp:Label ID="LBmessageTemplateName" runat="server" CssClass="template" Visible="false"></asp:Label>
                                                            </span>
                                                            <asp:Label ID="LBmessageSentOn" runat="server" CssClass="date right"></asp:Label>
                                                        </td>
                                                        <td class="numberview">
                                                            <span class="icons">
                                                                <asp:LinkButton ID="LNBdisplayUserMessage" runat="server" CssClass="icon infoalt" CommandName="display" ></asp:LinkButton>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                <tr id="TRempty" runat="server" visible="false">
                                                    <td id="TDemptyItems" runat="server">
                                                        <asp:Label ID="LBemptyItems" runat="server">*No items to select</asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </asp:View>
                                <asp:View ID="VIWmessages" runat="server">
                                    <asp:Repeater ID="RPTmessages" runat="server">
                                        <HeaderTemplate>
                                        <table class="table treetable light fullwidth msglistmessages treeTable">
                                        <thead>
                                            <tr>
                                                <th class="name">
                                                    <asp:Literal ID="LTmessageSentName_t" runat="server">*Name</asp:Literal>
                                                    <asp:LinkButton ID="LNBorderByNameUp" runat="server" cssclass="icon orderUp" CommandArgument="ByName.True" CommandName="orderby"></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBorderByNameDown" runat="server" cssclass="icon orderDown" CommandArgument="ByName.False" CommandName="orderby"></asp:LinkButton>
                                                </th>
                                                <th class="datecol">
                                                    <asp:Literal ID="LTmessageSentOn_t" runat="server">*Date</asp:Literal>
                                                    <asp:LinkButton ID="LNBorderByDateUp" runat="server" cssclass="icon orderUp" CommandArgument="ByDate.True" CommandName="orderby"></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBorderByDateDown" runat="server" cssclass="icon orderDown" CommandArgument="ByDate.False" CommandName="orderby"></asp:LinkButton>
                                                </th>
                                                <th class="actions">
                                                    <asp:Literal ID="LTmessageSentAction_t" runat="server">*Actions</asp:Literal>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <tr class="message initialized <%#MessageCssClass(Container.DataItem) %>">
                                            <td class="name">
                                                <span class="message">
                                                    <asp:Literal ID="LTidMessage" runat="server" Text='<%#Container.DataItem.Id %>' Visible="false"></asp:Literal>
                                                    <asp:Label ID="LBmessageSentName" runat="server" CssClass="name"></asp:Label>
                                                    <asp:Label ID="LBmessageSeparator" runat="server" CssClass="sep" Visible="false"></asp:Label>
                                                    <asp:Label ID="LBmessageTemplateName" runat="server" CssClass="template" Visible="false"></asp:Label>
                                                </span>
                                            </td>
                                            <td class="datecol">
                                                <asp:Label ID="LBmessageSentOn" runat="server" CssClass="date"></asp:Label>
                                            </td>
                                            <td class="actions">
                                                <span class="icons">
                                                    <asp:HyperLink ID="HYPviewMessage" runat="server" CssClass="icon view" Target="_blank"></asp:HyperLink>
                                                </span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                        <FooterTemplate>
                                            <tr id="TRempty" runat="server" visible="false">
                                                <td colspan="3">
                                                    <asp:Label ID="LBmessageSentEmptyItems" runat="server">*No message found.</asp:Label>
                                                </td>
                                            </tr>
                                            </tbody>
                                        </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                        <div class="pager">
                            <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false"  Visible="false"></CTRL:GridPager>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="view-modal view-previewmail" id="DVpreview" runat="server" visible="false">
        <CTRL:CTRLmailMessage ID="CTRLmailpreview" runat="server" AllowSendMail="false" DisplayTopWindowCloseButton="false" EditAddressTo="false"  />
        <div class="fieldobject clearfix">
            <div class="fieldrow right">
                <asp:Button ID="BTNcloseMailMessageWindow" runat="server" CssClass="Link_Menu" />
            </div>
        </div>
    </div>
</asp:Content>