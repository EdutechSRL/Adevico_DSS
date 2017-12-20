<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectUsersFromCall.ascx.vb"
    Inherits="Comunita_OnLine.UC_SelectUsersFromCall" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<asp:MultiView ID="MLVsubmissions" runat="server">
    <asp:View ID="VIWempty" runat="server">
        <div class="submissionsclone">
            <div class="fieldobject fielddescription">
                <div class="fieldrow">
                    <asp:Label ID="LBemptyCallSelectorDescription" runat="server" CssClass="description"></asp:Label>
                </div>
            </div>
            <div class="fieldobject">
                <div class="fieldrow right">
                    <asp:Button ID="BTNemptyCloseCallSelector" runat="server" CssClass="Link_Menu" />
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWselectCall" runat="server">
        <div class="submissionsclone">
            <div class="fieldobject fielddescription">
                <div class="fieldrow">
                    <asp:Label ID="LBcallSelectorDescription" runat="server" CssClass="description"></asp:Label>
                </div>
            </div>
            <div class="tablewrapper">
                <asp:Repeater ID="RPTcalls" runat="server">
                    <HeaderTemplate>
                        <table class="table light callforpapers">
                            <thead>
                                <tr id="sub-hdr">
                                    <th class="select">
                                        <asp:Literal ID="LTselectTitle" runat="server">*Select</asp:Literal>
                                    </th>
                                    <th class="cfp">
                                        <asp:Literal ID="LTcallNameTitle" runat="server">*Call name</asp:Literal>
                                    </th>
                                    <th class="details">
                                        <asp:Literal ID="LTsubmissionsCountTitle" runat="server">*Details</asp:Literal>
                                    </th>
                                    <th class="date">
                                        <asp:Literal ID="LTsubscriptionsClosedOnTitle" runat="server">*End On</asp:Literal>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="select">
                                <asp:LinkButton ID="LNBselectCall" runat="server" CommandArgument='<%#Container.DataItem.Id%>'></asp:LinkButton>
                            </td>
                            <td class="cfp">
                                <span title="<%#Container.DataItem.CommunityName%>">
                                    <%#Container.DataItem.Name%></span>
                            </td>
                            <td class="details">
                                <%#Container.DataItem.Submissions%>
                            </td>
                            <td class="date">
                                <asp:Literal ID="LTsubscriptionsClosedOn" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="pager">
                <CTRL:GridPager ID="PGcallsPager" runat="server" ShowNavigationButton="false" EnableQueryString="false"
                    Visible="false"></CTRL:GridPager>
            </div>
            <div class="fieldobject">
                <div class="fieldrow right">
                    <asp:Button ID="BTNcloseCallSelector" runat="server" CssClass="Link_Menu" />
                    <asp:Button ID="BTNundoCallSelection" runat="server" CssClass="Link_Menu" Visible="false" />
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWselectItems" runat="server">
        <div class="submissionsclone">
            <div class="fieldobject fielddescription">
                <div class="fieldrow">
                    <asp:Label ID="LBsubmissionsSelectorDescription" runat="server" CssClass="description"></asp:Label>
                </div>
            </div>
            <div class="fieldobject filters clearfix">
                <div class="fieldrow inline">
                    <asp:Label ID="LBcallname_t" runat="server" CssClass="LBcallname"></asp:Label>
                    <asp:Label ID="LBcallname" runat="server" CssClass="cfpname"></asp:Label>
                </div>
                <div class="fieldrow inline">
                    <asp:Button ID="BTNchangeSelectedCall" runat="server" CssClass="Link_Menu" />
                </div>
            </div>
            <div class="fieldobject filters clearfix">
                <div class="fieldrow filterwinners">
                    <asp:Label ID="LBfilterSubmissionsStatus_t" runat="server" CssClass="fieldlabel"
                        AssociatedControlID="DDLfilterStatus"></asp:Label>
                    <asp:DropDownList ID="DDLfilterStatus" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="fieldrow filtersubmission">
                    <asp:Label ID="LBfilterSubmissionsType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLfilterSubmitterTypes"></asp:Label>
                    <asp:DropDownList ID="DDLfilterSubmitterTypes" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="fieldrow filtersubmitter inline">
                    <asp:Label ID="LBfilterSubmitterName" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBsubmitterName"></asp:Label>
                    <asp:TextBox ID="TXBsubmitterName" runat="server" CssClass="fieldinput"></asp:TextBox>
                </div>
                <div class="fieldrow filtersubmitter inline">
                    <asp:Button ID="BTNapplySubmissionsFilters" runat="server" CssClass="Link_Menu" />
                </div>
            </div>
            <div class="tablewrapper">
                <asp:Repeater ID="RPTcallSubmissions" runat="server">
                    <HeaderTemplate>
                        <table class="table light submissions">
                            <thead>
                                <tr id="sub-hdr">
                                    <th class="check leftside">
                                        <span class="headercheckbox leftside">
                                            <input type="checkbox" class="checkall" />
                                            <div class="selectorpopup checkall" id="DVselectorAll" runat="server">
                                                <div class="inner">
                                                    &nbsp;<asp:LinkButton ID="LNBselectItemsIntoAllPages" runat="server" Text="*Seleziona su tutte le pagine" CommandName="all"></asp:LinkButton>
                                                </div>
                                                <div class="bottomarrow">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <div class="selectorpopup checknone" id="DVselectorNone" runat="server">
                                            <div class="inner">
                                                &nbsp;<asp:LinkButton ID="LNBunselectItemsIntoAllPages" runat="server" Text="*Deseleziona su tutte le pagine" CommandName="none"></asp:LinkButton>
                                            </div>
                                            <div class="bottomarrow">
                                                &nbsp;
                                            </div>
                                        </div>
                                        </span>
                                    </th>
                                    <th class="submission">
                                        <asp:Literal ID="LTsubmissionTitle" runat="server">*Submission</asp:Literal>
                                    </th>
                                    <th class="submittertype">
                                        <asp:Literal ID="LTsubmissionTypeTitle" runat="server">*Type</asp:Literal>
                                    </th>
                                    <th class="mark" runat="server" id="THmark">
                                        <asp:Literal ID="LTsubmissionEvaluationTitle" runat="server">*Type</asp:Literal>
                                    </th>
                                    <th class="date">
                                        <asp:Literal ID="LTsubmittedOnTitle" runat="server">*Last edit On</asp:Literal>
                                    </th>
                                   
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="sub-<%#Container.DataItem.Id %>">
                            <td class="check">
                                <span class="submittercheckbox">
                                    <input type="checkbox" runat="server" id="CBXsubmission" />
                                    <asp:Literal ID="LTidSubmission" runat="server" Visible="false" Text='<%#Container.DataItem.Id %>'></asp:Literal>
                                </span>
                            </td>
                            <td class="submission">
                                <%#Container.DataItem.SubmitterName %>
                            </td>
                            <td class="submittertype">
                                <%#Container.DataItem.SubmitterType %>
                            </td>
                            <td class="mark" runat="server" id="TDmark">
                                <asp:Label ID="LBmark" runat="server"></asp:Label>
                            </td>
                            <td class="date">
                                <asp:Label ID="LBlastActionOn" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Label ID="LBnoCallSubmissions" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="pager">
                <CTRL:GridPager ID="PGsubmissionsPager" runat="server" ShowNavigationButton="false"
                    EnableQueryString="false" Visible="false"></CTRL:GridPager>
            </div>
            <div class="fieldobject">
                <div class="fieldrow right">
                    <asp:Button ID="BTNcloseCallSumbissionsSelector" runat="server" CssClass="Link_Menu" />
                    <asp:Button ID="BTNaddSubmissions" runat="server" CssClass="Link_Menu"  Enabled="false" />
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>