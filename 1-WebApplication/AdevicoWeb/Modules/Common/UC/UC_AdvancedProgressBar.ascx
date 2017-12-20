<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AdvancedProgressBar.ascx.vb" Inherits="Comunita_OnLine.UC_AdvancedProgressBar" %>
<span class="completeness">                            
    <span class="progressbar"><asp:Repeater ID="RPTcompleteness" runat="server"><ItemTemplate><span class="progressdetail" runat="server" id="SPNprogressItem">&nbsp;<span class="text"><span><asp:literal ID="LTprogresItemTitleStatus" runat="server"></asp:literal><br><asp:literal ID="LTprogresItemStatusInfo" runat="server"></asp:literal></span></span><span class="arrow"></span></span></ItemTemplate></asp:Repeater></span>
</span>
<asp:Literal ID="LTspnprogressItem" runat="server" Visible="false">progressdetail</asp:Literal>