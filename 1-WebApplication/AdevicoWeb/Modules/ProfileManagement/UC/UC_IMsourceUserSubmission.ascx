<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMsourceUserSubmission.ascx.vb" Inherits="Comunita_OnLine.UC_IMsourceUserSubmission" %>
<%@ Register TagPrefix="CTRL" TagName="ToolTip" Src="./../../Common/UC/UCtoolTip.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
    <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
           <span class="Fieldrow">
                <br /><br /><br /><br />
                <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                <br /><br /><br /><br />
            </span>
        </asp:View>       
        <asp:View ID="VIWlist" runat="server">            
                <asp:Repeater ID="RPTlistRFM" runat="server" OnItemDataBound="RPTlistRFM_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table light fullwidth">
                            <tr >
                                <th>
                                    <asp:Literal ID="LTheaderSelect" Text = "*S" runat="server"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="LTheaderName" Text = "*Nome" runat="server"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="LTheaderDate" Text = "*Data" runat="server"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="LTheaderSubmittedQuestions" Text ="*Nr domande sottomesse" runat="server"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="LTheaderIncompletedQuestions" Text ="*NR domande incomplete" runat="server"></asp:Literal>
                                </th>
                            </tr>
                    </HeaderTemplate>
            
                    <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LNBselectRFM" runat="server" CommandName="SelectCall" CausesValidation="false"></asp:LinkButton> 
                                </td>
                                <td>
                                     <asp:Literal ID="LTname" Text = "" runat="server"></asp:Literal>
                                </td>
                                <td>
                                     <asp:Literal ID="LTdate" Text = "" runat="server"></asp:Literal>
                                </td >
                                <td>
                                     <asp:Literal ID="LTsubmittedQuestions" Text = "" runat="server"></asp:Literal>
                                </td>
                                <td>
                                     <asp:Literal ID="LTincompletedQuestions" Text = "" runat="server"></asp:Literal>
                                </td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
        </asp:View>
        <asp:View ID="VIWselectedItem" runat ="server">
            <span style="display:inline">
                <asp:Label ID="LBnameRFM" runat="server" Text="*Name:" CssClass="Titolo_Campo"></asp:Label>
                <asp:Literal ID="LTNameRFM" runat="server" Text="*"></asp:Literal>
            </span><br />
            <span style="display:inline">
                <asp:Label ID="LBstartDate" runat="server" Text="*Created on:" CssClass="Titolo_Campo"></asp:Label>
                <asp:Literal ID="LTstartDate" runat="server" Text="*" ></asp:Literal>
            </span><br />
            <span style="display:inline">
                <asp:Label ID="LBsubmittedItems" runat="server" Text="*Submitted:" CssClass="Titolo_Campo"></asp:Label>
                <asp:Literal ID="LTsubmittedItems" runat="server" Text="*" ></asp:Literal>
            </span><br />
            <span style="display:inline">
                <asp:Label ID="LBwaitingItems" runat="server" Text="*Incompleted:" CssClass="Titolo_Campo"></asp:Label>
                <asp:Literal ID="LTwaitingItems" runat="server" Text="*" ></asp:Literal>
            </span><br />
            <asp:Button ID="BTNreturn" runat="server" Text ="*Return"></asp:Button>
            <asp:RadioButtonList ID="RBLfilterRFM" runat="server" RepeatDirection="Horizontal" CssClass="Testo_Campo" RepeatLayout="Flow" AutoPostBack="true" >
            </asp:RadioButtonList>
            <telerik:RadDateTimePicker id="RDPstartDay" runat="server" TimeView-StartTime="0" TimeView-EndTime="23:59" TimeView-Interval="00:30" TimeView-Columns="4" AutoPostBackControl="Both"  ></telerik:RadDateTimePicker>
            <asp:DropDownList ID = "DDLsubmitters" runat = "server" Visible = "false" AutoPostBack="true" CssClass="Testo_Campo"></asp:DropDownList>

             <span class="Fieldrow" id="SPNpreview" runat="server" visible="false">
                <asp:Button ID="BTNpreviewCSV" runat="server" CausesValidation="false" />
                <span class="tbl_wizdata" id="SPNpreviewTable" runat="server" visible="false">
                <asp:Repeater ID="RPTpreview" runat="server">
                      <ItemTemplate>
                         <table class="table light fullwidth">
                            <tr>
                                <asp:Repeater ID="RPTheader" runat="server" DataSource="<%#Container.DataItem.ColumHeader%>" OnItemDataBound="RPTheader_ItemDataBound">
                                    <ItemTemplate>
                                        <th><asp:literal ID="LTcolumn" runat="server"></asp:literal></th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                            <asp:Repeater ID="RPTitemRow" runat="server" DataSource="<%#Container.DataItem.Rows%>">
                                <ItemTemplate>
                                    <tr>
                                        <asp:Repeater ID="RPTitems" runat="server" DataSource="<%#Container.DataItem.Cells%>" OnItemDataBound="RPTitems_ItemDataBound">
                                            <ItemTemplate>
                                                <td>
                                                    <asp:literal ID="LTitem" runat="server"></asp:literal>
                                                    <div class="itemsummary" runat="server" id="DVmultiple" visible="false">
                                                        <span class="itemcounter"><asp:literal ID="LTdisplayName" runat="server"/></span>&nbsp;<ctrl:ToolTip id="CTRLtoolTip" runat="server" ></ctrl:ToolTip>
                                                    </div>
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                         </table>
                    </ItemTemplate>
                    <FooterTemplate>
                       
                    </FooterTemplate>
                </asp:Repeater>
                </span>
            </span>
        </asp:View>
    </asp:MultiView>