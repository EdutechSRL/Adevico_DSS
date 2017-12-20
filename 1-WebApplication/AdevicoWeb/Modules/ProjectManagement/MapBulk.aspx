<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="MapBulk.aspx.vb" Inherits="Comunita_OnLine.MapBulk" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Info" Src="~/Modules/ProjectManagement/UC/UC_ProjectDateInfo.ascx" %>

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
        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }
        $(document).ready(function () {
            $(".editable .edit.datepicker input[type='text']").datepicker("option", $.datepicker.regional["<%=LoaderCultureInfo.TwoLetterISOLanguageName%>"]);
            $(".editable .edit.datepicker input[type='text']").datepicker("option", "dateFormat", '<%=CurrentDatePickerShortDatePattern %>');
            HighlightControlToValidate();

            $('#<%=BTNsaveProjectDateInfoTop.ClientID %>').click(function () {
                return ValidateInputControls();
            });
            $('#<%=BTNsaveProjectDateInfoBottom.ClientID %>').click(function () {
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
            <div class="contentwrapper">
                <div class="DivEpButton DivEpButtonTop">
                    <asp:Button ID="BTNsaveProjectDateInfoTop" runat="server" Text="*Save" CssClass="linkMenu savesubmit" CausesValidation="true" />
                    <asp:HyperLink ID="HYPgoToProjectEditTop" class="linkMenu" runat="server" Text="*Edit project" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToResourceDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToManagerDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToProjectsTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                </div>
                <div class="fieldobject header">	
        	        <div class="fieldrow title clearfix">
        		        <div class="left">
                             <h2><asp:Literal id="LTprojectName" runat="server"></asp:Literal></h2>
        		        </div>
        		        <div class="right">
        			        <span class="extra"></span>
        		        </div>
        	        </div>
                </div>
                <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                <CTRL:Info id="CTRLprojectInfo" runat="server"></CTRL:Info>
                <div class="fieldobject toolbar clearfix">
                    <div class="fieldrow left">
                        &nbsp;
                    </div>
                    <div class="fieldrow right">
                        <span class="btnswitchgroup small"><!--
                        --><asp:HyperLink ID="HYPprojectMap" runat="server" CssClass="btnswitch first">*List view</asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPprojectMapReorder" runat="server" CssClass="btnswitch active">*Tree view</asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPprojectGantt" runat="server" CssClass="btnswitch ">*Gantt</asp:HyperLink><!--
                        --><asp:HyperLink ID="HYPprojectMapBulkEdit" runat="server" CssClass="btnswitch active last">*Bulk Edit</asp:HyperLink><!--
                    --></span>
                    </div>
                </div>
                <br /><br />
                 <h2>PAGINA IN FASE DI REALIZZAZIONE</h2>
                <div class="DivEpButton DivEpButtonBottom" runat="server" visible="false" id="DVcommandsBottom">
                    <asp:Button ID="BTNsaveProjectDateInfoBottom" runat="server" Text="*Save" CssClass="linkMenu" OnClientClick="return onUpdating();"/>
                    <asp:HyperLink ID="HYPgoToProjectEditBottom" class="linkMenu" runat="server" Text="*Edit project" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToResourceDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToManagerDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPbackToProjectsBottom" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>