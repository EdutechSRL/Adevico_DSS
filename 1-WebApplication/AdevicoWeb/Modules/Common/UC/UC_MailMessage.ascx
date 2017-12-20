<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailMessage.ascx.vb" Inherits="Comunita_OnLine.UC_MailMessage" %>
<asp:MultiView ID="MLVmailMessage" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">

    </asp:View>
    <asp:View ID="VIWmail" runat="server">
        <div class="fieldobject clearfix" >
            <div class="fieldrow mailtitle left">
                <asp:Label ID="LBtitle" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="fieldrow mailtoolbar send right">
                <asp:Button id="BTNsendPreviewMail" runat="server" Text="*Send" CssClass="Link_Menu" Visible="false" />
                <asp:Button id="BTNclosePreviewMessageWindow" runat="server" Text="*Close" CssClass="Link_Menu" Visible="false" />
            </div>            
            <div class="fieldrow mailtoolbar right" id="DVswitchGroup" runat="server" visible="false">
                <span class="btnswitchgroup js showaddresses"><!--
                    --><asp:linkbutton ID="LNBaddressesOn" runat="server" CssClass="btnswitch first active" Text="*Addresses"></asp:linkbutton><!--
                    --><asp:linkbutton ID="LNBaddressesOff" runat="server" CssClass="btnswitch last" Text="*hide"></asp:linkbutton>
                </span>
                <span class="btnswitchgroup js showattachments"><!--
                    --><asp:linkbutton ID="LNBattachmentsOn" runat="server" CssClass="btnswitch first active" Text="*Attachments"></asp:linkbutton><!--
                    --><asp:linkbutton ID="LNBattachmentsOff" runat="server" CssClass="btnswitch last" Text="*hide"></asp:linkbutton>
                </span>
                <span class="btnswitchgroup js showoptions"><!--
                    --><asp:linkbutton ID="LNBoptionsOn" runat="server" CssClass="btnswitch first active" Text="*Options"></asp:linkbutton><!--
                    --><asp:linkbutton ID="LNBoptionsOff" runat="server" CssClass="btnswitch last" Text="*hide"></asp:linkbutton>
                </span>
            </div>
        </div>
        <div class="fieldobject" id="DVsentBy" runat="server" visible="false">
            <div class="fieldrow mailaddress">
                <asp:Label ID="LBsentBy_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsentBy">*Address:</asp:Label>
                <asp:Label ID="LBsentBy" runat="server" CssClass="fieldinput"></asp:Label>
            </div>
        </div>
        <div class="fieldobject" id="DVsendTo" runat="server" visible="false">
            <div class="fieldrow mailaddress">
                <asp:Label ID="LBaddressTo_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBaddressTo">*Address:</asp:Label>               
                    <asp:TextBox ID="TXBaddressTo" runat="server" CssClass="fieldinput tokeninputmail" ReadOnly="true"></asp:TextBox>
                    <span class="fieldinfo showiferror">
                        <span class="message">
                            <asp:Literal id="LTmailAddressInvalidErrorsInfo" runat="server"></asp:Literal>                            
                            <asp:HyperLink id="HYPhideErrors" CssClass="hideerrors" runat="server">*clicca qui per nascondere</asp:HyperLink>
                        </span>
                        <span class="details"></span>
                    </span>                
            </div>
        </div>
        <div class="fieldobject" id="DVrecipients" runat="server" visible="false">
            <div class="fieldrow mailaddress">
                <asp:Label ID="LBrecipients_t" runat="server" Text="*Address:" CssClass="fieldlabel"></asp:Label>
                <div>
                     <ul class="addresses expandablelist clearfix compressed" id="ULrecipients" runat="server">
                        <li class="expandableitem first"><asp:Label ID="LBshowAllrecipients" runat="server">...</asp:Label></li>
                    <asp:Repeater ID="RPTselectedItems" runat="server" Visible="false">
                        <ItemTemplate>
                                <li class="address expandableitem"><%#Container.DataItem.DisplayName %></li>
                        </ItemTemplate>
                    </asp:Repeater>
                        <li class="expandableitem last"><asp:Label ID="LBhideAllrecipients" runat="server">*Hide</asp:Label></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="fieldobject" id="DVoptions" runat="server" visible="false">
            <div class="fieldrow mailoptions">
                <label class="fieldlabel" for="">Options:</label>
                <table class="table minimal fullwidth options">
                    <thead>
                    <tr class="option">
                        <th class="key">Option Name</th>
                        <th class="value">Value</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr class="option">
                        <td class="key">Html Body</td>
                        <td class="value">Yes</td>
                    </tr>
                    <tr class="option">
                        <td class="key">Read Receipt</td>
                        <td class="value">Yes</td>
                    </tr>
                    <tr class="option">
                        <td class="key">Send Copy to the Sender</td>
                        <td class="value">Yes</td>
                    </tr>
                    <tr class="option">
                        <td class="key">Sent on</td>
                        <td class="value">01/01/2013</td>
                    </tr>
                    <tr class="option">
                        <td class="key">Mail Template</td>
                        <td class="value">xYz</td>
                    </tr>
                    <tr class="option">
                        <td class="key">Message Name</td>
                        <td class="value">xYz</td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="fieldobject" id="DVAttachments" runat="server" visible="false">
            <asp:Label ID="LBattachments_t" runat="server" Text="*Attachments:" CssClass="fieldlabel"></asp:Label>
            <div>
                 <ul class="attachments expandablelist clearfix compressed" id="ULattachments" runat="server">
                    <li class="expandableitem first"><asp:Label ID="LBshowAllAttachments" runat="server">...</asp:Label></li>
                    <asp:Repeater ID="Repeater1" runat="server" Visible="false">
                        <ItemTemplate>
                                <li class="address expandableitem">
                                    <span style="" class="iteminfo">
                                        <span style="" class="name">
                                            <span style="" class="actionbuttons">
                                                <span style="" class="fileIco extdoc" title="">&nbsp;</span>
                                            </span>
                                                <%#Container.DataItem.Fullname%>
                                                01 - Abstract bando.doc
                                        </span>
                                        <span style="" class="itemdetail">(1.17 mb)</span>
                                    </span>
                                </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li class="expandableitem last"><asp:Label ID="LBhideAllAttachments" runat="server">*Hide</asp:Label></li>
                </ul>
            </div>
        </div>
        <div class="fieldobject">
            <div class="fieldrow mailsubject">
                <asp:Label ID="LBmessageSubject_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBmessageSubject">*Subject:</asp:Label>
                <asp:Label ID="LBmessageSubject" runat="server" CssClass="subjectlabel" ></asp:Label>
            </div>
        </div>
        <div class="fieldobject">
            <div class="fieldrow mailbody">
                <div class="mailbodycontent">
                    <asp:Literal ID="LTmessageContent" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>