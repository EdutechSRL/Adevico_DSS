<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="List.aspx.vb" Inherits="Comunita_OnLine.DocTemplateList" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
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
    <asp:Literal ID="LTtitle_t" runat="server">#Certificate Management</asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="view templates">
        
        <div class="buttonwrapper">
            <asp:HyperLink ID="HYPaddNew" runat="server" class="linkMenu">#Add Template</asp:HyperLink>
        </div>

        <!-- Inizio Codice Html *** Come in pagina /Modules/CallForPapers/SubmissionsList.aspx --> 
        <div class="group filters">
            <span class="items">
                <asp:Label ID="lblFilter_T" CssClass="filter" runat="server">#filtri</asp:Label>
                <asp:RadioButtonList ID="RBLtype" runat="server" AutoPostBack="true" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputradiobuttonlist">
                    <asp:ListItem Selected="True" Value="-1" Text="#Tutti"></asp:ListItem>
                    <asp:ListItem Value="1" Text="#Definitivi"></asp:ListItem>
                    <asp:ListItem Value="2" Text="#In bozza"></asp:ListItem>
                    <asp:ListItem Value="3" Text="#Cancellate"></asp:ListItem>
                </asp:RadioButtonList>
            </span>
<%--            <span class="items">
                <asp:CheckBox ID="CBXadvEdit" Checked="false" runat="server" Text="#Advanced Edit" AutoPostBack="true"/>
            </span>--%>
        </div>

        <div class="noitem">
            <asp:Label ID="LBLnoItems" runat="server" class="noitems">#Nessun elemento per i parametri specificati.</asp:Label>
        </div>

        <asp:Repeater ID="RPTtemplate" runat="server" OnItemCommand="RPT_ItemCommand">

            <HeaderTemplate>
                <table class="light template treeTable">
                <thead>
                    <tr>
                        <th class="templatename">
                            <asp:Literal ID="LTpartecipant_t" runat="server">Partecipant</asp:Literal>
                            <asp:LinkButton ID="LNBpartecipantOrderUp" runat="server" cssclass="icon orderUp">&nbsp;</asp:LinkButton>
                            <asp:LinkButton ID="LNBpartecipantOrderDown" runat="server" cssclass="icon orderDown">&nbsp;</asp:LinkButton>
                        </th>
                        <th class="linkedservices">
                            <asp:Literal ID="LTassService_t" runat="server">Associated Services</asp:Literal>
                        </th>
                        <th class="status">
                            <asp:Literal ID="LTstatus_t" runat="server">Status</asp:Literal>
                        </th>
                        <th class="savedon">
                            <asp:Literal ID="LTsavedOn_t" runat="server">Saved on</asp:Literal>
                            <asp:LinkButton ID="LNBsavedOnOrderUp" runat="server" cssclass="icon orderUp">&nbsp;</asp:LinkButton>
                            <asp:LinkButton ID="LNBsavedOnOrderDown" runat="server" cssclass="icon orderDown">&nbsp;</asp:LinkButton>
                        </th>
                        <th class="actions">
                            <asp:Literal ID="LTaction_t" runat="server">Actions</asp:Literal>
                        </th>
                    </tr>
                </thead>
                <tbody>
            </HeaderTemplate>

            <ItemTemplate>
                <tr id="template-<%#Container.DataItem.Id%>" class="template <%#GetTemplateCssClass(Container.DataItem) %>">
                    <td class="templatename">
                        <asp:Literal ID="LTtemplateName" runat="server">Template 1</asp:Literal>
                    </td>
                    <td class="linkedservices">
                        <asp:Literal ID="LTtemplateServices" runat="server">Edupath; Questionnaire; Scorm</asp:Literal>
                    </td>
                    <td class="status">
                        &nbsp;
                    </td>
                    <td class="submittedon">
                        <asp:Literal ID="LTtemplateUpdateOn" runat="server">28/07/2012 23.58</asp:Literal>
                    </td>
                    <td class="actions">
                        <span class="icons">
                            <asp:LinkButton ID="LNBtemplateDeleteLogical" runat="server" class="icon virtualdelete">[d]</asp:LinkButton>
                            <asp:LinkButton ID="LNBtemplateDeletePhisical" runat="server" class="icon delete">[x]</asp:LinkButton>
                            <asp:LinkButton ID="LNBtemplateRecover" runat="server" class="icon recover">[r]</asp:LinkButton>
                    	    <asp:LinkButton ID="LNBtemplateAddVers" runat="server" class="icon addversion">[+]</asp:LinkButton>
                            <asp:HyperLink ID="HypTemplateCopy"  runat="server" class="icon copy">[c]</asp:HyperLink>
                            <asp:LinkButton ID="LNBtemplateEnable" runat="server" class="icon enabled">[en]</asp:LinkButton>
                            <asp:LinkButton ID="LNBtemplateDisable" runat="server" class="icon disabled">[dis]</asp:LinkButton>
                        </span>
                        <span class="icons">
                            <asp:HyperLink ID="HYPtemplatePreview" runat="server" class="icon view" Target="_blank" Visible="false">[v]</asp:HyperLink>
                            <asp:LinkButton ID="LNBtemplatePDF" runat="server" class="icon export pdf" OnClientClick="blockUIForDownload();return true;">[p]</asp:LinkButton>
                            <asp:LinkButton ID="LNBtemplateRTF" runat="server" class="icon export rtf" OnClientClick="blockUIForDownload();return true;">[r]</asp:LinkButton>
                        </span>
                    </td>
                </tr>
                <asp:Repeater ID="RPTversions" runat="server" OnItemCommand="RPT_ItemCommand">
                    <ItemTemplate>
                        <tr id="template-<%#Container.DataItem.Template.Id %>-version-<%#Container.DataItem.Id %>" class="revision child-of-template-<%#Container.DataItem.Template.Id %> <%#GetVersionCssClass(Container.DataItem) %>">
                            
                            <td class="templatename" colspan="2">
                                <span class="revisionname">
                                    <asp:literal runat="server" ID="LTversion">Version 1</asp:literal>
                                </span>
                                <span class="revisionby">
                                    <asp:literal runat="server" ID="LTversionBy">By</asp:literal>
                                </span>
                            </td>

                            <td class="status">
                                <span class="icons">
                                    <span class="icon status <%#GetVersionStatusCssClass(Container.DataItem) %>">&nbsp;</span>
                                </span>
                                <asp:literal ID="LITversionStatus" runat="server">Draft</asp:literal>
                            </td>

                            <td class="submittedon">
                                <asp:Literal ID="LITversionUpdateOn" runat="server">29/07/2012 10.25</asp:Literal>
                            </td>

                            <td class="actions">
                                <span class="icons">
                                    <asp:LinkButton ID="LNBversionDeleteLogical" runat="server" class="icon virtualdelete">[d]</asp:LinkButton>
                                    <asp:LinkButton ID="LNBversionDeletePhisical" runat="server" class="icon delete">[x]</asp:LinkButton>
                                    <asp:LinkButton ID="LNBversionRecover" runat="server" class="icon recover">[r]</asp:LinkButton>
                                    <asp:HyperLink ID="HYPversionEdit" runat="server" CssClass="icon edit">[e]</asp:HyperLink>
                                    <asp:LinkButton ID="LNBversionCopy" runat="server" class="icon copy">[c]</asp:LinkButton>
                                    <asp:LinkButton ID="LNBversionEnable" runat="server" class="icon enabled">[en]</asp:LinkButton>
                                    <asp:LinkButton ID="LNBversionDisable" runat="server" class="icon disabled">[dis]</asp:LinkButton>
                                </span>
                                <span class="icons">
                                    <asp:HyperLink ID="HYPversionPreview" runat="server" class="icon view" Target="_blank">[v]</asp:HyperLink>
                                    <asp:LinkButton ID="LNBversionPDF" runat="server" class="icon export pdf" OnClientClick="blockUIForDownload();return true;">[p]</asp:LinkButton>
                                    <asp:LinkButton ID="LNBversionRTF" runat="server" class="icon export rtf" OnClientClick="blockUIForDownload();return true;">[r]</asp:LinkButton>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>

            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>

        </asp:Repeater>
            
    </div>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />

</asp:Content>