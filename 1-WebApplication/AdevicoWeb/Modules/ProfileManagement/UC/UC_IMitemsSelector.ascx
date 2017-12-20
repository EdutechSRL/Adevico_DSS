<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMitemsSelector.ascx.vb" Inherits="Comunita_OnLine.UC_IMitemsSelector" %>
<%@ Register TagPrefix="CTRL" TagName="ToolTip" Src="./../../Common/UC/UCtoolTip.ascx" %>
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
<div class="StepData IMitemSelector">
    <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
           <span class="Fieldrow">
                <br /><br /><br /><br />
                <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                <br /><br /><br /><br />
            </span>
        </asp:View>
        <asp:View ID="VIWselectItems" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBselectedProvider_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="LBselectedProvider">Tipo di autenticazione:</asp:Label>
                <asp:Label ID="LBselectedProvider" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <span class="Fieldrow">
                <asp:Label ID="LBselectedProfile_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="LBselectedProfile">Profilo da creare:</asp:Label>
                <asp:Label ID="LBselectedProfile" runat="server" CssClass="Testo_Campo"></asp:Label>
            </span>
            <asp:Repeater ID="RPTinvalidItems" runat="server">
                <ItemTemplate>
                    <asp:Label ID="LBinvalidItem" runat="server" Text="<%#TranslateInvalidItem(Container.DataItem)%>"></asp:Label>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="RPTitems" runat="server">
                <ItemTemplate>
                    <div class="fixedWidth">
                    <table border="1" cellpadding="0" cellspacing="0" id="aTable">
                        <thead>
                            <tr class="ROW_header_Small_Center">
                                <th><asp:literal ID="LTselectRow_t" runat="server"></asp:literal></th>
                                <th><asp:literal ID="LTrowInfo_t" runat="server"></asp:literal></th>
                                <asp:Repeater ID="RPTcolumns" runat="server" DataSource="<%#Container.DataItem.ColumHeader%>" OnItemDataBound="RPTcolumns_ItemDataBound">
                                    <ItemTemplate>
                                        <th class="<%#GetColumnCss(Container.DataItem.Type)%>">
                                            <asp:literal ID="LTcolumn" runat="server"></asp:literal>
                                            <asp:literal ID="LTcolumnIndex" runat="server" Visible="false" Text="<%#Container.DataItem.Number%>"></asp:literal>
                                            <asp:literal ID="LTcolumnAttribute" runat="server" Visible="false" Text="<%#Container.DataItem.Attribute.ToString()%>"></asp:literal>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </thead>
                        <tbody>
                        <asp:Repeater ID="RPTitemRow" runat="server" DataSource="<%#Container.DataItem.GetOrderedRows%>" OnItemDataBound="RPTitemRow_ItemDataBound">
                            <ItemTemplate>
                                <tr id="tr<%#Container.DataItem.Number%>" class="<%#isImported(Container.DataItem)%>" >
                                    <td>
                                        &nbsp;<input type="checkbox" runat="server" ID="CBXselect" class="Testo_Campo" />&nbsp;
                                        <asp:literal ID="LTrowIndex" runat="server" Visible="false" Text="<%#Container.DataItem.Number%>"></asp:literal>
                                    </td>
                                    <td><asp:Label ID="LBrowInfo" runat="server" CssClass="tooltip"></asp:Label></td>
                                    <asp:Repeater ID="RPTcells" runat="server" DataSource="<%#Container.DataItem.Cells%>" OnItemDataBound="RPTcells_ItemDataBound">
                                        <ItemTemplate>
                                        <td class="<%#GetCellCss(Container.DataItem.Column)%>">
                                            <asp:literal ID="LTitem" runat="server"></asp:literal>
                                            <asp:literal ID="LTitemValue" runat="server" Visible="false" Text="<%#Container.DataItem.Value%>"></asp:literal>
                                            <div class="itemsummary" runat="server" id="DVmultiple" visible="false">
                                                <span class="itemcounter"><asp:literal ID="LTdisplayName" runat="server"/></span>&nbsp;
                                                <ctrl:ToolTip id="CTRLtoolTip" runat="server" ></ctrl:ToolTip>
                                            </div>
                                        </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        </tbody>
                    </table>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                       
                </FooterTemplate>
            </asp:Repeater>
        </asp:View>
    </asp:MultiView>
    <br /><br />
</div>