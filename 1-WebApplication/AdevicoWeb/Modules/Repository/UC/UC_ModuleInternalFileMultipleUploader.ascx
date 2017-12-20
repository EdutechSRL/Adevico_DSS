<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleInternalFileMultipleUploader.ascx.vb"
    Inherits="Comunita_OnLine.UC_ModuleInternalFileMultipleUploader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script language="javascript" type="text/javascript">
//    $(document).ready(function () {
//        $('#selectFolder').dialog({
//            appendTo: "form",
//            autoOpen: false,
//            draggable: true,
//            modal: true,
//            title: "",
//            width: 600,
//            height: 400,
//            minHeight: 400,
//            minWidth: 600,
//            zIndex: 99999,
//            open: function (type, data) {
//               // $(this).parent().appendTo("form");
//            }
//        });
//    });

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
<telerik:radprogressmanager id="RPBmanager" runat="server" registerforsubmit="false" />
<div style="clear: both; padding-top: 5px;">
    <div id="DVfile0" runat="server" style="clear: both; height: 25px;">
        <div style="float: left; width: 100px; text-align: left;">
            <span style="vertical-align: middle;">
                <asp:Label ID="LBfileName_0" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
        </div>
        <div style="float: left;">
            <input type="file" runat="server" id="INfile_0" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
            <asp:Label ID="LBfileType_0" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
            <asp:RadioButtonList ID="RBLtype_0" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                RepeatLayout="Flow">

            </asp:RadioButtonList>
        </div>
    </div>
    <div id="DVfile1" runat="server" style="clear: both; height: 25px;">
        <div style="float: left; width: 100px; text-align: left;">
            <span style="vertical-align: middle;">
                <asp:Label ID="LBfileName_1" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
        </div>
        <div style="float: left;">
            <input type="file" runat="server" id="INfile_1" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
            <asp:Label ID="LBfileType_1" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
            <asp:RadioButtonList ID="RBLtype_1" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                RepeatLayout="Flow">

            </asp:RadioButtonList>
        </div>
    </div>
    <div id="DVfile2" runat="server" style="clear: both; height: 25px;">
        <div style="float: left; width: 100px; text-align: left;">
            <span style="vertical-align: middle;">
                <asp:Label ID="LBfileName_2" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
        </div>
        <div style="float: left;">
            <input type="file" runat="server" id="INfile_2" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
            <asp:Label ID="LBfileType_2" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
            <asp:RadioButtonList ID="RBLtype_2" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                RepeatLayout="Flow">

            </asp:RadioButtonList>
        </div>
    </div>
    <div id="DVfile3" runat="server" style="clear: both; height: 25px;">
        <div style="float: left; width: 100px; text-align: left;">
            <span style="vertical-align: middle;">
                <asp:Label ID="LBfileName_3" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
        </div>
        <div style="float: left;">
            <input type="file" runat="server" id="INfile_3" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
            <asp:Label ID="LBfileType_3" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
            <asp:RadioButtonList ID="RBLtype_3" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                RepeatLayout="Flow">

            </asp:RadioButtonList>
        </div>
    </div>
    <div id="DVfile4" runat="server" style="clear: both; height: 25px;">
        <div style="float: left; width: 100px; text-align: left;">
            <span style="vertical-align: middle;">
                <asp:Label ID="LBfileName_4" runat="server" CssClass="Titolo_campoSmall">File name:</asp:Label></span>
        </div>
        <div style="float: left;">
            <input type="file" runat="server" id="INfile_4" size="50" class="Testo_campoSmall" />&nbsp;&nbsp;
            <asp:Label ID="LBfileType_4" runat="server" CssClass="Titolo_campoSmall">File type:</asp:Label>
            <asp:RadioButtonList ID="RBLtype_4" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                RepeatLayout="Flow">

            </asp:RadioButtonList>
        </div>
    </div>
    <div style="clear: both;">
    </div>
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
<telerik:radprogressarea id="PRAcommunityFileUpload" runat="server" displaycancelbutton="false"
    skin="Default">
</telerik:radprogressarea>
