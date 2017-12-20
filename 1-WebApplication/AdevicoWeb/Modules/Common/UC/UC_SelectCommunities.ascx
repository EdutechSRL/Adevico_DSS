<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectCommunities.ascx.vb"
    Inherits="Comunita_OnLine.UC_SelectCommunities" %>
<%@ Register TagPrefix="CTRL" TagName="CommunitySelector" Src="~/Modules/Common/UC/UC_ModalCommunitySelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>

<style type="text/css">
   /* .Pager
    {
        width: 800px; text-align: right; padding-top: 5px; clear: both; height: 22px;
        }
     */   
     .FilterRow
    {
       text-align: left; padding-top: 4px; clear: both;
        
        }
      .FilterRowRight
     {
         text-align: right; padding-top: 4px; clear: both;
         }
</style>
<asp:MultiView ID="MLVcommunities" runat="server" ActiveViewIndex="1">
    <asp:View ID="VIWsessionTimeout" runat="server">
        <br /><br /><br />
        <asp:Label ID="LBsessionTimeout" runat="server"></asp:Label>
        <br /><br /><br />
    </asp:View>
    <asp:View ID="VIWlist" runat="server">
        <div class="CommunitiesTable">
            <div class="FilterRow">
                <asp:Label ID="LBinfo" runat="server"></asp:Label>
            </div>
            <div class="FilterRowRight">
                <asp:Button ID="BTNaddUp" runat="server" Text="Add" />
            </div>
            <asp:Repeater ID="RPTcommunities" runat="server">
                <HeaderTemplate>
                    <table class="table light fullwidth">
                        <tr >
                            <th style="width: 4%;">
                                <asp:Label ID="LBremoveCommunity_t" runat="server">S</asp:Label>
                            </th>
                            <th style="width: 60%;">
                                <asp:Label ID="LBcommunityName_t" runat="server">Name</asp:Label>
                            </th>
                            <th style="width: 25%;">
                                <asp:Label ID="LBcommunityType_t" runat="server">Type</asp:Label>
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 4%; vertical-align: middle; text-align: center;">
                            <span class="icons">
                                <asp:Button ID="BTNdelete" runat="server" CommandName="remove" CommandArgument="<%#Container.DataItem.Community.Id%>" CssClass="img_btn icon delete" />
                            </span>
                        </td>
                        <td style="width: 60%; padding-left: 5px;">
                            <asp:Label ID="LBcommunityName" runat="server" Text="<%#Container.DataItem.Community.Name%>"></asp:Label>
                        </td>
                        <td style="width: 25%; text-align: center;">
                            <asp:Label ID="LBcommunityType" runat="server"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="FilterRowRight">
                <asp:Button ID="BTNaddDown" runat="server" Text="Add" />
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWerrors" runat="server">
        <div style="padding-top: 180px; padding-bottom: 180px;">
            <asp:Label ID="LBerrors" runat="server"></asp:Label>
        </div>
    </asp:View>
</asp:MultiView>
<CTRL:CommunitySelector id="CTRLaddCommunity" runat="server" visible="false"  SelectionMode="Multiple"></CTRL:CommunitySelector>