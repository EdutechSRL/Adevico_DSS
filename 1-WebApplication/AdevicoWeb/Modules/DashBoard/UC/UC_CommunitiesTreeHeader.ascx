<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunitiesTreeHeader.ascx.vb" Inherits="Comunita_OnLine.UC_CommunitiesTreeHeader" %>
<%@ Register TagPrefix="CTRL" TagName="unsubscriptionDialogHeader" Src="~/Modules/DashBoard/UC/UC_UnsubscriptionDialogHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="FiltersHeader" Src="~/Modules/Common/UC/UC_FiltersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EnrollDialogHeader" Src="~/Modules/DashBoard/UC/UC_EnrollDialogHeader.ascx" %>
<!--UC_CommunitiesTreeHeader START-->
<link href="<%=GetBaseUrl()%>Graphics/Modules/Dashboard/Css/homepage.css?v=201604071200lm" rel="Stylesheet" />
<link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
<script src="<%=GetBaseUrl()%>Scripts/angular.min.js" type="text/javascript"></script>    
<script src="<%=GetBaseUrl()%>Jscript/Modules/Dashboard/communitytree.js" type="text/javascript"></script>    
<script src="<%=GetBaseUrl()%>Jscript/Modules/Common/jquery.collapsableTreeAdv.js" type="text/javascript"></script>    
<script src="<%=GetBaseUrl()%>Jscript/jquery.cookie.js" type="text/javascript"></script>

<CTRL:FiltersHeader id="CTRLfiltersHeader" FilterModuleCode="SRVDSHBOARD" runat="server"></CTRL:FiltersHeader>
 <CTRL:EnrollDialogHeader id="CTRLenrollDialogHeader" runat="server"></CTRL:EnrollDialogHeader>
<CTRL:unsubscriptionDialogHeader id="CTRLunsubscriptionDialogHeader" runat="server"></CTRL:unsubscriptionDialogHeader>
<!--UC_CommunitiesTreeHeader END-->