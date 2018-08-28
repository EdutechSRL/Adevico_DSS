<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucStatisticheUtenti.ascx.vb"
    Inherits="Comunita_OnLine.ucStatisticheUtenti" %>
    <%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
    <div class="ddbuttonlist enabled" id="DVexport" runat="server"><!--
    --> <asp:HyperLink ID="HYPprintAll" runat="server" Target="_blank" CssClass="linkMenu"></asp:HyperLink><!--
    --><asp:HyperLink ID="HYPadvancedStatistics" runat="server" Target="_blank" CssClass="linkMenu" Visible="false"></asp:HyperLink><!--
    --><asp:LinkButton ID="LNBattemptsExportToCsv" runat="server" Text="Esporta to csv" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBattemptsExportToXml" runat="server" Text="Esporta to xml" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;" Visible="false"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportResultsToCsv" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;" ></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportResultsToXml" runat="server" Text="Esporta" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportResultsToSingleColumnCsv" runat="server" Text="*Export question answer - Single column CSV" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;" ></asp:LinkButton><!--
    --><asp:LinkButton ID="LNBexportResultsToSingleColumnXml" runat="server" Text="*Export question answer - Single column XML" CssClass="linkMenu"  OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--
    --></div>
    <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu"></asp:HyperLink>
    <asp:LinkButton ID="LNBIndietro" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp
</asp:Panel>
<br />
<asp:Panel runat="server" ID="PNLFiltri" CssClass="pnlFilter">
    <asp:Label runat="server" ID="LBTitolo"></asp:Label>
    <asp:CheckBox runat="server" ID="CHKUtentiComunita" />
    <asp:Label runat="server" ID="LBnrUtentiComunitaCorrente"></asp:Label>
    <asp:CheckBox runat="server" ID="CHKUtentiNonComunita" />
    <asp:Label runat="server" ID="LBnrUtentiTutteLeComunita"></asp:Label>
    <asp:CheckBox runat="server" ID="CHKUtentiEsterni" />
    <asp:Label runat="server" ID="LBnrUtentiEsterni"> </asp:Label>
    <asp:CheckBox runat="server" ID="CHKUtentiInvitati" />
    <asp:Label runat="server" ID="LBnrUtentiInvitati"></asp:Label><br />
    <asp:LinkButton ID="LNBOk" runat="server" CssClass="Link_Menu">Ok</asp:LinkButton>
</asp:Panel>
<div class="nomePagina" runat="server" id="DIVNomi">
    <div id="DVquestionnaireName" runat="server">
        <asp:Label runat="server" ID="LBNomeQuestionario" Font-Size="Large"></asp:Label>
    </div>
    <asp:Label Visible="false" runat="server" ID="LBNomeUtente"></asp:Label>
</div>
 <telerik:radtabstrip id="TBSQuestionari" runat="server" align="Justify" Width="100%" Height="26px" SelectedIndex="0" causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
    <tabs>
        <telerik:RadTab text="" value="0" runat="server"></telerik:RadTab>
        <telerik:RadTab text="" value="1" runat="server"></telerik:RadTab>
        <telerik:RadTab text="" value="2" runat="server"></telerik:RadTab>
    </tabs>
</telerik:radtabstrip>
<%--<radt:RadTabStrip ID="" runat="server" Skin="ClassicBlue" Align="justify"
    SelectedIndex="0" Width="100%" Height="31" CausesValidation="False" AutoPostBack="true">
    <Tabs>
        <radt:Tab ID="TAButentiQuestInviato" Value="0" Text="" ToolTip="">
        </radt:Tab>
        <radt:Tab ID="TAButentiQuestIncompleto" Value="1" Text="" ToolTip="">
        </radt:Tab>
        <radt:Tab ID="TAButentiQuestNonCompilato" Value="2" Text="" ToolTip="">
        </radt:Tab>
    </Tabs>
</radt:RadTabStrip>--%>
<asp:GridView ID="GRVUtenti" runat="server" DataKeyNames="RispostaID"
    AutoGenerateColumns="false" ShowFooter="false"
    CssClass="light fullwidth" AllowPaging="True" AllowSorting="True" PageSize="20">
    <Columns>
        <asp:TemplateField ControlStyle-CssClass="data name">
            <ItemTemplate>
                <%#Eval("cognome")%>
                <%#Eval("nome")%>
                <asp:Literal ID="LTattemptsInfo" runat="server" Visible="false"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ControlStyle-CssClass="data mail">
            <ItemTemplate>
                <%#Eval("mail")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ControlStyle-CssClass="data description">
            <ItemTemplate>
                <%#Eval("descrizione")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ControlStyle-CssClass="data icon">
            <ItemTemplate>
                <center>
                    <asp:ImageButton runat="server" ImageUrl="../img/entra.gif" ID="IMBVedi" CommandName="Vedi" CommandArgument='<%#Eval("RispostaID")%>'>
                    </asp:ImageButton>
                </center>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ControlStyle-CssClass="data icon">
            <ItemTemplate>
                <center>
                    <asp:ImageButton runat="server" ImageUrl="../img/elimina.gif" ID="IMBElimina" CommandName="Elimina" CommandArgument='<%#Eval("RispostaID")%>' />
                </center>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
<%--    <RowStyle CssClass="ROW_Normal_Small" Height="22px" />
    <EditRowStyle BackColor="#2461BF" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
   <HeaderStyle CssClass="ROW_header_Small_Center" />
    <AlternatingRowStyle CssClass="ROW_Alternate_Small" />--%>
</asp:GridView>
<asp:Panel runat="server" ID="PNLExcel" Visible="false">
    <asp:LinkButton ID="LKBEsportaExcel" runat="server" CssClass="Link_Menu">Esporta in Excel</asp:LinkButton>
</asp:Panel>
<asp:Panel runat="server" ID="PNLDettagli">

    <asp:PlaceHolder runat="server" ID="PHStat"></asp:PlaceHolder>
    <asp:LinkButton Visible="false" runat="server" ID="LNBStampaQuestionario" Text="Stampa Questionario"></asp:LinkButton>
    <asp:DataList ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4" CssClass="datalistPagine" RepeatLayout="Flow">
        <ItemTemplate>
            <span class="questionnairepage name">
                <b><%#DataBinder.Eval(Container.DataItem, "nomePagina")%></b>
            </span>
            <span class="questionnairepage description">
                <%#DataBinder.Eval(Container.DataItem, "descrizione")%>
            </span>
            <br />
            <hr />
            <asp:DataList ID="DLDomande" runat="server" OnItemDataBound="loadDomandeOpzioni" DataKeyField="id"
                CssClass="datalistDomande" RepeatLayout="Flow">
                <ItemTemplate>
                    <span class="questionnumber"><%#DataBinder.Eval(Container, "DataItem.numero")%>.</span>
                    <span class="questiontext"><%#DataBinder.Eval(Container, "DataItem.testo")%></span>
                    <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                </ItemTemplate>
                <FooterStyle CssClass="footer"/>
                <SelectedItemStyle CssClass="item-question Selected"/>
                <AlternatingItemStyle CssClass="item-question Alternate"/>
                <ItemStyle CssClass="item-question"/>
                <HeaderStyle CssClass="header"/>
            </asp:DataList>
        </ItemTemplate>
        <FooterStyle CssClass="footer"/>
        <SelectedItemStyle CssClass="item-page Selected"/>
        <AlternatingItemStyle CssClass="item-page alternate" />
        <ItemStyle CssClass="item-page"/>
        <HeaderStyle CssClass="header"/>
    </asp:DataList>
</asp:Panel>
