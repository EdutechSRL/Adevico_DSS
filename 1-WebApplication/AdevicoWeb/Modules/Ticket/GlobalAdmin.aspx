<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="GlobalAdmin.aspx.vb" Inherits="Comunita_OnLine.GlobalAdmin" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<%--<%@ Register TagPrefix="CTRL" TagName="CategoryTree" Src="~/Modules/Ticket/UC/Uc_CategoryTree.ascx" %>
--%>

<%@ Register TagPrefix="CTRL" TagName="Categories" Src="~/Modules/Ticket/UC/UC_CategoryDDL.ascx" %>

<%--<%@ Register TagName="CTRLMailSets" TagPrefix="CTRL" Src="~/Modules/Ticket/UC/UC_MailSettings.ascx" %>--%>

<%@ Register TagPrefix="CTRL" TagName="ActionMessages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="SelectPerson" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectPersonHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettingsNew.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script> 
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	
	<link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>
	
   <script type="text/javascript" src="../../Scripts/jquery-migrate-1.2.1.min.js"></script>
	<CTRL:SelectPersonHeader ID="CTRLselectUsersHeader" runat="server" />


	<script type="text/javascript">

		$(function () {

			$(".linkMenu.wide").click(function () {
				$(".page-width").addClass("fullwidth");

			});

			$(".linkMenu.narrow").click(function () {
				$(".page-width").removeClass("fullwidth");
			});


			$(".dialog.dlgcategories").dialog({
				width: 600,
				height: 400,
				autoOpen: false
			});

			$(".showcategories").click(function () {
				$(".dialog.dlgcategories").dialog("open");
				return false;
			});
		});
		$(document).ready(function () {
			$(".view-modal.view-users").dialog({
				appendTo: "form",
				closeOnEscape: true,
				modal: true,
				width: 890,
				height: 450,
				minHeight: 300,
				minWidth: 700,
				title: '<%=Me.InternalUserAddTitle %>',
				open: function (type, data) {
					$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
				}
			});
		});
	</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">

<div class="tickets mgmgt globalsettings">

	<div class="pageheader">
		<div class="DivEpButton top">
			
			<asp:LinkButton ID="LNBupdateComType" runat="server" CssClass="Link_Menu">*Aggiorna tipi comunità</asp:LinkButton>
			<asp:LinkButton ID="LNBsaveSettings" runat="server" CssClass="Link_Menu">*Save</asp:LinkButton>

		</div>

		<CTRL:ActionMessages ID="UCactionMessages" runat="server" />

	</div>

<asp:Panel ID="PNLcontent" runat="server">

	<div class="fieldobject globalsettings">	
		
		<div class="fieldrow objectheader">
			<h4 class="title">
				<asp:Literal ID="LTService_t" runat="server">*Global settings</asp:Literal>
			</h4>
			<div class="fieldrow description">
				<asp:Literal ID="LTService_d" runat="server">*Global settings description</asp:Literal>
			</div><!-- valutare se inserire descrizione -->

			<CTRL:Switch ID="CTRLswService" runat="server" Status="true" Enabled="false" MainCss="btnswitchgroup small"/>

		</div>
		
		
		<div class="contentwrapper">
			
			<div class="fieldrow optiongroup limits clearfix">
				
				<div class="fieldlabel title">
					<asp:Literal ID="LTlimits_t" runat="server">*Limits</asp:Literal>
				</div>

				<div class="options">
					<span class="fieldrow description"></span>
					<span class="option inputgroup">
						<asp:CheckBox ID="CBXexternalLimit" runat="server" Text="*Limite Ticket utenti esterni"/>
						<asp:TextBox ID="TXBexternalLimit" runat="server" CssClass="inputchar"></asp:TextBox>
					</span>
					<span class="option inputgroup">
						<asp:CheckBox ID="CBXinternalLimit" runat="server" Text="*Limite Ticket utenti esterni"/>
						<asp:TextBox ID="TXBinternalLimit" runat="server" CssClass="inputchar"></asp:TextBox>
					</span>
					<span class="option inputgroup">
						<asp:CheckBox ID="CBXdraftLimit" runat="server" Text="*Limite bozze"/>
						<asp:TextBox ID="TXBdraftLimit" runat="server" CssClass="inputchar"></asp:TextBox>
					</span>
				</div>
			</div>
			
			<div class="fieldrow optiongroup user notifications clearfix">
				<div class="fieldlabel title">
					<asp:Literal ID="LTnotificationUsr_t" runat="server">*Default user notification settings</asp:Literal>
					<CTRL:Switch ID="CTRLswUserNotification" runat="server" Status="true" Enabled="true" MainCss="btnswitchgroup small"/>
				</div>
				<div class="options">
					<asp:Label runat="server" ID="LBUsrNot" Visible="false" CssClass="optionlabel"></asp:Label>
					<span class="inlinewrapper">
						<span class="innerwrapper">
							<CTRL:MailSettings ID="CTRLmailSetUser" runat="server" />
						</span>
					</span>
				</div>
				<%--<div class="commands">
					<asp:linkbutton ID="LBsaveUsrSets" runat="server" CssClass="linkMenu ovverride">*Save user settings</asp:linkbutton>
				</div>--%>
			</div>

			<div class="fieldrow optiongroup user notifications clearfix">
				<div class="fieldlabel title">
					<asp:Literal ID="LTnotificationMan_t" runat="server">*Default manager/resolver notification settings</asp:Literal>
					<CTRL:Switch ID="CTRLswManNotification" runat="server" Status="true" Enabled="true" MainCss="btnswitchgroup small"/>
				</div>
				<div class="options">
					<asp:Label runat="server" ID="LBManNot" Visible="false" CssClass="optionlabel"></asp:Label>
					<span class="inlinewrapper">
						<span class="innerwrapper">
							<span class="option">
								<CTRL:MailSettings ID="CTRLmailSetMan" runat="server" />
							</span>
						</span>
					</span>
				</div>

