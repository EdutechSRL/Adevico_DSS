<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProjectManagementHeader.ascx.vb" Inherits="Comunita_OnLine.UC_ProjectManagementHeader" %>
<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="~/Modules/Common/UC/UC_AdvancedProgressBarHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Dialog" Src="~/Modules/ProjectManagement/UC/List/UC_TaskInfoHeader.ascx" %>

<link href="<%=Page.ResolveUrl("~/Graphics/Modules/ProjectManagement/Css/ProjectManagement.css?v=201605041410lm")%>" rel="Stylesheet" />
<link href="<%=Page.ResolveUrl("~/Jscript/Modules/Common/Choosen/chosen.css?v=201605041410lm")%>" rel="Stylesheet" />
<link rel="stylesheet" href="<%=Page.ResolveUrl("~/Graphics/Plugins/jquery.treeTable/jquery.treeTable.css?v=201605041410lm")%>" />
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/jquery.simpleEqualize.js")%>"></script>
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/Choosen/chosen.jquery.js")%>"></script>
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/jquery.blockableFieldset.js")%>"></script>
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/jquery.inputActivator.js")%>"></script>
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/jquery.collapsableTreeAdv.js")%>"></script>
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/jquery.treeTable.js")%>"></script>
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/jquery.ddbuttonlist.js")%>"></script>
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/Common/jquery.autoresize.js")%>"></script>
<CTRL:ProgressBar id="CTRLadvancedProgressBarHeader" runat="server" />
<script type="text/javascript" src="<%=Page.ResolveUrl("~/Jscript/Modules/ProjectManagement/projectmanagement.js")%>"></script>
<CTRL:Dialog id="CTRLdialogHeader" runat="server"></CTRL:Dialog>