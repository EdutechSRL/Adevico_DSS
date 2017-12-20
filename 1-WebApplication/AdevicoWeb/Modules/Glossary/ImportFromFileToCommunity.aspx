<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ImportFromFileToCommunity.aspx.vb" Inherits="Comunita_OnLine.ImportFromFileToCommunity" %>

<%@ Register TagPrefix="CTRL" TagName="CommunityGlossaryTerms" Src="~/Modules/Glossary/UC/UC_CommunityGlossaryTerms.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Glossary/UC/UC_GlossaryHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    <asp:Literal ID="LTpageTitle_t" runat="server">*Amministrazione globale TicketSystem</asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>

    <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
        <div class="fieldrow">
            <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
        </div>
    </div>

    <asp:Panel runat="server" CssClass="glossarywrapper importexport import" ID="PNLmain">

        <div class="DivEpButton">
            <asp:HyperLink ID="HYPback" runat="server" CssClass="big linkMenu">*Back</asp:HyperLink>
            <a class="big linkMenu openuploadfile" href="">
                <asp:Literal runat="server" ID="LTuploadFile_s"></asp:Literal>
            </a>
            <asp:LinkButton ID="LNBImportGlossaryFromFile" runat="server" CssClass="linkMenu" Visible="false">*Import</asp:LinkButton>
        </div>
        <div class="header">
            <div class="fieldrow objectheader">
                <h4 class="title">
                    <asp:Literal runat="server" ID="LTglossaryImportFromFile_s"></asp:Literal>
                </h4>
                <!-- valutare se inserire descrizione -->
                <asp:Label ID="LBglossaryImportFromFileDescription_s" runat="server" CssClass="description"></asp:Label>
            </div>

            <div class="fieldrow options">
                <label for="" class="fieldlabel">
                    <asp:Literal runat="server" ID="LTImportFromFileOptions">**Import Term in glossary</asp:Literal>
                </label>
                <div class="inlinewrapper">
                    <asp:RadioButtonList ID="RBImportOptions" CssClass="option" AutoPostBack="false" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>

        <asp:Panel runat="server" ID="PNLUploadInfo" CssClass="fieldobject uploadedfile" Visible="false">
            <div class="fieldrow">
                <%-- <label for="" class="fieldlabel">Uploaded file:</label> --%>
                <asp:Label runat="server" ID="LBuploadedfile_s" CssClass="fieldlabel">*Uploaded file:</asp:Label>
                <span class="inputtext">
                    <asp:Label runat="server" ID="LBselectedFileName" CssClass="fieldfilelabel"></asp:Label>
                    <%-- <span class="file">import.xml</span> --%>

                    <asp:HyperLink ID="HYPdelete" runat="server">*Delete</asp:HyperLink>
                    <asp:HyperLink ID="HYPchange" runat="server" CssClass="openuploadfile">*Change</asp:HyperLink>
                </span>
            </div>
        </asp:Panel>

        <CTRL:CommunityGlossaryTerms ID="CTRCommunityGlossaryTerms" runat="server"/>
    </asp:Panel>

    <div class="dialog fileupload">
        <div class="fieldobject">
            <div class="fieldrow title">
                <div class="description">
                    <asp:Literal runat="server" ID="LTuploadFileText_s"></asp:Literal>
                </div>
            </div>
            <div class="fieldrow file">
                <div class="temp">
                    <asp:FileUpload ID="FUPimportFile" runat="server"/>
                </div>
            </div>
            <div class="fieldrow clearfix commands">
                <div class="left">
                    &nbsp;
                </div>
                <div class="right">
                    <%--<a class="linkMenu">*cancel</a>--%>
                    <asp:HyperLink ID="HYPcloseModal" runat="server" CssClass="linkMenu">*cancel</asp:HyperLink>
                    <asp:LinkButton runat="server" ID="LNBImportGlossaries" CssClass="linkMenu">*ok</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel runat="server" CssClass="termscontainer container_12 clearfix" ID="PNLnoPermision" Visible="False">
        <asp:Label ID="LBLNoPermission" runat="server" CssClass="fieldlabel">* si dispongono i permessi necessari per visualizzare la pagina</asp:Label>
    </asp:Panel>
</asp:Content>