<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionariGestioneList.aspx.vb" Inherits="Comunita_OnLine._QuestionariGestioneList" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
     <script type="text/javascript" src="../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
      <script type="text/javascript">
          var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
          var CookieName = "<% = Me.CookieName %>";
          var DisplayMessage = "<% = Me.DisplayMessageToken %>";
          var DisplayTitle = "<% = Me.DisplayTitleToken %>";
    </script>

      <script type="text/javascript">
          var fileDownloadCheckTimer;
          function blockUIForDownload() {
              var token = new Date().getTime(); //use the current timestamp as the token value
              $("input[id='" + TokenHiddenFieldId + "']").val(token);
              $.blockUI({ message: DisplayMessage, title: DisplayTitle, draggable: false, theme: true });
              fileDownloadCheckTimer = window.setInterval(function () {
                  var cookieValue = $.cookie(CookieName);
                  if (cookieValue == token)
                      finishDownload();
              }, 1000);
          }

          function finishDownload() {
              window.clearInterval(fileDownloadCheckTimer);
              $.cookie(CookieName, null); //clears this cookie value
              $.unblockUI();
          }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:Panel ID="PNLmenu" runat="server" Width="99%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBNuovoQuestionario" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>&nbsp;
        <asp:LinkButton ID="LNBNuovaCartella" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <asp:LinkButton ID="LNBCestino" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
        <div class="DIVHelp">
            <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help20px.png" Style="margin-top: 10px;
                float: right;" />
            <asp:Label runat="server" ID="LBHelp" Style="margin-top: 12px; float: right;"></asp:Label>
        </div>
    </asp:Panel>
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <asp:Label ID="LBLCartella" runat="server" Font-Bold="True"></asp:Label>
            <br />
            <asp:Panel runat="server" ID="PNLQuestionari" Width="100%">
                <asp:GridView Width="100%" ID="GRVElenco" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                    OnRowCommand="GRVElenco_RowCommand"
                    ShowFooter="false" CssClass="light fullwidth" AllowPaging="True"
                    AllowSorting="True" PageSize="20">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="45px">
                            <ItemTemplate>
                                <center>
                                    <asp:Image runat="server" ImageUrl="img/chiuso.gif" ID="IMChiuso"></asp:Image>
                                    <asp:ImageButton runat="server" ID="IMBsbloccato" ImageUrl="img/verde.gif" CommandName="NotIsBloccato">
                                    </asp:ImageButton>
                                    <asp:ImageButton runat="server" ID="IMBbloccato" ImageUrl="img/rosso.gif" CommandName="NotIsBloccato">
                                    </asp:ImageButton>
                                </center>
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                    <asp:ImageButton runat="server" ImageUrl="img/entra.gif" ID="IMBAnteprima" CommandName="Anteprima">
                                    </asp:ImageButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ImageUrl="img/modifica-documento.gif" ID="IMBGestione"
                                    CommandName="Gestione"></asp:ImageButton>
                                <asp:LinkButton runat="server" ID="LNKNomeQuestionario" Text='<%#Eval("nome")%>'
                                    CommandName="Gestione"></asp:LinkButton>
                                <asp:Label ID="LBLNomeQuestionario" runat="server" Text='<%#Eval("nome")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                    <asp:ImageButton runat="server" ImageUrl="img/modifica-domande.gif" ID="IMBDomande"
                                        CommandName="Domande"></asp:ImageButton>
                                    <asp:ImageButton runat="server" ImageUrl="img/chart.gif" ID="IMBStatisticheGenerali"
                                        Visible="false" CommandName="StatisticheGenerali"></asp:ImageButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                    <asp:ImageButton runat="server" ImageUrl="img/elimina.gif" ID="IMBElimina" CommandName="Elimina">
                                    </asp:ImageButton>
                                    <asp:ImageButton runat="server" ImageUrl="img/userStat.gif" ID="IMBStatisticheUtenti"
                                        Visible="false" CommandName="StatisticheUtenti"></asp:ImageButton>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="dataInizio" HeaderStyle-Width="17%" />
                        <%-- DataFormatString="{0:MM/dd/yy hh:mm}" HtmlEncode="false" />--%>
                        <asp:BoundField DataField="dataFine" HeaderStyle-Width="17%" />
                        <%-- DataFormatString="{0:MM/dd/yy hh:mm}" HtmlEncode="false"/>--%>
                        <asp:TemplateField HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="DDLLingue" Width="43" DataTextField="sigla"
                                    DataValueField="id">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30">
                            <ItemTemplate>
                                <center>
                                    <asp:ImageButton runat="server" ID="IMBExport" ImageUrl="img/excel.gif" CommandName="Export"
                                        Visible="false" />
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                  <%--  <RowStyle CssClass="ROW_Normal_Small" Height="22px" />--%>
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="ROW_Page_Small" HorizontalAlign="Right" Height="25px" VerticalAlign="Bottom" />
<%--                    <HeaderStyle CssClass="ROW_header_Small_Center" />
                    <AlternatingRowStyle CssClass="ROW_Alternate_Small" />--%>
                </asp:GridView>
            </asp:Panel>
            &nbsp;<br />
            <asp:Panel runat="server" ID="PNLGruppi" BorderWidth="1" Visible="False" Width="100%">
                <asp:Label runat="server" ID="LBTitoloSottoCartelle"></asp:Label><br />
                <br />
                <asp:DataList ID="DLGruppi" runat="server" DataKeyField="id" CssClass="light fullwidth"
                    OnItemCommand="DLGruppiItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="LNBAnteprima" CommandName="viewGruppo">
                <%#Eval("nome")%>
                        </asp:LinkButton>
                        &nbsp&nbsp&nbsp
                    </ItemTemplate>
                  <%--  <ItemStyle CssClass="ROW_Normal_Small" Height="22px" />--%>
                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                 <%--   <HeaderStyle CssClass="ROW_header_Small_Center" />
                    <AlternatingItemStyle CssClass="ROW_Alternate_Small" />--%>
                </asp:DataList>
            </asp:Panel>
            &nbsp;
            <br />
        </asp:View>
        <asp:View runat="server" ID="VIWCancellaQuestionario">
            <div align="center">
                <asp:Label runat="server" ID="LBLNomeQuestionario" Font-Bold="true"></asp:Label><br />
                <asp:Label runat="server" ID="LBLTestoDescrizione"></asp:Label><br />
                <asp:DataList runat="server" ID="DLLingue" DataKeyField="id">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="CHKSelezionaLingua" Text='<%#Eval("nome")%>' />
                    </ItemTemplate>
                </asp:DataList>
                <br />
                <asp:Button runat="server" ID="BTNEliminaLingua" CssClass="Link_Menu" />
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <br />
            <br />
            <asp:Label ID="LBerrore" runat="server"></asp:Label>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>