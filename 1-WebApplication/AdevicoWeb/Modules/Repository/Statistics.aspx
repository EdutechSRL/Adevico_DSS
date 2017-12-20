<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="VoidAPI.aspx.vb" Inherits="Comunita_OnLine.VoidAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    Statistiche Scorm
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
    <!-- Service Localization (.js) -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Head Content: .ccs, .js -->
	
		
	<script type="text/javascript">
		var thisHref = location.href;
		var thisParams = "";
		if(thisHref && thisHref.indexOf("?") > 0)
		{
			thisParams = thisHref.split("?")[1];
		}
		
		location.href = "/ModulesForApi/Scorm/StatisticheScorm.aspx" + ((thisParams) ? "?" + thisParams : "");
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Statistiche Scorm<%--=GetLocalization("Title.test.text")--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="hide">
        <asp:Literal runat="server" ID="LTlocalizationService">APItest</asp:Literal>
        <asp:Literal runat="server" ID="LTlocalizationValue"></asp:Literal>
    </div>  			
	<div class="panel panel-default panel-heading" ng-hide="ViewMode == 'FromParams'">
	</div>           
	
	<div>
		<a href="/ModulesForApi/Scorm/StatisticheScorm.aspx">pagina statistiche</a>
	</div>	
	
	<div style="height:0;overflow:hidden;position:relative;">
	<!-- Service content (page) -->
	<h1>Test</h1>
    <ol>
        <li>
            <%=ShowCookie().Replace(";", "</li><li>")%>
        </li>
    </ol>
	</div>
	
	 <%--Add for Session--%>
    
    <asp:UpdatePanel ID="UPTempo" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Timer ID="TMsession" runat="server"></asp:Timer>
</asp:Content>
