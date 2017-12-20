<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EnrollDialogMessage.ascx.vb" Inherits="Comunita_OnLine.UC_EnrollDialogMessage" %>
<div class="messages">
    <div class="message <%=CssClass%>">
        <span class="icons"><span class="icon">&nbsp;</span></span>
        <span class="info">
            <asp:Label ID="LBnumber" runat="server" CssClass="number"></asp:Label>
            <asp:Label ID="LBmessage" runat="server" CssClass="number"></asp:Label>
            <span class="right" runat="server" id="DVseeMore">
                <a class="seemore">
                    <asp:Label ID="LBshowEnrollMessageDetails" cssclass="on" runat="server">*show</asp:Label>
                    <asp:Label ID="LBhideEnrollMessageDetails" cssclass="off" runat="server">*hide</asp:Label>
                </a>
            </span>
        </span>
    </div>
</div>   
<asp:Repeater ID="RPTdetails" runat="server">
    <HeaderTemplate>
        <div class="fieldrow message">
            <ul class="communities">
    </HeaderTemplate>
    <ItemTemplate>
         <li class="community">
            <asp:Literal ID="LTidCommunity" runat="server" Visible="false"></asp:Literal>
            <span class="inputgroup" id="SPNselect" runat="server" visible="false">
                <input type="checkbox" id="CBselect" runat="server" checked>
                <asp:Label ID="LBnameForCheckbox" runat="server" CssClass="name" AssociatedControlID="CBselect"></asp:Label>
            </span>
            <asp:Label ID="LBname" runat="server" CssClass="name" ></asp:Label>
            <span class="details"></span>
            <!--  other details</span>-->
        </li>
    </ItemTemplate>
    <FooterTemplate>
            </ul>
        </div>
    </FooterTemplate>
</asp:Repeater>