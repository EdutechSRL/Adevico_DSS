<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailSettingsNew.ascx.vb" Inherits="Comunita_OnLine.UC_MailSettingsNew" %>

<asp:Checkbox ID="CBXdefault" runat="server" Text="*default" CssClass="option external" AutoPostBack="True"/>

<asp:CheckBoxList runat="server" ID="CBLsettings" 
        RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="innerwrapper">
</asp:CheckBoxList >

<asp:Label ID="LBalternateText" runat="server" CssClass="notificationdisabled"></asp:Label>

<asp:Label runat="server" ID="LBsendDisabled" CssClass="notificationdisabled"></asp:Label>