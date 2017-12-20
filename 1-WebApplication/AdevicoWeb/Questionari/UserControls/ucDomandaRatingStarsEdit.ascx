<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucDomandaRatingStarsEdit.ascx.vb"
    Inherits="Comunita_OnLine.ucDomandaRatingStarsEdit" %>
<%@ Register TagPrefix="rade" Namespace="Telerik.WebControls" Assembly="RadEditor.Net2" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<!-- Rating Stars!!! -->
<asp:Panel ID="PNLDomanda" runat="server">
    
    <asp:FormView ID="FRVDomanda" runat="server" CellPadding="4" ForeColor="#333333"
        Width="100%">
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <ItemTemplate>
           <table width="100%">
                <td>
                     <div id="DIVpaginaCorrente" runat="server">
                        <asp:Label ID="LBLPaginaCorrente" runat="server" Text=""></asp:Label>
                        <asp:DropDownList Enabled='<%#Not isDomandaReadOnly%>' ID="DDLPagina" runat="server"
                            DataTextField="nomePagina" DataValueField="id">
                        </asp:DropDownList>
                    </div>
                </td>
                <td valign="middle" align="right" class="DIVHelp">
                    <asp:Label runat="server" ID="LBAiuto"></asp:Label>
                </td>
                <td width="30px">
                    <div class="DIVHelp">
                        <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="../img/Help.png" />
                    </div>
                </td>
            </table>
            <br />
            <br />
            <asp:Label ID="LBTestoDomanda" runat="server" Text=""></asp:Label>
            <br />
             <CTRL:CTRLeditor id="CTRLeditorTestoDomanda" runat="server" ContainerCssClass="containerclass"
                LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True"
                >
                </CTRL:CTRLeditor>

            <div style="display: <%#visibilityValutazione%>;">
                <br />
                <asp:Label ID="LBDifficolta" runat="server" Text=""></asp:Label>
                <asp:DropDownList Enabled='<%#Not isDomandaReadOnly%>' ID="DDLDifficolta" runat="server">
                    <asp:ListItem Value="0"></asp:ListItem>
                    <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                    <asp:ListItem Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <br />
            <asp:CheckBox runat="server" ID="CHKisObbligatoria" />
            <asp:Label ID="LBTitoloOpzioni" runat="server" Font-Bold="true"></asp:Label>
            <br />
            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="LBNumeroColonne" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDLNumeroColonne" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selezionaNumeroIntestazioni">
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem Selected="True">5</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            
                                        </asp:DropDownList>
                                    </td>
                                    <%--<td>
                                        <asp:Label ID="LBTipoIntestazione" runat="server" Text=""></asp:Label>
                                    </td>--%>
                                   <%-- <td>
                                        <asp:RadioButtonList runat="server" ID="RBLTipoIntestazione" RepeatDirection="Vertical"
                                            AutoPostBack="true" OnSelectedIndexChanged="selezionaTipoIntestazione">
                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>--%>
<%--                                </tr>
                                <tr>--%>
                                    <td>
                                        <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label><br />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDLNumeroOpzioni" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selezionaNumeroOpzioni">
                                            <asp:ListItem Selected="True">1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>6</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>8</asp:ListItem>
                                            <asp:ListItem>9</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
            <br />
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:DataList runat="server" ID="DLIntestazioni" RepeatDirection="Horizontal">
                            <ItemTemplate>
                                <asp:Label ID="LBIntestazione" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:RegularExpressionValidator ID="REVTXBIntestazione" runat="server" ControlToValidate="TXBIntestazione"
                                    Display="Dynamic" ValidationExpression="[\S\s]{1,100}">
                                </asp:RegularExpressionValidator>
                                <br />
                                <asp:TextBox ID="TXBIntestazione" Width="20px" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.testo")%>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataList runat="server" ID="DLOpzioni" OnItemCommand="eliminaOpzione" Enabled='<%#Not isDomandaReadOnly%>'>
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0" height="60">
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:ImageButton ID="IMBElimina" Visible='<%#Not isDomandaReadOnly%>' runat="server"
                                                ImageUrl="../img/elimina.gif" CommandName="elimina" AlternateText=""></asp:ImageButton>
                                            <asp:TextBox ID="TXBTestoMin" runat="server" Width="200px" Text='<%#DataBinder.Eval(Container, "DataItem.testo")%>'></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="REVTXBTestoMin" runat="server" ControlToValidate="TXBTestoMin"
                                                Display="Dynamic" ValidationExpression="[\S\s]{1,800}">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:CheckBox ID="CBisAltro" Visible="true" runat="server" TextAlign="Left" Checked='<%#DataBinder.Eval(Container, "DataItem.isAltro")%>' />
                                            <asp:Label ID="LBTestoMax" Visible="false" runat="server" Width="0" Text=""></asp:Label>
                                            <asp:TextBox ID="TXBTestoMax" runat="server" Visible="false" Width="0" Text='<%#DataBinder.Eval(Container, "DataItem.testoDopo")%>'></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="REVTXBTestoMax" runat="server" ControlToValidate="TXBTestoMax"
                                                Display="Dynamic" ValidationExpression="[\S\s]{1,200}">
                                            </asp:RegularExpressionValidator>
                                            <asp:Label ID="LBTestoMin" Visible="false" runat="server" Text=""></asp:Label><br />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                    <td>
                        <asp:Table runat="server" ID="TBLCheckbox">
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <asp:CheckBox Enabled='<%#Not isDomandaReadOnly%>' ID="CBmostraND" runat="server"
                Text="" />
            <br />
            <br />
            <asp:Label ID="LBTestoND" runat="server" Text=""></asp:Label>
            <br />
            <asp:TextBox ID="TXBTestoND" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.testoND")%>'
                Width="300"></asp:TextBox>
            <br />
            <asp:RegularExpressionValidator ID="REVTXBTestoND" runat="server" ControlToValidate="TXBTestoND"
                Display="Dynamic" ValidationExpression="[\S\s]{1,50}">
            </asp:RegularExpressionValidator>
            <br />
            <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label>
            <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"
                MaxLength="250"></asp:TextBox>
            <br />
            <asp:Label ID="LBSuggerimento" runat="server" Text=""></asp:Label>
            <br />
            <asp:TextBox ID="TXBSuggerimento" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
            <br />
            <br />
        </ItemTemplate>
    </asp:FormView>
</asp:Panel>
