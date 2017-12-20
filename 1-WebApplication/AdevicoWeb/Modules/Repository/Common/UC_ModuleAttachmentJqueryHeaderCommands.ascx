<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleAttachmentJqueryHeaderCommands.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleAttachmentJqueryHeaderCommands" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="Scripts" Src="~/Modules/Common/UC/UC_OpenDialogHeaderScripts.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalHeader" Src="~/Modules/Repository/Common/UC_ModuleUploaderHeader.ascx" %>
<!-- UC_AttachmentJqueryHeaderCommand START-->
<script type="text/javascript">
    <asp:Literal ID="LTdialogTitle" runat="server"></asp:Literal>
</script><asp:Literal ID="LTdialogTitleTemplate" runat="server" Visible="false">var title_#type#='#value#';</asp:Literal>
<CTRL:InternalHeader ID="CTRLheader" runat="server" DisplayCommonCss="true" DisplayCommonScripts="true" DisplayTagCssScripts="true" DisplayFancybox="true"  />
<asp:Literal ID="LTaddurltomoduleitemWindow" runat="server" Visible="false">700,350,400,200</asp:Literal><asp:Literal ID="LTaddurltomoduleitemandcommunityWindow" runat="server" Visible="false">700,500,400,200</asp:Literal><asp:Literal ID="LTuploadtomoduleitemandcommunityWindow" runat="server" Visible="false">700,500,400,200</asp:Literal>
<asp:Literal ID="LTcloseScripts" runat="server" Visible="false"> $(this).find("input[type='file']").val(''); $(this).find("input[type='text']").val('');</asp:Literal>
<CTRL:scripts id="CTRLscripts" runat="server"></CTRL:scripts>
<script type="text/javascript">
    //function HideCommunityUpload() {
    //    return true;
    //}
    //function HideItemUpload() {
    //    ProgressStart();
    //    return true;
    //}
    //function ProgressStart() {
    //    getRadProgressManager().startProgressPolling();
    //}

    $(function () {
        <asp:Literal ID="LTaddurltomoduleitem" runat="server" Visible="false"></asp:Literal>

        <asp:Literal ID="LTaddurltomoduleitemandcommunity" runat="server" Visible="false"></asp:Literal>
    });
</script>
<!-- UC_AttachmentJqueryHeaderCommand END-->