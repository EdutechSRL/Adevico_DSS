<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AggiungiPost.ascx.vb"
    Inherits="Comunita_OnLine.UC_AggiungiPost" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
    <%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<script language="javascript" type="text/javascript">
    function faccine(code) {

    }
</script>
<%--	document.forms[0].<%=Me.TXAbody.ClientID%>.value += code--%>
<input type="hidden" id="HDNpost_ID" runat="server" name="HDNpost_ID" />
<input type="hidden" id="HDNpost_ParentID" runat="server" name="HDNpost_ParentID" />
<input type="hidden" id="HDNpost_Level" runat="server" name="HDNpost_Level" />
<input type="hidden" id="HDNpostFRUM_ID" runat="server" name="HDNpostFRUM_ID" />
<input type="hidden" id="HDNpostThread_ID" runat="server" name="HDNpostThread_ID" />
<input type="hidden" id="HDNpost_visibile" runat="server" name="HDNpost_visibile" />
<input type="hidden" id="HDNpost_PRSN_ID" runat="server" name="HDNpost_PRSN_ID" />
<input type="hidden" id="HDNazione" runat="server" name="HDNazione" />
<div id="TBLmessage" class="fieldobject addpost" runat="server">
    <div class="fieldrow title">
        <asp:Label ID="LBlegenda" runat="server" AssociatedControlID="TBLmessage">Inserimento Messaggio</asp:Label>
    </div>
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
    <div class="fieldrow options" id="TBRevidenzia" runat="server">
        <asp:Label ID="LBevidenzia_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBLevidenzia">Evidenzia:</asp:Label>
        <asp:RadioButtonList ID="RBLevidenzia" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                CssClass="Testo_campo" RepeatLayout="Flow">
                <asp:ListItem Value="1">Normale&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <img src="./../images/forum_img/normal.gif" alt="" border="0"/></asp:ListItem>
                <asp:ListItem Value="2">Interessante&nbsp; <img src="./../images/forum_img/interessante.gif" alt="" border="0"/></asp:ListItem>
                <asp:ListItem Value="3">Attenzione&nbsp;&nbsp;&nbsp; <img src="./../images/forum_img/alert.gif" alt="" border="0"/></asp:ListItem>
                <asp:ListItem Value="5">Faq&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <img src="./../images/forum_img/faq.gif" alt="" border="0"/></asp:ListItem>
            </asp:RadioButtonList>
    </div>
</div>

<div class="fieldobject" id="TBLrevisione" runat="server">
    <div class="fieldrow title" runat="server" id="TBRpostatoDa1">
        <asp:Label ID="LBmessaggioReply" runat="server" AssociatedControlID="TBRpostatoDa1">Risposta a:</asp:Label>
        
    </div>
    <div class="fieldrow iframe">
        
        <iframe width="100%" height="280px" src="./RevisioneArgomento.aspx"></iframe>
        
    </div>
</div>
