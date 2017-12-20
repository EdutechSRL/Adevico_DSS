<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="MessageRecipients.aspx.vb" Inherits="Comunita_OnLine.MessageRecipients" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
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
    <CTRL:CTRLmailMessageHeader ID="CTRLmailMessageHeader" runat="server" />
    <script language="javascript" type="text/javascript">
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
    <div id="content">
        <div class="contentouter">
            <div class="content">
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWdefault" runat="server">
            <h3><asp:Literal ID="LTdefaultMessage" runat="server"></asp:Literal></h3>
        </asp:View>
        <asp:View ID="VIWcontent" runat="server">
            <div class="">
                <div class="sentmessages">
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
                        <div class="tablewrapper">
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
                                            <th class="actions">
                                                <asp:Literal ID="LTmessageRecipientActionsHeader_t" runat="server">*Actions</asp:Literal>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="user">
                                        <td class="name">
                                            <asp:literal ID="LTdisplayName" runat="server"></asp:literal>
                                        </td>
                                        <td class="agencyname" id="TDagencyName" runat="server" visible='<%#me.isColumnVisible(0) %>'>
                                            <%#Container.DataItem.AgencyName %>
                                        </td>   
                                        <td class="actions">
                                            <span class="icons">
                                                <asp:LinkButton ID="LNBdisplayUserMessage" runat="server" CssClass="icon infoalt" CommandName="display" ></asp:LinkButton>
                                            </span>
                                        </td>
                                    </tr>
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
            </div>
        </div>
    </div>
</asp:Content>