<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TextArea.ascx.vb" Inherits="Comunita_OnLine.UC_TextArea" %>
 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:radeditor ID="RDEtelerik" runat="server" Visible="true" 
    EditModes="Design" Skin="Default" ContentFilters="DefaultFilters"
    RenderMode="Lightweight"
    AutoResizeHeight="true" EnableResize="true" NewLineMode="Br"
     
    OnClientLoad="OnClientLoad" Width="100%" Height="250px"
    ToolsFile="~/Risorse_XML/Config/Editor/Export/ExportEmpty.xml" CssClass="TextAreaEditor"
    >
    <CssFiles>
        <telerik:EditorCssFile Value="~/Graphics/Generics/css/TelerikEditorBody.css" />
    </CssFiles>
    <Modules>
        <telerik:EditorModule Name="RadEditorStatistics" Visible="true" />
    </Modules>
</telerik:radeditor>
<%--<asp:Label ID="LBLwarningLenght" runat="server" CssClass="warning">Contenuto eccessivamente lungo: è stato troncato.</asp:Label>
<br />--%>
<span class="info">In fase di salvataggio tutte le formattazioni saranno eliminate.</span>
<%--OnClientPasteHtml="OnClientPasteHtml"--%>