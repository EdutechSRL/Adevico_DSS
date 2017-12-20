<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FindCommunitiesByServiceHeader.ascx.vb" Inherits="Comunita_OnLine.UC_FindCommunitiesByServiceHeader" %>
<!-- START FindCommunitiesByServiceHeade-->
<%@ Register TagPrefix="CTRL" TagName="FiltersHeader" Src="~/Modules/Common/UC/UC_FiltersHeader.ascx" %>
<script src="<%=GetBaseUrl()%>Scripts/angular.min.js" type="text/javascript"></script>  
<asp:Literal ID="LTcss" runat="server" EnableViewState="true"></asp:Literal>
<asp:Literal ID="LTscript" runat="server" EnableViewState="true"></asp:Literal>
<CTRL:FiltersHeader id="CTRLfiltersHeader" FilterModuleCode="SRVDSHBOARD" runat="server"></CTRL:FiltersHeader>
<asp:Literal ID="LTscriptSingle" runat="server" EnableViewState="true" Visible="false">
    <script type="text/javascript">
        $(function () {
            $('[name$="selectsingleradio"]').attr("name", $('[name$="selectsingleradio"]').attr("name"));

            $('[name$="selectsingleradio"]').click(function () {
                //set name for all to name of clicked 
                $('[name$="selectsingleradio"]').attr("name", $(this).attr("name"));
            });
        });
    </script>
</asp:Literal>
<!-- END FindCommunitiesByServiceHeade-->
<asp:Literal ID="LTcssTemplate" runat="server" Visible="false"><link href="{0}Graphics/Modules/Community/css/filters.css" rel="Stylesheet" /></asp:Literal>
<asp:Literal ID="LTscriptTemplate" runat="server" Visible="false"><script src="{0}Jscript/Modules/Common/Selector/filters.js" type="text/javascript"></script></asp:Literal>