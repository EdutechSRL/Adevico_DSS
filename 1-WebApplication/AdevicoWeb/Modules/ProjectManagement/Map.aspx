<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="Map.aspx.vb" Inherits="Comunita_OnLine.ProjectMap" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsersHeader" Src="~/Modules/Common/UC/UC_SelectUsersHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ManageProjectResources" Src="~/Modules/ProjectManagement/UC/UC_ManageProjectResources.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AddExternalResource" Src="~/Modules/ProjectManagement/UC/UC_AddExternalResource.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="RemoveResource" Src="~/Modules/ProjectManagement/UC/UC_ConfirmResourceToRemove.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="input" Src="~/Modules/Common/UC/UC_InLineInput.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InLineMenu" Src="~/Modules/ProjectManagement/UC/UC_ActivityInLineMenu.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InLineAdd" Src="~/Modules/ProjectManagement/UC/UC_ActivityInLineAdd.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InLineResources" Src="~/Modules/ProjectManagement/UC/UC_InLineResources.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Activity" Src="~/Modules/ProjectManagement/UC/UC_ActivityEditor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectResources" Src="~/Modules/ProjectManagement/UC/UC_SelectResources.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="PredecessorsHelper" Src="~/Modules/ProjectManagement/UC/UC_PredecessorsHelper.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Legend" Src="~/Modules/ProjectManagement/UC/UC_StatusLegend.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Info" Src="~/Modules/ProjectManagement/UC/UC_ProjectDateInfo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachment" Src="~/Modules/ProjectManagement/UC/UC_DialogProjectAttachments.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProjectManagement/Css/ProjectManagement.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.autoresize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-de.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-es.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-en-GB.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ui.datepicker-it.js"></script>
   <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(".view-modal.view-users-internal").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 890,
                height: 600,
                minHeight: 300,
                minWidth: 700,
                title: '<%=InternalDialogTitleTranslation() %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });

            $(".view-modal.view-users-external").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 600,
                height: 500,
                minHeight: 300,
                minWidth: 400,
                title: '<%=ExternalDialogTitleTranslation() %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
          
            $(".view-modal.view-projectresources").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 600,
                height: 450,
                minHeight: 200,
                minWidth: 200,
                title: '<%=ProjectResourcesDialogTitleTranslation() %>',
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });

            $(".view-modal.view-activity").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 600,
                height: 500,
                minHeight: 200,
                minWidth: 300,
                title: '<%=EditActivityDialogTitleTranslation() %>',
                open: function () {
                    //$(this).find(".chzn-select").chosen({});
                    //$(this)
                    var val = $(this).find("input.selectedtab").first().val();
                    if(val!=""){
                        $(this).find("div.tabs").tabs( "option", "active", val );
                    }

                    var $selres = $(this).find(".chzn-select.resources");
                    var $options = $selres.find("option");
                    var $tableres=$(this).find(".table.resourcescompletion");
                    $options.each(function(){
                        var id = $(this).val();
                        if($(this).is(":selected"))
                        {
                            $tableres.find("tr#"+id).removeClass("disabled");
                        }else
                        {
                            $tableres.find("tr#"+id).addClass("disabled");
                        }
                    });
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });


        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }     
    </script>
   
    <script type="text/javascript">
        $(document).ready(function () {
            $(".editable .edit.datepicker input[type='text']").datepicker("option", $.datepicker.regional["<%=LoaderCultureInfo.TwoLetterISOLanguageName%>"]);
            $(".editable .edit.datepicker input[type='text']").datepicker("option", "dateFormat", '<%=CurrentDatePickerShortDatePattern %>');
            //$(".selector").datepicker({ dateFormat: "<%=CurrentShortDatePattern %>" });
            //$.datepicker.setDefaults($.datepicker.regional["<%=LoaderCultureInfo.TwoLetterISOLanguageName%>"]);
            HighlightControlToValidate();


            $('#<%=BTNsaveProjectMapTop.ClientID %>').click(function () {
                return ValidateInputControls();
            });
            $('#<%=BTNsaveProjectMapBottom.ClientID %>').click(function () {
                return ValidateInputControls();
            });
        });

        function ValidateInputControls() {
            var error = false;
            if (typeof (Page_Validators) != "undefined") {
                for (var i = 0; i < Page_Validators.length; i++) {
                    if (!Page_Validators[i].isvalid) {
                        error = true;
                        if ($('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td")) {
                            $('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td").first().addClass("error"); //.css("background", "#f3d74f");
                            var $editable = $('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td").first().parents(".editable").first();
                            var $edit = $editable.children(".edit");
                            var $input = $edit.find("input[type='text']");
                            $input.val($(this).html().replace("&nbsp;", ""));
                            $editable.removeClass("viewmode").addClass("editmode");
                            $editable.find("input[type='hidden']").val("edit");
                            $input.focus();

                        }
                        else {
                            $('#' + Page_Validators[i].controltovalidate).parents(".editable").first().addClass("error");
                            var $editable = $('#' + Page_Validators[i].controltovalidate).parents(".editable").first();
                            var $edit = $editable.children(".edit");
                            var $input = $edit.find("input[type='text']");
                            $input.val($(this).html().replace("&nbsp;", ""));
                            $editable.removeClass("viewmode").addClass("editmode");
                            $editable.find("input[type='hidden']").val("edit");
                            $input.focus();
                        }
                    }
                    else {
                        $('#' + Page_Validators[i].controltovalidate).parents(".table.taskmap td").first().removeClass("error"); //.css("background", "white");
                        $('#' + Page_Validators[i].controltovalidate).parents(".editable").first().removeClass("error");
                    }
                }
            }
            if (!error) {
                return onUpdating();
            }
            else {
                return false;
            }

        }

        function HighlightControlToValidate() {
            if (typeof (Page_Validators) != "undefined") {
                for (var i = 0; i < Page_Validators.length; i++) {
                    $('#' + Page_Validators[i].controltovalidate).blur(function () {
                        var validatorctrl = getValidatorUsingControl($(this).attr("ID"));
                        if (validatorctrl != null && !validatorctrl.isvalid) {
                            $(this).parents(".table.taskmap td").first().addClass("error"); //.css("background", "#f3d74f");
                            $(this).parents(".editable").first().addClass("error");
                        }
                        else {
                            $(this).parents(".table.taskmap td").first().removeClass("error"); //.css("background", "white");
                            $(this).parents(".editable").first().removeClass("error");
                        }
                    });
                }
            }
        }

        function getValidatorUsingControl(controltovalidate) {
            var length = Page_Validators.length;
            for (var j = 0; j < length; j++) {
                if (Page_Validators[j].controltovalidate == controltovalidate) {
                    return Page_Validators[j];
                }
            }
            return null;
        }   
    </script>
        <CTRL:SelectUsersHeader ID="CTRLselectUsersHeader" runat="server" />
      <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVprojectMap" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBemptyMessage" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWmap" runat="server">
            <div class="DivEpButton DivEpButtonTop">
                <div class="ddbuttonlist enabled" id="DVresourcesTop" runat="server" visible="false"><!--
                    --><asp:LinkButton ID="LNBmanageProjectResources" runat="server" Text="*Manage resources" CssClass="linkMenu ddbutton" CausesValidation="false" /><!--
                    --><asp:LinkButton ID="LNBaddUsersFromCommunity" runat="server" Text="*Add User" CssClass="linkMenu ddbutton" CausesValidation="false" /><!--
                    --><asp:LinkButton ID="LNBaddCustomUser" runat="server" Text="*Add Custom" CssClass="linkMenu ddbutton" CausesValidation="false"/><!--
                    --><asp:LinkButton ID="LNBaddCustomUsers" runat="server" Text="*Add Custom" CssClass="linkMenu ddbutton" CausesValidation="false"/><!--
                --></div>
                <asp:LinkButton ID="LNBaddActivityTop" runat="server" Text="*Add activity" CssClass="linkMenu" CausesValidation="false"></asp:LinkButton>
                <asp:Button ID="BTNsaveProjectMapTop" runat="server" Text="*Save" CssClass="linkMenu savesubmit" CausesValidation="true" />
                <asp:HyperLink ID="HYPgoToProjectEditTop" class="linkMenu" runat="server" Text="*Edit project" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToResourceDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToManagerDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToProjectsTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
            </div>
            <div class="fieldobject header">	
        	    <div class="fieldrow title clearfix">
        		    <div class="left">
                         <h2>
                            <span class="projectnametitle">
                                <span><asp:Literal id="LTprojectName" runat="server"></asp:Literal></span>
                                <span class="icons">
                                    <asp:Label ID="LBattachments" runat="server" Visible="false" CssClass="icon xs attacchment">&nbsp;</asp:Label>
                                </span>
                            </span>
                        </h2>
        		    </div>
        		    <div class="right"><span class="extra"></span></div>
        	    </div>
            </div>
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
            <CTRL:Info id="CTRLprojectInfo" runat="server"></CTRL:Info>
            <div class="fieldobject toolbar clearfix">
                <div class="fieldrow left">
                    <span class="btnswitchgroup small" data-name="resources" data-rel="table.taskmap td.resources, table.taskmap th.resources" data-table="table.taskmap"><!--
                    --><asp:LinkButton ID="LNBshowResourcesColumn" runat="server" CssClass="btnswitch show first active" CausesValidation="false" OnClientClick="return false;">*Resources</asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBhideResourcesColumn" runat="server" CssClass="btnswitch hide last" CausesValidation="false" OnClientClick="return false;">*Hide</asp:LinkButton><!--
                    --></span>
                    <span class="btnswitchgroup small" data-name="dates" data-rel="table.taskmap td.dates, table.taskmap th.dates" data-table="table.taskmap"><!--
                    --><asp:LinkButton ID="LNBshowDateColumns" runat="server" CssClass="btnswitch show first active" CausesValidation="false" OnClientClick="return false;">*Date</asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBhideDateColumns" runat="server" CssClass="btnswitch hide last" CausesValidation="false" OnClientClick="return false;">*Hide</asp:LinkButton><!--
                    --></span>
                    <span class="btnswitchgroup small" data-name="links" data-rel="table.taskmap td.links, table.taskmap th.links" data-table="table.taskmap" runat="server" id="DVpredecessorsCommand"><!--
                    --><asp:LinkButton ID="LNBshowPredecessorsColumn" runat="server" CssClass="btnswitch show first active" CausesValidation="false" OnClientClick="return false;">*Predecessors</asp:LinkButton><!--
                    --><asp:LinkButton ID="LNBhidePredecessorsColumn" runat="server" CssClass="btnswitch hide last" CausesValidation="false" OnClientClick="return false;">*Hide</asp:LinkButton><!--
                    --></span>
                </div>
                <div class="fieldrow right">
                    <span class="btnswitchgroup small"><!--
                    --><asp:HyperLink ID="HYPprojectMap" runat="server" CssClass="btnswitch first active">*List view</asp:HyperLink><!--
                    --><asp:HyperLink ID="HYPprojectMapReorder" runat="server" CssClass="btnswitch ">*Tree view</asp:HyperLink><!--
                    --><asp:HyperLink ID="HYPprojectGantt" runat="server" CssClass="btnswitch last">*Gantt</asp:HyperLink><!--
                    --><asp:HyperLink ID="HYPprojectMapBulkEdit" runat="server" CssClass="btnswitch last" Visible="false">*Bulk Edit</asp:HyperLink><!--
                --></span>
                </div>
            </div>
            <div class="tablewrapper">
                <table class="table treetable light taskmap fullwidth" data-name="project-<%=IdProject %>">
                    <thead>
                        <tr>
                            <!--<th class="wbs">WBS</th>-->
                            <th class="id" data-name="id">
                                <asp:Label ID="LBthDisplayOrder" runat="server" CssClass="th">*ID</asp:Label>
                            </th>
                            <th class="name resizablecol" data-name="name">
                                <asp:Label ID="LBthName" runat="server" CssClass="th">*Name</asp:Label>
                            </th>
                            <th class="status" data-name="status">
                                <span class="th"><asp:Label ID="LBthStatus" CssClass="statuslight" runat="server"></asp:Label></span>
                            </th>
                            <th class="duration" data-name="duration">
                                <asp:Label ID="LBthDuration" runat="server" CssClass="th">*Duration</asp:Label>
                            </th>
                            <th class="links resizablecol" data-name="links" runat="server" id="THpredecessors">
                                <span class="th"><asp:Label ID="LBthPredecessors" runat="server" CssClass="text">*Links</asp:Label><span class="icons"><asp:Label ID="LBthPredecessorsLegend" runat="server" CssClass="icon infoxs openlegend"></asp:Label></span></span>
                            </th>
                            <th class="startdate dates" data-name="startdate">
                                <asp:Label ID="LBthStartDate" runat="server" CssClass="th">*StartDate</asp:Label>
                            </th>
                            <th class="enddate dates" data-name="enddate">
                                <asp:Label ID="LBthEndDate" runat="server" CssClass="th">*EndDate</asp:Label>
                            </th>
                            <th class="resources" data-name="resources">
                                <span class="th"><asp:Label ID="LBthResources" runat="server" CssClass="text">*Resources</asp:Label></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="RPTactivities" runat="server">
                            <ItemTemplate>
                                <tr id="<%#Container.DataItem.IdActivity %>" class="task<%#GetRowCssClass(Container.DataItem) %>">
                                    <td class="id<%#GetCellCssClass(Container.DataItem.DisplayOrder.Status) %>">
                                        <asp:Literal ID="LTidActivity" runat="server" Text='<%#Container.DataItem.IdActivity %>' Visible="false"></asp:Literal>
                                        <asp:Literal ID="LTidParent" runat="server" Text='<%#Container.DataItem.IdParent %>' Visible="false"></asp:Literal>
                                        <asp:Literal ID="LTdepth" runat="server" Text='<%#Container.DataItem.Depth %>' Visible="false"></asp:Literal>
                                        
                                        <asp:Literal ID="LTisSummary" runat="server" Text='<%#Container.DataItem.IsSummary %>' Visible="false"></asp:Literal>
                                        
                                        <CTRL:InLineMenu ID="CTRLinlineMenu" runat="server" Visible="false" />
                                        <span class="cellwrapper">
                                            <span class="text"><%#Container.DataItem.RowNumber %></span>
                                            <span class="icons">
                                                <asp:Label ID="LBerror" runat="server" CssClass="icon error xs" Visible="false"></asp:Label>
                                            </span>
                                        </span>
                                    </td>
                                    <td class="name resizablecol">
                                        <CTRL:input runat="server" ID="CTRLnameInput" ContainerCssClass="editable" InputMaxChar="1000" DataType="String"  ValidationEnabled="false"></CTRL:input>
                                    </td>
                                    <td class="status">
                                        <asp:Label ID="LBactivityStatus" CssClass="statuslight" runat="server"></asp:Label>
                                    </td>
                                    <td class="duration<%#GetCellCssClass(Container.DataItem.Duration.Status) %>">
                                       <CTRL:input runat="server" ID="CTRLdurationInput" ContainerCssClass="editable" InputMaxChar="5"  ValidationEnabled="true"></CTRL:input>
                                    </td>
                                    <td id="TDpredecessors" runat="server">
                                       <CTRL:input runat="server" ID="CTRLpredecessorsInput" ContainerCssClass="editable" DataType="String" ValidationExpression="^(\d+)(FF|FS|SS|SF)?([+-]\d+)?(;\s*(\d+)(FF|FS|SS|SF)?([+-]\d+)?)*$"></CTRL:input>
                                    </td>
                                    <td class="startdate dates<%#GetCellCssClass(Container.DataItem.EarlyStartDate.Status) %>">
                                       <CTRL:input runat="server" ID="CTRLstartDateInput" ContainerCssClass="editable" EditCssClass="datepicker" InputMaxChar="10" DataType="Date"></CTRL:input>
                                    </td>
                                    <td class="enddate dates<%#GetDeadlineCssClass(Container.DataItem.isAfterDeadline) %>">
                                        <span class="cellwrapper">
                                            <asp:Label ID="LBactivityEndDate" runat="server" CssClass="text"></asp:Label>
                                            <span class="icons">
                                                <asp:Label ID="LBactivityDeadLine" runat="server" CssClass="icon warning xs" Visible="false"></asp:Label>
                                            </span>
                                        </span>
                                    </td>
                                    <td class="resources resizablecol<%#GetResourcesCssClass(Container.DataItem) %>">
                                        <CTRL:InLineResources id="CTRLinLineResources" runat="server"></CTRL:InLineResources>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr class="footer toolbar">
                            <td class="addtask">
                                <CTRL:InLineAdd ID="CTRLinLineAdd" runat="server" Visible="false" />
                            </td>
                            <td colspan="8" id="TDfooterToolBar" runat="server" data-colspan="0">
                                <div class="clearfix">
                                    <div class="left">
                                    </div>
                                    <div class="right">
                                        <span class="commands" runat="server" id="DVtableCommands" Visible="false">
                                            <asp:LinkButton ID="LNBexpandNodes" runat="server" CssClass="command expandnodes" CausesValidation="false" OnClientClick="return false;">Expand nodes</asp:LinkButton>
                                            <asp:LinkButton ID="LNBcompressNodes" runat="server" CssClass="command compressnodes" CausesValidation="false" OnClientClick="return false;">Compress nodes</asp:LinkButton>
                                            <asp:LinkButton ID="LNBwideview" runat="server" CssClass="command wide" CausesValidation="false" OnClientClick="return false;">Wide view</asp:LinkButton>
                                            <asp:LinkButton ID="LNBnarrowview" runat="server" CssClass="command narrow" CausesValidation="false" OnClientClick="return false;">Narrow view</asp:LinkButton>
                                            <asp:LinkButton ID="LNBtoggleresources" runat="server" CssClass="command toggleresources" CausesValidation="false" OnClientClick="return false;">Toggle Resources</asp:LinkButton>
                                            <asp:LinkButton ID="LNBrestoreview" runat="server" CssClass="command restore" CausesValidation="false" OnClientClick="return false;">Restore view</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <CTRL:Legend ID="CTRLlegend" runat="server" OnlyAvailableStatus="true" Visible="false" />
            <CTRL:PredecessorsHelper id="CTRLpredecessorsHelper" runat="server" visible="false"></CTRL:PredecessorsHelper>
            <div class="DivEpButton DivEpButtonBottom" runat="server" visible="false" id="DVcommandsBottom">
                <div class="ddbuttonlist enabled" id="DVresourcesBottom" runat="server" visible="false"><!--
                    --><asp:LinkButton ID="LNBmanageProjectResourcesBottom" runat="server" Text="*Manage resources" CssClass="linkMenu ddbutton" CausesValidation="false"/><!--
                    --><asp:LinkButton ID="LNBaddUsersFromCommunityBottom" runat="server" Text="*Add User" CssClass="linkMenu ddbutton" CausesValidation="false"/><!--
                    --><asp:LinkButton ID="LNBaddCustomUserBottom" runat="server" Text="*Add Custom" CssClass="linkMenu ddbutton" CausesValidation="false"/><!--
                    --><asp:LinkButton ID="LNBaddCustomUsersBottom" runat="server" Text="*Add Custom" CssClass="linkMenu ddbutton" CausesValidation="false"/><!--
                --></div>
                <asp:LinkButton ID="LNBaddActivityBottom" runat="server" Text="*Add activity" CssClass="linkMenu" CausesValidation="false"></asp:LinkButton>
                <asp:Button ID="BTNsaveProjectMapBottom" runat="server" Text="*Save" CssClass="linkMenu" OnClientClick="return onUpdating();"/>
                <asp:HyperLink ID="HYPgoToProjectEditBottom" class="linkMenu" runat="server" Text="*Edit project" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToResourceDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToManagerDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToProjectsBottom" class="linkMenu" runat="server" Text="*Back"
                    Visible="false"></asp:HyperLink>
            </div>
            <div class="view-modal view-users-internal" id="DVselectUsers" runat="server" visible="false">
                <CTRL:SelectUsers ID="CLTRselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
                    DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True" DefaultMaxPreviewItems="5"
                    ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false" />
            </div>
            <div class="view-modal view-users-external" id="DVaddExternalResources" runat="server"
                visible="false">
                <CTRL:AddExternalResource ID="CTRLaddExternalResources" runat="server" Rows="10"
                    DisplayDescription="true" RaiseCommandEvents="true"></CTRL:AddExternalResource>
            </div>
            <div class="view-modal view-projectresources" id="DVprojectResources" runat="server" visible="false">
                <asp:MultiView ID="MLVmanageResources" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWmanageResources" runat="server">
                        <CTRL:ManageProjectResources id="CTRLmanageResources" runat="server" RaiseDeleteEvent="True" RaiseCommandEvents="True" DisplayCommands="True" SetMaxDisplayUsers="10" ></CTRL:ManageProjectResources>
                    </asp:View>
                    <asp:View ID="VIWvirtualDeleteResource" runat="server">
                        <CTRL:RemoveResource ID="CTRLconfirmRemoveResource" runat="server" DisplayDescription="true"
                    RaiseCommandEvents="true" DisplayCommands="true" AllowDelete="true"></CTRL:RemoveResource>    
                    </asp:View>
                </asp:MultiView>
            </div>
            <div class="view-modal view-activity dialog dlgedit" id="DVactivity" runat="server" visible="false">
                <CTRL:Activity id="CTRLactivityEdit" runat="server" DisplayDescription="true" RaiseCommandEvents="true"></CTRL:Activity>
            </div>
            <div class="dialog dlgresources" title="*Task Resources" runat="server" visible="false" id="DVselectResources">
                <input type="hidden" id="HDNactivityRowNumber" runat="server" />
                <div class="fieldobject">
                    <div class="fieldrow taskname header">
                        <asp:Label ID="LBactivity_t" runat="server" CssClass="fieldlabel title" AssociatedControlID="LBactivity"></asp:Label>
                        <asp:Label ID="LBactivity" runat="server" CssClass="task"></asp:Label>
                    </div>
                    <div class="fieldrow resources">
                        <CTRL:SelectResources id="CTRLselectActivityResources" runat="server" ></CTRL:SelectResources>
                    </div>
                    <div class="fieldrow buttons right">
                        <asp:Button ID="BTNselectActivityResources" runat="server" CssClass="linkMenu" />
                        <asp:LinkButton ID="LNBcloseSelectActivityResources" runat="server" CssClass="linkMenu close" OnClientClick="return false;" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <CTRL:Attachment id="CTRLattachment" runat="server" CssClass="dlgprojectattachments" Visible="false"></CTRL:Attachment>
    <asp:Literal ID="LTstatusContent" runat="server" Visible="false"><span class="th">{0}%</span></asp:Literal>
    <asp:Literal ID="LTstatuslight" runat="server" Visible="false">statuslight</asp:Literal>
    <asp:Literal ID="LTfullColSpan" runat="server" Visible="false">8</asp:Literal>
    <asp:Literal ID="LTnocpmColSpan" runat="server" Visible="false">7</asp:Literal>
    <asp:Literal ID="LTdurationEstimatedRegex" runat="server" Visible="false">^(\d+)(\?)?$</asp:Literal>
    <asp:Literal ID="LTdurationRegex" runat="server" Visible="false">\d</asp:Literal>
    <asp:Literal ID="LTpredecessorsCssClass" runat="server" Visible="false">links resizablecol</asp:Literal>
</asp:Content>