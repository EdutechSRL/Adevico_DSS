<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ItemVersions.ascx.vb" Inherits="Comunita_OnLine.UC_ItemVersions" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsectionOpener" Src="~/Modules/Common/UC/UC_ExpandAndCollapse.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaddVersion" Src="~/Modules/Repository/UC_New/UC_AddVersion.ascx" %>
<div class="section box versioning collapsable grid_12">
    <div class="sectioninner">
        <div class="cisectionheader clearfix">
            <div class="left">
                <h3><asp:Literal ID="LTversionSection" runat="server">*Versions</asp:Literal>
                     <CTRL:CTRLsectionOpener ID="CTRLopenDescription" runat="server" />
                </h3>
                <a name="action_manageVersions" href="#"></a><a name="action_removeVersion" href="#"></a><a name="action_addVersion" href="#"></a>
            </div>
            <div class="right hideme">
                <asp:Button ID="BTNaddVersionToItem" runat="server" Text="*Add new version" Visible="false"  CssClass="openitemversiondlg"/>
            </div>
        </div>
        <div class="cisectioncontent">
            <div class="tablewrapper">
                <table class="table minimal versions fullwidth">
                    <thead>
                        <tr>
                            <th class="name"><asp:Literal ID="LTversionName_t" runat="server" Text="*Name"></asp:Literal></th>
                            <th class="size" id="THsize" runat="server" visible="false"><asp:Literal ID="LTversionSize_t" runat="server" Text="*Size"></asp:Literal></th>
                            <th class="date"><asp:Literal ID="LTversionDate_t" runat="server" Text="*Date"></asp:Literal></th>
                            <th class="author"><asp:Literal ID="LTversionAuthor_t" runat="server" Text="*Author"></asp:Literal></th>
                            <th class="actions" id="THactions" runat="server" visible="false" >
                                <span class="icons"><span class="icon actions"></span></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="RPTversions" runat="server">
                            <ItemTemplate>
                            <tr class="version<%#IIf(Container.DataItem.IsActive, " current", IIf(Container.DataItem.IsDeleted, " deleted", ""))%>">
                                <td class="name">
                                    <asp:Label ID="LBversionName" runat="server" CssClass="filename"></asp:Label>
                                    <asp:HyperLink ID="HYPversionItem" runat="server"  Visible="false"></asp:HyperLink>
                                    <span>
                                        <asp:HyperLink ID="HYPdownloadItem" runat="server" CssClass="commandlight" Visible="false"></asp:HyperLink>
                                        <asp:Literal ID="LTseparatorItem" runat="server" Visible="false">|</asp:Literal>
                                        <asp:HyperLink ID="HYPplayItem" runat="server" CssClass="commandlight" Visible="false" Target="_blank"></asp:HyperLink>
                                    </span>
                                </td>
                                <td class="size" id="TDsize" runat="server" visible="false" >
                                    <asp:Label ID="LBfileSize" runat="server" CssClass="filesize"></asp:Label>
                                </td>
                                <td class="date"><asp:Label ID="LBdate" runat="server"></asp:Label></td>
                                <td class="author"><asp:Label ID="LBautor" runat="server"></asp:Label></td>
                                <td class="actions" id="TDactions" runat="server" visible="false" >
                                    <span class="icons">
                                        <asp:Button ID="BTNpromoteItemVersion" runat="server" CommandName="promote" CommandArgument="<%#Container.DataItem.Id %>" CssClass="icon promote" Visible="false" />
                                    </span>
                                </td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>                       
                    </tbody>
                </table>
            </div>
        </div>
        <div class="cisectionfooter"></div>
    </div>
</div>
<CTRL:CTRLaddVersion id="CTRLaddVersion" runat="server" visible="false" /><asp:Literal ID="LTmodalCssClass" runat="server" Visible="false">openmodal</asp:Literal>