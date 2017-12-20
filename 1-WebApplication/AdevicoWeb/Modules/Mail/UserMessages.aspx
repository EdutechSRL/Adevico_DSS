<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="UserMessages.aspx.vb" Inherits="Comunita_OnLine.UserMessages" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessage.ascx" TagName="CTRLmailMessage" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessageHeader.ascx" TagName="CTRLmailMessageHeader" TagPrefix="CTRL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
   <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
    <CTRL:CTRLmailMessageHeader ID="CTRLmailMessageHeader" runat="server" />
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $(".view-modal.view-previewmail").dialog({
            appendTo: "form",
            closeOnEscape: true,
            modal: true,
            width: 800,
            height: 550,
            minHeight: 200,
            minWidth: 400,
            open: function (type, data) {
                //$(this).parent().appendTo("form");
                //$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="content">
        <div class="messagepopupcontent">
            <div class="fieldobject title clearfix" id="DVtitle" runat="server" visible="true">
                <div class="fieldrow inline left">
                    <label  for=""><asp:Literal ID="LTuserInfo" runat="server"></asp:Literal></label>
                </div>
                <div class="fieldrow inline right">
                </div>
            </div>
            <div class="tablewrapper">
                <table class="table treetable light fullwidth msglistmessages treeTable">
                    <thead>
                        <tr>
                            <th class="name">
                                <asp:Literal ID="LTmessageSentSubject_t" runat="server">*Name</asp:Literal>
                            </th>
                            <th class="datecol">
                                <asp:Literal ID="LTmessageSentOn_t" runat="server">*Date</asp:Literal>
                            </th>
                             <th class="sentby">
                                <asp:Literal ID="LTmessageSentBy_t" runat="server">*By</asp:Literal>
                            </th>
                            <th class="actions">
                                <asp:Literal ID="LTmessageSentAction_t" runat="server">*Actions</asp:Literal>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    <asp:Repeater id="RPTmessages" runat="server">
                        <ItemTemplate>
                        <tr class="message initialized">
                            <td class="name">
                                <span class="message">
                                    <asp:Literal ID="LTidMessage" runat="server" Text='<%#Container.DataItem.Id %>' Visible="false"></asp:Literal>
                                    <asp:Label ID="LBmessageSubject" runat="server" CssClass="name checklenght" Text='<%#Container.DataItem.DisplayName %>'></asp:Label>
                                </span>
                            </td>
                            <td class="datecol">
                                <asp:Label ID="LBmessageSentOn" runat="server" CssClass="date"></asp:Label>
                            </td>
                             <td class="sentby">
                                <asp:Label ID="LTmessageSentBy_t" runat="server" Text='<%#Container.DataItem.CreatedBy %>' ></asp:Label>
                            </td>
                            <td class="actions">
                                <span class="icons">
                                    <asp:LinkButton ID="LNBdisplayMessage" runat="server" CssClass="icon view" CommandArgument='<%#Container.DataItem.Id %>' CommandName="display"></asp:LinkButton>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td colspan="4">
                                <asp:Label ID="LBmessageSentEmptyItems" runat="server">*No message found.</asp:Label>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
                    </tbody>
                </table>
                <div class="pager" runat="server" id="DVpagerBottom" visible="false">
                    <asp:literal ID="LTpageBottom" runat="server">Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
                </div>  
            </div>
            <div class="view-modal view-previewmail" id="DVpreview" runat="server" visible="false">
                <CTRL:CTRLmailMessage ID="CTRLmailpreview" runat="server" AllowSendMail="false" DisplayTopWindowCloseButton="false" EditAddressTo="false"  />
                <div class="fieldobject clearfix">
                    <div class="fieldrow right">
                        <asp:Button ID="BTNcloseMailMessageWindow" runat="server" CssClass="Link_Menu"  Visible="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>