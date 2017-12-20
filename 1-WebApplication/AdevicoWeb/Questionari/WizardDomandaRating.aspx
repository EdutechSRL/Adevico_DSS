<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WizardDomandaRating.aspx.vb" Inherits="Comunita_OnLine.WizardDomandaRating"
    Title="Rating" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="rade" Namespace="Telerik.WebControls" Assembly="RadEditor.Net2" %>
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
                            <asp:Label ID="LBTestoDomanda" runat="server" Text=""></asp:Label>
                            <br />
                            <CTRL:CTRLeditor id="CTRLeditorTestoDomanda" runat="server" ContainerCssClass="containerclass" 
                                LoaderCssClass="loadercssclass" EditorHeight="300px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True">
                            </CTRL:CTRLeditor>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                            <br />
                            <asp:Label ID="LBDifficolta" runat="server" Text=""></asp:Label>
                            <asp:DropDownList ID="DDLDifficolta" runat="server">
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                            <br />
                            <asp:Label ID="LBTitoloOpzioni" runat="server" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:CheckBox runat="server" ID="CHKisObbligatoria" Checked="false" />
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="LBNumeroColonne" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDLNumeroColonne" runat="server" AutoPostBack="true" OnSelectedIndexChanged="selezionaNumeroIntestazioni">
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
                                    <td>
                                        <asp:Label ID="LBTipoIntestazione" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="RBLTipoIntestazione" RepeatDirection="Vertical"
                                            AutoPostBack="true" OnSelectedIndexChanged="selezionaTipoIntestazione">
                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label><br />
                                    </td>
                                    <td>
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
                                        <asp:DataList runat="server" ID="DLOpzioni" OnItemCommand="eliminaOpzione">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0" height="60">
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:ImageButton ID="IMBElimina" runat="server" ImageUrl="img/elimina.gif" CommandName="elimina"
                                                                AlternateText=""></asp:ImageButton>
                                                            <asp:TextBox ID="TXBTestoMin" runat="server" Width="135px" Text='<%#DataBinder.Eval(Container, "DataItem.testo")%>'></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="REVTXBTestoMin" runat="server" ControlToValidate="TXBTestoMin"
                                                                Display="Dynamic" ValidationExpression="[\S\s]{1,200}">
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
                            <asp:CheckBox ID="CBmostraND" runat="server" Text="" />
                            <br />
                            <br />
                            <asp:Label ID="LBTestoND" runat="server" Text=""></asp:Label>
                            <br />
                            <asp:TextBox ID="TXBTestoND" runat="server" Width="300"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="REVTXBTestoND" runat="server" ControlToValidate="TXBTestoND"
                                Display="Dynamic" ValidationExpression="[\S\s]{1,50}">
                            </asp:RegularExpressionValidator>
                            <br />
                        </asp:WizardStep>
                        <asp:WizardStep ID="WizardStep4" runat="server" Title="Step 4">
                            <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"
                                MaxLength="250"></asp:TextBox>
                            <br />
                            <asp:Label ID="LBSuggerimento" runat="server" Text=""></asp:Label>
                            <br />
                            <asp:TextBox ID="TXBSuggerimento" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            <br />
                            <br />
                        </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
            </ItemTemplate>
        </asp:FormView>
    </asp:Panel>
    <asp:Label runat="server" ID="LBErroreSalvataggio" Visible="false" ForeColor="red"
        Text=""></asp:Label><br />
</asp:Content>
