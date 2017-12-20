<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucDomandaMeetingEdit.ascx.vb" Inherits="Comunita_OnLine.ucDomandaMeetingEdit" %>

<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
    <%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<asp:Panel ID="PNLDomanda" runat="server">
    <asp:FormView ID="FRVDomanda" runat="server" CellPadding="4" ForeColor="#333333"
        Width="100%">
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <ItemTemplate>
            <br />
            <asp:Label ID="LBTestoDomanda" runat="server" Text=""></asp:Label>
            <br />
               <CTRL:CTRLeditor id="CTRLeditorTestoDomanda" runat="server" ContainerCssClass="containerclass" AllAvailableFontnames="True"
                    LoaderCssClass="loadercssclass" EditorHeight="230px" EditorWidth="100%" ModuleCode="SRVQUST" AutoInitialize="True">
                    </CTRL:CTRLeditor>
            <div style="display: none;  ">
                <br />
                <asp:Label ID="LBDifficolta" runat="server" Text=""></asp:Label>
                <asp:DropDownList Visible="false" ID="DDLDifficolta" runat="server">
                    <asp:ListItem Value="0"></asp:ListItem>
                    <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                    <asp:ListItem Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:CheckBox runat="server" ID="CHKisObbligatoria" Visible="false" />
            <asp:Label ID="LBTitoloOpzioni" runat="server" Font-Bold="true"></asp:Label>
            <br />
            <radCln:RadCalendar runat="server" ID="RDCLCalendar" EnableMultiSelect="true" OnSelectionChanged="RDCLCalendar_SelectionChanged" Enabled='<%#Not isDomandaReadOnly%>'>
            </radCln:RadCalendar>
            <br />
            <asp:Label ID="LBNumeroOpzioni" runat="server" Text=""></asp:Label><br />
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
            <asp:Label runat="server" ID="LBZone"></asp:Label>
            <br />
            <asp:Repeater ID="RPTZone" runat="server">
                <ItemTemplate>
                    <asp:TextBox runat="server" margin-right="5px" ID="TXBZone" Text='<%#Eval("Testo")%>'></asp:TextBox>
                </ItemTemplate>
            </asp:Repeater>
            <asp:PlaceHolder runat="server" ID="PHZone"></asp:PlaceHolder>
            <br />
            <asp:Label ID="LBTestoDopoDomanda" runat="server" Text=""></asp:Label>
            <asp:TextBox ID="TXBTestoDopoDomanda" runat="server" Width="100%" TextMode="MultiLine"
                MaxLength="250"></asp:TextBox>
        </ItemTemplate>
    </asp:FormView>
</asp:Panel>
