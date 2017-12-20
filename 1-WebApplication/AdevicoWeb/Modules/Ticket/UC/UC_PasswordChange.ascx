<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PasswordChange.ascx.vb" Inherits="Comunita_OnLine.UC_PasswordChange" %>
<%--Nomi Standard: OK--%>
<%@ Register TagPrefix="CTRL" TagName="CTRLactionMsg" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<div class="fieldobject clearfix">
    <div class="fieldrow password">
        <span class="leftwrapper">
            <asp:Label ID="LBoldPwd_t" runat="server" AssociatedControlID="TXBoldPwd" CssClass="fieldlabel">*Old password:</asp:Label>
            <asp:TextBox ID="TXBoldPwd" runat="server" cssClass="inputtext" TextMode="Password"></asp:TextBox>
        </span>
        <%--<span class="rightwrapper">
            <span class="hint"></span>
            <span class="errormessage">Invalid format or not recognized</span>
        </span>--%>
    </div>

    <div class="fieldrow password">
        <span class="leftwrapper">
            <asp:Label ID="LBnewPwd_t" runat="server" AssociatedControlID="TXBnewPwd" CssClass="fieldlabel">*New password:</asp:Label>
            <asp:TextBox ID="TXBnewPwd" runat="server" cssClass="inputtext" TextMode="Password"></asp:TextBox>
        </span>
    </div>

    <div class="fieldrow password">
        <span class="leftwrapper">
            <asp:Label ID="LBrenewPwd_t" runat="server" AssociatedControlID="TXBrenewPwd" CssClass="fieldlabel">*Retype password:</asp:Label>
            <asp:TextBox ID="TXBrenewPwd" runat="server" cssClass="inputtext" TextMode="Password"></asp:TextBox>
        </span>
    </div>

    <div class="fieldrow messages">
        <span>
            <CTRL:CTRLactionMsg ID="CTRLactionMsg" runat="server" />    
        </span>
    </div>

    <div class="fieldrow right submit">
        <asp:LinkButton ID="LNBchangePwd" runat="server" CssClass="Link_Menu">*Modifica</asp:LinkButton>
    </div>
                    	
</div>