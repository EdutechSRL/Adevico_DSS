<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditUser.aspx.vb" Inherits="Comunita_OnLine.EditUser" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="Editor" Src="~/Modules/Common/Editor/UC_Editor.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Repository/Common/UC_ModuleAttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Repository/Common/UC_ModuleAttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/Ticket/UC/UC_TicketAddAttachment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsView" Src="~/Modules/Ticket/UC/UC_AttachmentsView.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectPerson" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectPersonHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettingsNew.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Ticket</asp:Literal>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">*Ticket</asp:Literal>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />
	<link media="print" href="../../Graphics/Modules/Ticket/Css/print.css<%=CssVersion()%>" rel="Stylesheet">

	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script> 
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsablerows.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	
	<link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>

	<CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" />
	<CTRL:SelectPersonHeader ID="CTRLselectUsersHeader" runat="server" />

	<script type="text/javascript">
		$(function () {
			$(".toggler").each(function () {
				var $toggler = $(this);
				var isOn = $toggler.is(".on");
				var $item = $($toggler.data("item"));
				$item.addClass("toggled");
				if (isOn) {
					$item.addClass("on").removeClass("off");
				} else {
					$item.addClass("off").removeClass("on");
				}
			});

			$(".toggler .toggle").click(function () {
				var $toggle = $(this);
				var $toggler = $(this).parents(".toggler").first();
				var isOn = $toggle.is(".on");
				var $item = $($toggler.data("item"));
				if (isOn) {
					$toggler.addClass("on").removeClass("off");
					$item.addClass("on").removeClass("off");
				} else {
					$toggler.addClass("off").removeClass("on");
					$item.addClass("off").removeClass("on");
				}
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

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="tickets">
	
	<!-- Top menu -->
	<div class="DivEpButton top">
		<%--<div runat="server" ID="DVbehalf">
			<asp:Label runat="server" ID="LBbehalfAction_t">*Behalf action: </asp:Label>--%>
			<div class="ddbuttonlist enabled" runat="Server" id="DVbehalf"><!--
				--><asp:linkbutton ID="LNBremoveBehalf" runat="server" CssClass="linkMenu">*Remove Behalf</asp:linkbutton><!--
				--><asp:linkbutton ID="LNBaddUsers" runat="server" CssClass="linkMenu" >*Add users</asp:linkbutton><!--
				--><asp:linkbutton ID="LNBhideToOwner" runat="server" CssClass="linkMenu" >*Hide</asp:linkbutton><!--
				--><asp:linkbutton ID="LNBshowToOwner" runat="server" CssClass="linkMenu" >*Show</asp:linkbutton><!--
				--><asp:linkbutton ID="LNBaddTicketUser" runat="server" CssClass="linkMenu" Visible="False">*Add Ticket users</asp:linkbutton><!--
				--><asp:linkbutton ID="LNBaddNewTicketUSer" runat="server" CssClass="linkMenu" Visible="false">*Add NEW ticket users</asp:linkbutton><!--
			--></div>
		<%--</div>--%>
		<asp:HyperLink ID="HYPbackTop" runat="server" CssClass="linkMenu"></asp:HyperLink>
		<asp:HyperLink ID="HYPbackManagementTop" runat="server" CssClass="linkMenu"></asp:HyperLink>
		<asp:HyperLink ID="HYPbackBehalfTop" runat="server" CssClass="linkMenu hide" Enabled="false"></asp:HyperLink>
	</div>
	<!-- End Top menu -->

<div class="tickets tickethistory ">
	
<%--	<div class="fieldobject" runat="server" ID="DVbehalf">
		<div class="DivEpButton toolbar">
			<!-- Create or Change depends if is a on behalf ticket-->
			
		</div>
	</div>--%>
	
	<asp:panel id="PNLmessages" runat="server" Visible ="false" CssClass="fieldobject">
		<%--<div class="fieldobject">--%>
			<div class="fieldrow">
				<CTRL:Messages ID="CTRLheadMessages" runat="server" Visible="true" />
			</div>
		<%--</div>--%>
	</asp:panel>
	<!-- Top info -->
	<asp:panel id="PNLinfo" runat="server" CssClass="fieldobject header">
	<%--<div class="fieldobject header">--%>
		<div class="fieldrow status clearfix">
			<div class="left">
				<span class="requestid">
					<asp:literal ID="LTticketId_t" runat="server">*Request Id</asp:literal>
					<span><asp:literal ID="LTticketId" runat="server">#9999</asp:literal></span>
				</span>
				<span class="status">
					<asp:literal ID="LTstatus_t" runat="server">*status:&nbsp; </asp:literal>
					<span><asp:literal ID="LTstatus" runat="server">#Solved and closed</asp:literal></span>
				</span>
				<asp:literal id="LTonBehalf" runat="server">
					<span class="status onbehalf">
						<span>{0}</span>
					</span>
				</asp:literal>
			</div>
			<div class="right">
				<span class="category">
					<span class="label">
						<asp:Literal ID="LTcategory_t" runat="server">*Category</asp:Literal>
					</span>
					<span class="catname"><asp:Literal ID="LTcategory" runat="server">#Category</asp:Literal></span>
				</span>
			</div>
		</div>
			
		<div class="fieldrow subject clearfix">
			<div class="left">
				<h2><asp:Literal ID="LTtitle" runat="server">*Subject</asp:Literal></h2>
			</div>
			<div class="right" id="DVnotificationToggler" runat="server">
				<span class="extra">
					<span class="toggler off" data-item="div.advancedsettings">
						<asp:Label ID="LBnotificationTogglerOn" runat="server" CssClass="toggle on">Show notification settings</asp:Label>
						<asp:Label ID="LBnotificationTogglerOff" runat="server" CssClass="toggle off">Hide notification settings</asp:Label>
						<%--<span class="toggle on">Show notification settings</span>
						<span class="toggle off">Hide notification settings</span>--%>
					</span>
				</span>
			</div>
		</div>
	<%--</div>--%>
	</asp:panel>
	<!-- End top info -->

		<!-- Mail notification -->
		
	<asp:panel ID="PNLmailSettings" runat="server" CssClass="fieldobject advancedsettings toggled on">
			<div class="DivEpButton toolbar">
				<asp:LinkButton ID="LNBsaveMailSet" runat="server" CssClass="linkMenu">*Save notification Settings</asp:LinkButton>
			</div>
			<div class="fieldrow notifications clearfix">
				<div class="fieldlabel title">
					<asp:literal ID="LTnotificationSettings_t" runat="server">*Email notification settings</asp:literal>
				</div>

				<asp:panel ID="PNLownerNotification" runat="server" CssClass="fieldrow options email" Visible="False">
					<asp:Label runat="server" ID="LBMailSetOwner_t" AssociatedControlID="CTRLmailSetOwner" CssClass="optionlabel">*label?</asp:Label>
					<span class="inlinewrapper">
						<CTRL:MailSettings ID="CTRLmailSetOwner" runat="server" />
						<asp:literal ID="LTnotificationInfoOwner" runat="server" Visible="false"></asp:literal>
					</span>
				</asp:panel>
				<asp:panel ID="PNLcreatorNotification" runat="server" CssClass="fieldrow options email" Visible="False">
					<asp:Label runat="server" ID="LBMailSetCreator_t" AssociatedControlID="CTRLmailSetCreator" CssClass="optionlabel">*label?</asp:Label>
					<span class="inlinewrapper">
						<CTRL:MailSettings ID="CTRLmailSetCreator" runat="server" />
						<asp:literal ID="LTnotificationInfoCreator" runat="server" Visible="false"></asp:literal>
					</span>
				</asp:panel>
			<%--</div>--%>
			</div>
		</asp:panel>
		
	
		<!-- END Mail notification -->
	
	<%--
	<div class="fieldobject advancedsettings">
		<div class="fieldrow notifications clearfix"  style="display: none">
			<!-- Uc Notification -->
			<div class="fieldlabel title">Email notification settings</div>
			<div class="fieldrow options email">
				<span class="fieldrow description">
					- description -
				</span>
					
					<span class="option">
						<input id="" type="checkbox" name="answer" value="all" checked />
						<label for="id">New answer</label>
					</span>
				
					<span class="option">
						<input id="" type="checkbox" name="status" value="all" checked />
						<label for="id">Status change</label>
					</span>
										
					<span class="option">
						<input id="" type="checkbox" name="file" value="all" checked />
						<label for="id">File attacched</label>
					</span>
						
					<span class="option">
						<input id="" type="checkbox" name="assignment" value="all" checked />
						<label for="id">Resolver assignment</label>
					</span>
						
					<span class="option">
						<input id="" type="checkbox" name="category" value="all" checked />
						<label for="id">Category re-assignment</label>
					</span>

			</div>
			<!-- /Uc Notification -->
		</div>
	</div>
		<!-- End mail notification -->
	--%>
	
	<!-- Messaggi -->
	<div class="historywrapper collapsablerows recentolder" id="DIVhystWrapper" runat="server">
	 
		<div class="viewoptions clearfix">
			<div class="visibilitynav">
				<asp:Literal ID="LTmessagesExpCol" runat="server">
					<label classe="fieldlabel">{4}</label>
					<span class="collapseall" title="{0}">{1}</span>
					<span class="expandall"title="{2}">{3}</span>
				</asp:Literal>
			</div>
		</div>
		<asp:Repeater ID="RPTmessages" runat="server">
			<ItemTemplate>
				<asp:Literal ID="LTanchors" runat="server"></asp:Literal>
				<asp:Literal ID="LTcontainter" runat="server">
				<div class="row message {CssClass}" id="row-{RowId}">
				</asp:Literal>

					<div class="left column">
						<span class="username">
							<asp:literal ID="LTusrName" runat="server">#User Name</asp:literal>
						</span>
						<span class="userrole">
							<asp:literal ID="LTusrRole" runat="server">#User Role</asp:literal>
						</span>
					</div>
				
					<div class="right column">
						<div class="messagewrapper">
							<asp:PlaceHolder ID="PLHnormalMessage" runat="server">
							
								<div class="messageheader clearfix">
									<div class="left">
										<span class="handle alt expanded" title="<%#getExpandText()%>">[-]</span>
										<div class="fieldrow dateinfo">
											<span class="date">
												<asp:Literal ID="LTdate" runat="server">dd/MM/aaaa</asp:Literal>
											</span>
											<span class="time">
												<asp:Literal ID="LTtime" runat="server">hh:mm</asp:Literal>
											</span>
										</div>
										<div class="fieldrow preview">
											<span class="text">
												<asp:Literal ID="LTtextPrev" runat="server"></asp:Literal>
											</span>
										</div>
									</div>
									<div class="right">
										<span class="actions">
											<span class="action">
												<%--<a href="" class="quote">Quote</a>--%>
											</span>
										</span>
									</div>
								</div>
								<div class="messagecontent">
									<asp:Literal ID="LTtext" runat="server"></asp:Literal>
								</div>
								
								<div class="messagefooter<%#GetCssFooter(Container.DataItem)%>">
									
									<asp:panel id="PNLbehalfInfo" runat="server" CssClass="behalf info">
										<asp:literal id="LTbehalfInfo" runat="server">*Created by: </asp:literal>
										<span class="user">
											<asp:literal ID="LTbhUser" runat="server">#Creator</asp:literal>
										</span>
									</asp:panel>
									
									<CTRL:AttachmentsView ID="CTRLattView" runat="server" />
								<%--<div class="messagefooter empty">
									<div class="fieldrow attachments">
										<%--NOTA: Togliere EMPTY da messagefooter se ho allegati:
										al momento così perchè funzione non implementata: nessun allegato--%>
										<!-- UC file render -->
								
											<%--<span class="objectRender file">
												<span class="leftDetail">
													<span class="itemTitle">
														<a title="(Premi per scaricare) Lorem_Ipsum.pdf (31 kb)." class="fileRepositoryCookie" href="" target="_blank"><span class="fileIco extpdf">&nbsp;</span>Lorem_Ipsum.pdf</a>
													</span>
													<span class="itemDetails">31 kb</span>
												</span>
											</span>
									
											<span class="objectRender file">
												<span class="leftDetail">
													<span class="itemTitle">
														<a title="(Premi per scaricare) Lorem_Ipsum.pdf (31 kb)." class="fileRepositoryCookie" href="" target="_blank"><span class="fileIco extpdf">&nbsp;</span>Dolor_sit_amet.pdf</a>
													</span>
													<span class="itemDetails">31 kb</span>
												</span>
											</span>--%>
								
										<!-- end UC file render -->						
								</div> 
							</asp:PlaceHolder>
							<asp:PlaceHolder ID="PLHsystemMessage" runat="server">
									<div class="messageheader empty clearfix"></div>
									
									<div class="messagecontent clearfix">
																				
										<div class="noticecontent">
											<asp:Literal ID="LTsysText" runat="server"></asp:Literal>
										</div>			
										<div class="fieldrow submit">
											<asp:LinkButton id="LNBreopen" runat="server" CssClass="Link_Menu big"></asp:LinkButton>
										</div>
									</div>
									<div class="messagefooter empty clearfix">
										<div class="fieldrow submit">
											<input type="button" class="Link_Menu big" value="Reopen" />
										</div>
									</div>

							</asp:PlaceHolder>

						</div>
					</div>
				</div>       
			</ItemTemplate>
		</asp:Repeater>

		<asp:PlaceHolder ID="PHedit" runat="server">
			<a name="editor"></a>
			<div class="row message reply clearfix last"  id="row-edit">
				<div class="left column">
					<span class="username">
						<asp:literal ID="LTcurUser" runat="server">#User Name</asp:literal>
					</span>
					<span class="userrole">
						<asp:literal ID="LTcurRole" runat="server">#CurrentRole</asp:literal>
					</span>
				</div>
				<asp:literal ID="LTmessageheaderCss" runat="server" Visible="false">messageheader clearfix empty</asp:literal>
				<div class="right column">
					<div class="messagewrapper">
						<asp:panel id="PNLmessageEmpy" runat="server" CssClass="messageheader empty clearfix">
						<%--<div class="messageheader empty clearfix" runat="server" id="DVmsgheader">--%>
							<CTRL:Messages ID="CTRLmessagesInfo" runat="server" />
						</asp:panel>
<%--<div class="left"></div>
							<div class="right"></div>--%>
						
							
						<div class="messagecontent clearfix">
							<a id="editor"></a>							
							<asp:Label ID="LByourMessage_t" runat="server" CssClass="fieldlabel title" AssociatedControlID="CTRLeditorText">*Your messagge:</asp:Label>	
								<%--<textarea class="fieldeditor" rows="10"></textarea>--%>
								<%--<asp:TextBox ID="TXBtext" runat="server" CssClass="fieldeditor" TextMode="MultiLine"></asp:TextBox>--%>
								<CTRL:Editor ID="CTRLeditorText" runat="server" 
									ContainerCssClass="containerclass"
									LoaderCssClass="loadercssclass fieldinput inputtext" EditorCssClass="editorcssclass"             
									AllAvailableFontnames="false" AutoInitialize="true"  MaxHtmlLength="800000" 
									EditorWidth="700px" />
								
								<!-- + classe empty se vuoto -->
								

						</div>
						<div class="fieldobject toolbar clearfix">
							<div id="DIVatchCom" runat="server" class="fieldrow attachments empty">
								<CTRL:AttachmentsView ID="CTRLattView" runat="server" /> 
							</div>
							<div class="fieldrow right">
								<CTRL:AttachmentsCommands ID="CTRLcommands" runat="server" Visible="true" />
							</div><%--Sembra che questo /div sia incapsulato nel controllo se non inizializzato...--%>
						</div>
						<div class="messagefooter clearfix">
							
							<div class="fieldrow submit">
								<div id="DIVsend" class="ddbuttonlist enabled" visible="true" runat="server"><!--   
									--><asp:LinkButton ID="LNBsubmit" runat="server" CssClass="linkMenu big">*Send</asp:LinkButton><!--   
									--><asp:LinkButton ID="LNBsubmitCloseRes" runat="server" CssClass="linkMenu big">*Send&Close (Resolved)</asp:LinkButton><!--
									--><asp:LinkButton ID="LNBsubmitCloseUnres" runat="server" CssClass="linkMenu big">*Send & Close (Unresolved)</asp:LinkButton><!--
								--></div>
								
								<%--<input type="button" class="Link_Menu big" value="Submit" />--%>
							</div>
						</div>
									
					</div>
				</div>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder id="PHsendError" runat="server" Visible="false">
			<div class="error">
				<asp:Literal ID="LTSendErrorInfo" runat="server"></asp:Literal>
			</div>
		</asp:PlaceHolder>
	</div>
	<!-- End messaggi -->

	<!-- Bottom menu -->
	<div class="DivEpButton bottom">
		<asp:HyperLink ID="HYPbackBot" runat="server" CssClass="linkMenu"></asp:HyperLink>
	</div>
	<!-- End Bottom menu -->
</div>
</div>
	
	<!--START User DIALOG	-->
	<div class="view-modal view-users" runat="server" id="DIVusers" visible="false">
		<asp:checkbox ID="CBXhideToOwner" runat="server" Text="*Hide to owner" Checked="true" />
		<CTRL:SelectPerson ID="CTRLselectUsers" runat="server"
		RaiseCommandEvents="True" DisplayDescription="true"
		DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True"
		DefaultMaxPreviewItems="5"
		ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="True"
		SelectionMode="CommunityUsers" MultipleSelection="True"
		InModalWindow="true"
		/>  
	</div>
	<!--END User DIALOG	-->

	<asp:Literal ID="LTanchorTemplate" runat="server" Visible="false"><a name="{id}"></a></asp:Literal>
	
	<CTRL:Attachments ID="CTRLcommunityUpload" runat="server" visible="false"  UploaderCssClass="dlguploadtomoduleitemandcommunity"/>
   <CTRL:Attachments ID="CTRLlinkFromCommunity" runat="server" visible="false"  UploaderCssClass="dlglinkfromcommunity"/>

	<CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" />

	<asp:UpdatePanel ID="UPTempo" runat="server">
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
		</Triggers>
	</asp:UpdatePanel>
	<asp:Timer ID="TMsession" runat="server">
	</asp:Timer>

</asp:Content>