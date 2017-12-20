<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ReorderTiles.aspx.vb" Inherits="Comunita_OnLine.ReorderTiles" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/Common/UC/UC_GenericWizardSteps.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <link href="<%=GetBaseUrl()%>Graphics/Modules/TileTag/css/TileTag.css?v=201604071200lm" rel="Stylesheet" />
    <script src="<%=GetBaseUrl()%>Jscript/Modules/TileTag/TileTag.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
         
        $(function () {

            $("ol.sortabletree").sortable({
                handle: "span.movecfielditem",
                tolerance: 'pointer',
                placeholder: 'ui-state-highlightHelper',
                forcePlaceholderSize: true,
                forceHelperSize: true,
                dragOnEmpty: true,
                refreshPositions: true,
                axis: 'y',
                helper: "clone",
                start: function (event, ui) {
                    $(ui.item).addClass("dragging");
                },
                stop: function (event, ui) {
                    $(ui.item).removeClass("dragging");
                    $(".serialize_output").val($(ui.item).parents("ol.sortabletree").first().sortable("serialize"));
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="contentwrapper edit clearfix persist-area">	        
    <div class="vwizard viewsetsettings step1 hasheader">
        <div class="column left persist-header copyThis">
            <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
        </div>
        <div class="column right resizeThis">
            <div class="rightcontent">
                <div class="header">
                    <div class="DivEpButton">
                         <asp:Button ID="BTNsaveDashboardTilesOrderTop" runat="server" Text="*Save" Visible="false"  class="linkMenu"/>
                         <asp:HyperLink ID="HYPpreviewDashboardTop" class="linkMenu" runat="server" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                         <asp:Button ID="BTNtilesOrderGenerateCommunityTileTop" runat="server" Text="*Generate tiles" Visible="false"  class="linkMenu"/>
                         <asp:HyperLink ID="HYPgotoManageTilesTop" class="linkMenu" runat="server" Text="*Manage tiles" Visible="false"></asp:HyperLink>
                         <asp:HyperLink ID="HYPbackToDashboardListTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                    </div>
                    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                </div>
                <div class="content">
			        <div class="fieldobject first">
				        <div class="fieldrow objectheader">
				    	    <h4 class="title"><asp:literal ID="LTreorderTileTitle" runat="server" Text="*Tiles view"></asp:literal></h4>
				    	    <div class="fieldrow description"><asp:literal ID="LTreorderTileDescription" runat="server" Text="*Override default tiles visibility and sorting"></asp:literal></div>
				        </div>
				        <div class="fieldrow list">
						    <div class="sortabletree header clearfix">						
							    <span class="innerwrapper clearfix">
								    <span class="left">
                                        <asp:Label ID="LBreorderTileName_t" runat="server" CssClass="name">*Tile name</asp:Label> 
								    </span>
							        <span class="right">
							   	        <span class="details">
                                            <asp:Label ID="LBreorderTileVisibility_t" runat="server" CssClass="visible">*Visibility</asp:Label> 
							            </span>
							        </span>
							    </span>                     
						    </div>
						    <div class="sortabletree wrapper">
                                <input type="hidden" id="HDMserializeTiles" runat="server" class="serialize_output" />
                                <ol class="sortabletree ">
                                <asp:Repeater ID="RPTtiles" runat="server">
                                    <ItemTemplate>
                                        <li id="srt-<%#Container.DataItem.IdTile%>" class="sortableitem <%#GetCssClass(Container.DataItem) %>">
					                        <span class="handlers">
                                                <asp:Label ID="LBmoveTile" runat="server" CssClass="movecfielditem"></asp:Label>
                                                <asp:Literal ID="LTidTile" runat="server" Visible="false" Text="<%#Container.DataItem.IdTile%>"></asp:Literal>
                                                <asp:Literal ID="LTidAssignment" runat="server" Visible="false" Text="<%#Container.DataItem.IdAssignment%>"></asp:Literal>
					                        </span>
					                        <span class="innerwrapper clearfix">
					                            <span class="left">
                                                    <asp:Label ID="LBname" runat="server" CssClass="name" Text="<%#Container.DataItem.Name%>"></asp:Label>
					                            </span>
					                            <span class="right">
					                           	    <span class="details">
					                                    <span class="visible">
                                                            <input type="checkbox" id ="CBXvisibility" runat="server" />
					                                    </span>
					                                </span>
					                            </span>
					                        </span>                
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
					            </ol>
				            </div>
				        </div>
			        </div>
                </div>
				<div class="footer">
					<div class="DivEpButton">
						<asp:Button ID="BTNsaveDashboardTilesOrderBottom" runat="server" Text="*Save" Visible="false"  class="linkMenu"/>
                        <asp:HyperLink ID="HYPpreviewDashboardBottom" class="linkMenu" runat="server" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                        <asp:Button ID="BTNtilesOrderGenerateCommunityTileBottom" runat="server" Text="*Generate tiles" Visible="false"  class="linkMenu"/>
                        <asp:HyperLink ID="HYPgotoManageTilesBottom" class="linkMenu" runat="server" Text="*Manage tiles" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="HYPbackToDashboardListBottom" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
					</div>
				</div>
            </div>
        </div>
    </div>
</div>
 <asp:Literal ID="LTcssClassDisabled" runat="server" Visible="false" >disabled</asp:Literal>
</asp:Content>