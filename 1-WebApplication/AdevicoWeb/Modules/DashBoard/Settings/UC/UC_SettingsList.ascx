<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SettingsList.ascx.vb" Inherits="Comunita_OnLine.UC_SettingsList" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>


    <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <div class="fieldobject tablewrapper">
        <table class="table light viewsetlist fullwidth plain showinfo">
            <thead>
                <tr>
                    <th class="name">
                        <span><asp:Literal ID="LTthSettingsName" runat="server">*Name</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderBySettingsNameUp" runat="server" cssclass="icon orderUp" CommandName="Name" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderBySettingsNameDown" runat="server" cssclass="icon orderDown" CommandName="Name" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="createdon">
                        <span><asp:Literal ID="LTthModifiedOn" runat="server">*Modified on</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderByModifiedOnUp" runat="server" cssclass="icon orderUp" CommandName="ModifiedOn" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByModifiedOnDown" runat="server" cssclass="icon orderDown" CommandName="ModifiedOn" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="createdby">
                        <span><asp:Literal ID="LTthModifiedBy" runat="server">*Modified by</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderByModifiedByUp" runat="server" cssclass="icon orderUp" CommandName="ModifiedBy" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderByModifiedByDown" runat="server" cssclass="icon orderDown" CommandName="ModifiedBy" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="status">
                        <span><asp:Literal ID="LTthSettingsStatus" runat="server">*Type</asp:Literal></span>
                        <asp:LinkButton ID="LNBorderSettingsByStatusUp" runat="server" cssclass="icon orderUp" CommandName="Status" CommandArgument="True" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LNBorderSettingsByStatusDown" runat="server" cssclass="icon orderDown" CommandName="Status" CommandArgument="False" Visible="false" OnClick="LNBorderBy_Click"></asp:LinkButton>
                    </th>
                    <th class="actions">
                        <span class="icons"><asp:Label ID="LBactions" runat="server" CssClass="icon actions"></asp:Label></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RPTsettings" runat="server">
                    <ItemTemplate>
                        <tr id="viewset-<%#Container.DataItem.Id %>" class="viewset <%#GetItemCssClass(Container.DataItem)%>">
                            <td class="name">
                                <a name="viewset<%#Container.DataItem.Id %>"></a>
                                <asp:Label ID="LBsettingsName" runat="server" cssclass="text" Text="<%#Container.DataItem.Name %>"></asp:Label>
                                <span class="text" id="SPNsettingsName" runat="server" visible="false"><asp:HyperLink ID="HYPviewSettings" runat="server" Text="<%#Container.DataItem.Name %>" Target="_blank"></asp:HyperLink></span>
                                <div class="extrainfo" id="DVextraInfo" runat="server" visible="false">
                                    <asp:Label ID="LBassignmentsInfo_t" cssclass="text" runat="server" Text="*Profiles:"></asp:Label>
                                    <span class="profiles">
                                        <asp:Repeater ID="RPTassignments" runat="server" DataSource="<%#Container.DataItem.Assignments%>">
                                            <ItemTemplate>
                                                <span class="profile <%#Container.DataItem.Type.ToString().ToLower() %>"><%#Container.DataItem.DisplayName %></span>
                                            </ItemTemplate>
                                            <SeparatorTemplate>
                                                 <span class="sep">|</span>
                                            </SeparatorTemplate>
                                        </asp:Repeater>
						            </span>
						        </div>
                            </td>
                            <td class="createdon"><asp:Label ID="LBmodifiedOn" runat="server"></asp:Label></td>
                            <td class="createdby"><%#Container.DataItem.ModifiedBy %></td>
                            <td class="status"><%#Container.DataItem.TranslatedStatus %></td>
                            <td class="actions">
                                <span class="icons">
                                    <asp:linkbutton ID="LNBcloneSettings" runat="server" Visible="false" CssClass="icon copy" CommandName="copy" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:HyperLink ID="HYPeditSettings" runat="server" CssClass="icon edit" Visible="false"></asp:HyperLink>
                                    <asp:linkbutton ID="LNBhideSettings" runat="server" Visible="false" CssClass="icon hidden" CommandName="hide" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBshowSettings" runat="server" Visible="false" CssClass="icon visible" CommandName="show" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBvirtualDeleteSettings" runat="server" Visible="false" CssClass="icon virtualdelete needconfirm" CommandName="virtualdelete" CommandArgument="<%#Container.DataItem.Id %>" ></asp:linkbutton>
                                    <asp:linkbutton ID="LNBvirtualUnDeleteSettings" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument="<%#Container.DataItem.Id %>"></asp:linkbutton>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="TRempty" runat="server" visible="false">
                            <td colspan="5">
                                <asp:Label ID="LBemptyItems" runat="server" CssClass="empty">*No tiles</asp:Label>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <span class="fieldrow legend hor" id="DVlegend" runat="server">
        <asp:Label ID="LBtableLegend" runat="server" CssClass="fieldlabel">*Legend</asp:Label>
        <span class="group first last">
            <asp:Literal ID="LTdraftItem" runat="server"></asp:Literal>
            <asp:Literal ID="LTdefaultItem" runat="server"></asp:Literal>
        </span>
    </span>

<asp:Literal ID="LTcssClassDefault" runat="server" Visible="false">default</asp:Literal>
<asp:Literal ID="LTtemplateLegendItem" runat="server" Visible="false">
    <span class="legenditem" title="{0}">
    <span class="legendicon {1}">&nbsp;</span>
        <span class="legendtext">{0}</span>
    </span>
</asp:Literal>