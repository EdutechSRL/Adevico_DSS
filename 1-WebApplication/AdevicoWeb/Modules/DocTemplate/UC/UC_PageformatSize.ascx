<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PageformatSize.ascx.vb" Inherits="Comunita_OnLine.UC_PageformatSize" %>

<fieldset class="light">
	<legend>
        <asp:literal ID="LITtag_t" runat="server">Page Format Size</asp:literal>
    </legend>
    <div class="fieldobject">
        <span class="field">
            <asp:Label ID="LBLpageFormat_t" runat="server">*format</asp:Label>
            <asp:DropDownList ID="DDLpageFormat" runat="server" AutoPostBack="true">
            </asp:DropDownList>
            <asp:DropDownList ID="DDLMeasure" runat="server" AutoPostBack="true">
                <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                <asp:ListItem Value="cm" Text="cm" Selected="True"></asp:ListItem>
                <asp:ListItem Value="inch" Text="inch"></asp:ListItem>
                <asp:ListItem Value="px" Text="px"></asp:ListItem>
            </asp:DropDownList>
        </span>
        <span class="field">
            <asp:Label ID="LBLwidth_t" runat="server"></asp:Label>
            <asp:Label ID="LBLwidth" runat="server"></asp:Label>
            
            <asp:Label ID="LBLheight_t" runat="server"></asp:Label>
            <asp:Label ID="LBLheight" runat="server"></asp:Label>
        </span>
    </div>
</fieldset>