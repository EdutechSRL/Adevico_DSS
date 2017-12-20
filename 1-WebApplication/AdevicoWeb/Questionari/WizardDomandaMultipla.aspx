<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardDomandaMultipla.aspx.vb" Inherits="Comunita_OnLine.WizardDomandaMultipla" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
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
                                    <asp:DropDownList Enabled='<%#isAperto%>' ID="DDLPagina" runat="server" DataTextField="nomePagina"
                                        DataValueField="id">
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
                        <br />
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                        <div style="display: <%#visibilityValutazione%>;">
                            <asp:Label ID="LBPesoDomanda" runat="server" Text="" Width="176px"></asp:Label>
                            <asp:TextBox ID="TXBPeso" runat="server" Enabled='<%#isAperto%>' Text="1"></asp:TextBox><br />
                            <br />
                            <asp:CompareValidator runat="server" ID="COVPesoInt" Operator="DataTypeCheck" Type="Integer"
                                Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                            <asp:CompareValidator runat="server" ID="COVPesoNot0" Operator="GreaterThan" ValueToCompare="0"
                                Type="Integer" Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                            <asp:Label ID="LBdifficolta" runat="server" Text="" Width="176px"></asp:Label>
                            <asp:DropDownList Enabled='<%#isAperto%>' ID="DDLDifficolta" runat="server">
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:CheckBox runat="server" ID="CHKisValutabile" Checked="true" />
                        </div>
                        <br />
                        <asp:CheckBox runat="server" ID="CHKisObbligatoria" Checked="false" />
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                        <asp:CustomValidator runat="server" ID="CUVvalutabile" OnServerValidate="CUVvalutabile_OnServerValidate"
                            ErrorMessage=""></asp:CustomValidator>
                        <br />
                        <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label>
                        <asp:DropDownList Enabled='<%#isAperto%>' ID="DDLNumeroOpzioni" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="selezionaNumeroOpzioni">
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
                        <asp:Label ID="LBnumeroMaxRisposte" runat="server" Text=""></asp:Label>
                        <asp:DropDownList Enabled='<%#isAperto%>' ID="DDLnumeroMaxRisposte" runat="server">
                        </asp:DropDownList>
                        <asp:Label ID="LBnumeroMaxRisposte2" runat="server" Text=""></asp:Label>
                        <br />
                        <br />
                        <asp:Label runat="server" ID="LBScelteMultiple" Text="" Font-Bold="true"></asp:Label><br />
                        <br />
                        <asp:DataList runat="server" ID="DLOpzioni" OnItemCommand="eliminaOpzione" Width="100%">
                            <ItemTemplate>
                                <table cellspacing="0" width="100%" bordercolor="black" bgcolor="white" style="border: 1px solid #333;"
                                    cellpadding="5">
                                    <tr>
                                        <td  align="left">
                                            <asp:Label ID="LBScelta" runat="server"></asp:Label>
                                            <asp:Label ID="LBLunghezzaOpzione" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="IMBElimina" Visible='<%#isAperto%>' runat="server" ImageUrl="img/elimina.gif"
                                                CommandName="elimina" AlternateText=""></asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <CTRL:CTRLeditor id="CTRLeditorScelta" runat="server" ContainerCssClass="containerclass" 
                                            LoaderCssClass="loadercssclass" Height="300px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True">
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
                                                <asp:TextBox ID="TXBPeso" Enabled='<%#isAperto%>' runat="server" Width="30px"></asp:TextBox>
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
                                            <asp:CheckBox ID="CBisAltro" Enabled='<%#isAperto%>' runat="server" Text=""></asp:CheckBox>
                                            <div style="display: <%#visibilityValutazione%>;">
                                                <asp:CheckBox ID="CBisCorretta" Enabled='<%#isAperto%>' runat="server" Text=""></asp:CheckBox>
                                            </div>
                                        </td>
                                        
                                    </tr>
                                </table>
                                <br />
                            </ItemTemplate>
                        </asp:DataList>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep4" runat="server" Title="Step 4">
                        <br />
                        <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label><br />
                        <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"
                            MaxLength="250"></asp:TextBox>
                        <br />
                        <asp:Label ID="LBSuggerimento" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:TextBox ID="TXBSuggerimento" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox><br />
                    </asp:WizardStep>
                </WizardSteps>
            </asp:Wizard>
        </ItemTemplate>
    </asp:FormView>
    <asp:Label runat="server" ID="LBErroreSalvataggio" Visible="false" ForeColor="red"
        Text=""></asp:Label><br />
</asp:Content>
