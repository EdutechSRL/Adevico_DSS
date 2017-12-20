<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleUploaderHeader.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleInternalUploaderHeader" %>
<!-- UC_ModuleInternalUploaderHeader Header-->
<asp:Literal ID="LTcommonRepositoryCss" runat="server" Visible="false"><link href="#baseurl#Graphics/Modules/FileRepository/Css/FileRepositoryCommon.css?v=201605041410lm" rel="Stylesheet" /></asp:Literal>
<asp:Literal ID="LTcommonFancybox" runat="server" Visible="false">
    <link href="#baseurl#Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript" src="#baseurl#Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
</asp:Literal>
<asp:Literal ID="LTtagCssScript" runat="server" Visible="false">
<script type="text/javascript" src="#baseurl#Jscript/Modules/Plugins/tagit/tag-it.min.js"></script>
<link href="#baseurl#/Graphics/Plugins/tagit/jquery.tagit.css" rel="Stylesheet" /></asp:Literal>
<asp:literal Id="LTscriptRender" runat="server" Visible="false"></asp:literal>
<asp:Literal ID="LTcommonRepositoryScript" runat="server" Visible="false">
<script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.ddlist.js"></script>
<script type="text/javascript" src="#baseurl#Jscript/Modules/FileRepository/filerepository.common.js"></script></asp:literal>
<!-- END UC_ModuleInternalUploaderHeader Header-->
<asp:literal Id="LTscript" runat="server" Visible="false">
    <script type="text/javascript">
        var itemError_Extension = "#itemError_Extension#";
        var itemError_Size = "#itemError_Size#";
        var itemError_NotSupported = "#itemError_NotSupported#";

        //1 = hidden, 0 = visible
        // Tree , Statistics , Extrainfo , Date , NarrowWideView 
        var filerepository_tags = [#tags#];
    </script>
</asp:literal>

<%--<script type="text/javascript">
    function showErrorDialog(id) {
        try {
            $find("ProgressAreaClientId").hide();
            getRadProgressManager().hideProgressAreas();
        }
        catch (ex) {
        }
        $('#' + id).dialog("open");
        return false;
    }
    function closeErrorDialog(id) {
        $('#' + id).dialog("close");
    }
</script><asp:Literal ID="LTprogressAreaClientId" runat="server">progressareaid</asp:Literal>--%>