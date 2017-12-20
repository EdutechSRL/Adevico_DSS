<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Uc_advScoreItem.ascx.vb" Inherits="Comunita_OnLine.Uc_advScoreItem" %>

<span class="score container">
    <asp:PlaceHolder ID="PHscoreNumeric" runat="server">
        <span class="score numeric">
            <asp:Literal ID="LTscoreNum" runat="server">{0}</asp:Literal>
        </span>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="PHscoreBoolean" runat="server">
    <span class="score boolean">
        <asp:Literal ID="LTboolCount" runat="server">{0}{1}</asp:Literal>
        <asp:Literal ID="LTboolRating" runat="server" Visible="false"><span class="booleanRating {0}" title="{1}"></span></asp:Literal>
    </span>
        </asp:PlaceHolder>
    <span class="score success">
        <asp:Literal ID="LTscoreSuccess" runat="server">
            <span class="submission {0}" title="{1}"></span>
        </asp:Literal>
    </span>
</span>