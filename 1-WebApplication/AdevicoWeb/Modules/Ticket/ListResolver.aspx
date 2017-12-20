<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ListResolver.aspx.vb" Inherits="Comunita_OnLine.ListResolver" %>
<%--Nomi Standard: OK--%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="CTRL" TagName="Categories" Src="~/Modules/Ticket/UC/UC_CategoryDDL.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>

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
		$(function () {

			$(".viewextra input[type='checkbox']").each(function () {
				var check = $(this).is(":checked");
				var info = $(this).parents(".viewextra").first().data("info");
				if (!check) {
					$(".extrainfo." + info).hide();
				}
				var $table = $(".table.ticketlist");
				var $datacolspan = $table.find("td[data-colspan]");

				var $visiblecols = $table.find("th:visible").size();

				$datacolspan.each(function () {
					var offset = $(this).data("colspan");
					$(this).attr("colspan", $visiblecols - offset);
				});
			});
			$(".viewextra input[type='checkbox']").change(function () {
				var info = $(this).parents(".viewextra").first().data("info");
				if ($(this).is(":checked")) {
					$(".extrainfo." + info).show();
				} else {
					$(".extrainfo." + info).hide();
				}

				var $table = $(".table.ticketlist");
				var $datacolspan = $table.find("td[data-colspan]");


				var $visiblecols = $table.find("th:visible").size();
				

				$datacolspan.each(function () {
					var offset = $(this).data("colspan");
					$(this).attr("colspan", $visiblecols - offset);
				});
			});

			//Per aggiungere la classe opportuna alla label delle radiobutton
			//$("div.hideme span.input-group label").addClass("label ng-binding");
		});
	</script>
	
	<script type="text/javascript">
		$(function () {

			$(".ddbuttonlist.enabled").dropdownButtonList();

			$(".switch").click(function () {
				$(this).parents().first().find("table").toggleClass("bulkoff");
				return false;
			});

			$("table .headercheckbox input[type='checkbox']").change(function () {
				var $checkbox = $(this);
				var $table = $checkbox.parents("div.tablewrapper table");
				var ischecked = $checkbox.is(":checked");
				var $rows = $table.children("tbody").children("tr").find(".submittercheckbox input[type='checkbox']").attr("checked", ischecked);
			});

			$("table .submittercheckbox input[type='checkbox']").change(function () {
				var $checkbox = $(this);
				var $table = $checkbox.parents("div.tablewrapper table");
				var checked = $table.find(".submittercheckbox input[type='checkbox']:checked").size();
				var total = $table.find(".submittercheckbox input[type='checkbox']").size();

				if (total != checked) {
					$table.find(".headercheckbox input[type='checkbox']").attr("checked", false);
				} else {
					$table.find(".headercheckbox input[type='checkbox']").attr("checked", true);
				}

			});

			$(".table th input[type='checkbox']").change(function () {
				var $this = $(this);
				$(this).parents("table").first().find("td input[type='checkbox']").prop("checked", $this.is(":checked"));

				var $el;
				var $elout;

				var ultrafast = 1;
				var fast = 200;
				var slow = 3000;

				if ($this.is(":checked")) {
					$el = $this.siblings(".selectorpopup.checkall");
					$elout = $this.siblings(".selectorpopup.checknone");
				} else {
					$el = $this.siblings(".selectorpopup.checknone");
					$elout = $this.siblings(".selectorpopup.checkall");
				}

				if ($el.size() > 0) {
					$el.fadeIn(fast).addClass("open");
					var ovt = setTimeout(function () { $el.fadeOut(fast, function () { $el.removeClass("open"); }); clearTimeout(ovt); }, slow);
				}
				if ($elout.size() > 0) {
					$elout.fadeOut(ultrafast, function () { $elout.removeClass("open"); });
				}

			});
		});
	</script>



