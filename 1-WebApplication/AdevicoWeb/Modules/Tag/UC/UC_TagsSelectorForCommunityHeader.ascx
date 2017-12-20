<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TagsSelectorForCommunityHeader.ascx.vb" Inherits="Comunita_OnLine.UC_TagsSelectorForCommunityHeader" %>
<!-- START UC_TagsSelectorForCommunityHeader -->
<link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
<script language="javascript" type="text/javascript">
    $(function () {
        $(".chzn-select").chosen();
    });
</script>
<!-- END UC_TagsSelectorForCommunityHeader -->