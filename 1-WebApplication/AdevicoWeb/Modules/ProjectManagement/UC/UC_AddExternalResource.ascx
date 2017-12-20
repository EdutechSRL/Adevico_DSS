<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddExternalResource.ascx.vb"
    Inherits="Comunita_OnLine.UC_AddExternalResource" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<div class="tableview" id="DVselectors" runat="server">
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <div class="fieldobject fielddescription" id="DVdescription" runat="server" visible="false">
        <div class="fieldrow">
            <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
        </div>
    </div>
    <div class="fieldobject">
        <div class="fieldrow">
            <asp:Label ID="LBexternalResourcesListTitle" runat="server">*External resources:</asp:Label>
            <table class="table minimal fullwidth">
                <thead>
                    <tr>
                        <th class="username">
                            <asp:Literal ID="LTlongName_t" runat="server">*Long name</asp:Literal>
                        </th>
                        <th class="shortname">
                            <asp:Literal ID="LTshortName_t" runat="server">*Short name</asp:Literal>
                        </th>
                        <th class="mail" id="THmail" runat="server" visible="false">
                            <asp:Literal ID="LTmail_t" runat="server">*Mail</asp:Literal>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RPTresources" runat="server">
                        <ItemTemplate>
                        <tr class="user external">
                            <td class="username">
                                <asp:TextBox ID="TXBlongName" runat="server"></asp:TextBox>
                            </td>
                            <td class="shortname">
                                <asp:TextBox ID="TXBshortName" runat="server" MaxLength="5"></asp:TextBox>
                            </td>
                            <td class="mail" id="TDmail" runat="server" visible="false">
                                <asp:TextBox ID="TXBmail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    <div class="fieldobject clearfix" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:Button ID="BTNaddExternalResources" runat="server" CssClass="linkMenu" />
            <asp:Button ID="BTNcancelExternalResources" runat="server" CssClass="linkMenu" CausesValidation="false" />
        </div>
    </div>
</div>