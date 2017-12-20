<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkBookItem.aspx.vb" Inherits="Comunita_OnLine.WorkBookItem" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="ManagementFile" Src="~/Modules/WorkBook/UC/UC_WorkBookItemFiles.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <asp:HyperLink ID="HYPbackToItems" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Back to items" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPgoToFileManagement" runat="server"
            CssClass="Link_Menu" Visible="false" Text="Go to file management" Height="18px"></asp:HyperLink>
        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Go to file management"
            ID="LNBgoToFileManagement" CausesValidation="false" Visible="false"></asp:LinkButton>
        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Save" ID="LNBsaveItem"
            CausesValidation="false" Visible="false"></asp:LinkButton>
    </div>
    <div>
        <asp:MultiView ID="MLVitemData" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWdata" runat="server">
                <div runat="server" id="DIVmetadata" style="height: 24px; text-align: left;">
                    <asp:Label ID="LBmetadata" runat="server" ></asp:Label>
                </div>
                <div id="DIVinfo" style="height: 24px; text-align: left;">
                    <div style="text-align: left; float: left; width: 100px">
                         <asp:Label ID="LBowner_t" runat="server"  CssClass="Titolo_campoSmall">Owner:</asp:Label>
                    </div>
                    <div style="float: left;">
                        <asp:Label ID="LBowner" runat="server"  CssClass="Testo_campoSmall"></asp:Label>
                    </div>
                </div>
                <div runat="server" id="DIVmetadataAdmin" style="height: 26px;">
                    <div style="text-align: left; float: left; width: 100px;">
                       <asp:Label ID="LBverificato_t" runat="server" AssociatedControlID="DDLstatus"  CssClass="Titolo_campoSmall">Verifyed:</asp:Label>
                    </div>
                    <div style="float: left; width: 250px">
                        <asp:DropDownList
                            ID="DDLstatus" runat="server" CssClass="Testo_campoSmall">
                            <asp:ListItem Value="3">In attesa di verifica</asp:ListItem>
                            <asp:ListItem Value="2">Non approvato</asp:ListItem>
                            <asp:ListItem Value="1">Approvato</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="LBverificato" runat="server"  CssClass="Testo_campoSmall"></asp:Label>
                    </div>
                    <div style="float: left;">    
                         <asp:Label ID="LBediting_t" runat="server" CssClass="Titolo_campoSmall" >Editing:</asp:Label>
                        <asp:DropDownList ID="DDLediting" runat="server" CssClass="Testo_campoSmall">
                            <asp:ListItem Text="Only workbook responsible" Value="9"></asp:ListItem>
                            <asp:ListItem Text="Only author" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Only authors" Value="15"></asp:ListItem>
                            <asp:ListItem Text="Only workbooks administrators" Value="8"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="LBediting" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                     </div>
                </div>
                <div style="height: 24px;" runat="server" id="DIVdraft">
                    <div style="text-align: left; float: left; width: 100px">
                        <asp:Label ID="LBdraft_t" runat="server"  AssociatedControlID="CBXdraft" CssClass="Titolo_campoSmall">Draft:</asp:Label></div>
                    <div style="float: left;">
                        <asp:CheckBox ID="CBXdraft" runat="server" />
                    </div>
                </div>
                <div runat="server" id="DIVdata" style="height: 24px;">
                    <div style="text-align: left; float: left; width: 100px">
                        <asp:Label ID="LBsceltaGiorno" runat="server"  AssociatedControlID="RDPstartDate" CssClass="Titolo_campoSmall">Day:&nbsp;</asp:Label></div>
                    <div style="float: left;">
                        <telerik:RadDatePicker ID="RDPstartDate" runat="server" />
                    </div>
                </div>
                <div runat="server" id="DIVtitle" style="height: 24px; clear: both;">
                    <div style="text-align: left; float: left; width: 100px">
                        <asp:Label ID="LBtitle" runat="server" AssociatedControlID="TXBtitle"  CssClass="Titolo_campoSmall">Title:</asp:Label></div>
                    <div style="text-align: left;">
                        <asp:TextBox ID="TXBtitle" runat="server" Columns="99"></asp:TextBox></div>
                </div>
                <div runat="server" id="DIVtext">
                    <div style="text-align: left; float: left; width: 100px">
                        <asp:Label ID="LBtext" runat="server" AssociatedControlID="CTRLeditor"  CssClass="Titolo_campoSmall">Text:</asp:Label></div>
                    <div style="width: 750px; margin-top: 5px; float: left; text-align: left;" align="left">
                        <CTRL:CTRLeditor id="CTRLeditor" runat="server" ContainerCssClass="containerclass" 
                            LoaderCssClass="loadercssclass" EditorHeight="180px" EditorWidth="750px"
                            AutoInitialize="true" ModuleCode="SRVLBEL" >
                        </CTRL:CTRLeditor>
                    </div>
                </div>
                <div runat="server" id="DIVnote" style="clear: left;">
                    <div style="text-align: left; float: left; width: 100px">
                        <asp:Label ID="LBnote" runat="server" AssociatedControlID="CTRLeditorNote"  CssClass="Titolo_campoSmall">Note:</asp:Label></div>
                    <div style="width: 750px; margin-top: 5px; float: left; text-align: left;" align="left">
                         <CTRL:CTRLeditor id="CTRLeditorNote" runat="server" ContainerCssClass="containerclass" 
                            LoaderCssClass="loadercssclass" EditorHeight="180px" EditorWidth="100%"
                            AutoInitialize="true" ModuleCode="SRVLBEL" >
                        </CTRL:CTRLeditor>

                    </div>
                </div>
                <div id="Div2" runat="server" style="clear: both; text-align:left;">
                    <b>
                        <asp:Literal ID="LTitemFiles_t" runat="server" >WorkBook Item's files</asp:Literal></b><hr />
                    <div style="width: 900px; padding-bottom: 10px; text-align:right;">
                        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Go to file management"
                            ID="LNBgoToFileManagementBottom" CausesValidation="false" 
                            Visible="false"></asp:LinkButton></div>
                    <div style="width: 900px; padding-bottom: 10px;  text-align:center;">
                        <CTRL:ManagementFile ID="CTRLmanagementFile" runat="server" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWnoItem" runat="server">
                <asp:Label ID="LBnoItem" runat="server" ></asp:Label>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
