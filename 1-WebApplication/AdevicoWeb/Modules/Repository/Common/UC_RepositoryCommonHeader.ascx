<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RepositoryCommonHeader.ascx.vb" Inherits="Comunita_OnLine.UC_RepositoryCommonHeader" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!-- Repository Header-->
<link href="<%=GetBaseUrl() %>Graphics/Modules/FileRepository/Css/FileRepository.css?v=201605041410lm" rel="Stylesheet" />
<link rel="stylesheet" href="<%=GetBaseUrl() %>Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.css?v=201605041410lm"/>
<!-- Start - Common imports-->
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Plugins/tagit/tag-it.min.js"></script>
<link href="<%=GetBaseUrl() %>/Graphics/Plugins/tagit/jquery.tagit.css" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl() %>Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
<!-- End - Common imports-->
<asp:literal Id="LTscriptRender" runat="server" Visible="false"></asp:literal>
<%--<script type="text/javascript" src="<%=GetBaseUrl() %>Jscript/Modules/FileRepository/filerepository.js"></script>
<script type="text/javascript" src="<%=GetBaseUrl() %>Jscript/Modules/FileRepository/filerepository.server.js"></script>--%>
<!-- END Repository Header-->
<asp:literal Id="LTscript" runat="server" Visible="false">
    <script type="text/javascript">
        var filerepository_userId = "-#idUser#";
        var filerepository_cmntId = "-#idCommunity#";
        var filerepository_alternate = "-#alternate#";
        var filerepository_cookiename = "comol_filerepository" + filerepository_userId + filerepository_cmntId + filerepository_alternate;
        var filerepository_default = "#default#";
        var itemError_Extension = "#itemError_Extension#";
        var itemError_Size = "#itemError_Size#";
        var itemError_NotSupported = "#itemError_NotSupported#";

        //1 = hidden, 0 = visible
        // Tree , Statistics , Extrainfo , Date , NarrowWideView 
        var filerepository_simple = "#PresetType.Simple.Tree#,#PresetType.Simple.Statistics#,#PresetType.Simple.Extrainfo#,#PresetType.Simple.Date#,#PresetType.Simple.NarrowWideView#";
        var filerepository_standard = "#PresetType.Standard.Tree#,#PresetType.Standard.Statistics#,#PresetType.Standard.Extrainfo#,#PresetType.Standard.Date#,#PresetType.Standard.NarrowWideView#";
        var filerepository_advanced = "#PresetType.Advanced.Tree#,#PresetType.Advanced.Statistics#,#PresetType.Advanced.Extrainfo#,#PresetType.Advanced.Date#,#PresetType.Advanced.NarrowWideView#";
        var filerepository_tags = [#tags#];
    </script>
</asp:literal>