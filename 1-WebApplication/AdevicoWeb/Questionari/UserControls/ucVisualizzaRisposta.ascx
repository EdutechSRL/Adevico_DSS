<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucVisualizzaRisposta.ascx.vb" Inherits="Comunita_OnLine.ucVisualizzaRisposta" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
    <%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div class="nomePagina">
    <asp:Label runat="server" ID="LBNomeQuestionario" Font-Size="Large"></asp:Label>
    <br />
    <asp:Label Visible="false" runat="server" ID="LBNomeUtente"></asp:Label>
</div>
<br />
<asp:Panel runat="server" ID="PNLDettagli">
<asp:PlaceHolder runat="server" ID="PHStat"></asp:PlaceHolder>
    <br />
    <asp:DataList Width="100%" ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4"
        ForeColor="#333333">
        <ItemTemplate>
            <b>
                <%#DataBinder.Eval(Container.DataItem, "nomePagina")%>
            </b>
            <%#DataBinder.Eval(Container.DataItem, "descrizione")%>
            <br />
            <hr />
            <asp:DataList ID="DLDomande" runat="server" OnItemDataBound="loadDomandeOpzioni"
                Width="100%" DataKeyField="id">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container, "DataItem.numero")%>
                    .
                    <%#DataBinder.Eval(Container, "DataItem.testo")%>
                    <br />
                    <br />
                    <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                    <br />
                    <br />
                </ItemTemplate>
                <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <AlternatingItemStyle BackColor="WHITE" />
                <ItemStyle BackColor="WHITE" />
                <HeaderStyle BackColor="#EFF3FB" Font-Bold="True" ForeColor="White" />
            </asp:DataList>
        </ItemTemplate>
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        <AlternatingItemStyle BackColor="#E3EAEB" />
        <ItemStyle BackColor="#E3EAEB" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
    </asp:DataList>
</asp:Panel>
