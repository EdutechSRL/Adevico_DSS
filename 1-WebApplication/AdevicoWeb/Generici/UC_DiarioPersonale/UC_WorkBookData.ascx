<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_WorkBookData.ascx.vb"
    Inherits="Comunita_OnLine.UC_WorkBookData" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<asp:MultiView ID="MLVpersonalDiary" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWdata" runat="server">
        <div>
            <div id="DIVtype" style="height: 24px;">
                <div style="text-align: left; float: left; width: 90px">
                    <asp:Label ID="LBtype" runat="server" AssociatedControlID="LTtype" CssClass="Titolo_campoSmall">Tipo:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:Literal runat="server" ID="LTtype" ></asp:Literal>
                </div>
            </div>
            <div id="DIVinfo" style="height: 24px; text-align: left;">
                <div style="text-align: left; float: left; width: 90px">
                    <asp:Label ID="LBowner_t" runat="server"  CssClass="Titolo_campoSmall">Owner:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:Label ID="LBowner" runat="server"  CssClass="Testo_campoSmall"></asp:Label>
                </div>
            </div>
            <div runat="server" id="DIVmetadataAdmin" style="height: 26px;">
                <div style="text-align: left; float: left; width: 100px">
                    <asp:Label ID="LBverificato_t" runat="server" AssociatedControlID="DDLstatus" 
                        CssClass="Titolo_campoSmall">Verifyed:</asp:Label>
                </div>
                <div style="float: left;">
                    <asp:DropDownList ID="DDLstatus" runat="server" CssClass="Testo_campoSmall">
                        <asp:ListItem Value="3">In attesa di verifica</asp:ListItem>
                        <asp:ListItem Value="2">Non approvato</asp:ListItem>
                        <asp:ListItem Value="1">Approvato</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="LBverificato" runat="server"  CssClass="Testo_campoSmall"></asp:Label>
                    <asp:Label ID="LBediting_t" runat="server" CssClass="Titolo_campoSmall" >Editing:</asp:Label>
                    <asp:DropDownList ID="DDLediting" runat="server" CssClass="Testo_campoSmall">
                        <asp:ListItem Text="Only workbook responsible" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Only author" Value="11"></asp:ListItem>
                        <asp:ListItem Text="Only authors" Value="15"></asp:ListItem>
                        <asp:ListItem Text="Only workbooks administrators" Value="8"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="LBediting" runat="server" CssClass="Testo_campoSmall" 
                        Width="150px"></asp:Label>
                </div>
            </div>
            <div style="height: 24px;" runat="server" id="DIVdraft">
                <div style="text-align: left; float: left; width: 100px">
                    <asp:Label ID="LBdraft_t" runat="server"  AssociatedControlID="CBXdraft"
                        CssClass="Titolo_campoSmall">Draft:</asp:Label></div>
                <div style="float: left;">
                    <asp:CheckBox ID="CBXdraft" runat="server" />
                </div>
            </div>
            <div id="DIVtitle" style="height: 24px;">
                <div style="text-align: left; float: left; width: 90px">
                    <asp:Label ID="LBtitle" runat="server" AssociatedControlID="TXBtitle" CssClass="Titolo_campoSmall">Title:</asp:Label>
                </div>
                <div style="text-align: left;">
                    <asp:TextBox ID="TXBtitle" runat="server" Columns="80"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVtitle" runat="server" ControlToValidate="TXBtitle"
                        CssClass="errore" Text="*"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div id="DIVitemSeparator1" style="padding: 0px 5px 5px 5px; text-align: left; clear: both;"
                runat="server">
                &nbsp;&nbsp;&nbsp;
            </div>
            <div runat="server" id="DIVtext">
                <div style="text-align: left; float: left; width: 90px">
                    <asp:Label ID="LBtext" runat="server" AssociatedControlID="CTRLeditorText" CssClass="Titolo_campoSmall">Text:</asp:Label>
                </div>
                <div style="text-align: left;">
                    <CTRL:CTRLeditor id="CTRLeditorText" runat="server" ContainerCssClass="containerclass" 
                        LoaderCssClass="loadercssclass" EditorHeight="180px" EditorWidth="100%"
                        AutoInitialize="true" ModuleCode="SRVLBEL" >
                    </CTRL:CTRLeditor>
                </div>
            </div>
            <div id="DIVitemSeparator" style="padding: 0px 5px 5px 5px; text-align: left; clear: both;"
                runat="server">
                &nbsp;&nbsp;&nbsp;
            </div>
            <div runat="server" id="DIVnote">
                <div style="text-align: left; float: left; width: 90px">
                    <asp:Label ID="LBnote" runat="server" AssociatedControlID="CTRLeditorNote" CssClass="Titolo_campoSmall">Note:</asp:Label>
                </div>
                <div style="text-align: left;">
                    <CTRL:CTRLeditor id="CTRLeditorNote" runat="server" ContainerCssClass="containerclass" 
                        LoaderCssClass="loadercssclass" EditorHeight="180px" EditorWidth="100%"
                        AutoInitialize="true" ModuleCode="SRVLBEL" >
                    </CTRL:CTRLeditor>
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWnone" runat="server">
        &nbsp;&nbsp;&nbsp;
    </asp:View>
</asp:MultiView>