<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CertificationAction.ascx.vb" Inherits="Comunita_OnLine.UC_CertificationAction" %>
<asp:MultiView ID="MLVcontrol" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
        <span class="objectRender <%= ContainerCSS %>">
            <asp:Label ID="LBempty" runat="server"></asp:Label>
        </span>
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <span class="objectRender <%= ContainerCSS %>">
            <asp:Literal ID="LTidentifier" runat="server"></asp:Literal>
            <span class="leftDetail" id="SPNdetail" runat="server">
                <asp:Label ID="LBplace0" runat="server" CssClass="plh plh0" Visible="false"></asp:Label>
                <span class="itemTitle" runat="server" id="SPNitemTitle">
                    <span class="actionIco certificate">&nbsp;</span>
                    <asp:Label ID="LBcertificationActionBegin" runat="server"></asp:Label>
                    <asp:HyperLink ID="HYPexecute" runat="server" Visible="false" Target="_blank" CssClass="fileRepositoryCookie"></asp:HyperLink>
                    <asp:Label ID="LBcertificationActionEnd" runat="server"></asp:Label>
                </span>
                <asp:Label ID="LBplace1" runat="server" CssClass="plh plh1" Visible="false"></asp:Label>
                <asp:Label ID="LBsize" CssClass="itemDetails" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="LBplace2" runat="server" CssClass="plh plh2" Visible="false"></asp:Label>
            </span>
            <asp:Label ID="LBplace3" runat="server" CssClass="plh plh3" Visible="false"></asp:Label>
            <asp:Repeater ID="RPTactions" runat="server">
                <HeaderTemplate>
                    <span class="itemActions">
                </HeaderTemplate>
                <ItemTemplate>
                    <span class="action">
                        <asp:HyperLink ID="HYPaction" runat="server">&nbsp;</asp:HyperLink>
                        <asp:HyperLink ID="HYPdownloadAction" runat="server" Visible="false" Target="_blank" CssClass="fileRepositoryCookie"></asp:HyperLink>
                        <asp:linkButton ID="LNBaction" runat="server" Visible="false" OnClientClick="blockUIForDownload(2);return true;" ></asp:linkButton>
                    </span>
                </ItemTemplate>
                <FooterTemplate>
                    </span>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Label ID="LBplace4" runat="server" CssClass="plh plh4" Visible="false"></asp:Label>
        </span>
    </asp:View>
    <asp:View ID="VIWsettingsInfo" runat="server">
        <div class="certificateaction">
            <div class="fieldobject">
                <div class="fieldrow">
                    <asp:Label ID="LBcertificationName_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcertificationName"></asp:Label>
                    <asp:Label ID="LBcertificationName"  runat="server" CssClass="option"></asp:Label>&nbsp;
                    <asp:HyperLink ID="HYPpreviewCertification" runat="server" Visible="false" Target="_blank"></asp:HyperLink>
                    <%--<label class="fieldlabel">Name:</label><span class="option">Certificate Name</span>--%>
                </div>
                <div class="fieldrow">
                    <asp:Label ID="LBcertificationAllowWithEmptyPlaceHolders_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcertificationAllowWithEmptyPlaceHolders">*Genera con campi vuoti:</asp:Label>
                    <asp:Label ID="LBcertificationAllowWithEmptyPlaceHolders"  runat="server" CssClass="option"></asp:Label>
                </div>
                <div class="fieldrow">
                    <asp:Label ID="LBcertificationAutogenerateOption_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcertificationAutogenerateOption"></asp:Label>
                    <asp:Label ID="LBcertificationAutogenerateOption"  runat="server" CssClass="option"></asp:Label>
                </div>
                <div class="fieldrow">
                    <asp:Label ID="LBcertificationSavingOption_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcertificationSavingOption"></asp:Label>
                    <asp:Label ID="LBcertificationSavingOption"  runat="server" CssClass="option"></asp:Label>
                </div>
                <div class="fieldrow" id="DVminCompletion" runat="server">
                     <asp:Label ID="LBcertificationMinCompletionActivationOption_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LTcertificationMinCompletionActivationOption"></asp:Label>
                     <span class="option">
                        <asp:Literal ID="LTcertificationMinCompletionActivationOption" runat="server"></asp:Literal>
                        <asp:Label ID="LBminCompletion" runat="server" CssClass="mincompletion"></asp:Label>
                     </span>
                </div>
                <div class="fieldrow" id="DVminMark" runat="server" visible="false">
                     <asp:Label ID="LBcertificationMinMarkActivationOption_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LTcertificationMinMarkActivationOption"></asp:Label>
                     <span class="option">
                        <asp:Literal ID="LTcertificationMinMarkActivationOption" runat="server"></asp:Literal>
                        <asp:Label ID="LBminMark" runat="server" CssClass="minmark"></asp:Label>
                     </span>
                </div>
                <div class="fieldrow">
                     <asp:Label ID="LBusePathEndDateStatistics_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LTusePathEndDateStatisticsOption"></asp:Label>
                     <span class="option">
                        <asp:Literal ID="LTusePathEndDateStatisticsOption" runat="server"></asp:Literal>
                        <asp:Label ID="LBusePathEndDateStatisticsDetails" runat="server"></asp:Label>
                     </span>
                </div>
                <div class="fieldrow">
                     <asp:Label ID="LBactiveAfterPathEndDate_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LTactiveAfterPathEndDateOption"></asp:Label>
                     <span class="option">
                        <asp:Literal ID="LTactiveAfterPathEndDateOption" runat="server"></asp:Literal>
                        <asp:Label ID="LBactiveAfterPathEndDateDetails" runat="server"></asp:Label>
                     </span>
                </div>
                <div class="fieldrow">
                    <div class="quizarea">
                        <asp:Repeater ID="RPTquestionnaires" runat="server">
                            <HeaderTemplate>
                            <table class="quizzes table minimal">
                                <thead>
                                    <tr>
                                        <th class="name"><asp:Label runat="server" ID="LBquizTitle">*Quiz</asp:Label></th>
                                        <th class="mandatory"><asp:Label runat="server" ID="LBmandatoryTitle">*Mandatory</asp:Label></th>
                                        <th class="visible"><asp:Label runat="server" ID="LBvisibleTitle">*Visible</asp:Label></th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="name"><%#Container.DataItem.Name %></td>
                                    <td class="mandatory">
                                        <span class="icons">
                                            <asp:Label ID="LBselectedMandatoryOption" runat="server" CssClass="icon yes">X</asp:Label>
                                        </span>
                                    </td>
                                    <td class="visible">
                                        <span class="icons">
                                            <asp:Label ID="LBselectedVisibleOption" runat="server" CssClass="icon yes">X</asp:Label>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView><asp:Literal ID="LTcssClassRefresh" runat="server" Visible="false">fileRepositoryCookie</asp:Literal><asp:Literal ID="LTcssClassNoRefresh" runat="server" Visible="false">fileRepositoryCookieNoRefresh</asp:Literal>