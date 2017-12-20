<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailEditorHeader.ascx.vb" Inherits="Comunita_OnLine.UC_MailEditorHeader" %>
<script type="text/javascript">
    $(function () {
        $(".dialog.dlgkeyword").dialog({
            autoOpen: false
        });

        $(".placeholderslist .more").click(function () {
            $(".dialog.dlgkeyword").dialog("open");
            return false;
        });
    });
</script>