<%--				<div class="commands">
					<asp:linkbutton ID="LNBsaveMailSets" runat="server" CssClass="linkMenu ovverride">*Save notification settings</asp:linkbutton>
				</div>--%>
			</div>

		</div>
	</div>

	<div class="fieldobject categorysettings">	
		
		<div class="fieldrow objectheader">
			<h4 class="title">
				<asp:Literal ID="LTcategories_t" runat="server">*Categories management</asp:Literal>
			</h4>
			<div class="fieldrow description">
				<asp:Literal ID="LTcategories_d" runat="server">*Global settings description</asp:Literal>
			</div><!-- valutare se inserire descrizione -->

			<CTRL:Switch ID="CTRLswCat" runat="server" Status="true" MainCss="btnswitchgroup small"/>
		</div>

		<div class="contentwrapper">
			<div class="fieldrow optiongroup ticketpermissions">
				<div class="fieldlabel title">
					<asp:Literal ID="LTpermission_t" runat="server">*Permission settings per community type</asp:Literal>
				</div>
				<div class="tablewrapper">
					<asp:Repeater ID="RPTcommunityTypes" runat="server" EnableViewState="true">
						<HeaderTemplate>
							<table class="table communitytypepermission light fullwidth">
								<thead>
									<tr>
										<th class="communitytype">
											<asp:Label ID="LBsettings_Name_t" runat="server" CssClass="text">*Name</asp:Label>
										</th>
										<th class="viewticket permission" data-col="viewticket">
											<asp:Label ID="LBsettings_ViewTicket_t" runat="server" CssClass="text">*Viewticket</asp:Label>
											<span class="icons">
												<span class="icon selectall" title="">   </span>
												<span class="icon selectnone" title="">   </span>
											</span>
										</th>
										<th class="public permission" data-col="public">
											<asp:Label ID="LBsettings_Public_t" runat="server" CssClass="text">*Create public</asp:Label>
										
											<span class="icons">
												<span class="icon selectall" title="">   </span>
												<span class="icon selectnone" title="">   </span>
											</span>
										</th>
										<th class="ticket permission" data-col="ticket">
											<asp:Label ID="LBsettings_Ticket_t" runat="server" CssClass="text">*Create Ticket</asp:Label>
											<span class="icons">
												<span class="icon selectall" title="">   </span>
												<span class="icon selectnone" title="">   </span>
											</span>
										</th>
										<th class="private permission" data-col="private">
											<asp:Label ID="LBsettings_Private_t" runat="server" CssClass="text">*Create Private</asp:Label>
											<span class="icons">
												<span class="icon selectall" title="">   </span>
												<span class="icon selectnone" title="">   </span>
											</span>
										</th>
									</tr>
								</thead>
						</HeaderTemplate>
						<ItemTemplate>
								<tbody>
									<tr>
										<td class="communitytype permission">
											<asp:HiddenField ID="hif_Id" runat="server" />
											<asp:Label ID="LBsettings_Name" runat="server">*Name</asp:Label>
											<span class="right icons hideonedit">
												<span class="icon selectall" title="">   </span>
												<span class="icon selectnone" title="">   </span>
											</span>
										</td>
										<td class="viewticket" data-col="viewticket">
											<asp:CheckBox ID="CBXsettingsView" runat="server" />
										</td>
										<td class="public" data-col="public">
											<asp:CheckBox ID="CBXsettingsPublic" runat="server" />
										</td>
										<td class="ticket" data-col="ticket">
											<asp:CheckBox ID="CBXsettingsTicket" runat="server" />
										</td>
										<td class="private" data-col="private">
											<asp:CheckBox ID="CBXsettingsPrivate" runat="server" />
										</td>
									</tr>
								</tbody>
						</ItemTemplate>
						<FooterTemplate>
							</table>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</div>

			<div class="fieldrow ticketcategories">

				<div class="fieldrow availablecategories">
					<asp:Label CssClass="fieldlabel" ID="LBcategoryTree_t" runat="server" AssociatedControlID="LBpublicNum">*Categorie disponibili</asp:Label>
					<span class="details">
						<span class="item">
							<asp:Label id="LBpublicNum" runat="server" CssClass="counter public"></asp:Label>
							<asp:Label id="LBpublicNum_t" runat="server" CssClass="label"></asp:Label>
						</span>
						<span class="item">
							<asp:Label id="LBcommunityNum" runat="server" CssClass="counter community"></asp:Label>
							<asp:Label id="LBcommunityNum_t" runat="server" CssClass="label"></asp:Label>
						</span>
						<span class="item">
							<asp:Label id="LBticketNum" runat="server" CssClass="counter tickettype"></asp:Label>
							<asp:Label id="LBticketNum_t" runat="server" CssClass="label"></asp:Label>
						</span>
					</span>          

					<%--<a class="linkMenu showcategories"><asp:literal id="LIT_hypCategory" runat="server">*Show Categories</asp:literal></a>--%>
			
				</div>

				<div class="fieldrow defaultcategory">
					<asp:Label ID="LBcateDef_t" runat="server" cssclass="fieldlabel" AssociatedControlID="CTRLddlCat">*Default public category</asp:Label>

					<CTRL:Categories id="CTRLddlCat" runat="server" RequiredValidator="true"></CTRL:Categories>
					<span class="item">
						<asp:LinkButton ID="LNBsetDefault" runat="server" CssClass="linkMenu"></asp:LinkButton>
						<asp:LinkButton ID="LNBremDefault" runat="server" CssClass="linkMenu"></asp:LinkButton>
					</span>


				</div>
			</div>

			<div class="fieldrow mandatorylegend">
				<asp:Literal ID="LTmandatory_l" runat="server">*Marked fields {0} are mandatory</asp:Literal>
			</div> 

		</div>
	</div>

	<div class="fieldobject viewedit">	
			
		<div class="fieldrow objectheader">
			<h4 class="title"><asp:Literal ID="LTedit_t" runat="server">*View and edit</asp:Literal></h4>
			<div class="fieldrow description">
				<asp:Literal ID="LTedit_d" runat="server">*Lorem usum dolor sit amet</asp:Literal><!-- valutare se inserire descrizione -->
			</div>

			<div class="contentwrapper">
				<div class="options">
					<div class="fieldrow option view">
						<CTRL:Switch ID="CTRLswView" runat="server" Status="true" MainCss="btnswitchgroup small"/>
					</div>
					<div class="fieldrow option edit">
						<CTRL:Switch ID="CTRLswEdit" runat="server" Status="true" MainCss="btnswitchgroup small"/>
					</div>
				</div>
			</div>

		</div>
	</div>
	
	
	
	<div class="fieldobject onbehalfmgmt">

		<div class="fieldrow objectheader">
			<h4 class="title">
				<asp:literal ID="LTbehalf_t" runat="server">*On behalf ticket management</asp:literal>
			</h4>
			<div class="fieldrow description">
				<asp:literal ID="LTbehalf_d" runat="server">*Lorem usum dolor sit amet</asp:literal>
			</div><!-- valutare se inserire descrizione -->

			<div class="contentwrapper">
				<div class="options">
					
					<div class="fieldrow option enable">
						<CTRL:Switch ID="CTRLswBehalf" runat="server" Status="true" MainCss="btnswitchgroup small"/>
					</div>
					
					<div class="fieldrow usertypes">
	 
						<div class="choseselect clearfix">
							<div class="left">
								<asp:Label runat="server" id="LBbehalfType" CssClass="fieldlabel">*Tipi persona abilitati</asp:Label><!--
								--><select runat="server" id="SLBbehalfType" class="usertypes chzn-select" multiple tabindex="2" EnableViewState="True">
								</select>
							</div>
							<div class="right">
								<span class="icons">
									<span class="icon selectall" title="All">&nbsp;</span><!--
									--><span class="icon selectnone" title="None">&nbsp;</span>
								</span>
							</div>
						</div>
					</div>
				
					<%--<div class="fieldrow usertypes">
	 
						<div class="choseselect clearfix">
							<div class="left">
								<label class="fieldlabel">Allowed user types</label><select data-placeholder="Nessun tipo persona selezionato..." class="usertypes chzn-select" multiple tabindex="2">
									<!-- <option value=""></option> -->
										<option value="Type1" >Type 1</option>
										<option value="Type2" >Type 2</option>
										<option value="Type3" >Type 3</option>
										<option value="Type4" >Type 4</option>
										<option value="Type5" >Type 5</option>
										<option value="Type6" >Type 6</option>
								</select>
							</div>
							<div class="right">
											<span class="icons">
												<span class="icon selectall" title="All">&nbsp;</span><span class="icon selectnone" title="None">&nbsp;</span>
											</span>
							</div>
						</div>
					</div>--%>

					<div class="fieldrow usersbyname">				
						<div class="adduser">
							<asp:label ID="LBdescritption_t" runat="server" CssClass="description">#No users selected / Allowed users by name</asp:label>
							<span class="item">
								<asp:Button ID="LNBaddUsers" runat="server" CssClass="linkMenu" Text="*Add users"/>
							</span>
						</div>
							
							
							
								<div class="tablewrapper users">
									
									<asp:Repeater ID="RPTusersBehalf" runat="server">
										<HeaderTemplate>
									
									<table class="table minimal fullwidth">
										<thead>
											<tr>
												<th class="name">
													<asp:literal ID="LTname_t" runat="server">*Name</asp:literal>
												</th>
												<th class="type">
													<asp:literal ID="LTuserType_t" runat="server">*UserType</asp:literal>
												</th>
												<th class="actions"><span class="icons"><span class="icon actions"></span></span></th>
											</tr>
										</thead>
										<tbody>

										</HeaderTemplate>
										<ItemTemplate>
											<tr>
												<td class="name">
													<span class="fullname">
														<asp:literal ID="LTuserName" runat="server">#User Name</asp:literal>
													</span>
												</td>
												<td class="type">
													<span class="name">
														<asp:literal ID="LTuserType" runat="server">#Type</asp:literal>
													</span>
												</td>
												<td class="actions">
													<span class="icons">
														<asp:linkbutton ID="LKBdelete" runat="server" CssClass="icon delete confirmdialog evaluator"/>
													</span>
												</td>
											</tr>

										</ItemTemplate>
										<FooterTemplate>
											</tbody>
									</table>
										</FooterTemplate>
									</asp:Repeater>
									

