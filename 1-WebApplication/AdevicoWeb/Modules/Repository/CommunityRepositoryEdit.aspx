<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryEdit.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="ContentHeader" runat="server" ContentPlaceHolderID="HeadContent">
    <script language="javascript" type="text/javascript">

        var regex = "[^\"'?|\\/*+^:;]"; //regular expression per consentire lettere numeri underscore e trattino
        $(document).ready(function () {
            $(".alphaNumeric").keypress(function (event) {
                var c = String.fromCharCode(event.which);
                if (c.match(regex)) {
                    return true;
                } else {
                    return false;
                }
            });

            $('#selectFolder').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 600,
                height: 400,
                minHeight: 400,
                minWidth: 600,
                zIndex: 99999,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });

            $('#applyToSubItems').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 500,
                height: 200,
                minHeight: 200,
                //                minWidth: 700,
                zIndex: 99999,
                open: function (type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                    //$(this).parent().appendTo("form");
                }

            });

            $('#editError').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 500,
                height: 200,
                minHeight: 200,
                //                minWidth: 700,
                zIndex: 99999,
                open: function (type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                    //$(this).parent().appendTo("form");
                }

            });

        });

        function showDialog(id) {
            $('#' + id).dialog("open");
            return false;
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
    </script>
    <style type="text/css">
        .TableFolder
        {
        }
        .TableFile
        {
        }
        .TableRow
        {
            clear: both;
        }
        .TableCellLeft
        {
            float: left;
            text-align: left;
        }
        .TableCellRight
        {
        }
        .TableHeader
        {
        }
        .h25
        {
            height: 25px;
        }
        
        .h120
        {
            height: 120px;
        }
        .h150
        {
            height: 150px;
        }
        .w100
        {
            width: 100px;
        }
        .w150
        {
            width: 150px;
        }
        .w200
        {
            width: 200px;
        }
        .DetailsContainerFile
        {
            border-top: solid 2px;
            border-bottom: solid 1px;
            border-left: solid 2px;
            border-right: solid 2px;
            height: 250px;
        }
        .DetailsContainerFolder
        {
            border-top: solid 2px;
            border-bottom: solid 1px;
            border-left: solid 2px;
            border-right: solid 2px;
            height: 150px;
        }
        .TableAdvancedRow
        {
            clear: both;
            padding-left: 10px;
        }
        .AdvancedContainer
        {
            border-top: solid 1px;
            border-bottom: solid 2px;
            border-left: solid 2px;
            border-right: solid 2px;
            clear: both;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        padding-bottom: 5px;">
        <div style="float: left; text-align: left; width: 420px">
            &nbsp;
        </div>
        <div style="float: left; text-align: right; width: 480px;">
            <span style="vertical-align: text-bottom;">
                <asp:HyperLink ID="HYPbackToDownloads" runat="server" CssClass="Link_Menu" Visible="false"
                    Text="Back to downloads" Height="18px"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Visible="false"
                    Text="Back to management" Height="18px"></asp:HyperLink>
                <asp:UpdatePanel ID="UPDsaveButtons" UpdateMode="Conditional" ChildrenAsTriggers="true"
                    runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:Button ID="BTNeditSave" runat="server" />
                        <asp:Button ID="BTNeditSaveAndPermission" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </span>
        </div>
    </div>
    <asp:MultiView ID="MLVeditItem" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWedit" runat="server">
            <asp:UpdatePanel ID="UPDdetails" UpdateMode="Conditional" ChildrenAsTriggers="true"
                runat="server">
                <ContentTemplate>
                    <div style="text-align: center;">
                        <div style="clear: both; padding-top: 5px;">
                            <div runat="server" id="DVdetailsContainer">
                                <div style="padding-left: 10px; padding-top: 4px; text-align: left;">
                                    <h3>
                                        <asp:Label ID="LBeditInfo" runat="server"></asp:Label></h3>
                                </div>
                                <div style="padding-left: 10px; padding-bottom: 10px;">
                                    <div class="TableRow">
                                        <div class="TableCellLeft w100">
                                            <asp:Label ID="LBcommunity_t" runat="server" CssClass="Titolo_campoSmall">Community:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:Label ID="LBcommunity" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="TableRow">
                                        <div style="float: left; width: 100px; text-align: left; padding-top: 5px; padding-bottom: 5px;">
                                            <span style="vertical-align: middle;">
                                                <asp:Label ID="LBpath_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="BTNeditPath"></asp:Label></span>
                                        </div>
                                        <div style="float: left;">
                                            <span style="vertical-align: middle;">
                                                <asp:Label ID="LBpath" runat="server" CssClass="Testo_campoSmall"></asp:Label>&nbsp;
                                                <asp:Button ID="BTNeditPath" runat="server" Text="Change" CausesValidation="false" />
                                            </span>
                                        </div>
                                    </div>
                                    <div class="TableRow h25">
                                        <div class="TableCellLeft w100">
                                            <asp:Label ID="LBname_t" runat="server" CssClass="Titolo_campoSmall">Name:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:TextBox ID="TXBitemName" MaxLength="100" Columns="60" CssClass="alphaNumeric Testo_campo_obbligatorioSmall"
                                                runat="server" />
                                            <asp:Label ID="LBfileExtension" runat="server"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RFVfolderName" ControlToValidate="TXBitemName" runat="server"
                                                CssClass="errore" Text="*"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="TableRow h25">
                                        <div class="TableCellLeft w100">
                                            <asp:Label ID="LBdescription_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="TXBdescription">Description:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:TextBox ID="TXBdescription" runat="server" CssClass="Testo_campoSmall" MaxLength="250"
                                                Columns="60"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="DVtype" runat="server" class="TableRow h25">
                                        <div class="TableCellLeft w100">
                                            <asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall">Type:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:Label ID="LBtype" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                                        </div>
                                    </div>
                                    <div id="DVtypeDownload" runat="server" class="TableRow h25" visible="false">
                                        <div style="float: left; width: 100px; text-align: left;">
                                            <asp:Label ID="LBplay" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="RBLplay">Play:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:RadioButtonList ID="RBLplay" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow">
                                                <asp:ListItem Value="False" Selected="True">Only on platform</asp:ListItem>
                                                <asp:ListItem Value="True">Allow also download</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div>
                                        &nbsp;</div>
                                </div>
                            </div>
                            <div class="AdvancedContainer h120" id="DVadvancedContainer" runat="server">
                                <div style="padding-left: 10px; clear: both; padding-top: 3px; text-align: left;">
                                    <h3>
                                        <asp:Label ID="LBadvancedInfo" runat="server">Advanced Options</asp:Label></h3>
                                </div>
                                <div style="clear: both;">
                                    <div class="TableAdvancedRow h25">
                                        <div class="TableCellLeft w150">
                                            <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="RBLvisibleTo">Visible To:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:RadioButtonList ID="RBLvisibleTo" runat="server" CssClass="Testo_campoSmall"
                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Selected="true" Value="True">A tutti</asp:ListItem>
                                                <asp:ListItem Value="False">Al proprietario</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="TableAdvancedRow h25" id="DVpermissionUpload" runat="server">
                                        <div class="TableCellLeft w150">
                                            <asp:Label ID="LBpermissionInfo_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="CBXallowUpload">Allowed actions:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:CheckBox ID="CBXallowUpload" runat="server" CssClass="Testo_campoSmall" AutoPostBack="true"/>
                                        </div>
                                    </div>
                                    <div class="TableAdvancedRow h25" id="DVpermissionDownload" runat="server">
                                        <div class="TableCellLeft w150">
                                            <asp:Label ID="LBallowTo" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="RBLallowTo">Download allowed to:</asp:Label>
                                        </div>
                                        <div style="float: left;">
                                            <asp:RadioButtonList ID="RBLallowTo" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow" AutoPostBack="true">
                                                <asp:ListItem Selected="true" Value="True">All community members</asp:ListItem>
                                                <asp:ListItem Value="False">Some users/role</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div style="clear: both;">
                                        &nbsp;</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="text-align: right; width: 900px; padding-top: 10px;">
                        <asp:Button ID="BTNeditSaveBottom" runat="server" />
                        <asp:Button ID="BTNeditSaveAndPermissionBottom" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="applyToSubItems" style="display: none;">
                <div style="clear: both;">
                    <asp:Label ID="LBapplyTo" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
                    <br />
                    <br />
                </div>
                <div style="clear: both; text-align: right;">
                    <asp:LinkButton ID="LNBundo" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('applyToSubItems');return false;"></asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="LNBapplyToThis" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('applyToSubItems');return true;"></asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="LNBapplyToSubItems" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('applyToSubItems');return true;"></asp:LinkButton>
                </div>
            </div>
            <div id="selectFolder">
                <asp:UpdatePanel ID="UDPselectFolder" UpdateMode="Conditional" ChildrenAsTriggers="true"
                    runat="server">
                    <ContentTemplate>
                        <asp:MultiView ID="MLVselector" runat="server" ActiveViewIndex="0">
                            <asp:View ID="VIWselectorEmpty" runat="server">
                            </asp:View>
                            <asp:View ID="VIWfolderSelector" runat="server">
                                <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="420px" SelectionMode="Single"
                                    ForUpload="True" AjaxEnabled="true" />
                            </asp:View>
                            <asp:View ID="VIWerrorSelector" runat="server">
                                <div>
                                    <div style="padding-bottom: 40px;">
                                        <asp:Label ID="LBerrorNotification" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <div style="float: left; text-align: left; width: 50%">
                                            <asp:Button ID="BTNbackToFolder" runat="server" Text="Change folder" />
                                        </div>
                                        <div style="float: left; text-align: right; width: 50%">
                                            <asp:Button ID="BTNclose" runat="server" Text="Close" />
                                        </div>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="editError">
                <asp:UpdatePanel ID="UDPerrors" UpdateMode="Conditional" ChildrenAsTriggers="true"
                    runat="server">
                    <ContentTemplate>
                        <asp:MultiView ID="MLVerrors" runat="server" ActiveViewIndex="0">
                            <asp:View ID="VIWerrorEmpty" runat="server">
                            </asp:View>
                            <asp:View ID="VIWerrorModal" runat="server">
                                <div>
                                    <div style="padding-bottom: 40px;">
                                        <asp:Label ID="LBerrorModal" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <div style="float: left; text-align: right;">
                                            <asp:Button ID="BTNcloseModal" runat="server" Text="Close" />
                                        </div>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="VIWpermissionToEdit" runat="server">
            <div style="padding-top: 100px; padding-bottom: 100px;">
                <asp:Label ID="LBnoPermissionToEdit" runat="server" CssClass=""></asp:Label>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
