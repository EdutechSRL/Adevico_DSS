<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_HelpStatus.ascx.vb"
    Inherits="Comunita_OnLine.UC_HelpStatus" %>
<script type="text/javascript">
    $(document).ready(function () {

        $('.helpStatus').dialog({
            appendTo: "form",
            autoOpen: false,
            draggable: true,
            modal: false,
            title: "",
            width: 600,
            height: 200,
            minHeight: 200,
            minWidth: 600,
            zIndex: 99999,
            open: function (type, data) {
                //$(this).parent().appendTo("form");
            },
            dragStart: function () {
                //{Fix IE Height
                if ($.browser.msie) {
                    var height = $(this).closest('.ui-dialog').outerHeight();
                    $(this).closest('.ui-dialog').css("height", height + 8);
                }
                //}
            },
            dragStop: function () {
                //{Fix IE Height
                if ($.browser.msie) {
                    var height = $(this).closest('.ui-dialog').outerHeight();
                    $(this).closest('.ui-dialog').css("height", height + 8);
                }
                //}
            }
        });

        $(".openStatus").live("click", function () {
            //{Fix IE Height
            if ($.browser.msie) {
                $(".helpStatus").dialog("option", "height", 300);
            }
            //}
            $(".helpStatus").dialog("open");
            return false;
        });

    });
</script>
<style>
    .table50
    {
        width: 550px;
    }
   

</style>
<div class="dialog helpStatus">
    <table cellpadding="0" cellspacing="0" border="1" class="DataGrid_Generica table50">
        <tr class="ROW_header_Small_Center">
            <th width="140px">
                <asp:Label ID="LBStatusTitle" runat="server" />
            </th>
            <th style="width: 40px">
                <asp:Label ID="LBStatusImg" runat="server" Text="*img" />
            </th>
        </tr>
        <tr class="ROW_Normal_Small">
            <td>
                <asp:Label ID="LBcompletedPassed" runat="server" />
            </td>
            <td>
                &nbsp<asp:Image ID="IMGcompletedPassed" runat="server" />&nbsp
            </td>
        </tr>
        <tr class="ROW_Normal_Small">
            <td>
                <asp:Label ID="LBstarted" runat="server" />
            </td>
            <td>
                &nbsp<asp:Image ID="IMGstarted" runat="server" />&nbsp
            </td>
        </tr>
        <tr class="ROW_Normal_Small">
            <td>
                <asp:Label ID="LBbrowsed" runat="server" />
            </td>
            <td>
                &nbsp<asp:Image ID="IMGbrowsed" runat="server" />&nbsp
            </td>
        </tr>
    </table>
</div>
