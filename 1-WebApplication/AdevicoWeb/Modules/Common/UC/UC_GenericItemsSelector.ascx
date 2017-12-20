<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GenericItemsSelector.ascx.vb" Inherits="Comunita_OnLine.UC_GenericItemsSelector" %>

<script language="javascript" type="text/javascript">
function toggleRow() {
    var $this = $(this);
    var $row = $this.parent().parent();
    
    if ($this.is(':checked')) {
        $row.addClass('on');
        $row.removeClass('off');
        var values = $this.attr('valueToDisable');
        if (values!=''){
            var array = values.split(',');
            for (var i = 0; i < array.length; i++) {

                if ($('#tr' + array[i]) != null) {
                    $('input:checkbox[value=' + array[i] + ']').attr('checked', false);
                    $('#tr' + array[i]).removeClass('on');
                    $('#tr' + array[i]).addClass('off');
                }
            }
        }
    }
    else
    {
        $row.addClass('off');
        $row.removeClass('on');
    }
}

$().ready(function () {
    //Assign the toggle
    $('#aTable').find(':checkbox').each(function () {
        $(this).click(toggleRow);
        if ($(this).is(':checked'))
            $(this).parent().parent().addClass('on');
        else
            $(this).parent().parent().addClass('off');
    });
});

</script>
<style type="text/css">
    .on
{
   
}

.off
{
    background-color:Gray;
}

.imported
{
    background-color: d3d3d3;
}
</style>
    <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
           <span class="Fieldrow">
                <br /><br /><br /><br />
                <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                <br /><br /><br /><br />
            </span>
        </asp:View>
        <asp:View ID="VIWselectItems" runat="server">
            <asp:Repeater ID="RPTinvalidItems" runat="server">
                <ItemTemplate>
                    <asp:Label ID="LBinvalidItem" runat="server" Text="<%#TranslateInvalidItem(Container.DataItem)%>"></asp:Label>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="RPTitems" runat="server">
                <ItemTemplate>
                    <table border="1" cellpadding="0" cellspacing="0" id="aTable">
                        <tr class="ROW_header_Small_Center">
                            <th><asp:literal ID="LTselectRow_t" runat="server"></asp:literal></th>
                            <th><asp:literal ID="LTrowInfo_t" runat="server"></asp:literal></th>
                            <asp:Repeater ID="RPTcolumns" runat="server" DataSource="<%#Container.DataItem.ColumHeader%>" OnItemDataBound="RPTcolumns_ItemDataBound">
                                <ItemTemplate>
                                    <th>
                                        <asp:literal ID="LTcolumn" runat="server"></asp:literal>
                                        <asp:literal ID="LTcolumnIndex" runat="server" Visible="false" Text="<%#Container.DataItem.Number%>"></asp:literal>
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                        <asp:Repeater ID="RPTitemRow" runat="server" DataSource="<%#Container.DataItem.Rows%>" OnItemDataBound="RPTitemRow_ItemDataBound">
                            <ItemTemplate>
                                <tr id="tr<%#Container.DataItem.Number%>" class="<%#isImported(Container.DataItem)%>" >
                                    <td>
                                        &nbsp;<input type="checkbox" runat="server" ID="CBXselect" class="Testo_Campo" />&nbsp;
                                        <asp:literal ID="LTrowIndex" runat="server" Visible="false" Text="<%#Container.DataItem.Number%>"></asp:literal>
                                    </td>
                                    <td><asp:Label ID="LBrowInfo" runat="server" CssClass="tooltip"></asp:Label></td>
                                    <asp:Repeater ID="RPTcells" runat="server" DataSource="<%#Container.DataItem.Cells%>" OnItemDataBound="RPTcells_ItemDataBound">
                                        <ItemTemplate>
                                        <td><asp:literal ID="LTitemInfo" runat="server"></asp:literal><asp:literal ID="LTitem" runat="server"></asp:literal><asp:literal ID="LTitemEmpty" runat="server" Visible="false">&nbsp;</asp:literal></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                       
                </FooterTemplate>
            </asp:Repeater>
        </asp:View>
    </asp:MultiView>
    <br /><br />