<%--									<table class="table minimal fullwidth">
										<thead>
											<tr>
												<th class="name">Name</th>
												<th class="type">User type</th>
												<th class="actions"><span class="icons"><span class="icon actions"></span></span></th>
											</tr>
										</thead>
										<tbody>
											
											<tr>
												<td class="name">
													<span class="fullname">Mario Rossi</span>
												</td>
												<td class="type"><span class="name">Type 1</span></td>
												<td class="actions">
													<span class="icons">
														<span class="icon delete confirmdialog evaluator" title="delete">&nbsp;</span>
													</span>
												</td>
											</tr>
											<tr>
												<td class="name">
													<span class="fullname">Filippa Verdi</span>
												</td>
												<td class="type"><span class="name">Type 2</span></td>
												<td class="actions">
													<span class="icons">
														<span class="icon delete confirmdialog evaluator" title="delete">&nbsp;</span>
													</span>
												</td>
											</tr>
											<tr class="last">
												<td class="name">
													   <span class="fullname">Andrea Banchi</span>
												   </td>
												   <td class="type"><span class="name">Type 3</span></td>
												   <td class="actions">
													   <span class="icons">
														   <span class="icon delete confirmdialog evaluator" title="delete">&nbsp;</span>
													   </span>
												   </td>
											 </tr>
										</tbody>
									</table>--%>
								</div>
					  </div>
				
				
				</div>
			</div>



		</div>
	</div>


	

	

</asp:Panel>
	
	<div class="DivEpButton bottom">
		<asp:LinkButton ID="LNBsaveSettings_Bot" runat="server" CssClass="Link_Menu">*Save</asp:LinkButton>
	</div>

	<%-- Literal di Template--%>
	<asp:Literal ID="LTcbxInputAttributes_Class" runat="server" Visible="false">inputtext activator</asp:Literal>
	<asp:Literal ID="LTcbxLabelAttributes_Class" runat="server" Visible="false">fieldlabel</asp:Literal>

	<asp:Literal ID="LTmandatoryTemplate" runat="server" Visible="false"><span class="mandatory">*</span></asp:Literal>
	<%-- END Literal di Template--%>
	


</div>
<!--START User DIALOG	-->
<div class="view-modal view-users" runat="server" id="DIVusers" visible="false">
	<CTRL:SelectPerson ID="CTRLselectUsers" runat="server"
	RaiseCommandEvents="True" DisplayDescription="true"
	DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True"
	DefaultMaxPreviewItems="5"
	ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="True"
	SelectionMode="SystemUsers" MultipleSelection="True"
	InModalWindow="true"
	/>  
</div>
<!--END User DIALOG	-->

</asp:Content>

