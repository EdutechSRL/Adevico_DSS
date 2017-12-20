<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucUserAttemptsStatistics.ascx.vb" Inherits="Comunita_OnLine.ucUserAttemptsStatistics" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
    <%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
    <div class="ddbuttonlist enabled" id="DVexport" runat="server"><!--
        --> <asp:HyperLink ID="HYPprintAll" runat="server" Target="_blank" CssClass="linkMenu"></asp:HyperLink><!--
        --><asp:LinkButton ID="LNBattemptsExportToCsv" runat="server" Text="Esporta to csv" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;" Visible="false"></asp:LinkButton><!--
        --><asp:LinkButton ID="LNBattemptsExportToXml" runat="server" Text="Esporta to xml" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;" Visible="false"></asp:LinkButton><!--
        --><asp:LinkButton ID="LNBexportResultsToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;" ></asp:LinkButton><!--
        --><asp:LinkButton ID="LNBexportResultsToXml" runat="server" Text="Esporta" CssClass="linkMenu active"  OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--
    --></div>
    <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu"></asp:HyperLink>
    <asp:LinkButton ID="LNBIndietro" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp
</asp:Panel>
<br />
<div class="nomePagina" runat="server" id="DIVNomi">
    <div id="DVquestionnaireName" runat="server">
        <asp:Label runat="server" ID="LBNomeQuestionario" Font-Size="Large"></asp:Label>
        <br />
        <br />
    </div>
    <asp:Label Visible="false" runat="server" ID="LBNomeUtente"></asp:Label>
</div>
 <telerik:radtabstrip id="TBSQuestionari" runat="server" align="Justify" Width="100%" Height="26px" SelectedIndex="0" causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
    <tabs>
        <telerik:RadTab text="" value="0" runat="server"></telerik:RadTab>
        <telerik:RadTab text="" value="1" runat="server"></telerik:RadTab>
    </tabs>
</telerik:radtabstrip>
<%--<radt:RadTabStrip ID="TBSQuestionari" runat="server" Skin="ClassicBlue" Align="justify"
    SelectedIndex="0" Width="100%" Height="31" CausesValidation="False" AutoPostBack="true">
    <Tabs>
        <radt:Tab ID="TAButentiQuestInviato" Value="0" Text="" ToolTip="">
        </radt:Tab>
        <radt:Tab ID="TAButentiQuestIncompleto" Value="1" Text="" ToolTip="">
        </radt:Tab>
    </Tabs>
</radt:RadTabStrip>--%>
<asp:GridView  ID="GRVUtenti" runat="server" DataKeyNames="RispostaID"
    AutoGenerateColumns="false"  ShowFooter="false"
    CssClass="light fullwidth" AllowPaging="True" AllowSorting="True" PageSize="20">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <%#Eval("cognome")%>
                <%#Eval("nome")%>
                <asp:Literal ID="LTattemptsInfo" runat="server" Visible="false"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%#Eval("ModifyedOn")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <center>
                    <asp:ImageButton runat="server" ImageUrl="../img/entra.gif" ID="IMBVedi" CommandName="Vedi" CommandArgument='<%#Eval("RispostaID")%>'>
                    </asp:ImageButton>
                </center>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <center>
                    <asp:ImageButton runat="server" ImageUrl="../img/elimina.gif" ID="IMBElimina" CommandName="Elimina" CommandArgument='<%#Eval("RispostaID")%>' />
                </center>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <%--<RowStyle CssClass="ROW_Normal_Small" Height="22px" />
    <EditRowStyle BackColor="#2461BF" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
    <HeaderStyle CssClass="ROW_header_Small_Center" />
    <AlternatingRowStyle CssClass="ROW_Alternate_Small" />--%>
</asp:GridView>
<asp:Panel runat="server" ID="PNLExcel" Visible="false">
    <asp:LinkButton ID="LKBEsportaExcel" runat="server" CssClass="Link_Menu">Esporta in Excel</asp:LinkButton>
</asp:Panel>
<br />
<asp:Panel runat="server" ID="PNLDettagli">

<asp:PlaceHolder runat="server" ID="PHStat"></asp:PlaceHolder>

    <br />
    <asp:LinkButton Visible="false" runat="server" ID="LNBStampaQuestionario" Text="Stampa Questionario"
        Width="160px"></asp:LinkButton><br />
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