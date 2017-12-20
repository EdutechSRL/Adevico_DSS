<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AdvancedProgressBarHeader.ascx.vb" Inherits="Comunita_OnLine.UC_AdvancedProgressBarHeader" %>
<script language="javascript" type="text/javascript" src="<%#ResolveUrl("~/Jscript/Modules/Common/jquery.progressbar.js")%>" ></script>
<script type="text/javascript">
    $(function () {
        $(".progressbar").myProgressBar();
    });
</script>