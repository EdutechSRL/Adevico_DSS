<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RepositorySize.ascx.vb" Inherits="Comunita_OnLine.UC_RepositorySize" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLbar" Src="~/Modules/Common/UC/UC_StackedBar.ascx" %>
<div class="freespace">
    <CTRL:CTRLbar id="CTRLprogressBar" runat="server" />
    <asp:Repeater ID="RPTfolders" runat="server">
        <ItemTemplate>
            <div class="directoryinfo<%#ItemCssClass(Container.DataItem)%>">
                <span class="info title">
                    <asp:Label ID="LBfolderDetailsTitle" runat="server" CssClass="text"></asp:Label>
                </span>
                <asp:Repeater ID="RPTdetails" runat="server" DataSource="<%#Container.DataItem.Items%>"  OnItemDataBound="RPTchildren_ItemDataBound">
                    <ItemTemplate>
                        <span class="info">
                            <asp:Label ID="LBtext" runat="server" CssClass="text"></asp:Label>
                            <asp:Label ID="LBnumber" runat="server" CssClass="number"></asp:Label>
                        </span>
                    </ItemTemplate>
                </asp:Repeater>                           
            </div>
        </ItemTemplate>
    </asp:Repeater><asp:Literal ID="LTtotalCssClass" runat="server" Visible="false">total</asp:Literal><asp:Literal ID="LTcurrentCssClass" runat="server" Visible="false">current</asp:Literal><asp:Literal ID="LTtemplateDisplayPercentage" runat="server" Visible="false">{0}% ({1})</asp:Literal>
</div>

<%--<span class="progressbar"><span class="busy" title="busy 50%">&nbsp;</span><span class="available" title="available 50%">&nbsp;</span></span> --%>