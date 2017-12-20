<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="WorkbookItemCommunityFile.aspx.vb" Inherits="Comunita_OnLine.WorkbookItemCommunityFile" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_SelectCommunityFiles.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <asp:HyperLink ID="HYPbackToFileManagement" runat="server" 
            CssClass="Link_Menu" Visible="false" Text="Back to file management" Height="18px"></asp:HyperLink>

        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Link" ID="LNBlinkToItem" CausesValidation="false"></asp:LinkButton>
    </div>
    <div>
         <CTRL:CommunityFile ID="CTRLCommunityFile" runat="server" width="900px" TriStateSelection="false" FolderSelectable="false"/>
    </div>
</asp:Content>