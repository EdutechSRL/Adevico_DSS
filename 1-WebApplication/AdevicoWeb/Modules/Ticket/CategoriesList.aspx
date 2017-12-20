<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CategoriesList.aspx.vb" Inherits="Comunita_OnLine.CategoriesList" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="DelCate" Src="~/Modules/Ticket/UC/UC_CategoryDelete.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />

	<!-- JS service -->
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script> 
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>" />
	<%--<link rel="stylesheet" href="~/Jscript/Modules/Ticket/All-Temp/jquery.ui.all.css">--%>
	
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	
	<link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>

	<script language="javascript" type="text/javascript">

		$(document).ready(function () {
			$(".view-modal.view-delete").dialog({
				appendTo: "form",
				closeOnEscape: true,
				modal: true,
				width: 890,
				height: 450,
				minHeight: 300,
				minWidth: 700,
				title: '<%=Me.DeleteTitle %>',
				open: function (type, data) {
					//$(this).parent().appendTo("form");
					$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
				}
			});
		});

	</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

<asp:Panel ID="PNLContent" runat="server" CssClass="tickets">

	<div class="DivEpButton top">
		<div id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
			--><asp:LinkButton ID="LNBnotifyAll" runat="server" CssClass="linkMenu big">*Notify ALL</asp:LinkButton><!--   
			--><asp:LinkButton ID="LNBnotifyManager" runat="server" CssClass="linkMenu big">*Notify Manager</asp:LinkButton><!--
			--><asp:LinkButton ID="LNBnotifyResolver" runat="server" CssClass="linkMenu big">*Notify Resolver</asp:LinkButton><!--
		--></div>
		<asp:HyperLink ID="HYPnew" runat="server" CssClass="linkMenu">*New</asp:HyperLink>
	</div>
	
	<div class="fieldobject">
		<div class="fieldrow">
			<CTRL:Messages ID="CTRLheadMessages" runat="server" Visible="false" />
		</div>
	</div>

<div class="dialog dlgdescription" title="<%=Me.InfoTitle %>">
	<div class="fieldobject">
		<div class="fieldrow">
			<label class="fieldlabel" for=""></label>

			<div class="description">description...</div>
		</div>
	</div>
</div>

<div class="fieldobject toolbar clearfix">
	<div class="fieldrow left">

	</div>
	<div class="fieldrow right">
		<%--<span class="btnswitchgroup"><!--
			--><asp:LinkButton ID="LNBtable" runat="server" CssClass="btnswitch first active" Enabled="false">*Table</asp:LinkButton><!--
			--><asp:LinkButton ID="LNBtree" runat="server" CssClass="btnswitch last">*Tree</asp:LinkButton><!--
			--><asp:LinkButton ID="LNBuser" runat="server" CssClass="btnswitch" Enabled="false" Visible="false">*Users</asp:LinkButton><!--
		--></span>--%>
		<span class="btnswitchgroup"><!--
			--><asp:HyperLink ID="HYPtable" runat="server" CssClass="btnswitch first active" Enabled="false">*Table</asp:HyperLink><!--
			--><asp:HyperLink ID="HYPtree" runat="server" CssClass="btnswitch last">*Tree</asp:HyperLink><!--
			--><asp:HyperLink ID="HYPuser" runat="server" CssClass="btnswitch last" Enabled="false" Visible="false">*User</asp:HyperLink><!--
		--></span>
	</div>
</div>


<%--<asp:Panel ID="PNLtblContent" runat="server" CssClass="tablewrapper">--%>
<div class="tablewrapper">
	<table class="table treetable light categorymap fullwidth">
		<thead>
			<tr>
				<th class="name"><asp:literal ID="LTthName" runat="server">*Name</asp:literal></th>
				<th class="resources"><asp:literal ID="LTthResource" runat="server">*Resources</asp:literal></th>
				<th class="actions"><asp:literal ID="LTthActions" runat="server">*Actions</asp:literal></th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="RPTcategoryItem" runat="server">
				<ItemTemplate>
					<asp:Literal ID="LTtdBegin" runat="server"><tr id="ctg-{0}" class="category {1}"></asp:Literal>
						<td class="name">
							<span class="text">
								<asp:literal ID="LTcatName" runat="server">Category 0</asp:literal>
							</span>
							<span class="icons">
								<asp:PlaceHolder ID="PHshowInfo" runat="server"><span class="icon viewdetails showdesc"></span></asp:PlaceHolder>
							</span>
							<div class="invisible description">
								<asp:literal ID="LTcatDesc" runat="server">description</asp:literal>    
							</div>
						</td>
						<td class="resources">
							<span class="numberedlist roleusers clickable">
								<span class="numbereditem managerrole">
									<span class="number">
										<asp:Literal ID="LTManNumber" runat="server"></asp:Literal>
									</span>
									&nbsp;
									<span class="text">
										<asp:Literal ID="LTMan_t" runat="server">*Manager</asp:Literal>
									</span>
								</span>
								<span class="numbereditem resolverrole">
									<span class="number">
										<asp:Literal ID="LTResNumber" runat="server"></asp:Literal>
									</span>
									&nbsp;
									<span class="text">
										<asp:Literal ID="LTRes_t" runat="server">*Reolver</asp:Literal>
									</span>
								</span>
							</span>
						</td>
						<td class="actions">
							<span class="icons">
								<asp:HyperLink ID="HYPedit" runat="server" CssClass="icon edit"></asp:HyperLink>
								<asp:LinkButton ID="LNBdelete" runat="server" CssClass="icon virtualdelete"></asp:LinkButton>
								<asp:LinkButton ID="LNBundelete" runat="server" CssClass="icon recover"></asp:LinkButton>
								<asp:HyperLink ID="HYPpreview" runat="server" CssClass="icon view" Visible="false"></asp:HyperLink>
							</span>
						</td>
					</tr>
				</ItemTemplate>
			</asp:Repeater>
		
			<asp:PlaceHolder ID="PH_noData" runat="server" Visible="false">
				<tr class="empty norecordrow">
					<td colspan="3">
						<asp:Literal ID="LTnoData_t" runat="server"></asp:Literal>
					</td>
				</tr>
			</asp:PlaceHolder>
		
		
		</tbody>
	</table>
	

	<!-- LEGEND -->
	<span class="fieldrow legend hor">
		<span class="fieldlabel">
			<asp:literal id="LTleged_t" runat="server">*Legend</asp:literal>
		</span>
		<span class="group first last">
			<span class="legenditem default" title="<%=GetLegendTitle("default")%>">
				<span class="legendicon default">&nbsp;</span>
				<span class="legendtext">
					<%=GetLegendText("default")%>
				</span>
			</span>
			<span class="legenditem deleted" title="<%=GetLegendTitle("deleted")%>">
				<span class="legendicon deleted">&nbsp;</span>
				<span class="legendtext">
					<%=GetLegendText("deleted")%>
				</span>
			</span>
			<asp:literal id="LTwarningItem" runat="server" Visible="false">
			<span class="legenditem nomanager" title="{title}">
				<span class="legendicon nomanager">&nbsp;</span>
				<span class="legendtext">
					{text}
				</span>
			</span>
			</asp:literal>
		</span>
	</span>
	<!-- /LEGEND -->

</div><%--
</asp:Panel>--%>

<CTRL:DelCate runat="server" id="CTRLdelCate"></CTRL:DelCate>
</asp:Panel>

<%--<asp:Label ID="LBLnoAccess" runat="server" CssClass="errormessage NoAccess" Visible="false">*Accesso alla pagina tempoaraneamente disabilitato da sistema.</asp:Label>--%>

</asp:Content>


