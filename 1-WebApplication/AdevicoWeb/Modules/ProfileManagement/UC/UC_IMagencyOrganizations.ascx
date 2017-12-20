<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMagencyOrganizations.ascx.vb" Inherits="Comunita_OnLine.UC_IMagencyOrganizations" %>
<div class="StepData IMSelectOrgn">
    <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
             <div class="fieldobject">
                <div class="fieldrow">
                    <br /><br /><br /><br />
                    <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                    <br /><br /><br /><br />
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWselectOrganizations" runat="server">
            <div class="fieldobject">
                <div class="fieldrow">
                    <asp:Label ID="LBimportAlwaysAvailable_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXalwaysAvailable">Sempre disponbile:</asp:Label>
                    <asp:CheckBox ID="CBXalwaysAvailable" runat="server" CssClass="Testo_Campo" AutoPostBack="true"  />
                    <asp:Label ID="LBimportAlwaysAvailable" runat="server" CssClass="Testo_Campo" AssociatedControlID="CBXalwaysAvailable"></asp:Label>
                </div>
                <div class="fieldrow" id="DVorganizations" runat="server">
                    <asp:Label ID="LBimportAvailableForOrganizations_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBLorganizations">Disponbile per:</asp:Label>  
                    <asp:CheckBoxList ID="CBLorganizations"  runat="server" CssClass="Testo_Campo"  RepeatDirection="Horizontal" RepeatColumns="3" RepeatLayout="Flow"></asp:CheckBoxList>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <br /><br />
</div>