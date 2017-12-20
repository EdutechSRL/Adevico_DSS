<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectUsers.ascx.vb"
    Inherits="Comunita_OnLine.UC_SelectUsers" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<asp:MultiView ID="MLVusers" runat="server" ActiveViewIndex="1">
    <asp:View ID="VIWsessionTimeout" runat="server">
        <br />
        <br />
        <br />
        <asp:Label ID="LBsessionTimeout" runat="server"></asp:Label>
        <br />
        <br />
        <br />
    </asp:View>
    <asp:View ID="VIWlist" runat="server">
        <div class="userselector <%=SelectionMode.ToString().ToLower() %> big">
            <div class="tableview" id="DVselectors" runat="server">
                <div class="fieldobject fielddescription" id="DVdescription" runat="server" visible="false">
                    <div class="fieldrow">
                        <asp:Label ID="LBdescription" runat="server" CssClass="description"></asp:Label>
                    </div>
                </div>
                <div class="fieldobject filters container_12 clearfix">
                    <asp:MultiView ID="MLVfilters" runat="server">
                        <asp:View ID="VIWemptyFilters" runat="server">
                        </asp:View>
                        <asp:View ID="VIWportalFilters" runat="server">
                            <div class="fieldrow grid_8 alpha organizationfilter">
                                <asp:Label ID="LBprofileOrganizationFilter_t" runat="server" CssClass="fieldlabel"
                                    AssociatedControlID="DDLorganizations">Organization:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLorganizations" runat="server" CssClass="fieldinput autoblock"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="fieldrow grid_4 omega authenticationfilter">
                                <asp:Label ID="LBprofileAuthenticationTypeFilter_t" runat="server" CssClass="fieldlabel"
                                    AssociatedControlID="DDLauthenticationType">Auth. Type:</asp:Label>
                                <asp:DropDownList ID="DDLauthenticationType" runat="server" CssClass="fieldinput"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            
                            <div class="clear">
                            </div>
                            <div class="fieldrow grid_8 alpha namefilter">
                                
                                    <asp:Label ID="LBsearchProfileByFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLsearchProfileBy">Find by:</asp:Label>&nbsp;
                                <span class="inputgroup">
                                    <asp:DropDownList ID="DDLsearchProfileBy" runat="server" CssClass="fieldinput">
                                        <asp:ListItem Value="-1">tutti</asp:ListItem>
                                        <asp:ListItem Value="1">Nome</asp:ListItem>
                                        <asp:ListItem Value="2" Selected="True">Cognome</asp:ListItem>
                                        <asp:ListItem Value="3">Data di Nascita</asp:ListItem>
                                        <asp:ListItem Value="4">Matricola</asp:ListItem>
                                        <asp:ListItem Value="5">Mail</asp:ListItem>
                                        <asp:ListItem Value="6">Codice Fiscale</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TXBprofileValue" runat="server" CssClass="fieldinput"></asp:TextBox>
                                </span>
                            </div>
                            <div class="fieldrow grid_4 omega statusfilter">
                                <asp:Label ID="LBprofileStatusFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLprofileStatus">Status:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLprofileStatus" runat="server" CssClass="fieldinput" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="fieldrow grid_6 alpha typefilter">
                                <asp:Label ID="LBprofileTypeFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLprofileType">Profile type:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLprofileType" runat="server" CssClass="fieldinput autoblock"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="fieldrow grid_6 omega agencyfilter">
                                <asp:Label ID="LBprofileAgencyFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLprofileAgencies"
                                    Visible="false">Agency:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLprofileAgencies" runat="server" CssClass="fieldinput" Visible="false">
                                </asp:DropDownList>
                            </div>
                            <div class="clear">
                            </div>
                        </asp:View>
                        <asp:View ID="VIWcommunityFilters" runat="server">
                            <div class="fieldrow grid_8 alpha namefilter">
                                <asp:Label ID="LBsearchSubscriptionsByFilter_t" runat="server" CssClass="fieldlabel" associatedControlID="DDLsearchSubscriptionsBy">Find by:</asp:Label>&nbsp;
                                <span class="inputgroup">
                                    <asp:DropDownList ID="DDLsearchSubscriptionsBy" runat="server" CssClass="fieldinput">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="TXBsubscriptionValue" runat="server" CssClass="fieldinput"></asp:TextBox>
                                </span>
                            </div>
                            <div class="fieldrow grid_4 omega statusfilter">
                                <asp:Label ID="LBsubscriptionStatusFilter_t" runat="server" CssClass="fieldlabel"
                                    AssociatedControlID="DDLsubscriptionStatus">Status:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLsubscriptionStatus" runat="server" CssClass="fieldinput"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="fieldrow grid_4 alpha rolefilter">
                                <asp:Label ID="LBcommunityRoleFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLcommunityRole">Role:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLcommunityRole" runat="server" CssClass="fieldinput autoblock"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="fieldrow grid_4 typefilter" id="DVcommunityProfileFilter" runat="server"
                                visible="false">
                                <asp:Label ID="LBcommunityProfileTypeFilter_t" runat="server" CssClass="fieldlabel"
                                    AssociatedControlID="DDLcommunityProfileType">Profile type:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLcommunityProfileType" runat="server" CssClass="fieldinput autoblock"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="fieldrow grid_4 omega agencyfilter" id="DVagencyProfileFilter" runat="server">
                                <asp:Label ID="LBcommunityAgencyFilter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLcommunityAgencies"
                                    Visible="false">Agency:</asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLcommunityAgencies" runat="server" CssClass="fieldinput" Visible="false">
                                </asp:DropDownList>
                            </div>
                            <div class="clear">
                            </div>
                        </asp:View>
                    </asp:MultiView>
                    <div class="abcupdate clearfix">
                    <div class="fieldrow left abcfilters" runat="server" id="DVletters"
                        visible="false">
                        <CTRL:AlphabetSelector ID="CTRLalphabetSelector" runat="server" RaiseSelectionEvent="true">
                        </CTRL:AlphabetSelector>
                    </div>
                    <div class="fieldrow right updatefilter clearfix">
                        <asp:Button ID="BTNapplyFilters" runat="server" CssClass="linkMenu" Text="*Update" />
                    </div>
                    </div>
                </div>
                <div class="pager clearfix" id="DVpagerTop" runat="server" visible="false">
                    <div class="left" id="DVpageSizeTop" runat="server" visible="false">
                        <asp:Label ID="LBrecordsForPageTop_t" runat="server" AssociatedControlID="DDLpageSizeTop">*Records for page:</asp:Label>
                        <asp:DropDownList id="DDLpageSizeTop" runat="server" AutoPostBack="false" >
                        </asp:DropDownList>
                    </div>
                    <div class="right">

                    </div>
                </div>
                <div class="tablewrapper">
                    <asp:Repeater ID="RPTprofiles" runat="server" Visible="false">
                        <HeaderTemplate>
                            <table class="table light users fullwidth">
                                <thead>
                                    <tr id="sub-hdr">
                                        <th class="check" id="THmultiSelect" runat="server" >
                                            <span class="headercheckbox leftside" id="SPNheaderSelectAll" runat="server">
                                                <input type="checkbox" class="checkall" />
                                                <div class="selectorpopup checkall" id="DVselectorAll" runat="server">
                                                    <div class="inner">
                                                        &nbsp;<asp:LinkButton ID="LNBselectProfileItemsIntoAllPages" runat="server" Text="*Seleziona su tutte le pagine" CommandName="all"></asp:LinkButton>
                                                    </div>
                                                    <div class="bottomarrow">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="selectorpopup checknone" id="DVselectorNone" runat="server">
                                                    <div class="inner">
                                                        &nbsp;<asp:LinkButton ID="LNBunselectProfileItemsIntoAllPages" runat="server" Text="*Deseleziona su tutte le pagine" CommandName="none"></asp:LinkButton>
                                                    </div>
                                                    <div class="bottomarrow">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </span>
                                        </th>
                                        <th class="select" id="THsingleSelect" runat="server" >
                                            <asp:Literal ID="LTsingleSelectHeader" runat="server">&nbsp;</asp:Literal>
                                        </th>
                                        <th class="lastname">
                                            <asp:Label ID="LBuserSurnameHeader_t" runat="server">*Surname</asp:Label>
                                        </th>
                                        <th class="firstname">
                                            <asp:Label ID="LBuserNameHeader_t" runat="server">*Name</asp:Label>
                                        </th>
                                        <th class="usermail" id="THprofileMail" visible='<%#me.isColumnVisible(8) %>'>
                                            <asp:Label ID="LBuserMailHeader_t" runat="server">*Mail</asp:Label>
                                        </th>
                                        <th id="THcompanyName" class="companyname" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                                            <asp:Label ID="LBuserCompanyNameHeader_t" runat="server">Company name</asp:Label>
                                        </th>
                                        <th id="THagencyName" class="agencyname" runat="server" visible='<%#me.isColumnVisible(7) %>'>
                                            <asp:Label ID="LBuserAgencyNameHeader_t" runat="server">Agency</asp:Label>
                                        </th>
                                        <th id="THprofileType" class="roles" runat="server" visible='<%#me.isColumnVisible(3) %>'>
                                            <asp:Label ID="LBuserProfileTypeHeader_t" runat="server">Profile</asp:Label>
                                        </th>
                                        <th id="THrole" class="roles" runat="server" visible='<%#me.isColumnVisible(9) %>'>
                                            <asp:Label ID="LBuserRoleHeader_t" runat="server">Role</asp:Label>
                                        </th>
                                        <th id="THprofileStatus" class="status" runat="server" visible='<%#me.isColumnVisible(5) %>'>
                                            <asp:Label ID="LBuserProfileStatusHeader_t" runat="server">S</asp:Label>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="check" id="TDmultiSelect" runat="server">
                                    <span class="submittercheckbox">
                                        <input type="checkbox" class="checkcommission" id="CBXuser" runat="server"/>
                                    </span>
                                    <asp:Literal ID="LTidProfile" runat="server" Visible="false" Text='<%#Container.DataItem.Profile.Id %>'></asp:Literal>
                                </td>
                                <td class="select" id="TDsingleSelect" runat="server">
                                    <asp:LinkButton ID="LNBselectUser" runat="server" CommandArgument='<%#Container.DataItem.Profile.Id%>' CommandName="selectuser"></asp:LinkButton>
                                </td>
                                <td class="lastname">
                                    <%#Container.DataItem.Profile.Surname%>
                                </td>
                                <td class="firstname">
                                    <%#Container.DataItem.Profile.Name%>
                                </td>
                                <td class="usermail" runat="server" id="TDprofileMail" visible='<%#me.isColumnVisible(8) %>'>
                                    <%#Container.DataItem.Profile.Mail%>
                                </td>
                                <td class="companyname" id="TDcompanyName" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                                    <asp:Label ID="LBcompanyName" runat="server"></asp:Label>
                                </td>
                                <td class="agencyname" id="TDagencyName" runat="server" visible='<%#me.isColumnVisible(7) %>'>
                                    <asp:Label ID="LBagencyName" runat="server"></asp:Label>
                                </td>
                                <td class="roles" id="TDtype" runat="server" visible='<%#me.isColumnVisible(3) %>'>
                                    <%#Container.DataItem.TypeName%>
                                </td>
                                <td class="roles" id="TDrole" runat="server" visible='<%#me.isColumnVisible(9) %>'>
                                    <asp:Label ID="LBroleName" runat="server"></asp:Label>
                                </td>
                                <td class="status" id="TDprofileStatus" runat="server" visible='<%#me.isColumnVisible(5) %>'>
                                    <asp:Label ID="LBprofileStatus" runat="server"></asp:Label>
                                </td>
                                
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr id="TRempty" runat="server" visible="false">
                                <td id="TDemptyItems" runat="server">
                                    <asp:Label ID="LBprofileEmptyItems" runat="server">S</asp:Label>
                                </td>
                            </tr>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div class="pager clearfix">
                    <div class="left" id="DVpageSizeBottom" runat="server" visible="false">
                        <asp:Label ID="LBrecordsForPage_t" runat="server" AssociatedControlID="DDLpageSizeBottom">*Records for page:</asp:Label>
                        <asp:DropDownList id="DDLpageSizeBottom" runat="server" AutoPostBack="true" >
                        </asp:DropDownList>
                    </div>
                    <div class="right">
                        <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false"
                        Visible="false"></CTRL:GridPager>
                    </div>
                </div>
                <div class="fieldobject clearfix" id="DVcommands" runat="server">
                    <div class="fieldrow right">
                        <asp:Button ID="BTNreviewSelection" runat="server" CssClass="linkMen temp_openingu"
                            CausesValidation="false" Visible="false" />
                        <asp:Button ID="BTNcancel" runat="server" CssClass="linkMenu" CausesValidation="false" />
                        <asp:Button ID="BTNselect" runat="server" CssClass="linkMenu" />
                    </div>
                </div>
            </div>
            <div class="userselection selectionreview" runat="server" id="DVpreview">
                 <div class="fieldobject fielddescription">
                    <div class="fieldrow">
                        <asp:Label ID="LBpreviewDescription" runat="server" CssClass="description"></asp:Label>
                    </div>
                </div>
                <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                <div class="fieldobject filters">
                    <div class="fieldrow">
                        <asp:TextBox ID="TXBsearchInPreview" runat="server" Columns="60"></asp:TextBox>
                        <asp:Button ID="BTNapplyPreviewFilters" runat="server" CssClass="linkMenu" Text="*Update" />
                    </div>
                    <div class="fieldrow abcfilters">
                        <CTRL:AlphabetSelector ID="CTRLpreviewAlphabetSelector" runat="server" RaiseSelectionEvent="true" AutoSelectLetter="true"/>
                    </div>
                </div>
                <asp:Repeater ID="RPTpreviewItems" runat="server" >
                    <HeaderTemplate>
                        <table class="checkboxtable table light">
                            <thead>
                                <tr id="sub-hdr">
                                    <th class="checkbox">
                                        &nbsp;
                                    </th>
                                    <th class="lastname">
                                        <asp:Label ID="LBuserSurnameHeader_t" runat="server">*Surname</asp:Label>
                                    </th>
                                    <th class="firstname">
                                        <asp:Label ID="LBuserNameHeader_t" runat="server">*Name</asp:Label>
                                    </th>
                                    <th class="actions">
                                         &nbsp;
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="checkbox">
                                <input type="checkbox" class="checkcommission" id="CBXuser" runat="server"/>
                                <asp:Literal ID="LTidProfile" runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                            </td>
                            <td class="lastname">
                                <%#Container.DataItem.Surname%>
                            </td>
                            <td class="firstname">
                                <%#Container.DataItem.Name%>
                            </td>
                            
                            <td class="actions"><span class="icons"><span class="icon delete"></span></span></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td id="TDemptyItems" runat="server">
                                <asp:Label ID="LBnoPreviewItems" runat="server"></asp:Label>
                            </td>
                        </tr>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div class="pager" id="DVpagerPreview" runat="server" visible="false">
                    <CTRL:GridPager ID="PGgridPreview" runat="server" ShowNavigationButton="false" EnableQueryString="false"></CTRL:GridPager>
                </div>
                <div class="fieldobject clearfix">
                    <div class="fieldrow right">
                        <asp:Button ID="BTNcloseUserSelectionPreview" runat="server" />
                        <asp:Button ID="BTNconfirmSelectionEdit" runat="server" />
                        <asp:Button ID="BTNconfirmSelectionEditAndInsert" runat="server"  />
                    </div>
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWnoPermission" runat="server">
        <br />
        <br />
        <br />
        <asp:Label ID="LBnoPermission" runat="server"></asp:Label>
        <br />
        <br />
        <br />
    </asp:View>
</asp:MultiView>