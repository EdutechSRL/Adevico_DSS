<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TicketUserSettings.ascx.vb" Inherits="Comunita_OnLine.UC_TicketUserSettings" %>

<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>


<%@ Register TagPrefix="CTRL" TagName="MailSettings" Src="~/Modules/Ticket/UC/UC_MailSettingsNew.ascx" %>

    <div class="fieldobject notifications">
        <div class="fieldrow objectheader">
            <h4 class="title">
                <asp:literal ID="LTmyNotification_t" runat="server">*My notifications settings</asp:literal>
            </h4>
            <div class="fieldrow description">
                <asp:literal runat="server" ID="LTmyNotificationDesc_t">*Impostazioni di notifica</asp:literal>
            </div>
            
           <%-- <CTRL:Switch ID="UCmainSwitch" runat="server" />--%>
        </div>
        
        <div class="contentwrapper">
        
            <div class="fieldrow optiongroup user notifications clearfix">
                
                <div class="fieldlabel title">
                    <asp:literal ID="LTtitleUser_t" runat="server">*My notification settings</asp:literal>
                    <CTRL:Switch ID="UCswitchUser" runat="server" />
                </div>

                <div class="description">
                    <asp:literal ID="LTusrDeacription_t" runat="server">*Receive notifications for:</asp:literal>
                </div>
                <div class="options">
                
                    <asp:Label runat="server" ID="LBusrMailSet_t" AssociatedControlID="UCmailUsrSett" CssClass="optionlabel">*label?</asp:Label>
                    <span class="inlinewrapper">
                        <CTRL:MailSettings ID="UCmailUsrSett" runat="server" />
                    </span>
                </div>
                <div class="commands">
                    <asp:linkbutton ID="LKBgetUsrGlobalSettings" runat="server" CssClass="linkMenu ovverride" Visible="False"></asp:linkbutton>
                    <%--<a class="linkMenu ovverride" href="#">Reset all notifications to system default</a>--%>
                    <asp:linkbutton ID="LKBsaveSettings" runat="server" CssClass="linkMenu ovverride" Visible="false">*Save Notification</asp:linkbutton>
                </div>
                <div class="extrainfo" runat="server" ID="DVextraInfoUsr">
                    <asp:Label runat="server" ID="LBextraInfoUsr"></asp:Label>

                </div>
            </div>
        
            <asp:panel ID="PNLmanager" runat="server" cssclass="fieldrow optiongroup user notifications clearfix">
                <div class="fieldlabel title">
                    <asp:literal ID="LTtitleManager_t" runat="server">*Manager notification settings</asp:literal>
                    <CTRL:Switch ID="UCswitchMan" runat="server" />
                </div>
            
                <div class="description">
                    <asp:literal ID="LTmanDescription_t" runat="server">*Receive notifications for:</asp:literal>
                </div>
                <div class="options">
                    <asp:Label runat="server" ID="LBmanMailSet_t" AssociatedControlID="UCmailUsrSett" CssClass="optionlabel">*label?</asp:Label>
                    <span class="inlinewrapper">
                        <CTRL:MailSettings ID="UCmailManSett" runat="server" />
                    </span>
                </div>
                <div class="commands">
                    <asp:linkbutton ID="LKBgetManGlobalSettings" runat="server" CssClass="linkMenu ovverride" Visible="false"></asp:linkbutton>
                    <%--<a class="linkMenu ovverride" href="#">Reset all notifications to system default</a>--%>
                </div>
                <div class="extrainfo" runat="server" ID="DVextraInfoMan">
                    <asp:Label runat="server" ID="LBextraInfoMan"></asp:Label>
                </div>
            </asp:panel>
        </div>
    
    
    </div>

<%--<asp:LinkButton runat="server" ID="LKBtest">-test-</asp:LinkButton>--%>