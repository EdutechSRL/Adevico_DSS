<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectRepositoryItemsTableMode.ascx.vb" Inherits="Comunita_OnLine.UC_SelectRepositoryItemsTableMode" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>

<div class="pager clearfix" id="DVpagerTop" runat="server" visible="false">
    <div class="left" id="DVpageSizeTop" runat="server" visible="false">
        <asp:Label ID="LBrecordsForPageTop_t" runat="server" AssociatedControlID="DDLpageSizeTop">*Records for page:</asp:Label>
        <asp:DropDownList ID="DDLpageSizeTop" runat="server" AutoPostBack="false" />
    </div>
    <div class="right"></div>
</div>
<div class="tablewrapper">
    <asp:Repeater ID="RPTrepositoryItems" runat="server">
        <HeaderTemplate>
            <table class="table light fullwidth fileselectortable">
                <thead>
                    <tr id="sub-hdr">
                        <th class="checktttt" id="THmultiSelect" runat="server">
                            <span class="headercheckbox leftside" id="SPNtopSelector" runat="server" >
                                <input type="checkbox" class="checkall" />
                                <div class="selectorpopup checkall" id="DVselectorAll" runat="server">
                                    <div class="inner">
                                        &nbsp;<asp:LinkButton ID="LNBselectItemsIntoAllPages" runat="server" Text="*Seleziona su tutte le pagine" CommandName="all"></asp:LinkButton>
                                    </div>
                                    <div class="bottomarrow">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="selectorpopup checknone" id="DVselectorNone" runat="server">
                                    <div class="inner">
                                        &nbsp;<asp:LinkButton ID="LNBunselectItemsIntoAllPages" runat="server" Text="*Deseleziona su tutte le pagine" CommandName="none"></asp:LinkButton>
                                    </div>
                                    <div class="bottomarrow">
                                        &nbsp;
                                    </div>
                                </div>
                            </span>
                        </th>
                        <th class="select" id="THsingleSelect" runat="server">
                            <asp:Literal ID="LTsingleSelectHeader" runat="server">&nbsp;</asp:Literal>
                        </th>
                        <th class="filename">
                            <asp:Label ID="LBitemNameHeader_t" runat="server">*File name</asp:Label>
                        </th>
                        <th class="plays">
                            <asp:Label ID="LBitemStatisticsHeader_t" runat="server">*Plays</asp:Label>
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="check">
                    <span class="fileitemcheckbox">
                        <input type="checkbox" id="CBXrepositoryItem" runat="server" />
                    </span>
                    <asp:RadioButton ID="RBselect" runat="server" GroupName="selectsingleradio" visible="false" />
                    <asp:Literal ID="LTidItem" runat="server" Visible="false" Text='<%#Container.DataItem.Id%>'></asp:Literal>
                    <asp:Literal ID="LTitem" runat="server" Visible="false" Text='<%#Container.DataItem.Id & "|" & Container.DataItem.UniqueId.ToString & "|" & Container.DataItem.IdVersion & "|" & Container.DataItem.UniqueIdVersion.ToString & "|" & Container.DataItem.IgnoreVersion.ToString%>'></asp:Literal>
                </td>
                <td class="filename">
                   <asp:Literal ID="LTfileName" runat="server"></asp:Literal>
                   <div class="extrainfo path" id="DVpath" runat="server" visible="false"><asp:Literal ID="LTitemPath" runat="server" Text="*Original path"></asp:Literal></div>
                </td>
                <td class="plays">
                    <asp:Label ID="LBitemStatistics" runat="server"></asp:Label>
                    <asp:Literal ID="LTstatDownloads" runat="server" Visible="false" Text='<%#Container.DataItem.Downloads%>'></asp:Literal>
                    <asp:Literal ID="LTstatMyDownloads" runat="server" Visible="false" Text='<%#Container.DataItem.MyDownloads%>'></asp:Literal>
                    <asp:Literal ID="LTstatPlays" runat="server" Visible="false" Text='<%#Container.DataItem.Plays%>'></asp:Literal>
                    <asp:Literal ID="LTstatMyPlays" runat="server" Visible="false" Text='<%#Container.DataItem.MyPlays%>'></asp:Literal>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr id="TRempty" runat="server" visible="false">
                <td id="TDemptyItems" runat="server" colspan="3">
                    <asp:Label ID="LBrepositoryItemsEmpty" runat="server">S</asp:Label>
                </td>
            </tr>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
<div class="pager clearfix">
    <div class="left" id="DVpageSizeBottom" runat="server" visible="false">
        <asp:Label ID="LBrecordsForPage_t" runat="server" AssociatedControlID="DDLpageSizeBottom">*Records for page:</asp:Label>
        <asp:DropDownList ID="DDLpageSizeBottom" runat="server" AutoPostBack="true"></asp:DropDownList>
    </div>
    <div class="right">
        <CTRL:GridPager ID="PGgridBottom" runat="server" ShowNavigationButton="false" EnableQueryString="false" Visible="false"></CTRL:GridPager>
    </div>
</div>
<asp:Literal ID="LTtemplateFile" runat="server"  Visible="false"><span class="iteminfo notdot"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>