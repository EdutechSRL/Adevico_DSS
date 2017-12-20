<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="SubActText.aspx.vb" Inherits="Comunita_OnLine.SubActText" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/PfStyle.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
<div class="Width940">
    <asp:MultiView ID="MLVeduPathList" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWeduPathList" runat="server">
            <div class="DivEpButton">
                <asp:Button ID="BTNsave" runat="server"  CssClass="Link_Menu" />
                <asp:HyperLink ID="HYPviewActivity" runat="server" Text="**view Act" CssClass="Link_Menu"></asp:HyperLink>
              
            </div>
            <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
            <div class="DetailItem">
                <div class="DetailLeftItem">
                    <asp:Label ID="LBdescriptionTitle" runat="server" CssClass="Titolo_campoSmall">Description:**</asp:Label>
                </div>
                <div class="DetailEditor">
                     <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass" 
                        LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" EditorHeight="360px" >
                        </CTRL:CTRLeditor>
                    <asp:Label ID="LBerrorEditor" runat="server" CssClass="erroreSmall" Visible="false" />
                </div>
            </div>

            <div id="DIVevalMode" runat="server" class="DetailItem">
                <div class="DetailLeftItem">
                   <asp:Label ID="LBevaluateTitle" runat="server" CssClass="Titolo_campoSmall" Text="*eval type" />        
                </div>
                <div class="DetailRightItem"> 
                    <asp:RadioButtonList ID="RBLevaluate" runat="server" AutoPostBack="false"  RepeatDirection="Horizontal">
                        <asp:ListItem Text="Digital*" Selected="True" Value="Digital"></asp:ListItem>
                        <asp:ListItem Text="Analog*"  Value="Analog"></asp:ListItem>
                    </asp:RadioButtonList>   
                </div>
            </div>

            <div class="DetailItem" runat="server" id="DIVweight">
                <div class="DetailLeftItem">
                    <asp:Label ID="LBweightTitle" runat="server" CssClass="Titolo_campoSmall">Weight:**</asp:Label>
                </div>
                <div class="DetailRightItem">
                    <asp:TextBox ID="TXBweight" runat="server" Width="30px" MaxLength="3" TextMode="SingleLine" Text="1"></asp:TextBox>
                    <asp:CompareValidator ID="COVweight" runat="server" ErrorMessage="Il valore deve essere un intero"
                        Text="" ControlToValidate="TXBweight" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
                </div>
            </div>
            <div id="DIVvisibility" class="DetailItem">
                <asp:CheckBox ID="CKBvisibility" runat="server" CssClass="Titolo_campoSmall" />
            </div>
            <div class="DetailItem" id="DIVmandatory" runat="server">
                <asp:CheckBox ID="CKBmandatory" runat="server" CssClass="Titolo_campoSmall" />
            </div>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWmessages" runat="server">
            <CTRL:Messages runat="server" ID="CTRLmessages"/>
        </asp:View>
    </asp:MultiView>
</div>
</asp:Content>