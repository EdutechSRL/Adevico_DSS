<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_HTMLExport.ascx.vb" Inherits="Comunita_OnLine.UC_HTMLExport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<span class="exportContainer">
    <asp:LinkButton ID="LKBPDF" runat="server" CssClass="Link_Menu Pdf" Text="PDF"></asp:LinkButton>
    <asp:LinkButton ID="LKBDOCX" runat="server" CssClass="Link_Menu Docx" Text="DOCX"></asp:LinkButton>
    <asp:LinkButton ID="LKBRTF" runat="server" CssClass="Link_Menu Rtf" Text="RTF"></asp:LinkButton>
    <asp:LinkButton ID="LKBshowhide" runat="server" CssClass="Link_Menu" Text="Hide"></asp:LinkButton>
</span>
<telerik:RadEditor ID="UCRADeditor" runat="server"
    ToolsFile="~/Risorse_XML/Config/Editor/Export/ExportDefault.xml"
    ContentFilters = "DefaultFilters,PdfExportFilter">
    <ExportSettings>
        <Docx DefaultFontName="Arial" DefaultFontSizeInPoints="8" HeaderFontSizeInPoints="6" PageHeader="" />
        <Rtf DefaultFontName="Arial" DefaultFontSizeInPoints="8" HeaderFontSizeInPoints="6" PageHeader="" />
        <Pdf PageTitle="Page Title" Author="Author" PageLeftMargin="10mm"></Pdf>
    </ExportSettings>
</telerik:RadEditor>
<asp:Label ID="LBerror" runat="server" Visible="false"></asp:Label>

