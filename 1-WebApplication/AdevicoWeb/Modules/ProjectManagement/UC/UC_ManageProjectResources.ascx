<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ManageProjectResources.ascx.vb"
    Inherits="Comunita_OnLine.UC_ManageProjectResources" %>

<asp:Literal id="LTdataMax" runat="server" Visible="false">5</asp:Literal>
<div class="fieldobject">
    <div class="fieldrow" id="DVdescription" runat="server" visible="false">
        <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
    </div>
    <div class="tablewrapper">
        <table class="table light users fullwidth expandable compressed" data-max='<%=MaxDisplayUsers %>'>
            <thead>
                <tr>
                    <th class="username">
                        <asp:Literal ID="LTlongName_t" runat="server">*Long name</asp:Literal>
                    </th>
                    <th class="shortname">
                        <asp:Literal ID="LTshortName_t" runat="server">*Short name</asp:Literal>
                    </th>
                    <th class="manager">
                        <asp:Literal ID="LTprojectRole_t" runat="server">*Ruolo di Progetto</asp:Literal>
                    </th>
                    <th class="visibility">
                        <asp:Literal ID="LTprojectVisibility" runat="server">*Task map</asp:Literal>
                    </th>
                    <th class="actions">
                        <asp:Literal ID="LTaction_t" runat="server">*Actions</asp:Literal>
                    </th>
                </tr>
            </thead>
        <asp:Repeater ID="RPTusers" runat="server" >
            <HeaderTemplate>
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="user <%#ContainerCssClass(Container.DataItem.ResourceType) %>">
                    <td class="username">
                    
                        <asp:Literal ID="LTidResource" runat="server" Visible="false" text='<%#Container.DataItem.IdResource %>'></asp:Literal>
                        <asp:Literal ID="LTidPerson" runat="server" Visible="false" text='<%#Container.DataItem.IdPerson %>'></asp:Literal>
                        <asp:Literal ID="LTidRole" runat="server" Visible="false" text='<%#cInt(Container.DataItem.ProjectRole) %>'></asp:Literal>
                        <a name="resource<%#Container.DataItem.IdResource %>"></a>
                        <asp:Literal ID="LTlongName" runat="server" text='<%#Container.DataItem.LongName %>'></asp:Literal>
                        <asp:TextBox ID="TXBlongName" runat="server" Visible="false" text='<%#Container.DataItem.LongName %>'></asp:TextBox>
                    </td>
                    <td class="shortname">
                        <asp:Literal ID="LTshortName" runat="server" text='<%#Container.DataItem.ShortName %>'></asp:Literal>
                        <asp:TextBox ID="TXBshortName" runat="server" Visible="false" text='<%#Container.DataItem.ShortName %>' MaxLength="5"></asp:TextBox>
                    </td>
                    <td class="manager">
                        <input type="checkbox" id="CBmanager" runat="server" /> 
                        <asp:Label ID="LBprojectRole" runat="server" AssociatedControlID="CBmanager" CssClass="alignr"></asp:Label>
                    </td>
                    <td class="visibility">
                        <input type="checkbox" id="CBvisibility" runat="server" /> 
                        <asp:Label ID="LBprojectVisibility" runat="server" AssociatedControlID="CBvisibility" CssClass="alignr">*Full</asp:Label>
                    </td>
                    <td class="actions">
                        <span class="icons">
                            <asp:Button ID="BTNdelete" CssClass="icon delete needconfirm" runat="server" Text="*Delete" CommandName="virtualdelete" CommandArgument='<%#Container.DataItem.IdResource  %>' />
                        </span>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            <tfoot>
                 <tr id="TRfooter" runat="server">
                    <td colspan="5">
                        <asp:Label ID="LBshowextraUserItems" CssClass="showextra" runat="server"></asp:Label>
                        <asp:Label ID="LBhideextraUserItems" CssClass="hideextra" runat="server"></asp:Label>
                    </td>
                </tr>
            </tfoot>
            </FooterTemplate>
        </asp:Repeater>
        </table>
    </div>
</div>
<div class="fieldobject clearfix" id="DVcommands" runat="server" >
    <div class="fieldrow buttons right">
        <asp:Button ID="BTNsaveProjectResources" runat="server" CssClass="linkMenu" />
        <asp:Button ID="BTNcancelSaveProjectResources" runat="server" CssClass="linkMenu" CausesValidation="false" />
    </div>
</div>