<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditResolver.aspx.vb" Inherits="Comunita_OnLine.EditResolver" 
	MaintainScrollPositionOnPostback="true" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="Categories" Src="~/Modules/Ticket/UC/UC_CategoryDDL.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MsgEdit" Src="~/Modules/Ticket/UC/UC_MessageEditRes.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="UserSelector" Src="~/Modules/Ticket/UC/UC_AssignerSelector.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="SelectPerson" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectPersonHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Repository/Common/UC_ModuleAttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/Ticket/UC/UC_TicketAddAttachment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsView" Src="~/Modules/Ticket/UC/UC_AttachmentsView.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettingsNew.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
	<asp:Literal ID="LTpageTitle_t" runat="server">*Lista Ticket</asp:Literal>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTcontentTitle_t" runat="server">*Lista Ticket</asp:Literal>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
	
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

	<link rel="Stylesheet" href="../../Graphics/Modules/Ticket/Css/tickets.css<%=CssVersion()%>" />
	<link media="print" href="../../Graphics/Modules/Ticket/Css/print.css<%=CssVersion()%>" rel="Stylesheet">
	<!-- JS service -->
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script> 
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsablerows.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
	<link rel="stylesheet" href="../../Jscript/Modules/Common/Choosen/chosen.css<%=CssVersion()%>" />
	
	<link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css<%=CssVersion()%>" />

	<script type="text/javascript" src="../../Jscript/Modules/Ticket/tickets.js"></script>

	<CTRL:SelectPersonHeader ID="CTRLselectUsersHeader" runat="server" />
	<CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" />

	<script language="javascript" type="text/javascript">

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

		$(function () {
			$(".dialog").dialog({
				modal: true,
				autoOpen: false,
				width: 800,
				height: 500,
				open: function (type, data) {
					$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
				}
			});

			$(".opendlgassigncategory").click(function () {
				//assigncategory
				$(".dialog.dlgassigncategory").dialog("open");
			});
			
		});

	</script>


</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="tickets">
	
	<div class="DivEpButton top">
		<asp:HyperLink ID="HYPbackTop" runat="server" CssClass="linkMenu">*Back</asp:HyperLink>
		<asp:HyperLink ID="HYPtoUserViewTop" runat="server" CssClass="linkMenu"></asp:HyperLink>
	</div>
	
	<asp:panel id="PNLerrorMsg" runat="server" CssClass="tickethistory edit " Visible="false">
		<div class="fieldobject">
			<div class="fieldrow">
				<CTRL:Messages ID="CTRLheadMessages" runat="server" Visible="false" />
			</div>
		</div>
	</asp:panel>

	<asp:Panel ID="PNLglobal" runat="server" CssClass="tickethistory edit ">
		

		<div class="fieldobject header">
			<div class="fieldrow status clearfix">
				<div class="left">
					<span class="requestid"><asp:Literal ID="LTrequest_t" runat="server">*Request id</asp:Literal>
						<span><asp:Literal ID="LTrequest" runat="server">#####</asp:Literal></span>
					</span>
					<span class="status"><asp:Literal ID="LTstatus_t" runat="server">*Status</asp:Literal>
						<span><asp:Literal ID="LTstatus" runat="server">#Status</asp:Literal></span>
					</span>
				</div>
				<div class="right">
					<span class="category">
						<span class="label"><asp:Literal ID="LTcategory_t" runat="server">*Category</asp:Literal></span>
						<span class="catname"><asp:Literal ID="LTcategory" runat="server">#Category</asp:Literal></span>
					</span>
				</div>
			</div>

			<div class="fieldrow subject clearfix">
				<div class="left">
					<h2><asp:Literal ID="LTtitle" runat="server">#title</asp:Literal></h2>
				</div>
				<div class="right">
					<span class="extra">
						<span class="toggler on" data-item="div.advancedsettings">
							<asp:literal ID="LTsettingsHideShow" runat="server"></asp:literal>
						</span>
					</span>
				</div>
			</div>

		</div>

		<div class="fieldobject advancedsettings">
			<div class="DivEpButton toolbar">
				<%--<asp:linkbutton ID="LNBassignCategoryOpen" runat="server" CssClass="linkMenu opendlgassigncategory">*Assegna categoria</asp:linkbutton>--%>
				<asp:Hyperlink ID="HYPcateOpen" runat="server" CssClass="linkMenu opendlgassigncategory">*Assegna categoria</asp:Hyperlink>
				<%--<div class="ddbuttonlist enabled">
					
				</div>--%>
				<%--<div class="ddbuttonlist enabled"><!--
				--><a class="linkMenu opendlgchangecategory"">Move to category</a><!--
			--></div>--%>
				<div class="ddbuttonlist enabled" id="DVddbuttonListAssign" runat="server"><!--
						--><asp:LinkButton id="LNBassignMe" runat="server" CssClass="linkMenu">*Assign to me</asp:LinkButton><!--
						--><asp:LinkButton id="LNBreassign" runat="server" CssClass="linkMenu opendlgassignresolvercategory">*Assign to Resolver</asp:LinkButton><!--
						--><asp:LinkButton id="LNBreassignCom" runat="server" CssClass="linkMenu opendlgassignresolvercommunity">*Assign to ComUser</asp:LinkButton><!--
				--></div>
				
				<asp:LinkButton ID="LNBsaveMailSet" runat="server" CssClass="linkMenu">*Save notification Settings</asp:LinkButton>
			</div>
			
			
			
			<div class="fieldobject ticketinfo clearfix">

