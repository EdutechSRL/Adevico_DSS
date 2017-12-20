<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ShowCurrentUser.aspx.vb" Inherits="Comunita_OnLine.ShowCurrentUser"
    ValidateRequest="false" Culture="IT-it" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201605041410lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <div>
        <asp:GridView ID="GRVIscritti" runat="server" AllowPaging="True" AllowSorting="true"
            ShowHeader="true" AutoGenerateColumns="False" DataKeyNames="Id" CellPadding="2"
            CssClass="DataGrid_Generica">
            <AlternatingRowStyle Height="22px" CssClass="ROW_Alternate_Small" />
            <HeaderStyle Font-Size="11px" BackColor="Navy" ForeColor="White"
                HorizontalAlign="Left"></HeaderStyle>
            <RowStyle CssClass="ROW_Normal_Small" Height="22px" />
            <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="18px" VerticalAlign="Bottom">
            </PagerStyle>
            <PagerSettings Position="TopAndBottom" />
            <Columns>
                <asp:CommandField ButtonType="Link" ShowEditButton="True" EditImageUrl="~/images/proprieta.gif"
                    ItemStyle-Width="18" HeaderText="?" />
                <asp:CommandField ButtonType="Link" ShowDeleteButton="True" DeleteImageUrl="~/images/DG/x.gif"
                    ItemStyle-Width="18" HeaderText="C" />
                <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Cognome" SortExpression="Cognome"
                    DataField="Cognome" />
                <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Nome" SortExpression="Nome"
                    DataField="Nome" />
                <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Matricola" SortExpression="Matricola"
                    DataField="Matricola" />
                <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="IP" SortExpression="Ip"
                    DataField="IP" />
                <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Rimanente" SortExpression="Rimanente"
                    DataField="Rimanente" />
                <asp:BoundField ItemStyle-CssClass="ROW_TD_Small" HeaderText="Ultimo salvataggio"
                    SortExpression="Salvataggio" DataField="Salvataggio" />
                <asp:TemplateField HeaderText="" ItemStyle-CssClass="ROW_TD_Small">
                    <ItemTemplate>
                        <div>
                            <div>
                                <asp:LinkButton ID="LNK_Invalida" runat="server">Invalida</asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton ID="LNK_Restar" runat="server">Ricomincia</asp:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
