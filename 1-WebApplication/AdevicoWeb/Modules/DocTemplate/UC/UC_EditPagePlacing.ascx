<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditPagePlacing.ascx.vb" Inherits="Comunita_OnLine.UC_EditPagePlacing" %>

<span>
    <asp:RadioButtonList ID="RblPageSelect" runat="server" AutoPostBack="true" RepeatDirection="Vertical" RepeatLayout="Flow" CssClass="fieldblockwrapper">
        <asp:ListItem Text="*None" Value="0"></asp:ListItem>
        <asp:ListItem Text="*All" Value="1"></asp:ListItem>
        <asp:ListItem Text="*Specific" Value="-1"></asp:ListItem>
    </asp:RadioButtonList>
</span>
<span>
    <ul class="fieldblockwrapper">
        <li>
            <asp:CheckBox ID="CBXeven" runat="server" Text="*Even" />
        </li><li>
            <asp:CheckBox ID="CBXodd" runat="server" Text="*Odd" />
        </li><li>
            <asp:CheckBox ID="CBXfirst" runat="server" Text="*First" />
        </li><li>
            <asp:CheckBox ID="CBXlast" runat="server" Text="*Last" />
        </li><li>
            <asp:CheckBox ID="CBXspecific" runat="server" Text="*Specific" />
            <asp:TextBox ID="TXBpagesRange" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator runat="server" ControlToValidate="TXBpagesRange" EnableClientScript="true" ErrorMessage="(1-3, 6)" ID="REVpageRange" ValidationExpression="^(\s*\d+\s*\-\s*\d+\s*,?|\s*\d+\s*,?)+$"></asp:RegularExpressionValidator>
        </li>
    
    </ul>
    
</span>