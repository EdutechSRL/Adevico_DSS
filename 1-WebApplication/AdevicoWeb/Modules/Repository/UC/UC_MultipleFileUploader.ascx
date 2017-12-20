<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MultipleFileUploader.ascx.vb"
    Inherits="Comunita_OnLine.UC_MultipleFileUploader" %>
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
        return false;
    }

    function closeDialog(id) {
        $('#' + id).dialog("close");
    }
</script>

<telerik:RadProgressManager ID="RPBmanager" runat="server" RegisterForSubmit="false" />
<div style="clear: both; padding-top: 5px;">
    <div style="border-top: solid 2px; border-bottom: solid 1px; border-left: solid 2px;
        border-right: solid 2px; height: 310px;">
        <div style="padding-left: 10px; padding-top: 5px; text-align: left;">
            <h3><asp:Label ID="LBuploadInfo" runat="server"></asp:Label></h3>
        </div>
        <div style="padding-left: 10px; padding-bottom: 10px;">
            <div style="clear: both;">
                <div style="float: left; width: 100px; text-align: left;">
                    <asp:Label ID="LBcommunity_t" runat="server" CssClass="Titolo_campoSmall">Community:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:Label ID="LBcommunity" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                </div>
            </div>
            <div style="clear: both;">
                <div style="float: left; width: 100px; text-align: left; padding-top: 5px;">
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
            <div id="DVfile0" runat="server" style="clear: both; height:25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                       <asp:Label ID="LBfileName_0" runat="server"  CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    
                    <input type="file" runat="server" id="INfile_0" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_0" runat="server"  CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_0" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile1" runat="server" style="clear: both; height:25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_1" runat="server"  CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    <input type="file" runat="server" id="INfile_1" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_1" runat="server"  CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_1" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile2" runat="server" style="clear: both; height:25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_2" runat="server"  CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    
                    <input type="file" runat="server" id="INfile_2" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_2" runat="server"  CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_2" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">

                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile3" runat="server" style="clear: both; height:25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_3" runat="server"  CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    
                    <input type="file" runat="server" id="INfile_3" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_3" runat="server"  CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_3" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVfile4" runat="server" style="clear: both; height:25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <span style="vertical-align: middle;">
                        <asp:Label ID="LBfileName_4" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
                </div>
                <div style="float: left;">
                    <input type="file" runat="server" id="INfile_4" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
                    <asp:Label ID="LBfileType_4" runat="server"  CssClass="Titolo_campoSmall">File type:</asp:Label>
                    <asp:RadioButtonList ID="RBLtype_4" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div style="clear: both;"></div>
        </div>
    </div>
    <div style="border-top: solid 1px; border-bottom: solid 2px; border-left: solid 2px;
        border-right: solid 2px; clear: both; height: 100px;">
        <div style="padding-left: 10px; clear: both;  padding-top: 3px; text-align: left;">
            <h3><asp:Label ID="LBadvancedInfo" runat="server">Advanced Options</asp:Label></h3>
        </div>
        <div style="padding-left: 10px; clear: both;">
            <div>
                <div style="float: left; width: 150px; text-align: left; ">
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
            <div style="clear: both;" runat="server" id="DVpermision">
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