</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="tickets">
<%--    <asp:literal id="LTcategoryTemplate" runat="server" Visible="false"><span>*</span></asp:literal>
	<asp:literal id="LTcategoryTRclass" runat="server" Visible="false">oldCategory</asp:literal>--%>
	<!-- BEGIN Top Button -->
	<div class="DivEpButton top">
		<asp:HyperLink ID="HYPbackTop" runat="server" CssClass="linkMenu" Visible="false">*Back</asp:HyperLink>
		<asp:HyperLink ID="HYPaddTop" runat="server" CssClass="linkMenu">*Apri nuovo</asp:HyperLink>
		<asp:HyperLink ID="HYPmineTop" runat="server" CssClass="linkMenu">*MyTicket</asp:HyperLink>
	</div>
	<!-- END Top Button -->

	<asp:Panel ID="PNLcontent" runat="server" CssClass="tickets ticketlist manager clearfix"  Visible="true">
	
		<!-- New Filters -->
		<div class="filters clearfix collapsable">
			
			<!-- Header -->
			<div class="sectionheader clearfix">
				
				<div class="left">
					<h3 class="sectiontitle clearifx">
						<asp:Label runat="server" ID="LBfilterTitle">Search tools</asp:Label>
						<span class="extrainfo expander">
							<asp:Label runat="server" ID="LBfiltersShow" CssClass="on">click to expand</asp:Label>
							<asp:Label runat="server" ID="LBfiltersHide" CssClass="off">click to collapse</asp:Label>
						</span>
					</h3>
				</div>
				<div class="right hideme"></div>
			</div>
			
			<!-- filtri hideme -->
			<div class="hideme">
				<!-- Per il momento tolta la parte "angular". -->

				<div class="filtercontainer container_12 clearfix">
					
					<div class="filter grid_6 select">
						<div class="filterinner">
							<asp:Label ID="LBstatus_t" runat="server" CssClass="title">#Status</asp:Label>
							<%--AssociatedControlID="DDLstatus"--%>
							<span class="content">
								<asp:DropDownList ID="DDLstatus" runat="server" CssClass="input"></asp:DropDownList>		    
							</span>
						</div>
					</div>

					<div class="filter grid_6 text">
						<div class="filterinner">
							<asp:Label ID="LBsubject_t" runat="server" CssClass="title">*Subject: </asp:Label>
							<span class="content">
								<asp:TextBox ID="TXBsubject" runat="server" CssClass="input"></asp:TextBox>        
							</span>
						</div>
					</div>
					
					 <div class="filter grid_6 checkbox">
						<div class="filterinner">
							<asp:label id="LBfiltcbxLeg_t" runat="server" CssClass="title"></asp:label>
							<span class="content">
								<asp:CheckBox ID="CBXonlyNoAnswers" runat="server" Text="*No Answers" CssClass="input-group" />
								<asp:CheckBox ID="CBXonlyNotAssigned" runat="server" Text="*Not assigned" CssClass="input-group"/>
								<asp:CheckBox ID="CBXonlyWithNews" runat="server" Text="*With News" CssClass="input-group"/>
							</span>
						</div>
					</div>
					
					<div class="filter grid_6 select">
						<div class="filterinner">
							<asp:Label ID="LBlang_t" runat="server" CssClass="title">*Language :</asp:Label>
							<span class="content">
								<asp:DropDownList ID="DDLlang" runat="server" CssClass="input"></asp:DropDownList>
							</span>
						</div>
					</div>
					
					<div class="filter grid_12 textx2select">
						<div class="filterinner">
							
							<asp:Label ID="LBdateField_t" runat="server" CssClass="title">*Data :</asp:Label>
							<span class="content">
								<asp:DropDownList ID="DDLdateField" runat="server" CssClass="input"></asp:DropDownList>

								<span class="input-group">  
									<%--<asp:CheckBox id="CBXStart" runat="server" text="*dopo il" CssClass="input" />--%>
									<asp:Label runat="server" ID="LBfiltDateStart" CssClass="label" AssociatedControlID="RDPstart">*after</asp:Label>
									<span class="input">
										<telerik:RadDateTimePicker id="RDPstart" runat="server" 
											TimeView-StartTime="0" TimeView-EndTime="23:59" TimeView-Interval="01:00" 
											TimeView-Columns="4" ></telerik:RadDateTimePicker>
									</span>
								</span>

								<span class="input-group">  
								<%--<asp:CheckBox id="CBXend" runat="server" Text="*prima del" CssClass="input"/>--%>
								<asp:Label runat="server" ID="LBfiltDateEnd" CssClass="label" AssociatedControlID="RDPend">*befor</asp:Label>
									<span class="input">
										<telerik:RadDateTimePicker id="RDPend" runat="server" 
											TimeView-StartTime="0" TimeView-EndTime="23:59" TimeView-Interval="01:00" 
											TimeView-Columns="4" ></telerik:RadDateTimePicker>
									</span>
								</span>

							</span>

						</div>
					</div>
					
					<div class="filter grid_12 select dropdown">
						<div class="filterinner">
							<asp:Label ID="LBcategory_t" runat="server" CssClass="title">*Category :</asp:Label>
							<span class="content">
								<CTRL:Categories id="CTRLddlCat" runat="server" RequiredValidator="true"></CTRL:Categories>
							</span>
							
						</div>
					</div>
				</div>
			</div>
			<!-- /filtri hideme -->
			
			<!-- filter footer -->
				
			<div class="sectionfooter hideme">
				<div class="viewbuttons bottom">
					<asp:LinkButton ID="LNBfilter" runat="server" CssClass="Link_Menu">*Filtra</asp:LinkButton>
				</div>
			</div>

			<!-- filter footer -->

		</div>
		<!-- end new filters -->
