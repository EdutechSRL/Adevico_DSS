<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ContentTranslationSelectorHeader.ascx.vb" Inherits="Comunita_OnLine.UC_ContentTranslationSelectorHeader" %>
<script language="javascript" type="text/javascript">
    $(function () {
        try{
            if (!ddbuttonlist)
                $(".ddbuttonlist.enabled").dropdownButtonList();
        }
        catch(e){
            $(".ddbuttonlist.enabled").dropdownButtonList();
        }

        $(".btnswitch.disabled").click(function () {
            return false;
        });

        $(".btnswitch:not(.disabled)").click(function () {
            var $parent = $(this).parents(".btnswitchgroup").first();
            $parent.find(".active").removeClass("active");
            $(this).addClass("active");
            return true;
        });
    });
</script>