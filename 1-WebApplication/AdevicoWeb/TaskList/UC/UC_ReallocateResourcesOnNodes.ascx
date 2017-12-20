<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ReallocateResourcesOnNodes.ascx.vb"
    Inherits="Comunita_OnLine.UC_ReallocateResourcesOnNodes" %>
<style type="text/css">
    .CellCheck
    {
        width: 40px;
    }
    .CellUserName
    {
        width: 200px;
    }
    .CellRole
    {
        width: 120px;
    }
    .CellCompleteness
    {
        width: 40px;
    }
    .ComponentCompleteness
    {
        width: 35px;
    }
</style>
<asp:Repeater runat="server" ID="RPTtaskResources" OnItemCommand="RPTtaskResources_ItemCommand">
    <HeaderTemplate>
        <div style="margin: 0,auto; padding:10px; " align="left">
    </HeaderTemplate>
    <ItemTemplate>
        <div style="width: auto;" align="left">
            <asp:Label runat="server" ID="LBtaskName" Text="" CssClass="Titolo_campo"></asp:Label>
            <asp:ImageButton ID="IMBmodify" text="Modify**" CommandName="Modify" CausesValidation="true"
                runat="server"></asp:ImageButton>
            <asp:Button ID="BTNaddUser" Text="Add User**" CommandName="AddUser" CausesValidation="true"
                runat="server" CssClass="Link_Menu"></asp:Button>
        </div>
        <asp:Repeater runat="server" ID="RPTcomponentResources">
            <HeaderTemplate>
                <table id="tableUsers" cellspacing="0" border="1" >
                    <tr class="ROW_header_Small_Center">
                        <td id="TDheaderCheckBox" runat="server" class="CellCheck">
                            <b>
                                <asp:Literal ID="LTheaderCheckBox" Text="C*" runat="server"></asp:Literal></b>
                        </td>
                        <td class="CellUserName">
                            <b>
                                <asp:Literal ID="LTheaderUser" Text="USER*" runat="server"></asp:Literal></b>
                        </td>
                        <td class="CellRole">
                            <b>
                                <asp:Literal ID="LTheaderRole" Text="ROLE*" runat="server" /></b>
                        </td>
                        <td class="CellCompleteness">
                            <b>
                                <asp:Literal ID="LTheaderCompleteness" Text="COMPLETENESS*" runat="server" /></b>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="ROW_Normal_Small">
                    <td id="TDitemCheckBox" runat="server" class="CellCheck">
                        <asp:CheckBox ID="CBXuser" runat="server"></asp:CheckBox>
                    </td>
                    <td class="CellUserName">
                        <asp:Literal ID="LTuser" runat="server" Text=""></asp:Literal>
                    </td>
                    <td class="CellRole">
                        <asp:Literal ID="LTrole" runat="server" Text=""></asp:Literal>
                    </td>
                    <td class="CellCompleteness">
                        <asp:TextBox ID="TBXcompleteness" runat="server" Visible="true" Text="" CssClass="ComponentCompleteness" MaxLength="3" Width="30px"></asp:TextBox>
                        <asp:Label ID="LBpercent" runat="server" Text=" %"></asp:Label>
                        <asp:Literal ID="LTcompleteness" runat="server" Visible="true" Text=""> </asp:Literal>
                        <asp:RangeValidator ID="RNVcompleteness" runat="server" Text="*" ControlToValidate="TBXcompleteness"
                            MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="RFVcompleteness" runat="server" ControlToValidate="TBXcompleteness"
                            ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="ROW_Alternate_Small">
                    <td id="TDitemCheckBox" runat="server" class="CellCheck">
                        <asp:CheckBox ID="CBXuser" runat="server"></asp:CheckBox>
                    </td>
                    <td class="CellUserName">
                        <asp:Literal ID="LTuser" runat="server" Text=""></asp:Literal>
                    </td>
                    <td class="CellRole">
                        <asp:Literal ID="LTrole" runat="server" Text=""></asp:Literal>
                    </td>
                    <td class="CellCompleteness">
                        <asp:TextBox ID="TBXcompleteness" runat="server" Visible="true" Text="" CssClass="ComponentCompleteness" MaxLength="3" Width="30px"></asp:TextBox>
                        <asp:Label ID="LBpercent" runat="server" Text=" %"></asp:Label>
                        <asp:Literal ID="LTcompleteness" runat="server" Visible="true" Text=""> </asp:Literal>
                        <asp:RangeValidator ID="RNVcompleteness" runat="server" Text="*" ControlToValidate="TBXcompleteness"
                            MinimumValue="0" MaximumValue="100" Type="Integer"></asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="RFVcompleteness" runat="server" ControlToValidate="TBXcompleteness"
                            ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
                <br />              
            </FooterTemplate>
        </asp:Repeater>
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>
