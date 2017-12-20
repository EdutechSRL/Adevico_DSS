<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DialogComment.ascx.vb" Inherits="Comunita_OnLine.UC_DialogComment" %>

<div class="commentdialog" runat="server" id="DVcomment">
    <div class="commentdialogtitle clearfix">
        <asp:Label ID="LBsubmitter" runat="server" cssclass="submitter"></asp:Label>
        <asp:Label ID="LBevaluator" runat="server" cssclass="evaluator"></asp:Label>
        <asp:Label ID="LBcriterion" runat="server" cssclass="criteria" Visible="false"></asp:Label>
    </div>
    <asp:Literal ID="LTcommment" runat="server"></asp:Literal>                   
</div>