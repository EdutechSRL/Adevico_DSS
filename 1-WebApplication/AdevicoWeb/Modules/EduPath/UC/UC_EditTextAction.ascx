<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditTextAction.ascx.vb"
    Inherits="Comunita_OnLine.UC_EditTextAction" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<asp:MultiView ID="MLVtextAction" runat="server" ActiveViewIndex="1">
    <asp:View ID="VIWempty" runat="server">
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <div class="DetailItem">
            <div class="DetailLeftItem">
                <asp:Label ID="LBdescriptionTitle" runat="server" CssClass="Titolo_campoSmall">Description:**</asp:Label>
            </div>
            <div class="DetailEditor">
                <asp:TextBox runat="server" ID="TXBmultiline" TextMode="multiline" CssClass="textarea"></asp:TextBox>

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
                <asp:RadioButtonList ID="RBLevaluate" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Digital*" Selected="True" Value="Digital"></asp:ListItem>
                    <asp:ListItem Text="Analog*" Value="Analog"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="DetailItem" runat="server" id="DIVweight">
            <div class="DetailLeftItem">
                <asp:Label ID="LBweightTitle" runat="server" CssClass="Titolo_campoSmall">Weight:**</asp:Label>
            </div>
            <div class="DetailRightItem">
                <asp:TextBox ID="TXBweight" runat="server" Width="30px" MaxLength="3" TextMode="SingleLine"
                    Text="1"></asp:TextBox>
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
            <div align="center">
                <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
            </div>
        </div>
    </asp:View>
</asp:MultiView>