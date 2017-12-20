<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityDetailsHeader.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityDetailsHeader" %>
<link href="<%=GetBaseUrl()%>Graphics/Modules/Community/Css/communityinfo.css?v=201604071200lm" rel="Stylesheet" />
<link href="<%=GetBaseUrl()%>Graphics/Modules/Dashboard/Css/homepage.css?v=201604071200lm" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/jquery.progressbar.js"></script>

<script language="javascript" type="text/javascript">
    $(function(){
        $(".collapsable .expander").click(function(){
            $(this).parents(".collapsable").first().toggleClass("collapsed").toggleClass("expanded");
        });

        $(".progressbar").myProgressBar();
    });
</script>