<%--				<div class="fieldrow status clearfix">
					<asp:Label ID="LBstatus_t" runat="server" CssClass="fieldlabel">*Status</asp:Label>
					<span class="value inputwrapper">
						<asp:DropDownList ID="DDLstatus" runat="server">
							<asp:ListItem>...</asp:ListItem>
						</asp:DropDownList>
					</span>
					<span class="actions">
						<span class="action">
							<asp:LinkButton ID="LNBchangeStatus" CssClass="linkMenu" runat="server">*Change status</asp:LinkButton>
						</span>
					</span>
				</div>--%>

				<div class="fieldrow community clearfix">
					<asp:Label ID="LBcommunity_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcommunity">*Community</asp:Label>
					<asp:Label ID="LBcommunity" runat="server" CssClass="value">#Community</asp:Label>
				</div>

				<div class="fieldrow initcategory clearfix">
					<asp:Label ID="LBcategoryInit_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcategoryInit">*Initial category</asp:Label>
					<asp:Label ID="LBcategoryInit" runat="server" CssClass="value">#Category name</asp:Label>
				</div>

				<div class="fieldrow categorygroup clearfix">
					<div class="fieldrow assigncategory">
						<asp:Label ID="LBcategoryCur_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLddlCat">*Current category</asp:Label>
						<asp:Label ID="LBcategoryCurrent" runat="server" CssClass="value">#Category name</asp:Label>
						
						
<%--						<span class="actions">
							<span class="action">
								
							</span>
						</span>--%>
					</div>
				</div>

				<div class="fieldrow resolver clearfix">
					<asp:Label ID="LBassignTo_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBassignTo">*Assigned to</asp:Label>
					<asp:Label ID="LBassignTo" runat="server" CssClass="value">#Ass. Name</asp:Label>
					
					

					<%--<span class="inputwrapper">
						<span class="actions">
							<span class="action">
								
							</span>
						</span>
					</span>--%>
					<%--<span class="inputwrapper">
							<input id="showmessages" type="checkbox" name="showmessages" value="all"><label for="showmessages">Show messages</label>
					</span>--%>
				</div>

			</div>
			
			
			
			
			<div class="fieldrow notifications clearfix">
				<div class="fieldlabel title">
					<asp:literal id="LTnotification_t" runat="server">*eMail notification settings</asp:literal>
				</div>
				<%--<asp:Label runat="server" ID="LBnotification_t" CssClass="fieldlabel title" AssociatedControlID="CTRLmailSettings">*eMail notification settings</asp:Label>--%>
				
				<div class="fieldrow options email">
					<CTRL:MailSettings ID="CTRLmailSettings" runat="server" />
					<%--<div class="right"></div>--%>
				</div>

