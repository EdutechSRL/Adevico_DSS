<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfilePolicy.ascx.vb" Inherits="Comunita_OnLine.UC_ProfilePolicy" %>

<asp:Repeater id="RPTpolicyInfo" runat="server">
    <ItemTemplate>
            <asp:literal runat="server" ID="LTitemId" Visible="false"></asp:literal>
            <asp:literal runat="server" ID="LTitemUserId" Visible="false"></asp:literal>
            <asp:label runat="server" ID="LBname"></asp:label>
            <div class="PrivacyBox" id="DVdescription" runat="server">
                <asp:literal ID="LTdescription" runat="server"></asp:literal>
            </div>
            <asp:CheckBox ID="CBXsingle" runat="server" Visible="false"/>
            <asp:RadioButtonList ID="RBLmultiple" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Visible="false"></asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="RFVmandatory" SetFocusOnError="true" Display="Static" Text="*" runat="server" Visible="false"></asp:RequiredFieldValidator>
            <asp:literal runat="server" ID="LTmandatory" Visible="false"></asp:literal>
            <asp:literal runat="server" ID="LTtype" Visible="false"></asp:literal>
            
            <br /><br />
    </ItemTemplate>
</asp:Repeater>