<%--		<!-- BEGIN Filters -->
		<div class="fieldobject filters">
			<div class="fieldrow filterstatus"></div>
			<div class="fieldrow filtertitle"></div>
			<div class="fieldrow filterupdated"></div>
			<br />
			<div class="fieldrow filterlanguage"></div>
			<div class="fieldrow filterdatetype"></div>
			<div class="fieldrow date after"></div>
			<div class="fieldrow date before"></div>

			<div class="fieldrow block clearfix">
				<div class="fieldrow filtercategory">
					
				</div>
			
				<div class="fieldrow filterupdate right">
					
				</div>

			</div>

			<div class="clearfix"></div>
		</div>
		<!-- END Filters -->--%>

		<asp:Panel ID="PNLdetails" CssClass="fieldobject details" runat="server">
			<!-- BEGIN - Details -->
			<div class="fieldrow">
				<div class="statuswrapper">
					<span class="status completion details first last" id="SPNinfo" runat="server">
						<asp:Label ID="LBnumTicket_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLlistInfo">*Tickets:</asp:Label>
						<%--<asp:Literal ID="LTnumLayout" runat="server" Visible="false">
							<span class="item statusitem open{emptyCss}">
								<span id="Span1" title="{title}" class="counter {field}">{num}</span>                       
								<span id="Span2" class="label">{text}</span>
							</span>
						</asp:Literal>

						<asp:Literal ID="LTnumOpen" runat="server"></asp:Literal>
						<asp:Literal ID="LTnumInProg" runat="server"></asp:Literal>
						<asp:Literal ID="LTnumRequest" runat="server"></asp:Literal>
						<asp:Literal ID="LTnumSolved" runat="server"></asp:Literal>
						<asp:Literal ID="LTnumClosed" runat="server"></asp:Literal>--%>
						<div class="inlinewrapper">
							<CTRL:ListInfo ID="CTRLlistInfo" runat="server" />
						</div>
					</span>
				</div>
			</div>
			<!-- END - Details -->
		</asp:panel>
		<asp:panel ID="PNLcolumns" CssClass="fieldobject clearfix" runat="server">
			<!-- BEGIN - hide/Show Columns -->
			<div class="commands viewoptions">
				<span class="viewextra id" data-info="id">
					<asp:CheckBox id="CBXshowId" runat="server" Text="*Show ticket ID" />
				</span>
				<span class="viewextra lang" data-info="lang">
					<asp:CheckBox id="CBXshowLang" runat="server" Checked="true" Text="*Show language" />
				</span>
				<span class="viewextra community" data-info="community">
					<asp:CheckBox id="CBXshowCommunity" runat="server" Text="*Show community" />
				</span>
				<span class="viewextra category" data-info="category">
					<asp:CheckBox id="CBXshowCategory" runat="server" Text="*Show category" />
				</span>
			</div>
			<!-- END - hide/Show Columns -->
		</asp:Panel>
		
		<!-- BEGIN - Table -->
		<div class="tablewrapper clearfix">
			<asp:Repeater ID="RptTicket" runat="server">
				<HeaderTemplate>
					<table class="table light ticketlist manager bulkoff">
						<thead>
							<tr>
								<th class="bulkcell bulkcheck check">
									<span class="headercheckbox leftside">
										<input type="checkbox" name="" id="hdr-chb" />
										<div class="selectorpopup checkall">
											<div class="inner">
												&nbsp;
												<a href=""><asp:Literal ID="LTtblSelAll_t" runat="server">Seleziona tutti</asp:Literal></a>
											</div>
											<div class="bottomarrow">
												&nbsp;
											</div>
										</div>
										<div class="selectorpopup checknone">
											<div class="inner">
												&nbsp;
												<a href=""><asp:Literal ID="LTtblDeSelAll_t" runat="server">Deseleziona tutti</asp:Literal></a>
											</div>
											<div class="bottomarrow">
												&nbsp;
											</div>
										</div>
									</span>
								</th>

								<th class="id extrainfo">
									<span>
										<asp:Literal ID="LTcode_t" runat="server">*Cod.</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderByIdUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderByIdDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>

								<th class="subject">
									<span>
										<asp:Literal ID="LTsubject_t" runat="server">*Subject</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderBySubjUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderBySubjDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>

								<th class="lang extrainfo">
									<span>
										<asp:Literal ID="LTlang_t" runat="server">*Lang.</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderByLangUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderByLangDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>

								<th class="community extrainfo">
									<span>
										<asp:Literal ID="LTcommunity_t" runat="server">*Community</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderByComUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderByComDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>

								<th class="assignedto">
									<span>
										<asp:Literal ID="LTass_t" runat="server">*Assigned to</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderByAssUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderByAssDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>

								<th class="category extrainfo">
									<span>
										<asp:Literal ID="LTcategory_t" runat="server">*Category</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderByCatUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderByCatDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>

								<th class="submitted">
									<span>
										<asp:Literal ID="LTsub_t" runat="server">*Submitted</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderBySubmUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderBySubmDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>
							
								<th class="status">
									<span>
										<asp:Literal ID="LTstatus_t" runat="server">*Status</asp:Literal>
									</span>
									<span class="sortgroup">
										<asp:LinkButton ID="LNBorderByStatUp" runat="server" cssclass="icon orderUp" ></asp:LinkButton><!--
									 --><asp:LinkButton ID="LNBorderByStatDown" runat="server" cssclass="icon orderDown"></asp:LinkButton>
									</span>
								</th>
							
								<th class="actions">
									<%--<span>
										<asp:Literal ID="LTact_t" runat="server">*Actions</asp:Literal>
									</span>--%>
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
								<td class="bulkcell bulkcheck check">
									<span class="submittercheckbox">
										<asp:CheckBox ID="CBXselect" runat="server" />
									</span>
								</td>

								<td class="id extrainfo"><asp:Literal ID="LTcode" runat="server">###</asp:Literal></td>

								<td class="subject" id="tdSubject" runat="server">
									<span class="subject">
										<asp:HyperLink ID="HYPsubject" runat="server">#Ticket subject</asp:HyperLink>
										<span class="icons">
											<asp:Literal ID="LTattach" runat="server">
												<span title="{text}" class="icon xs attacchment">&nbsp;</span>
											</asp:Literal>
										</span>
									</span>
								</td>

								<td class="lang extrainfo">
									<asp:literal ID="LTlang" runat="server">#IT</asp:literal>
								</td>
			
								<td class="community extrainfo">
									<asp:literal ID="LTcom" runat="server">#Community Name</asp:literal>
								</td>
			
								<td class="assignedto">
									<asp:literal ID="LTassUsr" runat="server"><span class="{class}">{text}</span></asp:literal>
								</td>

								<td class="extrainfo category">
									<asp:literal ID="LTcurCate" runat="server">Category name</asp:literal>
								</td>
			
								<td class="time">
									<asp:literal ID="LTsendedOn" runat="server">10/12/2012 12:52</asp:literal>
								</td>

								<td class="status">
									<asp:Literal ID="LTstatus" runat="server">
										<span title="{title}" class="">{text}
										</span>
									</asp:Literal>
								</td>
			
								<td class="actions">
									<span class="icons">
										<asp:LinkButton ID="LNBassign" runat="server" CssClass="icon assign" Visible="false" ToolTip="*Assign">*Assign</asp:LinkButton>
										<asp:HyperLink ID="HYPedit" runat="server" CssClass="icon edit">*edit</asp:HyperLink>
										<asp:LinkButton ID="LNBreopen" runat="server" CssClass="icon reopen" ToolTip="*Reopen">*Reopen</asp:LinkButton>
										<asp:LinkButton ID="LNBcloseSolved" runat="server" CssClass="icon closesolved" ToolTip="*Close Solved">*Close Unsolved</asp:LinkButton>
										<asp:LinkButton ID="LNBcloseUnSolved" runat="server" CssClass="icon closeunsolved" ToolTip="*Close unsolved">*Close unsolved</asp:LinkButton>
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
			
			<a class="linkMenu switch" href="" style="display:none; visibility:hidden;"><asp:Literal ID="LTlkbBulk" runat="server">*Bulk actions On/Off</asp:Literal></a>
			<!-- Table bottom -->
			<div class="tablebottom clearfix">
			
				<div class="right pager bottom">
					<CTRL:GridPager ID="PGgridBot" runat="server"
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
						<span class="legenditem hasnews" title="<%=GetLegendTitle("hasnews")%>">
							<span class="legendicon hasnews">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("hasnews")%>
							</span>
						</span>
						<span class="legenditem noanswers" title="<%=GetLegendTitle("noanswers")%>">
							<span class="legendicon noanswers">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("noanswers")%>
							</span>
						</span>
						<span class="legenditem filtercategory" title="<%=GetLegendTitle("filterCategory")%>">
							<span class="legendicon filtercategory">*</span>
							<span class="legendtext">
								<%=GetLegendText("filterCategory")%>
							</span>
						</span>
					</span>
					<%--<span class="group centergroup">
						<span class="legenditem mine" title="<%=GetLegendTitle("mine")%>">
							<span class="legendicon mine">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("mine")%>
							</span>
						</span>
						<span class="legenditem ismanager" title="<%=GetLegendTitle("manager")%>">
							<span class="legendicon ismanager">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("manager")%>
							</span>
						</span>
						<span class="legenditem isassigned" title="<%=GetLegendTitle("assigned")%>">
							<span class="legendicon isassigned">&nbsp;</span>
							<span class="legendtext">
								<%=GetLegendText("assigned")%>
							</span>
						</span>
					</span>
					<span class="group rightgroup last">
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
			<!-- Table bottom -->
		</div>
		<!-- END - Table -->
	</asp:Panel>

	<asp:Panel ID="PNLerror" runat="server" CssClass="tickets ticketlist manager clearfix error" Visible="false">
		<asp:Label ID="LBerror" runat="server" CssClass="error"></asp:Label>
	</asp:Panel>

	<!-- BEGIN Bottom Button -->
	<div class="DivEpButton bottom hide">
		<asp:HyperLink ID="HYPbackBot" runat="server" CssClass="linkMenu" Visible="false">*Back</asp:HyperLink>
		<asp:HyperLink ID="HYPaddBot" runat="server" CssClass="linkMenu">*Apri nuovo</asp:HyperLink>
		<asp:HyperLink ID="HYPmineBot" runat="server" CssClass="linkMenu">*MyTicket</asp:HyperLink>
	</div>
	<!-- END Bottom Button -->



</div>
</asp:Content>