<%--					<asp:literal ID="LTcreatorInfo" runat="server" Visible="false">*no notifiche se non diretti o configurati</asp:literal>--%>
			</div>
				
				<%--<div class="fieldrow notifications clearfix" style="">
					<asp:Label runat="server" ID="LBnotification_t" CssClass="fieldlabel title" AssociatedControlID="CTRLmailSetCreator">*eMail notification settings</asp:Label>
					<div class="fieldrow options email">
						<asp:Label runat="server" ID="LBMailSet_t" AssociatedControlID="CTRLmailSetCreator" CssClass="optionlabel" ></asp:Label>
						<span class="inlinewrapper">
							<CTRL:MailSettings ID="CTRLmailSetCreator" runat="server" />
							<asp:literal ID="LTcreatorInfo" runat="server" Visible="false"></asp:literal>
						</span>
					</div>
				</div>--%>
			
			
			

		</div>

		<div class="fieldobject toolbar clearfix">

			<div class="right fieldrow">
				<asp:Label ID="LBshow_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LNBshowAll"></asp:Label>
				<%--<span class="fieldlabel">
					<asp:Literal ID="LTswhow_t" runat="server">Show</asp:Literal>
				</span>--%>
				<span class="btnswitchgroup small"><!--
					--><asp:LinkButton ID="LNBshowAll" runat="server" CssClass="btnswitch first active">*All</asp:LinkButton><!--
					--><asp:LinkButton ID="LNBshowMsg" runat="server" CssClass="btnswitch">*Messages Only</asp:LinkButton><!--
					--><asp:LinkButton ID="LNBshowSys" runat="server" CssClass="btnswitch last">*Notifies Only</asp:LinkButton><!--
				--></span>
			</div>

			<div class="left fieldrow">
				<asp:Label ID="LBsort_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LNBorderOlder"></asp:Label>
				<%--<span class="fieldlabel"><asp:literal id="LTsort_t" runat="server">*Sort</asp:literal></span>--%>
				<span class="btnswitchgroup small"><!--
					--><asp:LinkButton ID="LNBorderOlder" runat="server" CssClass="btnswitch first active">Recent to older</asp:LinkButton><!--
					--><asp:LinkButton ID="LNBorderRecent" runat="server" CssClass="btnswitch last">Recent to older</asp:LinkButton><!--
				--></span>
			</div>
		</div>
		
		<div class="historywrapper collapsablerows recentolder" id="DIVhystoryWrapper" runat="server">
			
			<asp:Literal ID="LTmessagesExpCol" runat="server">
				<div class="viewoptions clearfix">
					<div class="visibilitynav">
						<label class="fieldlabel">{4}</label>
						<span class="collapseall" title="{0}">{1}</span>
						<span class="expandall" title="{2}">{3}</span>
					</div>
				</div>
			</asp:Literal>

			<asp:PlaceHolder ID="PHmessageEditorTop" runat="server">
				<a name="editor"></a>
				<div class="row message reply clearfix first">
					<CTRL:MsgEdit ID="CTRLeditTop" runat="server" />
				</div>
			</asp:PlaceHolder>
			
			
			<asp:Repeater ID="RPTmessages" runat="server">
				<ItemTemplate>
<%--					<asp:Literal ID="LTcontainter" runat="server">
					<div class="{CssClass}" id="row-{RowId}">
					</asp:Literal>--%>
					<asp:Literal ID="LTanchors" runat="server"></asp:Literal>
					<div class="<%#getRowCss(Container.DataItem) %>" id="row-<%#Container.DataItem.MessageId %>">
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
													<asp:LinkButton id="LNBhide" runat="server" CssClass="btnhide"></asp:LinkButton>
													<asp:LinkButton id="LNBshow" runat="server" CssClass="btnshow"></asp:LinkButton>
													<%--<a href="" class="quote">Quote</a>--%>
												</span>
											</span>
										</div>
									</div>
									<div class="messagecontent">
										<asp:Literal ID="LTtext" runat="server"></asp:Literal>
									</div>
									<%--<asp:literal id="LTcssFooter" runat="server">
									
									</asp:literal>--%>
									<div class="messagefooter <%#getItemEmptycss(Container.DataItem) %>">
										<CTRL:AttachmentsView ID="CTRLattView" runat="server" />

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

			<asp:PlaceHolder ID="PHmessageEditorBottom" runat="server">
				<a name="editor"></a>
				<div class="row message reply clearfix last">
					<CTRL:MsgEdit ID="CTRLeditBottom" runat="server" />
				</div>
			</asp:PlaceHolder>

		</div>
		

		<%--<div class="fieldrow mailsettings" style="display:none">
			<div class="fieldobject">
				<span class="description">
					**Impostazioni notifiche personali:
				</span>
				<CTRL:MailSettings ID="CTRLmailSettings" runat="server" ShowDefault="true" CssItem="item" CssDefault="item default"/>
			</div>
			
		</div>--%>
