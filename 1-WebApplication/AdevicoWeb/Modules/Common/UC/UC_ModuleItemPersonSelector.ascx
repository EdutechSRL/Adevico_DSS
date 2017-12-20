<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModuleItemPersonSelector.ascx.vb" Inherits="Comunita_OnLine.UC_ModuleItemPersonSelector" %>
<style type="text/css">
   
    .PS_Row
    {
        float:left;
        width: 100%;
    }
    .PS_Cell1
    {
        width: 15%;
        float:left;
        display:inline-block;
    }
    .PS_Cell2
    {
        width: 20%;
        float:left;
        display:inline-block;
    }
    .PS_Cell3
    {
        float:right;
        display:inline-block;
        text-align:right;
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

<div class="PS_Container">
    <div class="PS_Row">
        <div class="PS_Cell1">
            <asp:Label ID="lbl_Name_t" runat="server">#Nome:</asp:Label>
        </div>
        <div class="PS_Cell2">
            <asp:TextBox ID="txb_Name" runat="server"></asp:TextBox>
        </div>
        <div class="PS_Cell1">
            <asp:Label ID="lbl_Surname_t" runat="server">#Cognome:</asp:Label>
        </div>
        <div class="PS_Cell2">
            <asp:TextBox ID="txb_Surname" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="PS_Row">
        <div class="PS_Cell1">
            <asp:Label ID="lbl_Login_t" runat="server">#Login:</asp:Label>
        </div>
        <div class="PS_Cell2">
            <asp:TextBox ID="txb_Login" runat="server"></asp:TextBox>
        </div>
        <div class="PS_Cell3">
            <asp:LinkButton ID="lnk_Search" runat="server" CssClass="PulsanteFiltro">#Cerca</asp:LinkButton>
        </div>
    </div>

    <div class="PS_Row">
        <div class="PS_Cell3">
            <asp:Label ID="lbl_NumRecord_t" runat="server" CssClass="Filtro_RecordPaginazione">N° risultati per pagina</asp:Label>
            <asp:DropDownList ID="ddl_NumReconrd" runat="server" CssClass="Filtro_RecordPaginazione">
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="PS_Row">
        <br />
        <asp:GridView ID="GRVuser" runat="server" SkinID="griglia700small" DataKeyNames="Id"
            AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" PageSize="25"
            Width="100%" CellPadding="4" GridLines="None">
            <FooterStyle Font-Bold="True"/>
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
                <asp:TemplateField  HeaderText="Nome">
                    <ItemTemplate>
                        <%#Container.DataItem.Name%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cognome">
                    <ItemTemplate>
                        <%#Container.DataItem.Surname%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Info">
                    <ItemTemplate>
                        <%# Container.DataItem.Info%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Literal ID="CheckBoxIDsArray" runat="server"></asp:Literal>
        <asp:Label ID="LBnoRecord" runat="server" CssClass="NoRecord" Visible="false">No record.</asp:Label>
    </div>

</div>