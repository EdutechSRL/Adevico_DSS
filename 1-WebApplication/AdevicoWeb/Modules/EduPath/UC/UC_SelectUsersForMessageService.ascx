<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectUsersForMessageService.ascx.vb" Inherits="Comunita_OnLine.UC_SelectPathUsersForMessageService" %>
<%@ Register TagPrefix="CTRL" TagName="SelectMessage" Src="~/Modules/Mail/UC/UC_SelectMessage.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<div class="tableview">
    <div class="fieldobject filters container_12 clearfix">
        <div class="fieldrow grid_6 alpha fromUsersfilter">
            <asp:Label ID="LBselectUsersFrom_t" runat="server" AssociatedControlID="DDLselectFrom">*Select users from:</asp:Label>&nbsp;
            <asp:DropDownList ID="DDLselectFrom" runat="server" CssClass="fieldinput autoblock" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="fieldrow grid_6 omega filtertemplates">
            <asp:Label ID="LBselectMessages_t" runat="server" AssociatedControlID="LNBselectMessages">* and from those messages:</asp:Label>
            <asp:LinkButton ID="LNBselectMessages" runat="server" CssClass="linkMenu">* select messages</asp:LinkButton>
            <asp:Label ID="LBselectMessages" runat="server"></asp:Label>
        </div>
        <div class="clear"></div>
        <div class="fieldrow grid_6 alpha namefilter">   
            <asp:Label ID="LBsearchUserByFilter_t" runat="server" AssociatedControlID="DDLsearchUserBy">Find by:</asp:Label>&nbsp;
            <span class="inputgroup">
                <asp:DropDownList ID="DDLsearchUserBy" runat="server" >
                    <asp:ListItem Value="-1">tutti</asp:ListItem>
                    <asp:ListItem Value="1">Nome</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">Cognome</asp:ListItem>
                    <asp:ListItem Value="5">Mail</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="TXBsearchBy" runat="server" CssClass="fieldinput"></asp:TextBox>
            </span>
        </div>
        <div class="fieldrow grid_6 omega agencyfilter">
            <asp:Label ID="LBcommunityAgencyFilter_t" runat="server" AssociatedControlID="DDLcommunityAgencies" Visible="false">Agency:</asp:Label>&nbsp;
            <asp:DropDownList ID="DDLcommunityAgencies" runat="server" CssClass="fieldinput" Visible="false">
            </asp:DropDownList>
        </div>
        <div class="clear"></div>
        <asp:MultiView ID="MLVfilters" runat="server">
            <asp:View ID="VIWempty" runat="server"></asp:View>
            <asp:View ID="VIWparticipantFilters" runat="server">
                <div class="fieldrow grid_6 alpha submissionfilter">
                    <asp:Label ID="LBsubmissionStatusFilter_t" runat="server" AssociatedControlID="DDLsubmissionStatus">* with submission:</asp:Label>&nbsp;
                    <asp:DropDownList ID="DDLsubmissionStatus" runat="server" CssClass="fieldinput"></asp:DropDownList>
                </div>
                <div class="fieldrow grid_6 omega submissiontypefilter">
                    <asp:Label ID="LBsubmissionTypeFilter_t" runat="server" AssociatedControlID="DDLsubmitterTypes" Visible="false">of Type:</asp:Label>&nbsp;
                    <asp:DropDownList ID="DDLsubmitterTypes" runat="server" CssClass="fieldinput" Visible="false"></asp:DropDownList>
                </div>
                <div class="clear"></div>
            </asp:View>
        </asp:MultiView>
        <div class="abcupdate clearfix">
            <div class="fieldrow left abcfilters" runat="server" id="DVletters" visible="false">
                <CTRL:AlphabetSelector ID="CTRLalphabetSelector" runat="server" RaiseSelectionEvent="true">
                </CTRL:AlphabetSelector>
            </div>
            <div class="fieldrow right updatefilter clearfix">
                <asp:Button ID="BTNapplyFilters" runat="server" CssClass="linkMenu" Text="*Update" />
            </div>
        </div>
    </div>
    <div class="tablewrapper">
         <table class="table light msgeditusers fullwidth">
        <asp:Repeater ID="RPTitems" runat="server">
            <HeaderTemplate>
              <thead>
                <tr id="sub-hdr">
                    <th class="check">
                        <span class="headercheckbox leftside">
                            <input type="checkbox" class="checkall" />
                            <div class="selectorpopup checkall" id="DVselectorAll" runat="server">
                                <div class="inner">
                                    &nbsp;<asp:LinkButton ID="LNBselectRecipientsIntoAllPages" runat="server" Text="*Seleziona su tutte le pagine" CommandName="all"></asp:LinkButton>
                                </div>
                                <div class="bottomarrow">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="selectorpopup checknone" id="DVselectorNone" runat="server">
                                <div class="inner">
                                    &nbsp;<asp:LinkButton ID="LNBunselectRecipientsIntoAllPages" runat="server" Text="*Deseleziona su tutte le pagine" CommandName="none"></asp:LinkButton>
                                </div>
                                <div class="bottomarrow">
                                    &nbsp;
                                </div>
                            </div>
                        </span>
                    </th>
                    <th class="name">
                        <asp:Label ID="LBusernameHeader_t" runat="server">*Name</asp:Label>
                        <asp:LinkButton ID="LNBorderByUserUp" runat="server" cssclass="icon orderUp" CommandArgument="ByUser.True" CommandName="orderby"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByUserDown" runat="server" cssclass="icon orderDown" CommandArgument="ByUser.False" CommandName="orderby"></asp:LinkButton>
                    </th>
                    <th id="THagencyName" class="agencyname" runat="server" visible='<%#me.isColumnVisible(0) %>'>
                        <asp:Label ID="LBuserAgencyNameHeader_t" runat="server">*Agency</asp:Label>
                        <asp:LinkButton ID="LNBorderByAgencyUp" runat="server" cssclass="icon orderUp" CommandArgument="ByAgency.True" CommandName="orderby"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByAgencyDown" runat="server" cssclass="icon orderDown" CommandArgument="ByAgency.False" CommandName="orderby"></asp:LinkButton>
                    </th>
                    <th class="partecipanttype" id="THpartecipantType" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                        <asp:literal ID="LTsubPartecipantType_t" runat="server">Tipo</asp:literal>
                        <asp:LinkButton ID="LNBorderByTypeUp" runat="server" cssclass="icon orderUp" CommandArgument="ByType.True" CommandName="orderby"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByTypeDown" runat="server" cssclass="icon orderDown" CommandArgument="ByType.False" CommandName="orderby"></asp:LinkButton>          
                    </th>
                    <th class="status" id="THstatus" runat="server" visible='<%#me.isColumnVisible(2) %>'>
                        <asp:literal ID="LTsubStatus_t" runat="server">*Status</asp:literal>
                        <asp:LinkButton ID="LNBorderByStatusUp" runat="server" cssclass="icon orderUp" CommandArgument="ByStatus.True" CommandName="orderby"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByStatusDown" runat="server" cssclass="icon orderDown" CommandArgument="ByStatus.False" CommandName="orderby"></asp:LinkButton>
                    </th>
                    <th class="numberview">
                        <asp:Literal ID="LTmessages_t" runat="server">*Messages</asp:Literal>
                        <asp:LinkButton ID="LNBorderByMessageNumberUp" runat="server" cssclass="icon orderUp" CommandArgument="ByMessageNumber.True" CommandName="orderby"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByMessageNumberDown" runat="server" cssclass="icon orderDown" CommandArgument="ByMessageNumber.False" CommandName="orderby"></asp:LinkButton>
                    </th>
                    <th class="actions">
                        <asp:Literal ID="LTuserMessageActions_t" runat="server">*Actions</asp:Literal>
                    </th>
                </tr>
            </thead>
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="<%#me.RowItemCss(Container.DataItem)%>">
                    <td class="check">
                        <span class="submittercheckbox">
                            <input type="checkbox" class="checkcommission" id="CBXuser" runat="server"/>
                        </span>
                        <asp:Literal ID="LTidPerson" runat="server" Visible="false" Text='<%#Container.DataItem.IdPerson %>'></asp:Literal>
                        <asp:Literal ID="LTidUserModule" runat="server" Visible="false" Text='<%#Container.DataItem.IdUserModule %>'></asp:Literal>
                        <asp:Literal ID="LTidModuleObject" runat="server" Visible="false" Text='<%#Container.DataItem.IdModuleObject %>'></asp:Literal>
                    </td>
                    <td class="name">
                        <asp:Literal ID="LTname" runat="server"  Text='<%#Container.DataItem.DisplayName %>'></asp:Literal>
                        <asp:Literal ID="LTmailAddress" runat="server" Visible="false" Text='<%#Container.DataItem.MailAddress %>'></asp:Literal>
                    </td>
                    <td class="agencyname" id="TDagencyName" runat="server" visible='<%#me.isColumnVisible(0) %>'>
                        <%#Container.DataItem.AgencyName %>
                    </td>     
                    <td class="partecipanttype" id="TDpartecipantType" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                       <%#Container.DataItem.SubmitterType %>
                    </td>
                    <td class="status" id="TDstatus" runat="server" visible='<%#me.isColumnVisible(2) %>'>
                        <span class="icons">
                            <asp:Label ID="LBstatus" runat="server" CssClass="icon status" Visible="false"></asp:Label>
                        </span>
                        <asp:literal ID="LTsubStatus" runat="server" Visible="false"/>
                    </td>   
                    <td class="numberview">
                        <%#Container.DataItem.MessageNumber %>
                    </td>
                    <td class="actions">
                        <span class="icons">
                            <asp:HyperLink ID="HYPdisplayMessages" runat="server" CssClass="icon infoalt" Target="_blank"></asp:HyperLink>
                        </span>
                    </td>         
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr id="TRempty" runat="server" visible="false">
                    <td id="TDemptyItems" runat="server">
                        <asp:Label ID="LBemptyItems" runat="server">*No items to select</asp:Label>
                    </td>
                </tr>
             </tbody>
            </FooterTemplate>
        </asp:Repeater>
            <tfoot id="TBfooter" runat="server" visible="false">
                <tr>
                    <td id="TDfooter" runat="server">
                        <span class="legend">
                            <asp:Repeater ID="RPTlegend" runat="server" Visible="false">
                                <ItemTemplate>
                                    <span class="legenditem">
                                        <asp:label ID="LBlegendIcon" runat="server" CssClass="legendicon"></asp:label>
                                        <asp:label ID="LBlegendtext" runat="server" CssClass="legendtext"></asp:label>
                                    </span>
                                </ItemTemplate>
                            </asp:Repeater>
                        </span>
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    <div class="pager">
        <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false"  Visible="false"></CTRL:GridPager>
    </div>
</div>
<div class="view-modal view-dlgmessages" id="DVdialogMessages" runat="server" visible="false">
    <CTRL:SelectMessage id="CTRLselectMessages" runat="server" />
    <div class="fieldobject clearfix">
        <div class="fieldrow right">
            <asp:Button ID="BTNcloseMessageSelectorWindow" runat="server" CssClass="Link_Menu" />
        </div>
    </div>
</div>