<%--	</div>--%>
	</asp:Panel>
	<asp:Panel ID="PNLerrors" runat="server" CssClass="tickets tickethistory edit error">
		<asp:Label ID="LBerrors" runat="server"></asp:Label>
	</asp:Panel>



	<div class="DivEpButton bottom">
		<asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="linkMenu">*Back</asp:HyperLink>
	</div>

	
	<CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" />
	<CTRL:Attachments ID="CTRLcommunityUpload" runat="server" visible="false"  UploaderCssClass="dlguploadtomoduleitemandcommunity"/>
   <CTRL:Attachments ID="CTRLlinkFromCommunity" runat="server" visible="false"  UploaderCssClass="dlglinkfromcommunity"/>
	<div class="view-modal view-users" id="DVselectUsers" runat="server" visible="false">
		<asp:Panel ID="PNLSelectorInternal" runat="server">
			<CTRL:UserSelector ID="CTRLuserSelector" runat="server" />
		</asp:Panel>
		<asp:Panel ID="PNLSelectorCommunity" runat="server">
			<CTRL:SelectPerson ID="CLTRselectPerson" runat="server"
				RaiseCommandEvents="True" DisplayDescription="true"
				DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True"
				DefaultMaxPreviewItems="5" InModalWindow="true"
				ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false"
				SelectionMode="CommunityUsers" MultipleSelection="false"
				/>
		</asp:Panel>
	</div>
	
<%--	<div class="dialog dlgchangecategory" title="*Change Category">
		<div class="fieldobject changecategory">
			<div class="fieldrow description">
				<div class="description">
					*Description...
				</div>
			</div>
			
			<div class="fieldrow initcategory">
				<span class="fieldlabel"><asp:literal ID="LTcatPPInit_t" runat="server">*Initial Category</asp:literal></span>
				<span class="value"><asp:literal ID="LTcatPPInit" runat="server">*Initial Category</asp:literal></span>
			</div>
			
			<div class="fieldrow assigncategory">
				<asp:Label runat="server" ID="LBcatPPcur_t" AssociatedControlID="CTRLddlCat"></asp:Label>
				
			</div>

			 <div class="fieldrow clearfix commands">
				<div class="left">
					&nbsp;
				</div>
				<div class="right">
					<a class="linkMenu" href="">*cancel</a>
					
				</div>
			</div>
	</div>--%>


</div><!-- End div tickets -->
	
	
	<div class="dialog dlgassigncategory" title="<%=getCatModifyTitle() %>">
		<div class="fieldobject assigncategory">
			<div class="fieldrow description">
				<div class="description">
					<asp:literal id="LTcatModify_t" runat="server"></asp:literal>
				</div>
			</div>

			<div class="fieldrow initcategory">
				<asp:Label runat="server" ID="LBLcatModInit_t" AssociatedControlID="LBLcatModInit" CssClass="fieldlabel"></asp:Label>
				<asp:Label runat="server" ID="LBLcatModInit" CssClass="value"></asp:Label>
			</div>
		
			<div class="fieldrow assigncategory">
				<asp:label id="LBcatModCurrent_t" runat="server" cssclass="fieldlabel">*Current Category</asp:label>
				<!-- START DROPDOWN JS-->
				<CTRL:Categories id="CTRLddlCat" runat="server" RequiredValidator="true"></CTRL:Categories>
				<!--  END DROPDOWN JS -->
				<span class="actions">
					<span class="action">
					</span>
				</span>
			</div>
		
			<div class="fieldrow clearfix commands">
				<div class="left">
					&nbsp;
				</div>
				<div class="right">
					<%--<a class="linkMenu" href="">cancel</a>
						<a class="linkMenu" href="">Ok</a>--%>
					<asp:LinkButton ID="LNBassCateUndo" runat="server" CssClass="linkMenu">*Annulla</asp:LinkButton>
					<asp:LinkButton ID="LNBassCate" runat="server" CssClass="linkMenu">*Assign category</asp:LinkButton>
				</div>
			</div>

		</div>
	</div>
	

	<asp:UpdatePanel ID="UPTempo" runat="server">
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="TMsession" EventName="Tick" />
		</Triggers>
	</asp:UpdatePanel>
	<asp:Timer ID="TMsession" runat="server">
	</asp:Timer>

	<asp:Literal ID="LTanchorTemplate" runat="server" Visible="false"><a name="{id}"></a></asp:Literal>

	<asp:literal ID="LTsettingHideShow_template" runat="server" Visible="False">
		<span class="toggle on" title="{2}">{3}</span>
		<span class="toggle off" title="{0}">{1}</span>
	</asp:literal>
</asp:Content>