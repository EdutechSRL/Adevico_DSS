<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_HelpVisibility.ascx.vb" Inherits="Comunita_OnLine.UC_HelpVisibility" %>
<script type="text/javascript">
    $(document).ready(function () {

        $('.helpVisibility').dialog({
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

        $(".openVisibility").live("click", function () {
            //{Fix IE Height
            if ($.browser.msie) {
                $(".helpVisibility").dialog("option", "height", 300);
            }
            //}
            $(".helpVisibility").dialog("open");
            return false;
        });

    });
</script>

<div class="dialog helpVisibility">
    <div>
        <asp:Label ID="LBvisibilityHelp" runat="server" ></asp:Label>
    </div>
    <div>
        <asp:Image ID="IMGswitchOn" runat="server" />
        <asp:Label ID ="LBswitchOn" runat="server" />
        
    </div>
     <div>
        <asp:Image ID="IMGswitchOff" runat="server" />
        <asp:Label ID ="LBswitchOff" runat="server" />
        
    </div>   
</div>