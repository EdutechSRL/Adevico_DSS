<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailSettings.ascx.vb" Inherits="Comunita_OnLine.UC_MailSettings1" %>
<%--Nomi Standard: OK--%>
<asp:CheckBox ID="CBXdefault" runat="server" text="*Default" AutoPostBack="true"/>

<asp:CheckBoxList ID="CBLMailSettings" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputfield mailsets">    
</asp:CheckBoxList>

<asp:Literal ID="LTmainCss" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTitemCss" runat="server" Visible="false"></asp:Literal>