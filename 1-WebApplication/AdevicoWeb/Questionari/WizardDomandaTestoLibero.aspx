<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardDomandaTestoLibero.aspx.vb" Inherits="Comunita_OnLine.WizardDomandaTestoLibero" %>

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
                                        <asp:DropDownList Enabled="<%#isAperto%>" ID="DDLPagina" runat="server" DataTextField="nomePagina"
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
                                LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%">
                            </CTRL:CTRLeditor>
                            <br />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                            <div style="display: <%#visibilityValutazione%>;">
                                <asp:Label ID="LBPeso" runat="server" Text="Peso" Width="176px"></asp:Label>
                                <asp:TextBox ID="TXBPeso" runat="server" Enabled='<%#isAperto%>'></asp:TextBox><br />
                                <asp:CompareValidator runat="server" ID="COVPesoInt" Operator="DataTypeCheck" Type="Integer"
                                    Display="Dynamic" ControlToValidate="TXBPeso" />
                                <asp:CompareValidator runat="server" ID="COVPesoNot0" Operator="GreaterThan" ValueToCompare="0"
                                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPeso" />
                                <br />
                                <asp:Label ID="LBdifficolta" runat="server" Text="" Width="176px"></asp:Label>
                                <asp:DropDownList Enabled='<%#isAperto%>' ID="DDLDifficolta" runat="server">
                                    <asp:ListItem Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                                    <asp:ListItem Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <br />
                                <asp:CheckBox runat="server" ID="CHKisValutabile" />
                            </div>
                            <br />
                            <asp:CheckBox runat="server" ID="CHKisObbligatoria" Checked="false" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
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
                            <asp:DataList runat="server" Width="100%" ID="DLOpzioni" OnItemCommand="eliminaOpzione"
                                Visible="true">
                                <ItemTemplate>
                                    <table cellspacing="0" width="100%" bordercolor="black" bgcolor="white" style="border: 1px solid #333;"
                                        cellpadding="5">
                                        <tr>
                                            <td><asp:Label ID="LBLEtichetta" runat="server" Text=""></asp:Label></td>
                                            <td align="right">
                                                 <asp:ImageButton ID="IMBElimina" Visible='<%#isAperto%>' runat="server" ImageUrl="img/elimina.gif"
                                                    CommandName="elimina" AlternateText=""></asp:ImageButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bordercolor="#EFF3FB" colspan="2">
                                                <CTRL:CTRLeditor id="CTRLeditorEtichetta" runat="server" ContainerCssClass="containerclass" 
                                                    LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%">
                                                </CTRL:CTRLeditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="display: <%#visibilityValutazione%>;">
                                                    <asp:Label ID="LBPesoRisposta" runat="server"></asp:Label><asp:TextBox ID="TXBPesoRisposta" runat="server" Width="30px" Text='<%#DataBinder.Eval(Container, "DataItem.peso")%>'></asp:TextBox>%
                                                </div>
                                            </td>
                                            <td>
                                                <asp:CompareValidator runat="server" ID="COVPesoIntOpzioni" Operator="DataTypeCheck"
                                                    Type="Integer" Display="Dynamic" ControlToValidate="TXBPesoRisposta" ErrorMessage="" />
                                                <asp:RangeValidator runat="server" ID="RVPesoMin100" MinimumValue="-100" MaximumValue="100"
                                                    Type="Integer" ControlToValidate="TXBPesoRisposta" ErrorMessage="" />
                                            </td>
                                        </tr>
                                    </table>
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
                            <asp:TextBox ID="TXBSuggerimento" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            <br />
                        </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
                </center>
            </ItemTemplate>
        </asp:FormView>
        <asp:Label runat="server" ID="LBErroreSalvataggio" Visible="false" ForeColor="red"
            Text=""></asp:Label><br />
    </asp:Panel>
</asp:Content>
