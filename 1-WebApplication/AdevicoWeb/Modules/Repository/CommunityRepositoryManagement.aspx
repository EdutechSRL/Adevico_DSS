<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryManagement.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryManagement" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RepositorySize" Src="~/Modules/Repository/UC/UC_CommunityFolderSize.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FolderPathSelector" Src="~/Modules/Repository/UC/UC_RepositoryFolderPathSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FileDetail" Src="~/Modules/Repository/UC/UC_FileDetail.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/UC/UC_DisplayRepositoryItem.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
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

            $('#selectFolder').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 610,
                height: 400,
                minHeight: 400,
                minWidth: 610,
                zIndex: 99999,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });
        });

        function showDialog(id) {
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
    </script>
    <link href="Repository.Common.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div>
        &nbsp;</div>
    <div>
        <div style="float: left; padding-left: 4px; width: 684px;">
            <CTRL:RepositorySize ID="CTRLrepositorySize" runat="server" EnabledInit="false" />
        </div>
        <div style="float: left; padding-right: 4px; text-align: right; width: 200px;">
            <asp:HyperLink ID="HYPbackToDownloads" runat="server" CssClass="Link_Menu" Visible="false"
                Text="Back to download" Height="18px"></asp:HyperLink>
            <asp:HyperLink ID="HYPadvancedManagement" runat="server" CssClass="Link_Menu" Visible="false"
                Text="Advanced Management" Height="18px"></asp:HyperLink>
        </div>
    </div>
    <asp:UpdatePanel ID="UDPpath" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 894px; padding-top: 5px; text-align: left; border-color: Black;
                border-style: solid; border-width: 2px; height: 30px; clear: both;">
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
                <div style="float: left; text-align: left; width: 210px">
                    &nbsp;
                </div>
                <div style="float: left; text-align: right; width: 680px;">
                    <span style="vertical-align: text-bottom;">
                        <asp:HyperLink ID="HYPmultipleDelete" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Multiple delete" Height="18px"></asp:HyperLink>
                        <asp:HyperLink ID="HYPimport" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Import" Height="18px"></asp:HyperLink>
                        &nbsp;
                        <asp:HyperLink ID="HYPaddFolder" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="New Folder" Height="18px"></asp:HyperLink>
                        <asp:HyperLink ID="HYPupload" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Upload" Height="18px"></asp:HyperLink>
                        <asp:HyperLink ID="HYPmultipUpload" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Multiple Upload" Height="18px"></asp:HyperLink>
                        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Link" ID="LNBhideAll" CausesValidation="false"></asp:LinkButton>
                    </span>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="Progress" runat="server" DynamicLayout="False" Visible="true"
        AssociatedUpdatePanelID="UDPfiles" DisplayAfter="1000">
        <ProgressTemplate>
            <div id="progressBackgroundFilter">
            </div>
            <div id="processMessage">
                <%-- <div id="imgdivLoading" align="center" valign="middle" runat="server" style="border-style: dotted;
                            padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;
                            border-color: #000066 black black black; border-width: medium; background-color: Gray; width: 900px;">--%>
                <asp:Literal ID="LTprogressInfo" runat="server"></asp:Literal><br />
                <asp:Image ID="imgLoading" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
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
                        <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="420px" SelectionMode="Single"
                            AjaxEnabled="false"  AutoRedirect="true"/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="float: left; width: 10px;" id="DVseparator" runat="server">
            &nbsp;
        </div>
        <div style="float: left; text-align: left;" id="DVfiles" class="FolderFiles" runat="server">
            <asp:UpdatePanel ID="UDPfiles" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
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
                                        <td style="width: 60px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 20px;">
                                            <asp:Label ID="LBpermission_t" runat="server">P</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="LBname_t" runat="server">Name</asp:Label>
                                            <asp:LinkButton ID="LNBsortByName" CausesValidation="false" CommandName="sortName"
                                                runat="server"></asp:LinkButton>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:Label ID="LBdinfo_t" runat="server">Uploaded</asp:Label>
                                            <asp:LinkButton ID="LNBsortByDate" CausesValidation="false" CommandName="sortDate"
                                                runat="server"></asp:LinkButton>
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class='<%# me.BackGroundItem(Container.itemtype,Container.DataItem.File.isVisible)%>'>
                                    <td style="width: 60px; text-align: center;">
                                        <asp:LinkButton ID="LNBinfo" runat="server" CommandName="info" CausesValidation="false"></asp:LinkButton>
                                        <asp:HyperLink ID="HYPedit" runat="server" Target="_self"></asp:HyperLink>
                                        <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CausesValidation="false"></asp:LinkButton>
                                        <br />
                                    </td>
                                    <td style="width: 20px; text-align: center;">
                                        &nbsp;<asp:HyperLink ID="HYPeditPermission" runat="server" Target="_self"></asp:HyperLink>&nbsp;
                                    </td>
                                    <td>
                                        <CTRL:DisplayItem runat="server" ID="CTRLdisplayItem" IconSize="Small"></CTRL:DisplayItem>
                                        <%--<div>
                                            <div style="float: left; width: 35px;">
                                                &nbsp;<asp:Literal ID="LTfileImage" runat="server">
                                                </asp:Literal>
                                            </div>
                                            <div style="float: left;">
                                                <asp:Label ID="LBnomeFile" runat="server"></asp:Label>
                                                <asp:HyperLink runat="server" ID="HYPfile" CssClass="ROW_ItemLink_Small" Visible="False"
                                                    Target="_blank"></asp:HyperLink>
                                                <asp:LinkButton ID="LNBfolder" runat="server" Visible="false" CssClass="ROW_ItemLink_Small"
                                                    CommandName="updatefoldercontent"></asp:LinkButton>
                                                <asp:Label ID="LBdimensione" runat="server"></asp:Label>&nbsp;
                                                <asp:HyperLink runat="server" ID="HYPdownload" Target="_blank" CssClass="ROW_ItemLink_Small"
                                                    Visible="False">Download</asp:HyperLink>
                                                <asp:Label ID="LBscaricamenti" runat="server" Visible="False"></asp:Label>&nbsp;
                                                <asp:ImageButton ID="IMBcontenutoAttivo" runat="server" Visible="false" CausesValidation="false"
                                                    CommandName="play" />
                                                <asp:HyperLink runat="server" ID="HYPcontenutoAttivo" CssClass="ROW_ItemLink_Small"
                                                    Visible="false"></asp:HyperLink>
                                                <asp:HyperLink ID="HYPstatistics" runat="server" Target="_self" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
                                            </div>
                                        </div>
                                        <div id="DVdescription" runat="server">
                                            <div style="float: left; width: 35px;">
                                                &nbsp;</div>
                                            <div style="float: left;">
                                                <asp:Label ID="LBdescrizioneFile" runat="server" CssClass="descrizioneFile" />
                                            </div>
                                        </div>--%>
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:Label ID="LBowner" runat="server"></asp:Label>
                                        <br />
                                        <span style="text-align: center;">
                                            <asp:Label ID="LBdata" runat="server"></asp:Label></span>
                                        <br />
                                        <asp:LinkButton ID="LNBmoveTo" runat="server" CommandName="moveto" CausesValidation="false"
                                            CssClass="ROW_ItemLink_Small"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBhide" runat="server" CommandName="editvisibility" CausesValidation="false"
                                            CssClass="ROW_ItemLink_Small"></asp:LinkButton>&nbsp;
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
    <div id="detailFile" style="display: none;">
        <asp:UpdatePanel ID="UDPdetails" UpdateMode="Conditional" ChildrenAsTriggers="true"
            runat="server">
            <ContentTemplate>
                <CTRL:FileDetail ID="CTRLfileDetail" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="selectFolder" style="display: none;">
        <asp:UpdatePanel ID="UDPselectFolder" UpdateMode="Conditional" ChildrenAsTriggers="true"
            runat="server">
            <ContentTemplate>
                <asp:MultiView ID="MLVselector" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWfolderSelector" runat="server">
                        <CTRL:CommunityFolder ID="CTRLselectDestinationFolder" runat="server" width="420px"
                            SelectionMode="Single" AjaxEnabled="true" />
                    </asp:View>
                    <asp:View ID="VIWerrorSelector" runat="server">
                        <div>
                            <div style="padding-bottom: 40px;">
                                <asp:Label ID="LBerrorNotification" runat="server"></asp:Label>
                            </div>
                            <div>
                                <div style="float: left; text-align: left; width: 50%">
                                    <asp:Button ID="BTNbackToFolder" runat="server" Text="Change folder" />
                                </div>
                                <div style="float: left; text-align: right; width: 50%">
                                    <asp:Button ID="BTNclose" runat="server" Text="Close" />
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="VIWconfirmMoveTo" runat="server">
                        <div>
                            <div style="padding-bottom: 40px;">
                                <asp:Label ID="LBconfirmMoveTo" runat="server"></asp:Label>
                            </div>
                            <div>
                                <div style="float: left; text-align: left; width: 50%">
                                    <asp:Button ID="BTNselectOtherFolder" runat="server" Text="Change folder" />
                                </div>
                                <div style="float: left; text-align: right; width: 50%">
                                    <asp:Button ID="BTNconfirmFolder" runat="server" Text="Close" />
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
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
