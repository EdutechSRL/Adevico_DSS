<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectQuiz.ascx.vb"
    Inherits="Comunita_OnLine.UC_SelectQuiz" %>
<div style="text-align:center;">
<br /><br /><br />
<asp:Literal ID="LTnoQuiz" runat="server" Visible="false"></asp:Literal>
<asp:Repeater ID="RPTquiz" runat="server">
    <HeaderTemplate>
        <table class="table" border="1" cellpadding="0" cellspacing="0" width="80%">
            <thead>
                <tr>
                <th width="60%" class="ROW_header_Small_Center"><asp:Literal ID="LTquestionnaireName_t" runat="server"></asp:Literal></th>
                <th width="25%" class="ROW_header_Small_Center"><asp:Literal ID="LTquestionnaireType_t" runat="server"></asp:Literal></th>
                <th width="15%" class="ROW_header_Small_Center"><asp:Literal ID="LTquestionnaireAction_t" runat="server"></asp:Literal></th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td width="60%">
                    <%#Container.DataItem.Nome%>
                </td>
                <td width="25%">
                    <asp:Literal ID="LTquestionnaireType" runat="server"></asp:Literal>
                </td>
                <td width="15%" style="text-align:center;">
                    <asp:LinkButton ID="LNBimportQuestionnaire" runat="server" Text="*No quiz" CommandName="select" CommandArgument="<%#Container.DataItem.Id%>"></asp:LinkButton>    
                </td>
            </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
</div>