<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardDomandaDropDown.aspx.vb" Inherits="Comunita_OnLine.WizardDomandaDropDown"
    Title="Pagina senza titolo" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
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
                <asp:Wizard ID="WIZDomanda" runat="server" DisplaySideBar="false">
                    <WizardSteps>
                        <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1">
                            <table width="100%">
                                <td>
                                    <div id="DIVpaginaCorrente" runat="server">
                                        <asp:Label ID="LBLPaginaCorrente" runat="server" Text=""></asp:Label>
                                        <asp:DropDownList ID="DDLPagina" runat="server" DataTextField="nomePagina" DataValueField="id">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td valign="middle" align="right" class="DIVHelp">
                                    <asp:Label runat="server" ID="LBAiuto"></asp:Label>
                                </td>
                                <td width="30px" class="DIVHelp">
                                    <asp:ImageButton ID="IMBHelp" runat="server" ImageUrl="img/Help.png" />
                                </td>
                            </table>
                            <br />
                            <br />
                            <asp:Label ID="LBTestoDomanda" runat="server" Text=""></asp:Label><br />
                               <CTRL:CTRLeditor id="CTRLeditorTestoDomanda" runat="server" ContainerCssClass="containerclass" 
                            LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True">
                            </CTRL:CTRLeditor>
                            <br />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                            <asp:Label ID="LBPeso" runat="server" Text=""></asp:Label><br />
                            <asp:TextBox ID="TXBPeso" runat="server"></asp:TextBox><br />
                            <asp:CompareValidator runat="server" ID="COVPesoInt" Operator="DataTypeCheck" Type="Integer"
                                Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                            <asp:CompareValidator runat="server" ID="COVPesoNot0" Operator="GreaterThan" ValueToCompare="0"
                                Type="Integer" Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                            <br />
                            <asp:Label ID="LBdifficolta" runat="server" Text=""></asp:Label>
                            <asp:DropDownList ID="DDLDifficolta" runat="server">
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:CheckBox runat="server" ID="CHKisValutabile" /><br />
                            <asp:CheckBox runat="server" ID="CHKisObbligatoria" Checked="false" />
                            <asp:Label ID="LBEtichetta" runat="server" Text=""></asp:Label><br />
                            <asp:TextBox ID="TXBEtichetta" runat="server"></asp:TextBox>
                            <br />
                            <asp:Label ID="LBNome" runat="server" Text="" Visible="false"></asp:Label>
                            <br />
                            <asp:TextBox ID="TXBNomeDropDown" Visible="false" runat="server"></asp:TextBox>
                            <asp:CheckBox ID="CBOrdina" runat="server" Text="" /><br />
                            <br />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                            <asp:CustomValidator runat="server" ID="CUVvalutabile" OnServerValidate="CUVvalutabile_OnServerValidate"
                                ErrorMessage="ERRORE"></asp:CustomValidator><br />
                            <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label>
                            <asp:DropDownList ID="DDLNumeroOpzioni" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selezionaOpzioni">
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
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Label runat="server" ID="LBScelteMultiple" Text=""></asp:Label><br />
                            <br />
                            <asp:DataList runat="server" Width="100%" ID="DLOpzioni" OnItemCommand="eliminaOpzione">
                                <ItemTemplate>
                                    <table border="1px" bordercolor="black" cellspacing="0" cellpadding="5">
                                        <tr>
                                            <td align="left" bordercolor="#EFF3FB">
                                                <asp:Label ID="LBScelta" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td bordercolor="#EFF3FB">
                                                <asp:Label ID="LBPesoRisposta" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bordercolor="#EFF3FB">
                                                <asp:ImageButton ID="IMBElimina" runat="server" ImageUrl="img/elimina.gif" CommandName="elimina"
                                                    AlternateText=""></asp:ImageButton>
                                                <asp:TextBox ID="TXBScelta" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.testo")%>'></asp:TextBox><br />
                                                <br />
                                                <asp:RegularExpressionValidator ID="REVTXBScelta" runat="server" ErrorMessage=""
                                                    ControlToValidate="TXBScelta" Display="Dynamic" ValidationExpression="[\S\s]{1,50}">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                            <td bordercolor="#EFF3FB" align="left">
                                                <asp:CheckBox ID="CBisCorretta" runat="server" Text="" Checked='<%#DataBinder.Eval(Container, "DataItem.isCorretta")%>'>
                                                </asp:CheckBox>
                                            </td>
                                            <td bordercolor="#EFF3FB">
                                                <asp:TextBox ID="TXBPesoRisposta" runat="server" Width="30px" Text='<%#DataBinder.Eval(Container, "DataItem.peso")%>'></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="COVPesoIntOpzioni" Operator="DataTypeCheck"
                                                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPesoRisposta" ErrorMessage="" />
                                                <asp:RangeValidator runat="server" ID="RVPesoMin100" MinimumValue="0" MaximumValue="100"
                                                    ControlToValidate="TXBPesoRisposta" Type="Integer" ErrorMessage="" />
                                            </td>
                                            <td colspan="3">
                                                <asp:Label runat="server" ID="LBsuggestionOption"></asp:Label>
                                                <asp:TextBox runat="server" ID="TXBsuggestionOption" Width="100%" TextMode="MultiLine"
                                                    Rows="3" Text='<%#DataBinder.Eval(Container, "DataItem.suggestion")%>'></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep4" runat="server" Title="Step 4">
                            <br />
                            <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label>
                            <br />
                            <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                            <br />
                            <asp:Label ID="LBSuggerimento" runat="server" Text=""></asp:Label>
                            <br />
                            <asp:TextBox ID="TXBSuggerimento" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox><br />
                        </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
            </ItemTemplate>
        </asp:FormView>
    </asp:Panel>
    <asp:Label runat="server" ID="LBErroreSalvataggio" Visible="false" ForeColor="red"
        Text=""></asp:Label><br />
    <asp:LinkButton ID="LNBSalva2" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
</asp:Content>
