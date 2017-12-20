<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileTypeAssignment.ascx.vb" Inherits="Comunita_OnLine.UC_ItemProfileTypeAssignment" %>
<div>
    <div style="clear:both">
        <div style="float: left;" class="Menu_FieldTitle">
            <asp:Label ID="LBprofileTypes_t" runat="server" CssClass="Titolo_campo">Tipi profilo:</asp:Label>
        </div>
        <div style="float: left;">
            <asp:Repeater ID="RPTprofileTypes" runat="server">
                <HeaderTemplate>
                    <ul class="Menu_ProfileType">
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <input type="checkbox" class="Testo_campo" value='<%#Container.DataItem.Id %>' id="HTCprofileType" runat="server" /> <%#Container.DataItem.Descrizione %>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater> 
        </div>
    </div>
</div>