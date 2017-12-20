<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FiltersHeader.ascx.vb" Inherits="Comunita_OnLine.UC_FiltersHeader" %>
<script language="javascript" type="text/javascript">
    var filterasmx = "<%=GetBaseUrl()%>Modules/Common/Filters.asmx";
    var filterhtml = "<%=GetBaseUrl()%>Filters";
    var filtermodule = "<%=FilterModuleCode%>";
    var filtermodulescope = "<%=FilterModuleScope%>";
    var transactionid = "<%=FilterIdTransaction%>";
    var filteridLanguage = "<%=FilterIdLanguage%>";
    var requiredp = "<%=FilterInLinePermissions%>";
    var unloadcommunitites = "<%=FilterInLineUnloadcommunitites%>";
    var onlyfromOrganizations = "<%=FilterInLineOnlyfromOrganizations%>";
    var availabilitystring = "<%=FilterInLineAvailability%>";
    
</script>
<script src="<%=GetBaseUrl()%>Jscript/Modules/Common/ngfilters_version1.js" type="text/javascript"></script>

<asp:Literal ID="LTtransactionId" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTrequiredPermissions" runat="server" Visible="false"></asp:Literal>