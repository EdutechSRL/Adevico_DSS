<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Category.aspx.vb" Inherits="Comunita_OnLine.Category" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="CTRL"    TagName="CategoryTree" Src="~/Modules/Ticket/UC/Uc_CategoryTree.ascx" %>--%>

<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Common/UC/UC_ContentTranslationSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectorHeader" Src="~/Modules/Common/UC/UC_ContentTranslationSelectorHeader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>


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
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>">
	<%--<link rel="stylesheet" href="~/Jscript/Modules/Ticket/All-Temp/jquery.ui.all.css">--%>
	<%--<script type="text/javascript" src="~/Jscript/Modules/Ticket/All-Temp/callforpapers.js"></script>--%>

		
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>

	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>

	<CTRL:SelectorHeader ID="CTRLselectorHeader" runat="server" />
	<CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />

	<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

	<script language="javascript" type="text/javascript">

		$(document).ready(function () {
			$(".view-modal.view-users").dialog({
				appendTo: "form",
				closeOnEscape: false,
				modal: true,
				width: 890,
				height: 450,
				minHeight: 300,
				minWidth: 700,
				title: '<%=Me.InternalUserAddTitle %>',
				open: function (type, data) {
					//$(this).parent().appendTo("form");
					$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
				}
			});
		});

	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

<asp:Panel ID="PNLcontent" runat="server">
<div class="fieldobject toolbar clearfix">
	<div class="fieldrow left">

	</div>
	<div class="fieldrow right">
		<div id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
			--><asp:LinkButton ID="LNBnotifyAll" runat="server" CssClass="linkMenu big">*Notify ALL</asp:LinkButton><!--   
			--><asp:LinkButton ID="LNBnotifyManager" runat="server" CssClass="linkMenu big">*Notify Manager</asp:LinkButton><!--
			--><asp:LinkButton ID="LNBnotifyResolver" runat="server" CssClass="linkMenu big">*Notify Resolver</asp:LinkButton><!--
		--></div>
		<asp:LinkButton ID="LNBadd" runat="server" CssClass="linkMenu" Visible="False">*Add new</asp:LinkButton>
		<asp:LinkButton ID="LNBmodify" runat="server" CssClass="linkMenu">*Save</asp:LinkButton>
		<asp:LinkButton ID="LNBtoList" runat="server" CssClass="linkMenu">*ToList</asp:LinkButton>
	</div>
</div>



<%--<asp:MultiView ID="MLVmain" runat="server">
	<asp:View ID="V_Create" runat="server">
		<div class="fieldobject">
			<div class="fieldrow createname">
				<asp:Label ID="LBLnameC_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBnameC">*Name</asp:Label>
				<asp:TextBox ID="TXBnameC" runat="server" CssClass="fieldinput"></asp:TextBox>
			</div>
			<div class="fieldrow createdesc">
				<asp:Label ID="LBLdescC_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBdescC">*Description</asp:Label>
				<asp:TextBox ID="TXBdescC" runat="server" CssClass="fieldinput"></asp:TextBox>
			</div>
		</div>
	
	</asp:View>
	<asp:View ID="V_Modify" runat="server">--%>
	

	<div id="DVmessages" class="fieldswrapper fullwidth hide" runat="server">
		<div class="fieldrow">
			<CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false" />
		</div>
	</div>



