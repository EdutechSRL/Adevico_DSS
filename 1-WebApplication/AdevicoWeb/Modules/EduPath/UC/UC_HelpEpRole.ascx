<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_HelpEpRole.ascx.vb" Inherits="Comunita_OnLine.UC_HelpEpRole" %>
<script type="text/javascript">
    $(document).ready(function () {

        $('.helpPerm').dialog({
            appendTo: "form",
            autoOpen: false,
            draggable: true,
            modal: true,
            title: "",
            width: 600,
            height: 250,
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

        $(".helpRole").live("click", function () {
            //<asp:Image ID="IMGhelpRole" runat="server" CssClass="helpRole"/>
            //{Fix IE Height
            if ($.browser.msie) {
                $(".helpPerm").dialog("option", "height", 300);
            }
            //}
            $(".helpPerm").dialog("open");
            return false;
        });
    });
</script>
<style>
    .table50
    {
        width:550px;
    }
    
    .helpRole
    {
        cursor:pointer;
    }
</style>

<div class="dialog helpPerm">
    <table cellpadding="0" cellspacing="0" border="1" class="DataGrid_Generica table50">
        <tr class="ROW_header_Small_Center">
            <th width= "140px"><asp:Label ID="LbPermissionTitle" runat="server" Text="*nome permesso" /></th>
            <th style="width:40px"><asp:Label ID="LBpartecipantTitle" runat="server" Text="*partecipant" /></th>
            <th style="width:40px"><asp:Label ID="LBstatviewerTitle" runat="server" Text="*statviewer" /></th>
            <th id="THeval" runat="server" style="width:40px"><asp:Label ID="LBevaluatorTitle" runat="server" Text="*eval" /></th>
            <th style="width:40px"><asp:Label ID="LBmanagerTitle" runat="server" Text="*Manager" /></th>
        </tr>
        <asp:Repeater ID="RPpermission" runat="server">
            <ItemTemplate>
                <tr class="ROW_Normal_Small">
                    <td ><asp:Label ID="LBpermission" runat="server" Text="*permesso" /></td>
                   
                    <td >&nbsp<asp:Image ID="IMGpartecipant" runat="server" Visible="false" />&nbsp</td>
                    
                    <td >&nbsp<asp:Image ID="IMGstatviewer" runat="server" Visible="false" />&nbsp</td>
                    
                    <td id="TDeval" runat="server">&nbsp<asp:Image ID="IMGevaluator" runat="server" Visible="false" />&nbsp</td>
                    
                    <td>&nbsp<asp:Image ID="IMGmanager" runat="server"  Visible="false" />&nbsp</td>
                </tr>        
            </ItemTemplate>
        </asp:Repeater>
    
    </table>
</div>