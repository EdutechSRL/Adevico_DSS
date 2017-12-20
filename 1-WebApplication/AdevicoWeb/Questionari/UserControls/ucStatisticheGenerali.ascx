<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucStatisticheGenerali.ascx.vb" Inherits="Comunita_OnLine.ucStatisticheGenerali"  %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="radC" %>
<asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
    <asp:LinkButton ID="LNBTornaLista" Visible="true" runat="server" CssClass="Link_Menu"></asp:LinkButton>
</asp:Panel>
<div style="display:none;">
<asp:LinkButton ID="LKBEsportaExcel" runat="server"></asp:LinkButton>
<asp:Image ImageUrl="..\img\excel.gif" AlternateText="Excel" ID="IMGExcel" runat="server" /><br /></div>
<br />
<asp:Label ID="LBNumeroRisposteQuest" runat="server"></asp:Label><br />
<br />
<asp:MultiView runat="server" ID="MLVDomande" ActiveViewIndex="0">
    <asp:View runat="server" ID="VIWQuestionari"  >
        <asp:DataList ID="DLDomande" runat="server" Width="100%" DataKeyField="id" OnItemDataBound="loadRisposteOpzioni"
            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
            CellPadding="3" GridLines="Horizontal" OnItemCommand="visualizzaRisposte">
            <ItemTemplate>
                <table width="100%" class="question statistics">
                    <tr id="TRlibraryName" runat="server" visible="false">
                        <td>
                            <h2><asp:Literal ID="LTlibraryName" runat="server"></asp:Literal></h2>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="question">
                            <b>
                                <span class="questionnumber"><%#DataBinder.Eval(Container, "DataItem.numero")%>.</span>
                                <span class="questiontext"><%#Me.SmartTagsAvailable.TagAll(DataBinder.Eval(Container, "DataItem.testo"))%></span>
                            </b>
                            <br />
                            <span class="questionanswers">
                                <asp:Label runat="server" ID="LBLnumeroRisposte"></asp:Label>
                                <span class="questionanswersnumber"><%#DataBinder.Eval(Container, "DataItem.numeroRisposteDomanda")%></span>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="statistics">
                            <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterStyle BackColor="White" ForeColor="#4A3C8C" />
            <SelectedItemStyle BackColor="White" Font-Bold="True" ForeColor="#F7F7F7" />
            <AlternatingItemStyle BackColor="White" />
            <ItemStyle BackColor="White" ForeColor="#4A3C8C" />
            <HeaderStyle BackColor="White" Font-Bold="True" ForeColor="#F7F7F7" />
        </asp:DataList>
    </asp:View>
    <asp:View runat="server" ID="VIWMeeting">
        <asp:Repeater ID="RPTRisposte" runat="server">
            <HeaderTemplate>
                <table style="border-color: black; border-style: solid; border-collapse: collapse;
                    border-width: 1px;">
                    <tr class="ROW_header_Small_Center">
                        <td>
                            <asp:Label runat="server" ID="LBHeadDate" Text="Day"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LBHeadZone" Text="Day"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LBHeadVotes" Text="Day"></asp:Label>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="ROW_Normal_Small" style="border-color: black; border-style: solid; border-collapse: collapse;
                    border-width: 1px;">
                    <td>
                        <asp:Label runat="server" margin-right="5px" ID="LBDate" Text='<%#Container.Dataitem.meetingDateStr%>'></asp:Label>
                    </td>
                    <td style="Border-Color:black; Border-Style:solid; border-collapse:collapse; Border-Width:1px;">
                        <asp:Label runat="server" margin-right="5px" ID="LBZone" Text='<%#Container.Dataitem.zone%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" margin-right="5px" ID="LBvotes" Text='<%#Container.Dataitem.votes%>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="ROW_Alternate_Small">
                    <td >
                        <asp:Label runat="server" margin-right="5px" ID="LBDate" Text='<%#Container.Dataitem.meetingDateStr%>'></asp:Label>
                    </td>
                    <td style="Border-Color:black; Border-Style:solid; border-collapse:collapse; Border-Width:1px; ">
                        <asp:Label runat="server" margin-right="5px" ID="LBZone" Text='<%#Container.Dataitem.zone%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" margin-right="5px" ID="LBvotes" Text='<%#Container.Dataitem.votes%>'></asp:Label>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:View>
</asp:MultiView>
