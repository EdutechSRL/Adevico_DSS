<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DashboardTopBar.ascx.vb" Inherits="Comunita_OnLine.UC_DashboardTopBar" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLmessage" Src="~/Modules/Common/UC/UC_ActionMessage.ascx" %>
<%@ Register Src="~/Modules/Questionnaire/UC/UC_DashboardDaySurvey.ascx" TagName="DaySurvey" TagPrefix="CTRL" %>

<div class="messages" runat="server" id="CTRLmessages" visible="false">
    <CTRL:CTRLmessage runat="server" ID="CTRLmessage"  Visible="false" />
   <!-- <div class="user message">
        <div class="renderedtext">Home message</div>
        <a class="viewall">view messagge history</a>
    </div>-->
</div>
<CTRL:DaySurvey runat="server" ID="UCdaySurvey" IsActive="true"  />            
<asp:Literal ID="LTtemplateviewTile" Visible="false" runat="server"><span title="{0}" class="icon tile {2}">{1}</span></asp:Literal>
<asp:Literal ID="LTtemplateviewCombined" Visible="false" runat="server"><span title="{0}" class="icon combo {2}">{1}</span></asp:Literal>
<asp:Literal ID="LTtemplateviewList" Visible="false" runat="server"><span title="{0}" class="icon list {2}">{1}</span></asp:Literal>
<asp:Literal ID="LTcssActiveClass" Visible="false" runat="server">active</asp:Literal>
<asp:Literal ID="LTgroupItemsByTemplate" Visible="false" runat="server"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:Literal>
<div class="toolbar container_12 view clearfix" id="DVtoolbar" runat="server">
    <div class="viewstyle grid_4 alpha">
        <span class="viewnav icons" id="SPNviewSelector" runat="server">
            <asp:HyperLink ID="HYPgotoTileView" runat="server"></asp:HyperLink>
            <asp:HyperLink ID="HYPgotoCombinedView" runat="server"></asp:HyperLink>
            <asp:HyperLink ID="HYPgotoListView" runat="server"></asp:HyperLink>
        </span>
        <asp:Label ID="LBviewSelectorDescription" runat="server" CssClass="description">*Change view style</asp:Label>
    </div>
    <div class="grid_4 viewoptions">
        <div class="groupedselector" id="DVgroupedSelector" runat="server" visible="false">
            <asp:Label ID="LBgroupedSelectorDescription" runat="server" CssClass="description">*Group by: </asp:Label>
            <div class="selectorgroup">
                <asp:Label ID="LBgroupBySelected" runat="server" CssClass="selectorlabel"></asp:Label>
                <span class="selectoricon">&nbsp;</span>
            </div>
            <div class="selectormenu">
                <div class="selectorinner">
                    <div class="selectoritems">
                        <asp:Repeater ID="RPTgroupBy" runat="server">
                            <ItemTemplate>
                                <div class="selectoritem" id="DVitemGroupBy" runat="server" >
                                    <asp:LinkButton ID="LNBgroupItemsBy" runat="server"  CssClass="selectorlabel"/>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="search right grid_4 omega">
        <div class="input-group" id="DVsearch" runat="server">
            <asp:TextBox ID="TXBsearchByName" runat="server" CssClass="form-control"></asp:TextBox>
            <span class="input-group-btn">
                <asp:Button ID="BTNsearchByName" runat="server" CssClass="btn btn-default" Text="*Search" />
            </span>
        </div>
    </div>
</div>
<asp:Literal ID="LTcssClassActive" runat="server" Visible="false">active</asp:Literal>
<asp:Literal ID="LTcssClassGroupBy" runat="server" Visible="false">selectoritem</asp:Literal>