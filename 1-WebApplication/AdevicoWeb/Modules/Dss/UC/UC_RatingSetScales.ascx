<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RatingSetScales.ascx.vb" Inherits="Comunita_OnLine.UC_RatingSetScales" %>
<table class="fullwidth table minimal tablescales">
    <thead>
        <tr>
            <th class="name"><asp:Literal ID="LTratingSetHeader" runat="server">*Name</asp:Literal></th>
            <th class="values"><asp:Literal ID="LTratingSetValues" runat="server">*Values</asp:Literal></th>
            <th class="number" id="THnumber" runat="server"><asp:Literal ID="LTratingSetNumber" runat="server">*Number</asp:Literal></th>
        </tr>
    </thead>
    <tbody>
 <asp:Repeater ID="RPTratingSets" runat="server">
     <ItemTemplate>
        <tr>
            <td class="name"><%#Container.DataItem.Name %></td>
            <td class="values">
                <select id="SLratingValues" runat="server"></select>
            </td>
            <td class="number" id="TDnumber" runat="server">
                <asp:Literal ID="LTidRatingSet" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                <asp:TextBox ID="TBXratingSet" runat="server" cssclass="inputtext" MaxLength="2" ></asp:TextBox>
                <asp:RangeValidator ID="RNVratingSet" runat="server" ControlToValidate="TBXratingSet" MinimumValue="1" MaximumValue="30" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
    </tbody>
</table>