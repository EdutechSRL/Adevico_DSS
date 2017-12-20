<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucDomandaMultiplaEdit.ascx.vb"
    Inherits="Comunita_OnLine.ucDomandaMultiplaEdit" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<script type="text/javascript">
    function changeText(oCheck, controlloTesto) {
        var oTesto;
        oTesto = document.getElementById(controlloTesto)
        if (oCheck.checked)
            oTesto.value = '100'
        else
            oTesto.value = '0'
    }

</script>
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
            <asp:Label ID="LBTestoDomanda" runat="server" Text=""></asp:Label><br />
              <CTRL:CTRLeditor id="CTRLeditorTestoDomanda" runat="server" ContainerCssClass="containerclass" 
                    LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%"  >
                    </CTRL:CTRLeditor>
            <br />
            <br />
            <div style="display: <%#visibilityValutazione%>;">
                <asp:Label ID="LBPesoDomanda" runat="server" Text="" Width="176px"></asp:Label>
                <asp:TextBox ID="TXBPeso" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.peso")%>'></asp:TextBox><br />
                <br />
                <asp:CompareValidator runat="server" ID="COVPesoInt" Operator="DataTypeCheck" Type="Integer"
                    Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                <asp:CompareValidator runat="server" ID="COVPesoNot0" Operator="GreaterThan" ValueToCompare="0"
                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                <asp:Label ID="LBdifficolta" runat="server" Text="" Width="176px"></asp:Label>
                <asp:DropDownList Enabled='<%#Not isDomandaReadOnly%>' ID="DDLDifficolta" runat="server">
                    <asp:ListItem Value="0"></asp:ListItem>
                    <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                    <asp:ListItem Value="2"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                <asp:CheckBox runat="server" ID="CHKisValutabile" Checked='<%#DataBinder.Eval(Container, "DataItem.isValutabile")%>' />
                <asp:CustomValidator runat="server" ID="CUVvalutabile" OnServerValidate="CUVvalutabile_OnServerValidate"
                    ErrorMessage=""></asp:CustomValidator>
            </div>
            <br />
            <asp:CheckBox runat="server" ID="CHKisObbligatoria" Checked='<%#DataBinder.Eval(Container, "DataItem.isObbligatoria")%>' />
            <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label>
            <asp:DropDownList Enabled='<%#Not isDomandaReadOnly%>' ID="DDLNumeroOpzioni" runat="server"
                AutoPostBack="true" OnSelectedIndexChanged="selezionaNumeroOpzioni">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>16</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>18</asp:ListItem>
                <asp:ListItem>19</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>21</asp:ListItem>
                <asp:ListItem>22</asp:ListItem>
                <asp:ListItem>23</asp:ListItem>
                <asp:ListItem>24</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="LBnumeroMaxRisposte" runat="server" Text=""></asp:Label>
            <asp:DropDownList Enabled='<%#Not isDomandaReadOnly%>' ID="DDLnumeroMaxRisposte"
                runat="server">
            </asp:DropDownList>
            <asp:Label ID="LBnumeroMaxRisposte2" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <asp:Label runat="server" ID="LBScelteMultiple" Text="" Font-Bold="true"></asp:Label><br />
            <br />
            <asp:DataList runat="server" ID="DLOpzioni" OnItemCommand="eliminaOpzione" Width="100%">
                <ItemTemplate>
                    <table cellspacing="0" width="100%" bordercolor="black" style="background-color: White;
                        border: 1px solid #333;" cellpadding="5">
                        <tr>
                            <td align="left">
                                <asp:Label ID="LBScelta" runat="server"></asp:Label>
                                <asp:Label ID="LBLunghezzaOpzione" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="IMBElimina" Visible='<%#Not isDomandaReadOnly%>' runat="server"
                                    ImageUrl="../img/elimina.gif" CommandName="elimina" AlternateText=""></asp:ImageButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <CTRL:CTRLeditor id="CTRLeditorScelta" runat="server" ContainerCssClass="containerclass" 
                                LoaderCssClass="loadercssclass" EditorHeight="230px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True" >
                                </CTRL:CTRLeditor>

                                <br />
                                <asp:Label runat="server" ID="LBsuggerimentoOpzione">sugg.</asp:Label>
                                <br />
                                <asp:TextBox runat="server" ID="TXBSuggerimentoOpzione" Width="100%" TextMode="MultiLine"
                                    Rows="3" Text='<%#DataBinder.Eval(Container, "DataItem.suggestion")%>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="display: <%#visibilityValutazione%>;">
                                    <asp:Label ID="LBPeso" runat="server"></asp:Label>
                                    <asp:TextBox ID="TXBPeso" runat="server" Width="30px" Text='<%#DataBinder.Eval(Container, "DataItem.peso")%>'></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <asp:CompareValidator runat="server" ID="COVPesoIntOpzioni" Operator="DataTypeCheck"
                                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                                <asp:RangeValidator runat="server" ID="RVPesoMin100" MinimumValue="-100" MaximumValue="100"
                                    Type="Integer" ControlToValidate="TXBPeso" ErrorMessage="" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="CBisAltro" Enabled='<%#Not isDomandaReadOnly%>' runat="server"
                                    Text="" Checked='<%#DataBinder.Eval(Container, "DataItem.isAltro")%>'></asp:CheckBox>
                                <div style="display: <%#visibilityValutazione%>;">
                                    <asp:CheckBox ID="CBisCorretta" runat="server" Text="" Checked='<%#DataBinder.Eval(Container, "DataItem.isCorretta")%>'>
                                    </asp:CheckBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                </ItemTemplate>
            </asp:DataList>
            <br />
            <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label>
            <br />
            <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"
                MaxLength="250"></asp:TextBox>
            <br />
            <asp:Label ID="LBSuggerimento" runat="server" Text=""></asp:Label>
            <br />
            <asp:TextBox ID="TXBSuggerimento" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox><br />
        </ItemTemplate>
    </asp:FormView>
</asp:Panel>
