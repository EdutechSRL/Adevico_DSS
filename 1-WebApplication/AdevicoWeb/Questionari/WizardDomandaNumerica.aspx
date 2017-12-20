<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardDomandaNumerica.aspx.vb" Inherits="Comunita_OnLine.WizardDomandaNumerica"
    Title="Pagina senza titolo" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
   <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
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
                            <asp:Label ID="LBPeso" runat="server" Text="" Width="176px"></asp:Label>
                            <asp:TextBox ID="TXBPeso" runat="server"></asp:TextBox><br />
                            <asp:CompareValidator runat="server" ID="COVPesoInt" Operator="DataTypeCheck" Type="Integer"
                                Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                            <asp:CompareValidator runat="server" ID="COVPesoNot0" Operator="GreaterThan" ValueToCompare="0"
                                Type="Integer" Display="Dynamic" ControlToValidate="TXBPeso" ErrorMessage="" />
                            <br />
                            <asp:Label ID="LBdifficolta" runat="server" Text="" Width="176px"></asp:Label>
                            <asp:DropDownList ID="DDLDifficolta" runat="server">
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:CheckBox runat="server" ID="CHKisValutabile" />
                            <br />
                            <asp:CheckBox runat="server" ID="CHKisObbligatoria" Checked="false" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                            <br />
                            <br />
                            <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label>
                            <asp:DropDownList ID="DDLNumeroOpzioni" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selezionaNumeroOpzioni">
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
                            <asp:DataList runat="server" Width="100%" ID="DLOpzioni" OnItemCommand="eliminaOpzione">
                                <ItemTemplate>
                                    <table cellspacing="0" width="100%" bordercolor="black" bgcolor="white" style="border: 1px solid #333;"
                                        cellpadding="5">
                                        <tr>
                                            <td bordercolor="black" colspan="2">
                                                <asp:Label ID="LBLTestoPrima" runat="server" Text=""></asp:Label><br />
                                                  <CTRL:CTRLeditor id="CTRLeditorTestoPrima" runat="server" ContainerCssClass="containerclass" 
                                                    LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True">
                                                  </CTRL:CTRLeditor>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBLRispostaCorretta" runat="server" Text=""></asp:Label><br />
                                                <asp:TextBox Width="120" ID="TXBRispostaCorretta" runat="server"></asp:TextBox>
                                                <br />
                                                <asp:CompareValidator ID="COVRispostaNumerica_isDouble" runat="server" Operator="DataTypeCheck"
                                                    Type="Double" Width="120" Display="Dynamic" ControlToValidate="TXBRispostaCorretta">
                                                </asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LBPesoRisposta" runat="server"></asp:Label><br />
                                                <asp:TextBox ID="TXBPesoRisposta" runat="server" Width="30px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:CompareValidator runat="server" ID="COVPesoIntOpzioni" Operator="DataTypeCheck"
                                                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPesoRisposta" ErrorMessage="" />
                                                <asp:RangeValidator runat="server" ID="RVPesoMin100" MinimumValue="-100" MaximumValue="100"
                                                    Type="Integer" ControlToValidate="TXBPesoRisposta" ErrorMessage="" />
                                            </td>
                                            <td>
                                                <asp:Label ID="LBLTestoDopo" runat="server" Text=""></asp:Label>
                                                <br />
                                                <asp:TextBox ID="TXBTestoDopo" runat="server" Width="80"></asp:TextBox>
                                                <br />
                                                <asp:RegularExpressionValidator ID="REVTXBTestoDopo" runat="server" ControlToValidate="TXBTestoDopo"
                                                    Display="Dynamic" ValidationExpression="[\S\s]{1,50}">
                                                </asp:RegularExpressionValidator>
                                                <asp:Label ID="LBLDimensione" runat="server" Visible="false" Text=""></asp:Label>
                                                <asp:TextBox Width="30" Visible="false" ID="TXBDimensione" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="3" align="right">
                                                <asp:ImageButton ID="IMBElimina" runat="server" ImageUrl="img/elimina.gif" CommandName="elimina"
                                                    AlternateText=""></asp:ImageButton>
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
    </asp:Panel>
</asp:Content>
