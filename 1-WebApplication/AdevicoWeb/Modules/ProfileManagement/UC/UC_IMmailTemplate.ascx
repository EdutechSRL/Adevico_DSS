<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMmailTemplate.ascx.vb" Inherits="Comunita_OnLine.UC_IMmailTemplate" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditor.ascx" TagName="CTRLmailEditor" TagPrefix="CTRL" %>

<div class="StepData">
    <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
           <div class="fieldobject">
                <div class="fieldrow">
                <br /><br /><br /><br />
                <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                <br /><br /><br /><br />
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWmailTemplate" runat="server">
            <span class="Fieldrow">
                <asp:Label ID="LBmailSelector_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="RBLmailSelector">Send mail:</asp:Label>
                <asp:RadioButtonList ID="RBLmailSelector" runat="server" CssClass="Testo_campo rbl_MultiElement_small" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true">
                    <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:RadioButtonList>
            </span>
            <div class="messageBox">
                <CTRL:CTRLmailEditor ID="CTRLmailEditor" ContainerLeft="[" ContainerRight="]" AllowCopyToSender="False" AllowNotifyToSender="False" AllowValidation="True" runat="server" />
            </div>
        </asp:View>
    </asp:MultiView>
</div>