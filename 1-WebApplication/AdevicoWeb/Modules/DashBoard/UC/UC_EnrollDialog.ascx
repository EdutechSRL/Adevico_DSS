<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EnrollDialog.ascx.vb" Inherits="Comunita_OnLine.UC_EnrollDialog" %>
<%@ Register TagPrefix="CTRL" TagName="EnrollMessage" Src="~/Modules/DashBoard/UC/UC_EnrollDialogMessage.ascx" %>
 <div class="dlgconfirmsubscription <%=CssClassDialog%> hiddendialog" title="<%=GetDialogTitle() %>">
    <div class="fieldrow message" id="DVdescription" runat="server" visible="false">
        <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
    </div>
    <div class="fieldobject subscription nowarning clearfix" id="DVnoWarning" runat="server" visible="false">
        <CTRL:EnrollMessage id="CTRLnoWarningMessage" CssClass="ok" runat="server" AllowSelection="false"   />
    </div>
    <div class="fieldobject subscription error clearfix" id="DVerror" runat="server" visible="false">
       <CTRL:EnrollMessage id="CTRLerrorMessage" CssClass="error" runat="server" AllowSelection="false"  />
    </div>
    <div class="fieldobject subscription warning clearfix" id="DVunableToUnsubscribe" runat="server" visible="false">
        <CTRL:EnrollMessage id="CTRLunableToUnsubscribeMessage" CssClass="alert" AllowSelection="true"  runat="server" />
    </div>
    <div class="fieldobject subscription conflict clearfix" id="DVconflicts" runat="server" visible="false">
       <CTRL:EnrollMessage id="CTRLconflicts" CssClass="alert" runat="server" AllowSelection="false"  />
    </div>
    <div class="fieldobject clearfix">
        <div class="fieldrow buttons right">
            <asp:Button ID="BTNapplyEnrollTo" runat="server" CssClass="linkMenu" />
            <asp:HyperLink ID="HYPcloseEnrollToConfirmDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
        </div>
    </div>
</div>
<asp:Literal ID="LTcssClassDialog" runat="server" Visible="false">.dlgconfirmsubscription</asp:Literal>
<asp:Literal ID="LTcssClassMulti" runat="server" Visible="false">multiple</asp:Literal>
<asp:Literal ID="LTcssClassSingle" runat="server" Visible="false">singular</asp:Literal>
<asp:Literal ID="LTtemplateMessageDetails" runat="server" Visible="false"><ul class="messagedetails">{0}</ul></asp:Literal>
<asp:Literal ID="LTtemplateMessageDetail" runat="server" Visible="false"><li class="messagedetail">{0}</li></asp:Literal>