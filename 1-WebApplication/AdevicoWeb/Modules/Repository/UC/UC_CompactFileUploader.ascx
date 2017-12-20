<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CompactFileUploader.ascx.vb" Inherits="Comunita_OnLine.UC_CompactFileUploader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFolder" Src="~/Modules/Repository/UC/UC_SelectCommunityFolder.ascx" %>

<script language="javascript" type="text/javascript">
    $(document).ready(function() {
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
            open: function(type, data) {
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

<telerik:RadProgressManager ID="RPBmanager" runat="server" RegisterForSubmit="false" />
<div style="clear: both; padding-top: 5px;">
    <div style="border-top: solid 1px; border-bottom: solid 1px; border-left: solid 1px;
        border-right: solid 1px; height: 170px; border-color:Gray;">
        <div style="padding-left: 10px; text-align: left; padding-top:5px; padding-bottom:5px; height:30px; clear:both;">
            <span style="vertical-align:middle;"><asp:Label ID="LBuploadInfo" runat="server" CssClass="Titolo_campoSmall"></asp:Label> - <asp:Label ID="LBcommunity" runat="server" CssClass="Testo_campoSmall"></asp:Label></span>
            <br />
        </div>
        <div style="padding-left: 10px; padding-bottom: 10px;">
            <div style="clear: both;">
                <div style="float: left; width: 100px; text-align: left; padding-top: 10px; height:22px;">
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="DVfile" runat="server" style="clear: both;  height:25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <asp:Label ID="LBname_t" runat="server" CssClass="Titolo_campoSmall">Name:</asp:Label>
                </div>
                <div style="float: left;">
                    <input class="Testo_campo_obbligatorioSmall" id="TXBFile" type="file" size="60" name="TXBFile"
                        runat="server" />
                </div>
            </div>
            <div style="clear: both; height:25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <asp:Label ID="LBdescription_t" runat="server" CssClass="Titolo_campoSmall">Description:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="TXBdescription" runat="server" CssClass="Testo_campoSmall" MaxLength="250"
                        Columns="60"></asp:TextBox>
                </div>
            </div>
            <asp:UpdatePanel ID="UDPtype" UpdateMode="Conditional" ChildrenAsTriggers="true"
                runat="server">
                <ContentTemplate>
                    <div id="DVtype" runat="server" style="clear: both;  height:25px;">
                        <div style="float: left; width: 100px; text-align: left;">
                            <asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall">Type:</asp:Label>
                        </div>
                        <div style="float: left;">
                            <asp:RadioButtonList ID="RBLtype" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" AutoPostBack="true">

                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div id="DVtypeDownload" runat="server" style="clear: both;  height:25px;">
                        <div style="float: left; width: 100px; text-align: left;">
                            <asp:Label ID="LBplay" runat="server" CssClass="Titolo_campoSmall">Play:</asp:Label>
                        </div>
                        <div style="float: left;">
                            <asp:RadioButtonList ID="RBLplay" runat="server" CssClass="Testo_campoSmall"
                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="False" Selected="True">Only on platform</asp:ListItem>
                                <asp:ListItem Value="True">Allow also download</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="clear:both;">&nbsp;</div>
        </div>
    </div>
    <div style="border-top: solid 0px; border-bottom: solid 1px; border-left: solid 1px;
        border-right: solid 1px; clear: both; height: 90px; border-color:Gray;">
          <div style="padding-left: 10px; text-align: left; padding-top:5px; padding-bottom:5px; height:30px;">
            <span style="vertical-align:middle;">
                <asp:Label ID="LBadvancedInfo" runat="server" CssClass="Titolo_campoSmall">Advanced Options</asp:Label></span>
        </div>
        <div style="padding-left: 10px; clear: both;">
            <div>
                <div style="float: left; width: 150px; text-align: left;">
                    <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="Titolo_campoSmall">Visible To:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:RadioButtonList ID="RBLvisibleTo" runat="server" CssClass="Testo_campoSmall"
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Selected="true" Value="True">A tutti</asp:ListItem>
                        <asp:ListItem Value="False">Al proprietario</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div style="clear: both;">
                <div style="float: left; width: 150px; text-align: left;">
                    <asp:Label ID="LBallowTo" runat="server" CssClass="Titolo_campoSmall">Download allowed to:</asp:Label>
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
            <CTRL:CommunityFolder ID="CTRLCommunityFolder" runat="server" width="420px" SelectionMode="Single"
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
<asp:Literal ID="LTscript" runat="server"></asp:Literal>
<telerik:RadProgressArea ID="PRAcommunityFileUpload" runat="server" DisplayCancelButton="false"
    Skin="Default">
</telerik:RadProgressArea>
