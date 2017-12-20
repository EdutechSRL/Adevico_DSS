<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMsourceSelector.ascx.vb" Inherits="Comunita_OnLine.UC_IMsourceSelector" %>

<div class="StepData IMsourceSelector">
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
        <asp:View ID="VIWsource" runat="server">
            <div class="fieldobject">
                <div class="fieldrow">
                    <asp:Label ID="LBsourceSelector_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBLsourceType">Source:</asp:Label>
                    <asp:RadioButtonList ID="RBLsourceType" runat="server"  CssClass="Testo_campo" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</div>