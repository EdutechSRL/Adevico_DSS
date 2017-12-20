<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailSettings.ascx.vb" Inherits="Comunita_OnLine.UC_MailSettings" %>
<div class="fieldrow" runat="server" id="DVmailType">
    <asp:Label ID="LBmailType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBhtml">*Body</asp:Label>
    <span class="group">
        <asp:RadioButton ID="RBhtml" runat="server" CssClass="fieldinput" GroupName="mailbody" Checked="true" />
        <asp:Label ID="LBmailbodyHTML" runat="server" CssClass="alignr" AssociatedControlID="RBhtml"></asp:Label>
        <asp:RadioButton ID="RBtext" runat="server" CssClass="fieldinput" GroupName="mailbody" />
        <asp:Label ID="LBmailbodyTEXT" runat="server" CssClass="alignr" AssociatedControlID="RBtext"></asp:Label>
    </span>
</div>
<div class="fieldrow" id="DVsender" runat="server">
    <asp:Label ID="LBsender_t" runat="server" CssClass="fieldlabel" AssociatedControlID="">*Sender</asp:Label>
    <div class="inlinewrapper">
        <span class="group">
            <asp:RadioButton ID="RBsenderUser" runat="server" CssClass="fieldinput" GroupName="sender"  Checked="true" />
            <asp:Label ID="LBmailUserSender" runat="server" CssClass="alignr" AssociatedControlID="RBsenderUser"></asp:Label>
        </span>
        <span class="group">
            <asp:RadioButton ID="RBsenderSystem" runat="server" CssClass="fieldinput" GroupName="sender" />
            <asp:Label ID="LBmailSystemSender" runat="server" CssClass="alignr" AssociatedControlID="RBsenderSystem"></asp:Label>
        </span>
    </div>
</div>
<div class="fieldrow" id="DVsubject" runat="server">
    <asp:Label ID="LBsubjectType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="">*Subject start with</asp:Label>
    <div class="inlinewrapper">
        <span class="group">
            <asp:RadioButton ID="RBsubjectSystem" runat="server" CssClass="fieldinput" GroupName="subject"  Checked="true" />
            <asp:Label ID="LBsubjectSystem" runat="server" CssClass="alignr" AssociatedControlID="RBsubjectSystem"></asp:Label>
        </span>
        <span class="group">
            <asp:RadioButton ID="RBsubjectNone" runat="server" CssClass="fieldinput" GroupName="subject" />
            <asp:Label ID="LBsubjectNone" runat="server" CssClass="alignr" AssociatedControlID="RBsubjectNone"></asp:Label>
        </span>
    </div>
</div>
<div class="fieldrow" id="DVsignature" runat="server">
     <asp:Label ID="LBsignatureType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="">*Signature</asp:Label>
     <div class="inlinewrapper hor">
        <span class="group" runat="server" id="SPNsignatureNone">
            <asp:RadioButton ID="RBsignatureNone" runat="server" CssClass="fieldinput" GroupName="signature" Checked="true" />
            <asp:Label ID="LBsignatureNone" runat="server" CssClass="alignr" AssociatedControlID="RBsignatureNone" ></asp:Label>
        </span>
        <span class="group" runat="server" id="SPNsignatureFromConfigurationSettings" visible="false">
            <asp:RadioButton ID="RBsignatureFromConfigurationSettings" runat="server" CssClass="fieldinput" GroupName="signature" />
            <asp:Label ID="LBsignatureFromConfigurationSettings" runat="server" CssClass="alignr" AssociatedControlID="RBsignatureFromConfigurationSettings" ></asp:Label>
        </span>
        <span class="group" runat="server" id="SPNsignatureFromNoReplySettings" visible="false">
            <asp:RadioButton ID="RBsignatureFromNoReplySettings" runat="server" CssClass="fieldinput" GroupName="signature" />
            <asp:Label ID="LBsignatureFromNoReplySettings" runat="server" CssClass="alignr" AssociatedControlID="RBsignatureFromNoReplySettings"></asp:Label>
        </span>
        <span class="group" runat="server" id="SPNsignatureFromTemplate" visible="false">
            <asp:RadioButton ID="RBsignatureFromTemplate" runat="server" CssClass="fieldinput" GroupName="signature" />
            <asp:Label ID="LBsignatureFromTemplate" runat="server" CssClass="alignr" AssociatedControlID="RBsignatureFromTemplate"></asp:Label>
        </span>
        <span class="group" runat="server" id="SPNsignatureFromField" visible="false">
            <asp:RadioButton ID="RBsignatureFromField" runat="server" CssClass="fieldinput" GroupName="signature" />
            <asp:Label ID="LBsignatureFromField" runat="server" CssClass="alignr" AssociatedControlID="RBsignatureFromField"></asp:Label>
        </span>
        <span class="group" runat="server" id="SPNsignatureFromSkin" visible="false">
            <asp:RadioButton ID="RBsignatureFromSkin" runat="server" CssClass="fieldinput" GroupName="signature"/>
            <asp:Label ID="LBsignatureFromSkin" runat="server" CssClass="alignr" AssociatedControlID="RBsignatureFromSkin" ></asp:Label>
        </span>
     </div>
</div>
<div class="fieldrow readreceipt" id="DVnotifyToSender" runat="server" visible="false">
    <asp:Label ID="LBnotifyToSender" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXnotifyToSender">*Request Read Receipt</asp:Label>
    <asp:CheckBox ID="CBnotifyToSender" CssClass="fieldlabel" runat="server" />
</div>
<div class="fieldrow sendcopy" id="DVcopyToSender" runat="server" visible="false">
    <asp:Label ID="LBcopyToSender" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXcopyToSender">*Send Copy to the Sender</asp:Label>
    <asp:CheckBox ID="CBcopyToSender" CssClass="fieldlabel" runat="server" />
</div>