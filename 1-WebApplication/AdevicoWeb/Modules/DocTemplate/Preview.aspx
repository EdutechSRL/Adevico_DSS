<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="Preview.aspx.vb" Inherits="Comunita_OnLine.TemplatePreview" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>

<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <!-- Stili docTemplate -->
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <link href="../../Graphics/Modules/DocTemplate/css/certificates.css" rel="Stylesheet" type="text/css" />
    <!-- fine stili docTemplate -->

    <!-- Script usati da DocTemplate-->
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/DocTemplate/DocTemplate.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript">
        var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieName = "<% = Me.CookieName %>";
        var DisplayMessage = "<% = Me.DisplayMessageToken %>";
        var DisplayTitle = "<% = Me.DisplayTitleToken %>";
    </script>
    <!-- Fine script DocTemplate-->

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="TemplateContainer">
        <div class="buttonwrapper">
            <div id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
                    --><asp:LinkButton ID="LKBexportPDF" runat="server" Text="*PDF" CssClass="linkMenu" Visible="false"  OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--   
                    --><asp:LinkButton ID="LKBexportRTF" runat="server" Text="*RTF" CssClass="linkMenu" Visible="false"  OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--   
         --></div>
        </div>
        <asp:MultiView ID="MLVtemplate" runat="server">
            <asp:View ID="VIWempty" runat="server">
                <br /><br /><br /><br />
                <asp:Label ID="LBmessage" runat="server"></asp:Label>
            </asp:View>
            <asp:View ID="VIWtemplate" runat="server">
                <div class="TemplateContainerRow">
                    <div class="TemplateHeaderRow">
                        <div runat="server" id="DIVheaderLeft">
                            <asp:Literal ID="LITitemHeaderLeft" runat="server"></asp:Literal>
                        </div>
                        <div runat="server" id="DIVheaderCenter">
                            <asp:Literal ID="LITitemHeaderCenter" runat="server"></asp:Literal>
                        </div>
                        <div runat="server" id="DIVheaderRight">
                            <asp:Literal ID="LITitemHeaderRight" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="TemplateContentRow">
                        <asp:Literal ID="LITitemBody" runat="server"></asp:Literal>
                    </div>
                    <asp:Panel ID="PNLsignatures" runat="server">
                    <div class="TemplateContentRow">
                        <asp:Literal ID="LITsignatures" runat="server"></asp:Literal>
                    </div>
                    </asp:Panel>
                    <div class="TemplateFooterRow">
                        <div runat="server" id="DIVfooterLeft">
                            <asp:Literal ID="LITitemFooterLeft" runat="server"></asp:Literal>
                        </div>
                        <div runat="server" id="DIVfooterCenter">
                            <asp:Literal ID="LITitemFooterCenter" runat="server"></asp:Literal>
                        </div>
                        <div runat="server" id="DIVfooterRight">
                            <asp:Literal ID="LITitemFooterRight" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    <br /><br />
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>