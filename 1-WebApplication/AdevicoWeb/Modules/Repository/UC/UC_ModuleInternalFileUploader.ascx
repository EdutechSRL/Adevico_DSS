<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleInternalFileUploader.ascx.vb"
    Inherits="Comunita_OnLine.UC_ModuleInternalFileUploader" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script language="javascript" type="text/javascript">
    function showDialog(id) {
        try {
            $find("<%= PRAcommunityFileUpload.ClientID %>").hide();
            getRadProgressManager().hideProgressAreas();
        }
        catch (ex) { }
        return false;
    }
</script>
<telerik:radprogressmanager id="RPBmanager" runat="server" registerforsubmit="false" />
<div style="clear: both; padding-top: 5px;">
    <div id="DVfile" runat="server" style="clear: both; height: 25px;">
        <div style="float: left; width: 100px; text-align: left;">
            <asp:Label ID="LBname_t" runat="server" CssClass="Titolo_campoSmall">Name:</asp:Label>
        </div>
        <div style="float: left;">
            <input class="Testo_campo_obbligatorioSmall" id="TXBFile" type="file" size="60" name="TXBFile"
                runat="server" />
        </div>
    </div>
    <asp:UpdatePanel ID="UDPtype" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <div id="DVtype" runat="server" style="clear: both; height: 25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <asp:Label ID="LBtype_t" runat="server" CssClass="Titolo_campoSmall">Type:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:RadioButtonList ID="RBLtype" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" AutoPostBack="true">
                    </asp:RadioButtonList>
                </div>
            </div>
            <div id="DVtypeDownload" runat="server" style="clear: both; height: 25px;">
                <div style="float: left; width: 100px; text-align: left;">
                    <asp:Label ID="LBplay" runat="server" CssClass="Titolo_campoSmall">Play:</asp:Label>
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
    <div style="clear: both;">
        &nbsp;</div>
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