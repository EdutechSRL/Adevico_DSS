<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditSkin1.aspx.vb" Inherits="Comunita_OnLine.EditSkin1" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="UC/UC_EditSteps.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Settings" Src="UC/UC_EditSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Body" Src="UC/UC_EditBody.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Signatures" Src="UC/UC_EditSignatures.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Skin" Src="~/Modules/DocTemplate/Uc/UC_SkinSelector.ascx" %>


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
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/DocTemplate/DocTemplate.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" language="javascript" >
        var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
        var CookieName = "<% = Me.CookieName %>";
        var DisplayMessage = "<% = Me.DisplayMessageToken %>";
        var DisplayTitle = "<% = Me.DisplayTitleToken %>";
    </script>
    <!-- Fine script DocTemplate-->

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal ID="LTtitle_t" runat="server">Certificate Management</asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="buttonwrapper">
        <span class="buttongroup">
            <asp:HyperLink ID="HYPbackUrl" runat="server" Text="*Return" CssClass="linkMenu " Visible="false">#Torna al servizio</asp:HyperLink>
            <div id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
                    --><asp:LinkButton ID="LKBexportPDF" runat="server" Text="*PDF" CssClass="linkMenu" Visible="false"  OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--   
                    --><asp:LinkButton ID="LKBexportRTF" runat="server" Text="*RTF" CssClass="linkMenu" Visible="false"  OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--   
            --></div>
            <asp:HyperLink ID="HYPlist" runat="server" Text="*Return_P" CssClass="linkMenu " Visible="false">#Torna alla lista</asp:HyperLink>
        </span>
        <span class="buttongroup">
            <asp:HyperLink ID="HYPGoToAdvance" runat="server" Text="*Return" CssClass="linkMenu " Visible="false">#Advance</asp:HyperLink>
            <asp:HyperLink ID="HYPGoToSimple" runat="server" Text="*Return" CssClass="linkMenu " Visible="false">#Simple</asp:HyperLink>
        </span>
        <span class="buttongroup">
            <asp:LinkButton ID="LKBundo" runat="server" CssClass="linkMenu" CausesValidation="false">#Annulla</asp:LinkButton>
		    <asp:LinkButton ID="LKBsave" runat="server" CssClass="linkMenu">#Salva</asp:LinkButton>
        </span>
    </div>
    
    <div class="contentwrapper edit clearfix persist-area _hasFloating">

        <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>

        <div class="column right resizeThis" style="height: auto;">
            <div class="rightcontent">
<%--                <div class="header">
                    <div class="messages">
                        <div class="message alert">
                            <asp:Literal ID="Lit_Message" runat="server">Warning message!!!</asp:Literal>
                        </div>
                    </div>
                    <div class="buttonwrapper">
                        
                    </div>
                </div>--%>

                <div class="contentouter">
                    <div class="content clearfix">
                        <asp:MultiView ID="MLVtemplatePart" runat="server">

                            <asp:View ID="VWskin" runat="server">
                                <CTRL:Skin ID="UCskin" runat="server" />
                            </asp:View>

                            <asp:View ID="VWproperty" runat="server">
                                <CTRL:Settings ID="UCsettings" runat="server" />
                            </asp:View>


                            <asp:View ID="VWbody" runat="server">
                                <CTRL:Body ID="UCbody" runat="server" />
                            </asp:View>

                            <asp:View ID="VWsignatures" runat="server">
                                <CTRL:Signatures ID="UCsignatures" runat="server" />
                            </asp:View>

                        </asp:MultiView>
                    </div>
                </div>
                <div class="footer"> </div>
            </div>
        </div>
    </div>
    
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />

</asp:Content>