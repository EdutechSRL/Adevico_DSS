<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectOwnerFromResources.ascx.vb"
    Inherits="Comunita_OnLine.UC_SelectOwnerFromResources" %>
<div class="tableview" id="DVselectors" runat="server">
    <div class="fieldobject fielddescription" id="DVdescription" runat="server" visible="false">
        <div class="fieldrow">
            <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
        </div>
    </div>
    <div class="fieldobject">
        <div class="fieldrow">
             <select runat="server" id="SLBownerFromResources" class="resources chzn-select">            
             </select>
        </div>
    </div>
    <div class="fieldobject clearfix" id="DVcommands" runat="server">
        <div class="fieldrow buttons right">
            <asp:Button ID="BTNselectResourceOwner" runat="server" CssClass="linkMenu" />
            <asp:Button ID="BTNcancelSelectResourceOwner" runat="server" CssClass="linkMenu" CausesValidation="false" />
        </div>
    </div>
</div>