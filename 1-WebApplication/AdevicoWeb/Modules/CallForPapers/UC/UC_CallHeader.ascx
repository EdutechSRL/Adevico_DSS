<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CallHeader.ascx.vb" Inherits="Comunita_OnLine.UC_CallHeader" %>
<!-- START Call HEADER -->
<link href="<%=GetBaseUrl %>Graphics/Modules/CallForPapers/Css/callforpapers.css?v=201605041410lm" rel="Stylesheet" />
<link href="<%=GetBaseUrl %>Graphics/Modules/CallForPapers/css/callforpapers_print.css?v=201605041410lm" rel="Stylesheet" media="print" />
<asp:Literal ID="LTchoosenScripts" runat="server">
    <link href="#baseurl#Jscript/Modules/Common/Choosen/chosen.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript" src="#baseurl#Jscript/Modules/Common/Choosen/chosen.jquery.js"></script></asp:Literal>
<asp:Literal ID="LTcommonScripts" runat="server">
<script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.fixedEqualizer.js"></script>
<script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
<script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.inputActivator.js"></script>
<script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
<script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.checkboxList.js"></script>
<script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.textVal.js"></script>
</asp:Literal>
<asp:Literal ID="LTsemiFixedScripts" runat="server" Visible="false" ><script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery-semifixed.js"></script></asp:Literal>
<asp:Literal ID="LTdropDownButtonsScript" runat="server" Visible="false" ><script src="#baseurl#Jscript/Modules/Common/jquery.ddbuttonlist.js" type="text/javascript"></script></asp:Literal>
<asp:Literal ID="LTtreeTableScripts" runat="server" Visible="false" >
    <link rel="stylesheet" href="#baseurl#Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="#baseurl#Jscript/Modules/Common/jquery.treeTable.js"></script></asp:Literal>
<asp:Literal ID="LTmoduleScripts" runat="server"><script type="text/javascript" src="#baseurl#Jscript/Modules/CallForPapers/callforpapers.js"></script></asp:Literal>
<!--BASE URL -->
<asp:Literal ID="LTbaseUrl" runat="server" Visible="false"></asp:Literal>
<!--END Medico da caBASE URL -->
<!-- END Call HEADER -->