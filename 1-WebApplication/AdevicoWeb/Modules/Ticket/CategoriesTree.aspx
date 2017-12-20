<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CategoriesTree.aspx.vb" Inherits="Comunita_OnLine.CategoriesTree" %>
<%--Nomi Standard: OK--%>
<%@ Register TagPrefix="CTRL" TagName="TreeItem" Src="~/Modules/Ticket/UC/UC_CategoryTreeItem.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

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
	<link rel="stylesheet" href="../../Graphics/JQuery/Css/jquery-sortable.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script> 
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>">

	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery-sortable.js"></script>

	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>

<%--		<script type="text/javascript">
			$(function () {
				var group = $("ul.sortabletree").sortable({
					handle: ".text"
				,
					onDrop: function (item, container, _super) {
						var ser = "";

						group.find("li.sortableitem").each(function () {

							var $parent = $(this).parents("li.sortableitem").first();
							var parentId = 0;
							if ($parent.size() > 0) {
								parentId = $parent.attr("id")
							}

							ser = ser + $(this).attr("id") + ":" + parentId + ";";
						});
						$('.serialize_output').val(ser);


						_super(item, container);
					}
				});
			});
	</script>--%>
<script type="text/javascript">
		$(function () {
			var group = $("ul.sortabletree").sortable({
				handle: ".text",
				exclude: ".default"
			,
				onDrop: function (item, container, _super) {
					var ser = "";

					group.find("li.sortableitem").each(function () {

						var $parent = $(this).parents("li.sortableitem").first();
						var parentId = 0;
						if ($parent.size() > 0) {
							parentId = $parent.attr("id")
						}

						ser = ser + $(this).attr("id") + ":" + parentId + ";";
					});
					$('.serialize_output').val(ser);


					_super(item, container);
				}
			});
		});
</script>


</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<input type="text" class="serialize_output" id="HDNreorderedData" runat="server" style="display:none;" />
	<div class="fieldobject clearfix toolbar top">
		<div class="fieldrow left">

		</div>
		<div class="fieldrow right">
		    <div id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
			    --><asp:LinkButton ID="LNBnotifyAll" runat="server" CssClass="linkMenu big">*Notify ALL</asp:LinkButton><!--   
			    --><asp:LinkButton ID="LNBnotifyManager" runat="server" CssClass="linkMenu big">*Notify Manager</asp:LinkButton><!--
			    --><asp:LinkButton ID="LNBnotifyResolver" runat="server" CssClass="linkMenu big">*Notify Resolver</asp:LinkButton><!--
		    --></div>
			<asp:LinkButton ID="LNBsave" runat="server" CssClass="linkMenu">*Save</asp:LinkButton>
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
				--><asp:HyperLink ID="HYPtable" runat="server" CssClass="btnswitch first" Enabled="true">*Table</asp:HyperLink><!--
				--><asp:HyperLink ID="HYPtree" runat="server" CssClass="btnswitch last active" Enabled="false">*Tree</asp:HyperLink><!--
				--><asp:HyperLink ID="HYPuser" runat="server" CssClass="btnswitch last" Enabled="false" Visible="false">*User</asp:HyperLink><!--
			--></span>
		</div>
	</div>
	<CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
	<asp:Panel ID="PNLtree" runat="server" Visible="true">
		<div class="fieldobject"><%--left--%>
			<div class="fieldrow"><%--description--%>
				<asp:Label ID="LBmain_t" runat="server" CssClass="text">*Categories' Tree:</asp:Label>
			</div>
		</div>
		<asp:Repeater ID="RPTcategoriesTree" runat="server">
			<HeaderTemplate>
				<ul class="sortabletree categories root">        
			</HeaderTemplate>
			<ItemTemplate>
				<CTRL:TreeItem ID="UCTreeItem" runat="server" />
			
			</ItemTemplate>
			<FooterTemplate>
				</ul>        
			</FooterTemplate>
		</asp:Repeater>
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
			</span>
		</span>
		<!-- /LEGEND -->
	</asp:Panel>
</asp:Content>