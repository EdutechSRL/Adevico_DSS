<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GlossaryShare.ascx.vb" Inherits="Comunita_OnLine.UC_GlossaryShare" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>


<div class="content">

    <div class="messages">
        <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>
        <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
            <div class="fieldrow">
                <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
            </div>
        </div>
    </div>

    <div class="content">

        <div class="fieldobject box sharing first">
            <div class="fieldrow objectheader">
                <h4 class="title">
                    <asp:Literal ID="LTsystemWideSharing_t" runat="server">* System wide sharing 11</asp:Literal>
                </h4>
            </div>
            <div class="fieldrow description">
                <asp:Literal ID="LTpublicDescription" runat="server">* Lorem usum dolor sit amet</asp:Literal>
            </div>
            <div class="fieldrow option enable">
                <CTRL:Switch ID="SWHpublic" runat="server" Status="true"/>
            </div>
        </div>

        <div class="fieldobject box">
            <div class="fieldrow objectheader">
                <h4 class="title">
                    <asp:Literal ID="LTcommunitySharing_t" runat="server">* Community sharing 22</asp:Literal>
                </h4>

                <!-- valutare se inserire descrizione -->

            </div>
            <div class="fieldrow description">
                <asp:Literal ID="LTcommunityDescription" runat="server">* Lorem usum dolor sit amet</asp:Literal>
            </div>
            <div class="fieldrow option enable">
                <CTRL:Switch ID="SWshared" runat="server" Status="true"/>
            </div>
            <div class="fieldrow communityselection clearfix">
                <span class="left description">
                    <asp:Literal ID="LTselectedCommunity" runat="server">* No community selected# / Sharing is active on communities#</asp:Literal>
                </span>
                <span class="item right">
                    <asp:Button ID="BTNaddCommunity" runat="server" Text="*Add community" />
                </span>
            </div>

            <asp:Repeater runat="server" ID="RPTshare" OnItemCommand="RPTshare_ItemCommand">
                <HeaderTemplate>
                    <ul class="communities defaultservicecontainers clearfix">
                </HeaderTemplate>
                <ItemTemplate>

                    <asp:HiddenField runat="server" ID="HDFid"/>

                    <li class="community defaultservicecontainer default toolbar collapsable">
                        <div class="innerwrapper">
                            <div class="itemheader clearfix">
                                <div class="left">
                                    <h4 class="title">
                                        <span class="handle expander"></span>
                                        <span class="text">
                                            <asp:Literal ID="LTcommunityName" runat="server">* CommunityName</asp:Literal>
                                        </span>
                                    </h4>
                                    <span class="status">
                                        <span class="<%# GetStateClass(Container.DataItem.Status)%>">
                                            <span class="statusitem">
                                                <asp:Literal ID="LTactive" runat="server">* active 1</asp:Literal>
                                            </span>
                                        </span>
                                    </span>
                                    </span>
                                </div>
                                <div class="right">
                                    <span class="icons">
                                        <asp:LinkButton runat="server" ID="LNBvirtualDeleteShare" CssClass="icon virtualdelete"></asp:LinkButton>
                                    </span>
                                </div>
                                <div class="clearer"></div>
                            </div>
                            <div class="itemcontent">
                                <div class="fieldobject">
                                    <div class="optiongroup status">
                                        <label class="fieldlabel title" for="">
                                            <asp:Literal ID="LTstatus_t" runat="server">* STATUS</asp:Literal>
                                        </label>
                                        <div class="inlinewrapper">
                                            <div class="fieldrow status">
                                                <span class="<%# GetStateClass(Container.DataItem.Status)%>">
                                                    <span class="statusitem">
                                                        <asp:Literal ID="LTactive2" runat="server">* active 2</asp:Literal>
                                                    </span>
                                                </span>
                                            </div>
                                            <div class="fieldrow forceactive">
                                                <span class="option">
                                                    <asp:CheckBox ID="CBXforceActive" runat="server" CssClass="fieldinput"></asp:CheckBox>
                                                     <asp:Label ID="LBforceGlossary_t" AssociatedControlID="CBXforceActive" runat="server">* Force glossary to be always visible on this community</asp:Label>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="optiongroup permissions">
                                        <label class="fieldlabel title">
                                            <asp:Literal ID="LT_permissions_t" runat="server">* PERMISSIONS</asp:Literal>
                                        </label>
                                        <div class="inlinewrapper">
                                            <div class="fieldrow">
                                                <span class="description">
                                                    <asp:Literal ID="LTgrantPermissions_t" runat="server">* Grant permissions to allowed user on recipient community</asp:Literal>
                                                </span>
                                            </div>
                                            <div class="options">
                                                <span class="option">
                                                    <asp:CheckBox ID="CBXaddTerm" runat="server" CssClass="fieldinput"></asp:CheckBox>
                                                    <asp:Label ID="LBaddTerm_t" AssociatedControlID="CBXaddTerm" runat="server">* Add term</asp:Label>
                                                </span><span class="option">
                                                    <asp:CheckBox ID="CBXdeleteTerm" runat="server" CssClass="fieldinput"></asp:CheckBox>
                                                    <asp:Label ID="LBdeleteTerm_t" AssociatedControlID="CBXdeleteTerm" runat="server">* Delete term</asp:Label>
                                                </span>
                                                <span class="option">
                                                    <asp:CheckBox ID="CBXeditTerm" runat="server" CssClass="fieldinput"></asp:CheckBox>
                                                    <asp:Label ID="LBeditTerm_t" AssociatedControlID="CBXeditTerm" runat="server">* Edit term</asp:Label>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false"/>
</div>

<div class="footer">
    <div class="DivEpButton">
        <asp:LinkButton ID="LNBback" runat="server" CssClass="linkMenu">*Torna</asp:LinkButton>
        <asp:LinkButton ID="LNBsaveGlossaryShare" runat="server" CssClass="linkMenu">*Salva Condivisioni</asp:LinkButton>
    </div>
</div>