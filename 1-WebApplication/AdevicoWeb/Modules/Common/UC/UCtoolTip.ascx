<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UCtoolTip.ascx.vb"
    Inherits="Comunita_OnLine.UCtoolTip" %>
<span class="itemdetails tooltipHook">
    <asp:Literal ID="LTdisplayAction" runat="server" />
</span>
<div class="tooltip tt-outerwrapper">
    <div class="tt-innerwrapper">
        <div class="tt-content">
            <ul class="itemlist">
                <asp:Literal ID="LTitem" runat="server"></asp:Literal>
            </ul>
        </div>
    </div>
</div>