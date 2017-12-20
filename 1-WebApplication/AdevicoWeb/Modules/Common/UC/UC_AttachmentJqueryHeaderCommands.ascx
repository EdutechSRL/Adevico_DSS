<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AttachmentJqueryHeaderCommands.ascx.vb" Inherits="Comunita_OnLine.UC_AttachmentJqueryHeaderCommands" %>
<%@ Register TagPrefix="CTRL" TagName="Scripts" Src="~/Modules/Common/UC/UC_OpenDialogHeaderScripts.ascx" %>
<asp:Literal ID="LTaddurltomoduleitemWindow" runat="server" Visible="false">700,350,400,200</asp:Literal>
<asp:Literal ID="LTaddurltomoduleitemandcommunityWindow" runat="server" Visible="false">700,500,400,200</asp:Literal>
<asp:Literal ID="LTlinkfromcommunityWindow" runat="server" Visible="false">700,500,400,200</asp:Literal>
<asp:Literal ID="LTuploadtomoduleitemWindow" runat="server" Visible="false">700,500,400,200</asp:Literal>
<asp:Literal ID="LTuploadtomoduleitemandcommunityWindow" runat="server" Visible="false">700,500,400,200</asp:Literal>
<asp:Literal ID="LTcloseScripts" runat="server" Visible="false"> $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');</asp:Literal>
<!-- UC_AttachmentJqueryHeaderCommand START-->
<CTRL:scripts id="CTRLscripts" runat="server"></CTRL:scripts>
<script language="javascript" type="text/javascript">
    function HideCommunityUpload() {
        return true;
    }
    function HideItemUpload() {
        ProgressStart();
        return true;
    }
    function ProgressStart() {
        getRadProgressManager().startProgressPolling();
    }

    $(function () {
        <asp:Literal ID="LTuploadtomoduleitem" runat="server" Visible="false"></asp:Literal>

        <asp:Literal ID="LTuploadtomoduleitemandcommunity" runat="server" Visible="false"></asp:Literal>

        <asp:Literal ID="LTlinkfromcommunity" runat="server" Visible="false"></asp:Literal>

        <asp:Literal ID="LTaddurltomoduleitem" runat="server" Visible="false"></asp:Literal>

        <asp:Literal ID="LTaddurltomoduleitemandcommunity" runat="server" Visible="false"></asp:Literal>
    });
</script>
<!-- UC_AttachmentJqueryHeaderCommand END-->