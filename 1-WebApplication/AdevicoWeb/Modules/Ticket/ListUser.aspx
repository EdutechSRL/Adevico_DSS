<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ListUser.aspx.vb" Inherits="Comunita_OnLine.ListUser" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettings.ascx" %>


<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="ListInfo" Src="~/Modules/Ticket/UC/UC_ListInfo.ascx" %>

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

	<script type="text/javascript">
		$(function() {

			$(".viewextra input[type='checkbox']").each(function() {
				var check = $(this).is(":checked");
				var info = $(this).parents(".viewextra").first().data("info");
				if (!check) {
					$(".extrainfo." + info).hide();
				}
				var $table = $(".table.ticketlist");
				var $datacolspan = $table.find("td[data-colspan]");

				var $visiblecols = $table.find("th:visible").size();

				$datacolspan.each(function() {
					var offset = $(this).data("colspan");
					$(this).attr("colspan", $visiblecols - offset);
				});
			});
			$(".viewextra input[type='checkbox']").change(function() {
				var info = $(this).parents(".viewextra").first().data("info");
				if ($(this).is(":checked")) {
					$(".extrainfo." + info).show();
				} else {
					$(".extrainfo." + info).hide();
				}

				var $table = $(".table.ticketlist");
				var $datacolspan = $table.find("td[data-colspan]");


				var $visiblecols = $table.find("th:visible").size();


				$datacolspan.each(function() {
					var offset = $(this).data("colspan");
					$(this).attr("colspan", $visiblecols - offset);
				});
			});

			//Per aggiungere la classe opportuna alla label delle radiobutton
			//$("div.hideme span.input-group label").addClass("label ng-binding");
			
		});
	</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	
	<div class="ticketlist ticketlist-user">

		<div class="DivEpButton">
			<asp:HyperLink ID="HYPadd" runat="server" CssClass="linkMenu">*Apri nuovo</asp:HyperLink>
			<asp:HyperLink ID="HYPmanagement" runat="server" CssClass="linkMenu">*Management</asp:HyperLink>
		</div>


		<asp:Panel ID="PNLmessages" runat="server" CssClass="fieldobject messages">
			<div class="fieldrow">
				<CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false" />
			</div>
		</asp:Panel>

		<div class="filters clearfix collapsable"><%--fieldobject filters clearfix--%>
			
			<div class="sectionheader clearfix"> 
				

				<div class="left">
					<h3 class="sectiontitle clearifx">
						<asp:literal id="LTfilterTitle" runat="server">*Search tools</asp:literal>
						<%--<asp:Label runat="server" ID="LBfilterTitle">Search tools</asp:Label>--%>
						<span class="extrainfo expander">
							<asp:Label runat="server" ID="LBfiltersShow" CssClass="on">click to expand</asp:Label>
							<asp:Label runat="server" ID="LBfiltersHide" CssClass="off">click to collapse</asp:Label>
						</span>
					</h3>
				</div>
				<div class="right hideme"></div>
			</div>
			
			<div class="hideme">
				
				<div class="filtercontainer container_12 clearfix">
					
					<div class="filter grid_6 select">
						<div class="filterinner">
							<asp:Label ID="LBStatus_t" runat="server" CssClass="title" >*Stato</asp:Label><%--AssociatedControlID="DDLStatus"--%>
							<span class="content">
								<asp:DropDownList ID="DDLStatus" runat="server" CssClass="input"></asp:DropDownList>            
							</span>
						</div>
					</div>
					
					<div class="filter grid_6 text">
						<div class="filterinner">
							<asp:Label ID="LBtitle_t" runat="server" CssClass="title">*Titolo</asp:Label><%--AssociatedControlID="TXBtitle_t"--%>
							<span class="content">
								<asp:TextBox ID="TXBtitle_t" runat="server" CssClass="input"></asp:TextBox>
							</span>
						</div>
					</div>
					
					<%--<asp:panel ID="PNLbehalf" runat="server">--%>
						<div class="filter grid_6 radio" id="DVbehalf1" runat="server">
							<div class="filterinner">
								<asp:Label runat="server" ID="LBownedBy" CssClass="title"></asp:Label>
								<span class="content">
									<asp:radiobuttonlist ID="RBLbehalf" runat="server"
										RepeatLayout="Flow" RepeatDirection="Horizontal"
										CssClass="input-group">
										<asp:ListItem Text="*All" Value="-1" Selected="True" class="input"></asp:ListItem>
										<asp:ListItem Text="*Only my" Value="0" class="input"></asp:ListItem>
										<asp:ListItem Text="*Only behalf" Value="1" class="input"></asp:ListItem>
									</asp:radiobuttonlist>
								</span>
							</div>
						</div>
						<div class="filter grid_6 text" id="DVbehalf2" runat="server">
							<div class="filterinner">
								<asp:Label ID="LBbehalfOt_t" runat="server" CssClass="title" >*Behalf of:</asp:Label><%--AssociatedControlID="TXBtitle_t"--%>
								<span class="content">
									<asp:TextBox ID="TXBbehalf" runat="server" CssClass="input"></asp:TextBox>	
								</span>
							</div>
						</div>
					<%--</asp:panel>--%>
					
					<div class="filter grid_12 textx2select"><!-- con radio al posto di textx2select in parte si risolve. -->
						<div class="filterinner">
							<asp:Label ID="LBdate_t" runat="server" CssClass="title">*Per data di</asp:Label>
						
							<span class="content">

								<%--<asp:RadioButtonList runat="server" ID="RBLdateField" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="input">
									<asp:ListItem Text="*Creazione" Value="CreatedOn" class="input-group"></asp:ListItem>
									<asp:ListItem Text="*Ultima modifica" Value="LastModify" class="input-group"></asp:ListItem>
								</asp:RadioButtonList>--%>
								<asp:DropDownList runat="server" ID="DDLdateField" CssClass="input">
									<asp:ListItem Text="*Creazione" Value="CreatedOn"></asp:ListItem>
									<asp:ListItem Text="*Ultima modifica" Value="LastModify"></asp:ListItem>
								</asp:DropDownList>
								
								<span class="input-group first datetimepicker">
									<%--<asp:CheckBox id="CBXStart" runat="server" text="*dopo il" CssClass="title"/>--%>
									<asp:Label runat="server" ID="LBfiltDateStart" AssociatedControlID="RDPstart">*after</asp:Label>
									<telerik:RadDateTimePicker id="RDPstart" runat="server" TimeView-StartTime="0" TimeView-EndTime="23:59" TimeView-Interval="01:00" TimeView-Columns="4" ></telerik:RadDateTimePicker>
								</span>
								<span class="input-group last datetimepicker">
									<%--<asp:CheckBox id="CBXend" runat="server" Text="*prima del"  CssClass="title"/>--%>
									<asp:Label runat="server" ID="LBfiltDateEnd" AssociatedControlID="RDPend">*befor</asp:Label>
									<telerik:RadDateTimePicker id="RDPend" runat="server" TimeView-StartTime="0" TimeView-EndTime="23:59" TimeView-Interval="01:00" TimeView-Columns="4" ></telerik:RadDateTimePicker>
								</span>

							</span>
						</div>

					</div>

					<div class="filter grid_12 checkbox notitle">
						<div class="filterinner">
							<span class="input-group">
								<asp:CheckBox ID="CBXupdatedOnly" Text="*Solo con aggiornamenti" runat="server" CssClass="input"/>
							</span>
						</div>
					</div>

				</div>

				
				<%--<div class="fieldrow filterstatus">
					label status
					DDL status
				</div>
				<div class="fieldrow filtertitle">
					
				</div>
				<div class="fieldrow filterupdated">
					
				</div>
				<br />
					
					<asp:panel ID="PNLbehalf" runat="server">
					<div class="fieldrow filteronbehalfof">
						
					</div>
					<div class="fieldrow filteropenedonbehalfof">
				<asp:radiobuttonlist ID="RBLbehalf" runat="server"
							RepeatLayout="Flow" RepeatDirection="Horizontal">
							<asp:ListItem Text="*All" Value="-1" Selected="True" class="behalfof"></asp:ListItem>
							<asp:ListItem Text="*Only my" Value="0" class="behalfof"></asp:ListItem>
							<asp:ListItem Text="*Only behalf" Value="1" class="behalfof"></asp:ListItem>
						</asp:radiobuttonlist>		
					</div>

				</asp:panel>

				<div class="fieldrow filterdatetype">
					
					<asp:DropDownList ID="DDLdateField" runat="server">
						<asp:ListItem Text="*Creazione" Value="CreateOn"></asp:ListItem>
						<asp:ListItem Text="*Ultima modifica" Value="LastModify"></asp:ListItem>
					</asp:DropDownList>
				</div>
				<div class="fieldrow date after">
					
				</div>
				<div class="fieldrow date before">
					
				</div>--%>
			</div>
			
			<div class="sectionfooter hideme">
				<div class="viewbuttons bottom">
					<asp:LinkButton ID="LNBfilter" runat="server" CssClass="Link_Menu">*Filtra</asp:LinkButton>
				</div>
			</div>
			<%--<div class="fieldrow filterupdate right">
				button				
			</div>--%>
		</div>
		
		<asp:Panel ID="PNLdetails" CssClass="fieldobject details" runat="server">
			<!-- BEGIN - Details -->
				<div class="fieldrow">
					<asp:Label ID="LBusr_t" runat="server" CssClass="fieldlabel">*User</asp:Label>
					<asp:Label ID="LBusr" runat="server" CssClass="username">### ###</asp:Label>
					<div class="statuswrapper">
						<span class="status completion details first" runat="server" id="SPNinfoUser">
							<asp:Label ID="LBnumTicket_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLlistInfo">*Tickets:</asp:Label>
							<div class="inlinewrapper">
								<%--<asp:Literal ID="LTnumDraft" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumOpen" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumInProg" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumRequest" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumSolved" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumClosed" runat="server"></asp:Literal>--%>
								<CTRL:ListInfo ID="CTRLlistInfo" runat="server" />
							</div>
						</span>
					
						<span class="status completion details onbehalf last" runat="server" id="SPNinfoBehalf">
						
							<asp:Label ID="LBnumBehalf_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLlistInfoBH">*Behalf:</asp:Label>
							<div class="inlinewrapper">
								<%--<asp:Literal ID="LTnumBHDraft" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumBHOpen" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumBHInProg" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumBHRequest" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumBHSolved" runat="server"></asp:Literal>
								<asp:Literal ID="LTnumBHClosed" runat="server"></asp:Literal>--%>
								<CTRL:ListInfo ID="CTRLlistInfoBH" runat="server" />
							</div>
						</span>
					</div>
					
					<%--<asp:literal id="LTInfocssContainer" runat="server" Visible="false">status completion details</asp:literal>--%>
					<%--<asp:Literal ID="LTnumLayout" runat="server" Visible="false">
							<span class="item statusitem open {emptyCss}">
								<span id="Span3" title="{title}" class="counter {field}">{num}</span>                       
								<span id="Span4" class="label">{text}</span>
							</span>
					</asp:Literal>--%>
				</div>
			<!-- END - Details -->
		</asp:Panel>

		<asp:panel ID="PNLcolumns" CssClass="fieldobject clearfix" runat="server">
			<!-- BEGIN - hide/Show Columns -->
			<div class="fieldrow commands viewoptions"> 
				<span class="viewextra preview" data-info="preview">
					<asp:CheckBox id="CBXshowPreview" runat="server" Checked="true" Text="*Show ticket preview" />
				</span>
				<span class="viewextra" data-info="id">
					<asp:CheckBox id="CBXshowId" runat="server" Text="*Show ticket ID" />
				</span>
				<span class="viewextra" data-info="category">
					<asp:CheckBox id="CBXshowCategory" runat="server" Text="*Show category" />
				</span>
				<span class="viewextra" data-info="lastupdate">
					<asp:CheckBox id="CBXshowLastUpd" runat="server" Text="*Show last update" />
				</span>
			</div>
			<!-- END - hide/Show Columns -->
		</asp:panel>

		<div class="tablewrapper clearfix">
			
			<asp:Repeater ID="RptTickets" runat="server">
				<HeaderTemplate>
				<table class="table light ticketlist user" >
							<thead>
								<tr>
									<th class="id extrainfo">
										<span>
											<asp:Literal ID="LTcode_t" runat="server">*Cod.</asp:Literal>
										</span>
										<asp:LinkButton ID="LNBorderByIdUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
										--><asp:LinkButton ID="LNBorderByIdDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</th>
									<th class="subject">
										<span>
											<asp:Literal ID="LTtitle_t" runat="server">*Titolo</asp:Literal>
										</span>
										<asp:LinkButton ID="LNBorderByTitleUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
										--><asp:LinkButton ID="LNBorderByTitleDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</th>
									<th class="extrainfo category">
										<span>
											<asp:Literal ID="LTcategory_t" runat="server">*Category</asp:Literal>
										</span>
										 <asp:LinkButton ID="LNBorderByCatUp" runat="server" cssclass="icon orderUp"
										CommandArgument="ByCat.True" CommandName="orderby"></asp:LinkButton><!--
										--><asp:LinkButton ID="LNBorderByCatDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</th>
									
									
									<th class="behalf" id="THbehalf" runat="server">
										<span>
											<asp:Literal ID="LTbehalft_t" runat="server">*Owner...</asp:Literal>
										</span>
										 <asp:LinkButton ID="LNBorderByBehalfUp" runat="server" cssclass="icon orderUp"
										CommandArgument="ByCat.True" CommandName="orderby"></asp:LinkButton><!--
										--><asp:LinkButton ID="LNBorderByBehalfDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</th>
									

									<th class="status">
										<span>
											<asp:Literal ID="LTstatus_t" runat="server">*Stato</asp:Literal>
										</span>
										<asp:LinkButton ID="LNBorderByStatusUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
										--><asp:LinkButton ID="LNBorderByStatusDown" runat="server" cssclass="icon orderDown" ></asp:LinkButton>
									</th>
									<th class="time">
										<span>
											<asp:Literal ID="LTlastModify_t" runat="server">*Submitted</asp:Literal>
										</span>
										<asp:LinkButton ID="LNBorderByLastModifyUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
										--><asp:LinkButton ID="LNBorderByLastModifyDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</th>

									<th class="actions" runat="server" id="THaction">
										<span class="icons">
											<span class="icon actions" title="<%=GetActionTitle()%>"></span>
										</span>
									</th>
								</tr>
							</thead>
							<tbody>
				</HeaderTemplate>
				<ItemTemplate>
								<tr id="trItem" runat="server">
								
									<td class="id extrainfo">
										<asp:Label ID="LBcode" runat="server">#Cod.</asp:Label>
									</td>
									<td class="subject">
										<asp:HyperLink ID="HYPsubject" runat="server">#Titolo</asp:HyperLink>
										<div class="extrainfo preview"><asp:Literal id="LTDescription" runat="server"></asp:Literal></div>
									</td>
									<td class="extrainfo category">
										<asp:Label ID="LBcategory" runat="server">#Category</asp:Label>
									</td>
									<td class="behalf" id="TDbehalf" runat="server">
										<asp:Label ID="LBbehalf" runat="server">#behalf</asp:Label>
									</td>
									<td class="status">
										<asp:Label ID="LBstatus" runat="server">#Status</asp:Label>
									</td>
									<td class="time">
										<asp:Label ID="LBtime" runat="server">#DateTime</asp:Label>
										<br/>
										<span class="extrainfo lastupdate">
											<asp:literal ID="LTtimeMore" runat="server">
												*Updated: 01/01/2013 16:15
											</asp:literal>
										</span>
									</td>
									<td class="actions" runat="server" id="TDaction">
										<span class="icons">
											<asp:LinkButton ID="LNBdelete" runat="server" CssClass="icon delete" Visible="false" ToolTip="*Delete draft">*Delete draft</asp:LinkButton>
										</span>
									</td>
								</tr>
				</ItemTemplate>				
				<FooterTemplate>
					<asp:PlaceHolder ID="PLHfooterVoid" runat="server">
								<tr class="empty norecordrow">
									<td colspan="6" data-colspan="0">
										<asp:Literal ID="LTempty" runat="server">Nessun ticket trovato.</asp:Literal>
									</td>
								</tr>
					</asp:PlaceHolder>
							</tbody>
						</table>
				</FooterTemplate>
			</asp:Repeater>			
		
			<!-- Table bottom -->
			<div class="tablebottom clearfix">
			
				<div class="right pager bottom">
					<CTRL:GridPager ID="PGgridBot" runat="server" EnableViewState="true"
						ShowNavigationButton="false" EnableQueryString="false"
						Visible="false">
					</CTRL:GridPager>
				</div>

				<!-- LEGEND -->
				<span class="fieldrow legend hor">
					<span class="fieldlabel">
						<asp:literal id="LTleged_t" runat="server">*Legend</asp:literal>
					</span>
					<span class="group leftgroup first last">
						<span class="legenditem isdraft" title="<%=GetLegendTitle("draft")%>">
							<span class="legendicon isdraft">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("draft")%>
							</span>
						</span>
						<span class="legenditem hasnews" title="<%=GetLegendTitle("hasnews")%>">
							<span class="legendicon hasnews">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("hasnews")%>
							</span>
						</span>
					</span>
<%--					<span class="group rightgroup last">
						<span class="legenditem islocked" title="<%=GetLegendTitle("locked")%>">
							<span class="legendicon islocked">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("locked")%>
							</span>
						</span>
						<span class="legenditem isreported" title="<%=GetLegendTitle("reported")%>">
							<span class="legendicon isreported">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("reported")%>
							</span>
						</span>
						<span class="legenditem isdeleted" title="<%=GetLegendTitle("deleted")%>">
							<span class="legendicon isdeleted">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("deleted")%>
							</span>
						</span>
					</span>--%>
				</span>
				<!-- /LEGEND -->
			</div>

		</div>
		
<%--		<div class="fieldrow mailsettings" style="display:none;">
			<div class="fieldobject">
				<span class="description">
					**Impostazioni notifiche personali:
				</span>
				<CTRL:MailSettings ID="CTRLmailSettings" runat="server" ShowDefault="true" CssItem="item" CssDefault="item default"/>
			</div>
			
		</div>--%>

	</div>
</asp:Content>