<div class="fieldswrapper fullwidth">
	<div class="fieldobject categorymgmt">
	
		<div class="fieldrow">
			<CTRL:Selector ID="CTRLlanguageSelector" runat="server" RaiseSelectionEvent="true" LanguageTitleCssClass="languagelabel" />
		</div>
	
		<div class="fieldrow haslang">
			<asp:Label ID="LBname_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBname">*Name</asp:Label>
			
			<span class="languagewrapper">
				<asp:Label id="LBnameLanguage" runat="server" CssClass="templatelanguage" AssociatedControlID="TXBname"></asp:Label>
				<asp:TextBox ID="TXBname" runat="server" CssClass="fieldinput"></asp:TextBox>
				<asp:Literal ID="LTnameError" runat="server" Visible="false"><span class="error">*</span></asp:Literal>
				<span class="icons">
					<asp:LinkButton ID="LNBdelLang" runat="server" CssClass="icon delete">D</asp:LinkButton>
				</span>
			</span>
		</div>
	
		<div class="fieldrow description" id="DVsubject" runat="server">
			<asp:Label ID="LBdescription_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBdescription">*Subject</asp:Label>
			<span class="languagewrapper">
				<asp:Label id="LBdescriptionLang" runat="server" CssClass="templatelanguage" AssociatedControlID="TXBdescription"></asp:Label>
				<asp:TextBox ID="TXBdescription" runat="server" CssClass="fieldinput textarea big" TextMode="MultiLine"></asp:TextBox>
			</span>
		</div>
	
		<div class="fieldrow">
			<asp:Label ID="LBtype_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLtype">*Tipo</asp:Label>

			<span class="typewrapper">
				<asp:DropDownList ID="DDLtype" runat="server">
					<asp:ListItem Text="*Pubblica" Value="1"></asp:ListItem>
					<asp:ListItem Text="*Privata" Value="2"></asp:ListItem>
					<asp:ListItem Text="*Solo Ticket" Value="3"></asp:ListItem>
				</asp:DropDownList>
			</span>
		</div>

		<div class="fieldobject clearfix">
			<div class="fieldrow managers resolvers">
			   <asp:Label ID="LBusers_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RPTUsers">*Users</asp:Label>
			   
			   <div class="tablewrapper inlinewrapper">
					<asp:Repeater ID="RPTUsers" runat="server">
						<HeaderTemplate>
							<table class="table light users fullwidth expandable compressed" data-max="5">
								<thead>
									<tr>
											<th class="username">
												<asp:label ID="LBuserName_t" runat="server">*User Name</asp:label>
											</th>
											<th class="role">
												<asp:label ID="LBrole_t" runat="server">*Role</asp:label>
											</th>
											<th class="actions">
												<asp:label ID="LBaction_t" runat="server">*Actions</asp:label>
											</th>
									</tr>
								</thead>
								<tbody>
						</HeaderTemplate>
						<ItemTemplate>
									<tr>
										<td class="username">
											<asp:HiddenField ID="HDFid" runat="server" />
											<asp:label ID="LBuserName" runat="server">*User Name</asp:label>
										</td>
										<td class="role">
											<asp:RadioButtonList ID="RBLrole" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
												<asp:ListItem Text="*Manager" Value="1" Selected="True" class="role"></asp:ListItem>
												<asp:ListItem Text="*Resolver" Value="0" class="role"></asp:ListItem>
											</asp:RadioButtonList>
										</td>
										<td class="actions">
											<span class="icons">
												<asp:LinkButton ID="LNBdelete" runat="server" CssClass="icon delete"></asp:LinkButton>
												<asp:LinkButton ID="LNBsend" runat="server" CssClass="icon mail"></asp:LinkButton>
											</span>
										</td>
									</tr>
						</ItemTemplate>
						<FooterTemplate>
								</tbody>
							<asp:Panel ID="PNLfooter" runat="server">
								<tfoot>
									<tr>
										<td colspan="4">
											<span class="showextra">
												<asp:literal id="LTfootShow" runat="server">
													show all items
												</asp:literal>
											</span>
											<span class="hideextra">
												<asp:literal id="LTfootHide" runat="server">
													compress table (show first 5 elements)
												</asp:literal>
											</span>
										</td>
									</tr>
								</tfoot>
							</asp:Panel>
							</table>
						</FooterTemplate>
					</asp:Repeater>

					<div class="buttonwrapper">
						<asp:LinkButton ID="LNBaddUser" runat="server" CssClass="linkMenu">*Add user</asp:LinkButton>    
					</div>
			   </div>
			</div>
		</div>
	</div>
	
<%--		</asp:View>
</asp:MultiView>--%>


	<div class="view-modal view-users" id="DVselectUsers" runat="server" visible="false">

		<div class="fieldobject permission">
			<div class="fieldrow">
				<asp:Label runat="server" ID="LBintUserRoles_t" CssClass="fieldlabel" AssociatedControlID="RBLroles">*Ruolo</asp:Label>
				<%--<span class="fieldlabel">
					<asp:Literal ID="LTintUsrRoles_t" runat="server">*Ruolo</asp:Literal>
				</span>--%>
				
					<asp:RadioButtonList ID="RBLroles" runat="server" CssClass="inputgroup"
						RepeatLayout="Flow" RepeatDirection="Horizontal">
						<asp:ListItem Text="Manager" Value="1" Selected="True"></asp:ListItem>
						<asp:ListItem Text="Resolver" Value="0" Selected="False"></asp:ListItem>
					</asp:RadioButtonList>

			</div>
		</div>

		<CTRL:SelectUsers ID="CTRLselectUsers" runat="server"
			RaiseCommandEvents="True" DisplayDescription="true"
			DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True"
			DefaultMaxPreviewItems="5"
			ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false"/>
	</div>
</asp:Panel>



<%--<asp:Panel ID="PNLnoPermission" runat="server" Visible="false">
	<asp:label ID="LBnoPermission" runat="server">Wrong category...</asp:label>
</asp:Panel>--%>


</asp:Content>