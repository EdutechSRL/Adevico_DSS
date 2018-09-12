<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="List.aspx.vb" Inherits="Comunita_OnLine.RepositoryList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLheader" Src="~/Modules/Repository/UC_New/UC_RepositoryHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsize" Src="~/Modules/Repository/UC_New/UC_RepositorySize.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLtreeTelerik" Src="~/Modules/Repository/UC_New/UC_TreeTelerik.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLitem" Src="~/Modules/Repository/UC_New/UC_RenderItem.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLbreadCrumb" Src="~/Modules/Repository/UC_New/UC_FoldersBreadCrumb.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLcommands" Src="~/Modules/Repository/UC_New/UC_FolderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLoptions" Src="~/Modules/Repository/UC_New/UC_ViewOptions.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLorderBy" Src="~/Modules/Repository/UC_New/UC_OrderBy.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLmoveHeader" Src="~/Modules/Repository/UC_New/UC_MoveItemsDialogHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLmove" Src="~/Modules/Repository/UC_New/UC_MoveItemsDialog.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaddFolder" Src="~/Modules/Repository/UC_New/UC_AddFolder.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaddLinks" Src="~/Modules/Repository/UC_New/UC_AddLinks.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaddFiles" Src="~/Modules/Repository/UC_New/UC_AddFiles.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaddVersion" Src="~/Modules/Repository/UC_New/UC_AddVersion.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:CTRLheader runat="server" ID="CTRLheader"/>
    <CTRL:CTRLmoveHeader runat="server" ID="CTRLmoveHeader"/>
    <!-- MODAL SCRIPT START -->
    <CTRL:ModalPlayerHeader ID="CTRLmodalHeader" runat="server" />
    <!-- MODAL SCRIPT END -->
    <script type="text/javascript">
        $(function(){
            var badCloseScormWithX;
            $(".section.box.height.files .iteminfo a[target='_blank']").click(function(){
                window.clearInterval(badCloseScormWithX);
                badCloseScormWithX = setInterval(function(){
		            var urlClosePayer = jQuery.cookie("urlClosePayer");
		            if(urlClosePayer){				
			            $.cookie("urlClosePayer", null, { path: '/' }); //clears this cookie value
			            ClosePlayer(urlClosePayer);
		            }
	            },1000);
            });

            function ClosePlayer(_iframehref){
                window.clearInterval(badCloseScormWithX);
	            var myDomain = location.href.replace("//","§§");
	            if(myDomain.indexOf("/") > 0){
		            var myDomainPart2 = myDomain.substring(myDomain.indexOf("/"));
		            if(myDomain.indexOf("?") >= 0){
			            var myDomainParams = myDomainPart2.split("?")[1];
			            myDomain = myDomain.split("/")[0];
			            myDomain = myDomain.replace("§§","//");
			            var iframehref = _iframehref;
			            if(jQuery("#iframehrefUrlClosePayer").size() > 0)
				            jQuery("#iframehrefUrlClosePayer").remove();
			            var ifrm = document.createElement("iframe");
			            ifrm.setAttribute("id", "iframehrefUrlClosePayer");
			            ifrm.setAttribute("src", iframehref);
			            ifrm.style.width = "0px";
			            ifrm.style.height = "0px";
			            document.body.appendChild(ifrm);
		            }
	            }
            }
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function(){
            let radioInputMultimediaPack = $('.controlSubType .showSubtype input[type="radio"]:first');
            if(radioInputMultimediaPack) {
                $('.controlSubType input[name="' + radioInputMultimediaPack.attr('name') + '"]').on('change', function(){
                    if ($(this).attr("id") == radioInputMultimediaPack.attr("id")) {
                        $('.multimediaPackageSubType').addClass("show");
                        $('.multimediaPackageSubType').removeClass("hide");
                    } else {
                        $('.multimediaPackageSubType').addClass("hide");
                        $('.multimediaPackageSubType').removeClass("show");
                    }
                });
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<div class="filerepository mainview container_12 clearfix">

    <CTRL:Messages ID="CTRLfolderInfo" runat="server" Visible="false"/>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false"/>
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWcontent" runat="server">
            <div class="section toolbar grid_12 alpha omega">
                <div class="clearfix">
                    <div class="left">  
                        <ctrl:CTRLcommands id="CTRLcommands" runat="server" AutoPostBack="true"></ctrl:CTRLcommands>                   
                    </div>
                    <div class="right">
                        <CTRL:CTRLorderBy ID="CTRLorderBy" runat="server" ContainerCssClass="normal sort" Visible="false" />
                        <CTRL:CTRLoptions runat="server" ID="CTRLviewOptions"/>
                    </div>
                </div>
            </div>
            <div class="section box boxheader grid_12 alpha omega" id="DVbreadcrumb" runat="server" visible="false">
                <div class="sectioninner">
                    <CTRL:CTRLbreadCrumb runat="server" ID="CTRLbreadcrumb" AutoPostBack="true"/>
                </div>
            </div>
            <div class="content">
                <div class="section box height tree grid_3 alpha">
                    <div class="sectioninner">
                        <div class="treewrapper">
                            <CTRL:CTRLtreeTelerik ID="CTRLtree" EnableFolderAutoNavigation="true" AutoOpenFolder="false"  AutoPostBack="true" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="section box height files grid_9 omega">
                    <div class="sectioninner">
                        <table class="table files minimal fullwidth noextrainfo<%=CssClassNoReorder %>">
                            <thead>
                                <tr>
                                    <th class="sorting" runat="server" id="THdisplayOrder">#</th>
                                    <th class="selection" runat="server" id="THselect"><input type="checkbox"/></th>
                                    <th class="name"><asp:literal id="LTtitleName" runat="server" Text="*name"></asp:literal></th>
                                    <th class="indicators" runat="server" id="THindicators">&nbsp;</th>
                                    <th class="date" runat="server" id="THdate"><asp:literal id="LTtitleDate" runat="server" Text="*date"/></th>
                                    <th class="stats" runat="server" id="THstatistics"><asp:literal id="LTtitleStatistics" runat="server" Text="*stats"/></th>
                                    <th class="actions" runat="server" id="THactions">
                                        <span class="icons">
                                            <asp:Label ID="LBtitleActionInfo" runat="server" CssClass="cssicon xs info">i</asp:Label>
                                        </span>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater ID="RPTitems" runat="server">
                                <ItemTemplate>
                                    <tr class="file" data-size="<%#Container.DataItem.Size%>" data-commands="<%#ItemDataCommands(Container.DataItem)%>" id="TRrowItem" runat="server">
                                        <td class="sorting" runat="server" id="TDdisplayOrder"><%#Container.DataItem.DisplayOrder %></td>
                                        <td class="selection" runat="server" id="TDselect"><input type="checkbox" runat="server" id="CBselect"/></td>
                                        <td class="name">
                                            <input type="hidden" runat="server" id="HDNidItem" value="<%#Container.DataItem.Id %>" />
                                            <CTRL:CTRLitem ID="CTRLitem" runat="server" OnMenuItemCommand="CTRLmenu_MenuItemCommand" OnSelectedFolder="CTRL_ItemSelectedFolder" AutoPostBack="true" />
                                        </td>
                                        <td class="indicators" runat="server" id="TDindicators">
                                            <span class="icons">
                                                <asp:Label ID="LBitemVersions" runat="server" Visible="false" CssClass="icon infoxs history"></asp:Label>
                                                <asp:Label ID="LBitemOtherPermissions" runat="server" Visible="false" CssClass="icon infoxs permission"></asp:Label>
                                            </span>
                                        </td>
                                        <td class="date" runat="server" id="TDdate"><asp:literal id="LTdate" runat="server"/></td>
                                        <td class="stats" runat="server" id="TDstatistics"><asp:literal id="LTstatistics" runat="server"/></td>
                                        <td class="actions" runat="server" id="TDactions">
                                            <span class="icons">
                                                <asp:HyperLink ID="HYPdetails" runat="server"><span class="icon infoalt first last"></span></asp:HyperLink>
                                            </span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                     <tr id="TRempty" runat="server" visible="false">
                                        <td id="TDemptyItems" runat="server" colspan="7">
                                            <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No files</asp:Label>
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="section box footer grid_12 alpha omega" id="DVrepositorySize" runat="server" visible="false">
                <div class="sectioninner">
                    <CTRL:CTRLsize runat="server" ID="CTRLrepositorySize"/>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWempty" runat="server"></asp:View>
    </asp:MultiView>
</div><asp:Literal ID="LTcookieTemplate" runat="server" Visible="false">comol_CollapsableStatus{0}</asp:Literal><asp:Literal ID="LThiddenItemCssClass" runat="server" Visible="false">hiddenfile</asp:Literal><asp:Literal ID="LTtemplateStats" runat="server" Visible="false"><span class="stats {0}" title="{2}"><span class="text"></span><span class="number">{1}</span></span></asp:Literal><asp:Literal ID="LTmyStatsCssClass" runat="server" Visible="false">my</asp:Literal><asp:Literal ID="LTstatsCssClass" runat="server" Visible="false">my</asp:Literal><asp:Literal ID="LTtemplateFile" runat="server"  Visible="false"><span class="iteminfo"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>
<CTRL:CTRLmove id="CTRLmoveDialog" runat="server" visible="false"/>
<CTRL:CTRLaddFolder id="CTRLaddFolder" runat="server" visible="false" MaxItems="3"/>
<CTRL:CTRLaddLinks id="CTRLaddLinks" runat="server" visible="false" MaxItems="3"/>
<CTRL:CTRLaddFiles id="CTRLaddFiles" runat="server" visible="false" MaxItems="4"/>
<CTRL:CTRLaddVersion id="CTRLaddVersion" runat="server" visible="false" />

</asp:Content>