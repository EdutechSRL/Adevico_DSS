<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AggiungiThread.ascx.vb"
    Inherits="Comunita_OnLine.UC_AggiungiThread" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<script language="javascript" type="text/javascript">
    function faccine(code) {

    }
</script>
<%--document.forms[0].<%=Me.TXAbody.ClientID%>.value += code--%>
<div class="fieldobject addthread">
    <div class="fieldrow subject">
        <asp:Label ID="LBoggetto_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBsubject">Oggetto:</asp:Label>
        <span class="fieldinput">
            <asp:TextBox ID="TXBsubject" runat="server" Columns="80" MaxLength="70" CssClass="Testo_campo_obbligatorioSmall" />
            <asp:RequiredFieldValidator ID="RFVsubject" runat="server" ControlToValidate="TXBsubject"
                EnableClientScript="True"></asp:RequiredFieldValidator>
        </span>
    </div>
    <div class="fieldrow message">
        <asp:Label ID="LBtesto_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLeditor">Testo:</asp:Label>
        <div class="inlinewrapper">
            <CTRL:CTRLeditor id="CTRLeditor" runat="server" ContainerCssClass="containerclass" 
            LoaderCssClass="loadercssclass" EditorCssClass="editorcssclass" EditorHeight="400px" >
            </CTRL:CTRLeditor>
        </div>
    </div>
    <div class="fieldrow options" ID="TBRevidenzia" runat="server">
        <asp:Label ID="LBevidenzia_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBLevidenzia">Evidenzia:</asp:Label>
        <asp:RadioButtonList ID="RBLevidenzia" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                RepeatLayout="Flow" CssClass="Testo_campoSmall">
                <asp:ListItem Value="1">Normale&nbsp;&nbsp;&nbsp;</asp:ListItem>
                <asp:ListItem Value="2">Interessante(<img src="./../images/forum_img/interessante.gif" alt="" border="0">)</asp:ListItem>
                <asp:ListItem Value="3">Attenzione&nbsp;&nbsp;&nbsp; <img src="./../images/forum_img/alert.gif" alt="" border="0" align=absmiddle ></asp:ListItem>
                <asp:ListItem Value="5">Faq&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <img src="./../images/forum_img/faq.gif" alt="" border="0"></asp:ListItem>
            </asp:RadioButtonList>
    </div>
</div>

