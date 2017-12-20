<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SingleFileUploader.ascx.vb"
    Inherits="Comunita_OnLine.UC_SingleFileUploader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
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
               // $(this).parent().appendTo("form");
            }
        });
    });

    function showDialog(id) {
        try {
            $find("<%= PRAcommunityFileUpload.ClientID %>").hide();
            getRadProgressManager().hideProgressAreas();
        }
        catch (ex) { }

        $('#' + id).dialog("open");
        //        alert($('<%= PRAcommunityFileUpload.clientID %>').enabled);
        //        $('<%= PRAcommunityFileUpload.clientID %>').css("display", "none");

        return false;
    }

    function closeDialog(id) {
        $('#' + id).dialog("close");
    }
</script>
<telerik:radprogressmanager id="RPBmanager" runat="server" registerforsubmit="false" />
<div style="clear: both; padding-top: 5px;">
    <div class="DetailsContainerFile">
        <div style="padding-left: 10px; text-align: left; padding-top: 5px;">
            <h3>
                <asp:Label ID="LBuploadInfo" runat="server"></asp:Label></h3>
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
                <div style="float: left; width: 100px; text-align: left; padding-top: 10px;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBpath_t" runat="server" CssClass="Titolo_campoSmall"></asp:Label></span>
                </div>
                <div style="float: left;">
                    <asp:UpdatePanel ID="UDPdata" UpdateMode="Conditional" ChildrenAsTriggers="true"
                        runat="server">
                        <ContentTemplate>
                            <span style="vertical-align: middle;">
                                <asp:Label ID="LBpath" runat="server" CssClass="Testo_campoSmall"></asp:Label>&nbsp;
                                <asp:Button ID="BTNeditPath" runat="server" Text="Change" OnClientClick="showDialog('selectFolder');"
                                    CausesValidation="false" /></span>
                            <asp:Literal ID="LTscript" runat="server"></asp:Literal>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="DVfolder" runat="server" class="TableRow h25">
                <div class="TableCellLeft w100">
                    <asp:Label ID="LBfolderName_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="TXBfolderName">Name:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="TXBfolderName" MaxLength="100" Columns="60" CssClass="alphaNumeric Testo_campo_obbligatorioSmall"
                        runat="server" />
                    <asp:RequiredFieldValidator ID="RFVfolderName" ControlToValidate="TXBfolderName"
                        runat="server" CssClass="errore" Text="*"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div id="DVfile" runat="server" class="TableRow h25">
                <div class="TableCellLeft w100">
                    <asp:Label ID="LBname_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="TXBFile">Name:</asp:Label>
                </div>
                <div style="float: left;">
                    <input class="Testo_campo_obbligatorioSmall" id="TXBFile" type="file" size="60" name="TXBFile"
                        runat="server" />
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
            <asp:UpdatePanel ID="UDPtype" UpdateMode="Conditional" ChildrenAsTriggers="true"
                runat="server">
                <ContentTemplate>
                    <div id="DVtype" runat="server" class="TableRow h25">
                        <div class="TableCellLeft w100">
                            <asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="RBLtype">Type:</asp:Label>
                        </div>
                        <div style="float: left;">
                            <asp:RadioButtonList ID="RBLtype" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" AutoPostBack="true">
                               
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div id="DVtypeDownload" runat="server" class="TableRow h25">
                        <div class="TableCellLeft w100">
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="TableRow">&nbsp;</div>
        </div>
    </div>
    <div class="AdvancedContainer h120" id="DVadvancedContainer" runat="server">
        <div style="padding-left: 10px; padding-top: 5px; clear: both; text-align: left;">
            <h3>
                <asp:Label ID="LBadvancedInfo" runat="server">Advanced Options</asp:Label></h3>
        </div>
        <div class="TableAdvancedRow h25">
            <div>
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
            <div class="TableRow h25" id="DVpermissionUpload" runat="server">
                <div class="TableCellLeft w150">
                    <asp:Label ID="LBpermissionInfo_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="CBXallowUpload">Allowed actions:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:CheckBox ID="CBXallowUpload" runat="server" CssClass="Testo_campoSmall" />
                </div>
            </div>
            <div class="TableRow" id="DVpermissionDownload" runat="server">
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
        </div>
    </div>
</div>
<div id="selectFolder">
    <asp:UpdatePanel ID="UDPselectFolder" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="420px" SelectionMode="Single" ForUpload="True"
                AjaxEnabled="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="uploadError">
    <asp:UpdatePanel ID="UDPerrors" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <asp:Label ID="LBerrorNotification" runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<telerik:radprogressarea id="PRAcommunityFileUpload" runat="server" displaycancelbutton="false">
</telerik:radprogressarea>
