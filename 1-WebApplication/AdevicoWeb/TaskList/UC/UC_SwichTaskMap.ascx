<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SwichTaskMap.ascx.vb"
    Inherits="Comunita_OnLine.UC_SwichTaskMap" %>
<style type="text/css">
    .CellMove
    {
        width: auto;
    }
        .CellDeadline
    {
        width: 90px;
    }
            .CellTaskName
    {
        width: 650px;
    }

</style>

<script type="text/javascript" language="javascript">
    $(document).ready(function() {
        $(".LevelHider").change(function() {
            var name = $(this).attr("name");
            var value = $(this).val();

            $("#" + name + " tr").each(function() {
                var cur = $(this).attr("title");
                if (cur > value) {
                    $(this).hide();
                } else {
                    $(this).show();
                }

            });
        });
    });
</script>

<div style="height: 24px;">
    <div style="text-align: left; padding:5px;float: left; width: 100px">
        <asp:Label ID="LBwbsLevel" runat="server" CssClass="Titolo_campoSmall">WBS Level:</asp:Label>
    </div>
    <div style="text-align: left;padding:5px;">
        <select class="LevelHider" name="tableMap">
            <asp:Literal ID="LTddl" runat="server"></asp:Literal>
        </select>
    </div>
</div>
<br />
<div>
    <asp:Repeater ID="RPlistOfTask" runat="server" EnableViewState="false" >
        <HeaderTemplate>
            <table id="tableMap" border="1"  cellspacing="0" >
                <tr class="ROW_header_Small_Center">
                    <td class="CellMove">
                    </td>
                    <td class="CellTaskName">
                        <asp:Label ID="LBtaskNameTitle" runat="server">TASK</asp:Label>
                    </td>
                    <td class="CellDeadline">
                        <asp:Label ID="LBdeadlineTitle" runat="server">DEADLINE</asp:Label>
                    </td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr title="<%#(Container.DataItem).Level %>" class="ROW_Normal_Small">
                <td class="CellMove">
                    <asp:Literal ID="LTspaceButton" runat="server"></asp:Literal>
                    <asp:LinkButton ID="LNBup" runat="server" CommandName="up" CausesValidation="false"></asp:LinkButton>
                    <asp:Image ID="IMup" runat="server" />
                    <asp:LinkButton ID="LNBdown" runat="server" CommandName="down" CausesValidation="false"></asp:LinkButton>
                    <asp:Image ID="IMdown" runat="server" />
                </td>
                <td class="CellTaskName">
                    <asp:Literal ID="LTspaceWBS" runat="server"></asp:Literal>
                    <asp:Label ID="LBwbs" runat="server"></asp:Label>
                    <asp:HyperLink ID="HYPtaskName" runat="server" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
                </td>
                <td class="CellDeadline">
                    <asp:Label ID="LBdeadline" runat="server"></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr title="<%#(Container.DataItem).Level %>" class="ROW_Alternate_Small">
                <td class="CellMove">
                    <asp:Literal ID="LTspaceButton" runat="server"></asp:Literal>
                    <asp:LinkButton ID="LNBup" runat="server" CommandName="up" CausesValidation="false"></asp:LinkButton>
                    <asp:Image ID="IMup" runat="server" />
                    <asp:LinkButton ID="LNBdown" runat="server" CommandName="down" CausesValidation="false"></asp:LinkButton>
                    <asp:Image ID="IMdown" runat="server" />
                </td>
                <td class="CellTaskName">
                    <asp:Literal ID="LTspaceWBS" runat="server"></asp:Literal>
                    <asp:Label ID="LBwbs" runat="server"></asp:Label>
                    <asp:HyperLink ID="HYPtaskName" runat="server" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
                </td>
                <td class="CellDeadline">
                    <asp:Label ID="LBdeadline" runat="server"></asp:Label>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
