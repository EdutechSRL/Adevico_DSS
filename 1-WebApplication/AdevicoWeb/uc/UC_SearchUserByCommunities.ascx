<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SearchUserByCommunities.ascx.vb"
    Inherits="Comunita_OnLine.UC_SearchUserByCommunities" %>
<style type="text/css">
    .RigaFiltro
    {
        width: 100%;
        text-align: left;
        padding-bottom: 5px;
        clear: left;
    }
    .StartCella120
    {
        width: 110px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella
    {
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella100
    {
        width: 100px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella120
    {
        width: 110px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella130
    {
        width: 130px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella150
    {
        width: 150px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella200
    {
        width: 190px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .RigaFiltroSearch
    {
        text-align: right;
        padding-bottom: 5px;
        clear: left;
    }
</style>

   <script type="text/javascript" language="Javascript">
       function ChangeCheckBoxState(id, checkState) {
           var cb = document.getElementById(id);
           if (cb != null)
               cb.checked = checkState;
       }

       function ChangeAllCheckBoxStates(checkState) {
           // Toggles through all of the checkboxes defined in the CheckBoxIDs array
           // and updates their value to the checkState input parameter
           if (CheckBoxIDs != null) {
               for (var i = 0; i < CheckBoxIDs.length; i++)
                   ChangeCheckBoxState(CheckBoxIDs[i], checkState);
           }
       }

       function ChangeHeaderAsNeeded() {
           // Whenever a checkbox in the GridView is toggled, we need to
           // check the Header checkbox if ALL of the GridView checkboxes are
           // checked, and uncheck it otherwise
           if (CheckBoxIDs != null) {
               // check to see if all other checkboxes are checked
               for (var i = 1; i < CheckBoxIDs.length; i++) {
                   var cb = document.getElementById(CheckBoxIDs[i]);
                   if (!cb.checked) {
                       // Whoops, there is an unchecked checkbox, make sure
                       // that the header checkbox is unchecked
                       ChangeCheckBoxState(CheckBoxIDs[0], false);
                       return;
                   }
               }

               // If we reach here, ALL GridView checkboxes are checked
               ChangeCheckBoxState(CheckBoxIDs[0], true);
           }
       }
       
      
    </script>
<div id="DIVricerca" runat="server">
    <table width="100%">
        <tr>
            <td colspan="6">
                <asp:Label ID="LBsearchTitle" runat="server" CssClass="Titolo_campo">Cerca nominativo</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="LBname" runat="server" CssClass="Titolo_campoSmall">Nome: </asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TXBname" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td>
            <td align="right">
                <asp:Label ID="LBsurname" runat="server" CssClass="Titolo_campoSmall">Cognome: </asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TXBsurname" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="LBmail" runat="server" CssClass="Titolo_campoSmall">Mail: </asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TXBmail" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td>
            <td align="right">
                <asp:Label ID="LBlogin" runat="server" CssClass="Titolo_campoSmall">Login: </asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TXBlogin" runat="server" CssClass="FiltroCampoSmall"></asp:TextBox>
            </td>
            <td align="right">
                <asp:Label ID="LBrole" runat="server" CssClass="Titolo_campoSmall">Tipo: </asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DDLrole" runat="server" CssClass="FiltroCampoSmall">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td valign="bottom">
                <asp:Button ID="BTNsearch" runat="server" Text="Cerca" CssClass="PulsanteFiltro" />
            </td>
        </tr>
    </table>
</div>
<div id="DIVcontenuto" runat="server">
    <div style="position: static; display: inline; text-align: right; width: 100%;">
        <asp:Label ID="LBuserPerPage" runat="server" CssClass="Filtro_RecordPaginazione">Num. user</asp:Label>
        <asp:DropDownList ID="DDLnumUserPerPage" runat="server" AutoPostBack="True" CssClass="Filtro_RecordPaginazione">
            <asp:ListItem Text="15" Value="15"></asp:ListItem>
            <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
            <asp:ListItem Text="50" Value="50"></asp:ListItem>
            <asp:ListItem Text="100" Value="100"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;
    </div>
    <asp:GridView ID="GRVuser" runat="server" SkinID="griglia700small" DataKeyNames="Id"
        AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" PageSize="25"
        Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
        <FooterStyle Font-Bold="True" ForeColor="White" />
        <HeaderStyle CssClass="ROW_header_Small" />
        <RowStyle CssClass="ROW_Normal_Small" />
        <AlternatingRowStyle CssClass="ROW_Alternate_Small" />
        <Columns>
            <asp:BoundField DataField="Id" Visible="False" />
            <asp:CommandField SelectText="Seleziona" ShowSelectButton="True" HeaderText="Opzioni"
                AccessibleHeaderText="Seleziona l'utente desiderato" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    <asp:CheckBox ID="CBselectAll" ToolTip="Click here to select/deselect all rows" runat="server" OnCheckedChanged="CBselectAll_CheckedChanged" AutoPostBack="true" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="CBselect" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%#Container.DataItem.Name%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%#Container.DataItem.Surname%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Mail">
                <ItemTemplate>
                    <%#Container.DataItem.Mail%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
</div>
<asp:Label ID="LBnoRecord" runat="server" CssClass="NoRecord" Visible="false">No record.</asp:Label>
<br />
<asp:CheckBox ID="CBshowPreview" runat="server" AutoPostBack="true" Text="Visualizza utenti selezionati"
    class="testo_campoSmall" />
<br />
<asp:Label runat="server" ID="LBnoRecordPreview" Visible="false" CssClass="NoRecord"></asp:Label>
<asp:Button ID="BTNupdatePreview" runat="server" CssClass="PulsanteFiltro" Text="Aggiorna"
    CausesValidation="false" Visible="true"></asp:Button>
<br />
<div runat="server" id="DIVpreview" visible="true" style="display:none">
    <asp:Panel runat="server" ID="PNLpreview" Height="60" ScrollBars="Vertical" BorderColor="Black"
        BorderWidth="1px">
        <asp:Repeater ID="RPTuserListPreview" runat="server">
            <ItemTemplate>
                <asp:ImageButton runat="server" ImageUrl="../Questionari/img/elimina.gif" ID="IMBElimina"
                    OnClick="IMBuser_OnClientClick" CommandArgument='<%# Eval("Id") %>' CommandName="delete" />
                <asp:LinkButton runat="server" ID="LNBuser" OnClick="LNBuser_OnClientClick" CommandName="delete"
                    CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("FullName") %>'>
                </asp:LinkButton>,
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>
</div>
<br />
