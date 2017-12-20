<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GlossarySelect.ascx.vb" Inherits="Comunita_OnLine.UC_GlossarySelect" %>
<div>

    <asp:DataGrid ID="DG_Glossary" runat="server" EnableViewState="true" AutoGenerateColumns="false" CssClass="Tbl_Glossary">
        <HeaderStyle CssClass="ROW_header_Small"/>
        <ItemStyle CssClass="ROW_Normal_Small"/>
        <AlternatingItemStyle CssClass="ROW_Alternate_Small"/>
        <Columns>

            <asp:TemplateColumn>
                <HeaderTemplate>
                    <%--<asp:CheckBox ID="Cbx_SelectAll" runat="server" />--%>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="Cbx_SelectItem" runat="server"/>
                    <asp:HiddenField ID="HF_Id" runat="server"/>
                </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
                <HeaderTemplate>
                    <asp:Label ID="Lbl_GlossaryName_t" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Lbl_GlossaryName" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
                <HeaderTemplate>
                    <asp:Label ID="Lbl_GlossaryItmNum_t" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Lbl_GlossaryItmNum" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>

        </Columns>
    </asp:DataGrid>

    <asp:Label ID="Lbl_NoGlossary" runat="server" Visible="false" CssClass="Testo_Campo no_item"></asp:Label>
</div>