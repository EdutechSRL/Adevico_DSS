<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepository.aspx.vb" Inherits="Comunita_OnLine.CommunityRepository" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RepositorySize" Src="~/Modules/Repository/UC/UC_CommunityFolderSize.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FolderPathSelector" Src="~/Modules/Repository/UC/UC_RepositoryFolderPathSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FileDetail" Src="~/Modules/Repository/UC/UC_FileDetail.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/UC/UC_DisplayRepositoryItem.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function showDialog(id) {
            //            if ($.browser.msie && id =='addField') {
            //                $('#' + id).dialog("option", "height", 870);
            //            }
            //            else if ($.browser.msie && id =='addRequestedFile') {
            //                $('#' + id).dialog("option", "height", 350);
            //            }

            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

        $(document).ready(function () {
            $('#detailFile').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 600,
                height: 400,
                minHeight: 400,
                minWidth: 600,
                zIndex: 99999,
                open: function (type, data) {
                   // $(this).parent().appendTo("form");
                }
            });
        });
    </script>
   <link href="Repository.Common.css" type="text/css" rel="stylesheet"/>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div> &nbsp;</div>
    <asp:UpdatePanel ID="UDPpath" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>

            <div style="width: 894px; padding-top: 5px; text-align: left; border-color: Black;
                border-style: solid; border-width: 2px; height: 30px;">
                <div style="float: left; padding-left: 4px; width: 794px;">
                    <CTRL:FolderPathSelector ID="CTRLPathSelector" runat="server" AjaxEnabled="true"
                        UrlNavigation="false" />
                    <input type="hidden" runat="server" id="HDview" />
                </div>
                <div style="float: left; padding-right: 4px; text-align: right; width: 90px;">
                    <asp:HyperLink ID="HYPlist" runat="server"></asp:HyperLink>
                    <asp:HyperLink ID="HYPfolders" runat="server"></asp:HyperLink>
                </div>
            </div>
            <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
                padding-bottom: 5px;">
                <div style="float: left; text-align: left; width: 420px">
                    &nbsp;
                </div>
                <div style="float: left; text-align: right; width: 480px;">
                    <span style="vertical-align: text-bottom;">
                        <asp:HyperLink ID="HYPadvancedManagement" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Advanced Management" Height="18px"></asp:HyperLink>
                        <asp:HyperLink ID="HYPmanagement" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Management" Height="18px"></asp:HyperLink>
                        <asp:HyperLink ID="HYPupload" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Upload" Height="18px"></asp:HyperLink>
                    </span>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="Progress" runat="server" DynamicLayout="False" Visible="true"
        AssociatedUpdatePanelID="UDPfiles">
        <ProgressTemplate>
            <div id="LoadingImage" style="clear: both;">
                <img src="../../images/Ajax/ajax-loader_1.gif" alt=""/>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="clear: both; text-align: center; padding-top: 5px;">
        <div style="float: left; text-align: right; width: 370px; border-color: Black; border-style: solid;
            border-width: 2px;" id="DVfolders" runat="server">
            <asp:UpdatePanel ID="UDPfolders" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="text-align: left; height: 30px; padding-left: 10px; padding-top: 3px;
                        background-color: #666; color: White; font-size: medium;">
                        <span style="vertical-align: middle;">
                            <asp:Label ID="LBfolderTitle" runat="server">Esplora</asp:Label>
                        </span>
                    </div>
                    <div style="width: 370px; text-align: left; overflow: auto; height: 500px;">
                        <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="370px" SelectionMode="Single"
                            AutoRedirect="true" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="float: left; width: 10px;" id="DVseparator"
            runat="server">
            &nbsp;
        </div>
        <div style="float: left; text-align: left;" id="DVfiles" class="FolderFiles" runat="server">
            <asp:UpdatePanel ID="UDPfiles" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <%--                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="CTRLCommunityFolder" />
                    <asp:AsyncPostBackTrigger ControlID="CBXshowDescriptionTop" />
                    <asp:AsyncPostBackTrigger ControlID="PGgridTop" />
                    <asp:AsyncPostBackTrigger ControlID="CBXshowDescriptionBottom" />
                    <asp:AsyncPostBackTrigger ControlID="PGgridBottom" />
                    <asp:AsyncPostBackTrigger ControlID="RPTfile" />
                </Triggers>--%>
                <ContentTemplate>
                    <div style="text-align: right; width: 100%">
                        <div style="text-align: left; float: left; width: 50%">
                            <asp:CheckBox ID="CBXshowDescriptionTop" runat="server" Text="Show description" AutoPostBack="true" />
                        </div>
                        <div style="text-align: right; float: left; width: 50%">
                            <CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false" ShowNavigationButton="false">
                            </CTRL:GridPager>
                        </div>
                    </div>
                    <div style="clear: both;">
                        <asp:Repeater ID="RPTfile" runat="server">
                            <HeaderTemplate>
                                <table border="1" cellpadding="0" cellspacing="0" id="TBfiles" width="100%">
                                    <tr class="ROW_header_Small_Center" style="height: 25px;">
                                        <td style="width: 20px;">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="LBname_t" runat="server">Name</asp:Label>
                                            <asp:LinkButton ID="LNBsortByName" CausesValidation="false" CommandName="sortName"
                                                runat="server"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label ID="LBdinfo_t" runat="server">Uploaded</asp:Label>
                                            <asp:LinkButton ID="LNBsortByDate" CausesValidation="false" CommandName="sortDate"
                                                runat="server"></asp:LinkButton>
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class='<%# me.BackGroundItem(Container.itemtype,Container.DataItem.File.isVisible)%>'>
                                    <td style="width: 20px; text-align: center;">
                                        <asp:LinkButton ID="LNBinfo" runat="server" CommandName="info" CausesValidation="false"></asp:LinkButton>
                                    </td>
                                    <td>
                                                <asp:LinkButton ID="LNBfolder" runat="server" Visible="false" CssClass="ROW_ItemLink_Small"
                                                    CommandName="updatefoldercontent"></asp:LinkButton>
                                                <CTRL:DisplayItem runat="server" ID="CTRLdisplayItem" IconSize="Small"></CTRL:DisplayItem>
                                    </td>
                                    <td>
                                        <span style="text-align: center;">
                                            <asp:Label ID="LBdata" runat="server"></asp:Label></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <div style="text-align: right; width: 100%;">
                        <div style="text-align: left; float: left; width: 50%;">
                            <asp:CheckBox ID="CBXshowDescriptionBottom" runat="server" Text="Show description"
                                AutoPostBack="true" />
                        </div>
                        <div style="text-align: right; float: left; width: 50%;">
                            <CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false" ShowNavigationButton="false">
                            </CTRL:GridPager>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="detailFile" style="display:none;">
        <asp:UpdatePanel ID="UDPdetails" UpdateMode="Conditional" ChildrenAsTriggers="true"
            runat="server">
            <ContentTemplate>
                <CTRL:FileDetail ID="CTRLfileDetail" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server">
    </asp:Timer>
</asp:Content>