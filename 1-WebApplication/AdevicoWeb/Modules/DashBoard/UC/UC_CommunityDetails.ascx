<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityDetails.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityDetails" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLexpand" Src="~/Modules/Common/UC/UC_ExpandAndCollapse.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLinfo" Src="~/Modules/Common/UC/UC_HelperSimpleContainerItems.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLdetails" Src="~/Modules/Dashboard/UC/UC_ConstraintsDetails.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLbar" Src="~/Modules/Common/UC/UC_StackedBar.ascx" %>

<div class="homecontent">
    <div class="communityinfo container_12 clearfix">
        <div class="section grid_9">
            <div class="sectioninner">
                <div class="cisectionheader clearfix">
                    <div class="left">
                        <h3><asp:Literal ID="LTcommunityInfoTitle" runat="server">*information</asp:Literal></h3>
                    </div>
                    <div class="right">
                        &nbsp;
                    </div>
                </div>
                <div class="cisectioncontent">
                    <div class="tablewrapper">
                        <table class="table minimal fullwidth communitydetails">
                            <tbody>
                            <tr>
                                <td class="key"><asp:Literal ID="LTdetailsCommunityType_t" runat="server">*Community type</asp:Literal></td>
                                <td class="value"><asp:Literal ID="LTdetailsCommunityType" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="key"><asp:Literal ID="LTdetailsStatus_t" runat="server">*Status</asp:Literal></td>
                                <td class="value"><asp:Literal ID="LTdetailsStatus" runat="server"/></td>
                            </tr>
                            <tr>
                                <td class="key"><asp:Literal ID="LTdetailsEnrollments_t" runat="server">*Subscriptions</asp:Literal></td>
                                <td class="value"><asp:Literal ID="LTdetailsEnrollments" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td class="key"><asp:Literal ID="LTdetailsCreatedOn_t" runat="server">*Created on</asp:Literal></td>
                                <td class="value"><asp:Label ID="LBdetailsCreatedOn" runat="server" CssClass="date"></asp:Label></td>
                            </tr>
                            <tr id="TRclosedOn" runat="server" visible="false"> 
                                <td class="key"><asp:Literal ID="LTdetailsClosedOn_t" runat="server">*Created on</asp:Literal></td>
                                <td class="value"><asp:Label ID="LBdetailsClosedOn" runat="server" CssClass="date"></asp:Label></td>
                            </tr>
                            <tr id="TRcourseYear" runat="server" visible="false"> 
                                <td class="key"><asp:Literal ID="LTdetailsCourseYear_t" runat="server">*Academic Year</asp:Literal></td>
                                <td class="value"><asp:Literal ID="LTdetailsCourseYear" runat="server"></asp:Literal></td>
                            </tr>
                            <tr id="TRcourseCode" runat="server" visible="false"> 
                                <td class="key"><asp:Literal ID="LTdetailsCourseCode_t" runat="server">*Course code</asp:Literal></td>
                                <td class="value"><asp:Literal ID="LTdetailsCourseCode" runat="server"></asp:Literal></td>
                            </tr>
                            <tr id="TRcourseTimespan" runat="server" visible="false"> 
                                <td class="key"><asp:Literal ID="LTdetailsCourseTimespan_t" runat="server">*Periode</asp:Literal></td>
                                <td class="value"><asp:Literal ID="LTdetailsCourseTimespan" runat="server"></asp:Literal></td>
                            </tr>
                            <tr id="TRenrollmentWindow" runat="server" visible="false"> 
                                <td class="key"><asp:Literal ID="LTenrollmentWindow_t" runat="server">*Enrollment window</asp:Literal></td>
                                <td class="value">
                                    <span class="daterange">
                                        <CTRL:CTRLinfo id="CTRLstart" runat="server" ContainerCssClass="start" FirstCssClass="label" FirstAsLabelOf="Second" SecondCssClass="date" Visible="false"/> 
                                        <CTRL:CTRLinfo id="CTRLend" runat="server" ContainerCssClass="end" FirstCssClass="label" FirstAsLabelOf="Second" SecondCssClass="date" Visible="false"/> 
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td class="key"><asp:Literal ID="LTmaxEnrollments_t" runat="server">*Max subscriptions</asp:Literal></td>
                                <td class="value">
                                    <asp:Label ID="LBmaxEnrollments" runat="server">*unlimited</asp:Label>
                                    <CTRL:CTRLinfo id="CTRLlimitedSeats" runat="server" ContainerCssClass="total seats" FirstCssClass="text" SecondCssClass="number" Visible="false"/> 
                                    <CTRL:CTRLinfo id="CTRLextraSeats" runat="server" ContainerCssClass="total extra seats"  FirstCssClass="text" SecondCssClass="number" Visible="false"/> 
                                </td>
                            </tr>
                            <tr>
                                <td class="key"><asp:Literal ID="LTenrollments_t" runat="server">*Enrollments</asp:Literal></td>
                                <td class="value">
                                    <span class="total seats">
                                        <asp:label ID="LBenrollments" runat="server" CssClass="text">*total subscriptions:</asp:label>
                                        <asp:label ID="LBenrollmentsNumber" runat="server" CssClass="number"></asp:label>
                                    </span>
                                </td>
                            </tr>
                            <tr id="TRseatsAvailable" runat="server" visible="false">
                                <td class="key"><asp:Literal ID="LTdetailsAvailableSeats_t" runat="server">*Seats</asp:Literal></td>
                                <td class="value">
                                    <span class="progressbarwrapper">
                                        <span class="available seats">
                                            <asp:Label ID="LBdetailsAvailableSeats" runat="server" CssClass="text">*available seats:</asp:Label>
                                            <asp:Label ID="LBseatsNumber" runat="server" CssClass="number"></asp:Label>
                                        </span>
                                        <span class="seats progress">
                                            <CTRL:CTRLbar id="CTRLprogressBar" runat="server" />
                                        </span>
                                    </span>
                                </td>
                            </tr>
                            <tr id="TRspecialAccess" runat="server" visible="false">
                                <td class="key"><asp:Literal ID="LTspecialAccess_t" runat="server">*Special Access</asp:Literal></td>
                                <td class="value"><asp:Literal ID="LTspecialAccess" runat="server"/></td>
                            </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="cisectionfooter">
                    <asp:Label ID="LBcommunityTagsTitle" CssClass="label" runat="server">*Tags:</asp:Label>
                    <span class="tags">
                        <asp:Label ID="LBtagCommunityType" runat="server" CssClass="tag type" Visible="false"></asp:Label>
                    <asp:Repeater ID="RPTtags" runat="server">
                        <ItemTemplate>
                            <span class="tag"><%#Container.DataItem %></span>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <span class="sep">|</span>
                        </SeparatorTemplate>
                    </asp:Repeater>
                    </span>
                </div>
            </div>
        </div>
        <div class="section box grid_3">
            <div class="sectioninner">
                <div class="cisectionheader clearfix">
                    <div class="left">
                        <h3><asp:Literal ID="LTcommunityOwnerTitle" runat="server">*Owner</asp:Literal></h3>
                    </div>
                    <div class="right">
                        &nbsp;
                    </div>
                </div>
                <div class="cisectioncontent">
                    <div class="profile">
                        <div class="img">
                            <asp:Image ID="IMGavatar" runat="server" />
                        </div>
                        <div class="fieldobject ownerdetails owner" id="DVowner" runat="server" >
                            <div class="fieldrow name">
                                <asp:Label ID="LBownerName" runat="server"></asp:Label>
                            </div>
                            <div class="fieldrow mail">
                                <asp:Label ID="LBownerMail" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="fieldobject ownerdetails creator" id="DVcreator" runat="server" visible="false">
                            <div class="fieldrow name">
                                <asp:Label ID="LBcommunityCreatedBy_t" CssClass="fieldlabel" runat="server">*Creator:</asp:Label>
                                <asp:Label ID="LBcommunityCreatedBy" runat="server"/>
                            </div>
                            <div class="fieldrow mail">
                                <asp:Label ID="LBcommunityCreatedByMail" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="cisectionfooter">
 
                </div>
            </div>
        </div>
        <div class="section box requirements collapsable expanded grid_12" id="DVconstraints" runat="server" visible="false">
            <div class="sectioninner">
                <div class="cisectionheader clearfix">
                    <div class="left">
                        <h3>
                            <asp:Literal ID="LTconstraintsTitle" runat="server">*Constraints and prerequisites</asp:Literal>
                            <CTRL:CTRLexpand id="CTRLexpandRequirements" runat="server"></CTRL:CTRLexpand>
                        </h3>
                    </div>
                    <div class="right">
                        &nbsp;
                    </div>
                </div>
                <div class="cisectioncontent">
                    <CTRL:CTRLdetails id="CTRLconstraintsEnrolledTo" visible="false" runat="server" ContainerCssClass="should" MessageCssClass="ok"></CTRL:CTRLdetails>
                    <CTRL:CTRLdetails id="CTRLconstraintsNotEnrolledTo" visible="false" runat="server" ContainerCssClass="shouldnt" MessageCssClass="error"></CTRL:CTRLdetails>
                    <CTRL:CTRLdetails id="CTRLconstraintsEnrollingUnavailableFor" visible="false" runat="server" ContainerCssClass="shouldnt" MessageCssClass="alert"></CTRL:CTRLdetails>
                </div>
                <div class="cisectionfooter">
                </div>
            </div>
        </div>
        <div class="section box collapsable collapsed grid_12" id="DVdescription" runat="server">
            <div class="sectioninner">
                <div class="cisectionheader clearfix">
                    <div class="left">
                        <h3>
                            <asp:Literal ID="LTdescriptionTile" runat="server">*description</asp:Literal>
                            <CTRL:CTRLexpand id="CTRLexpandDescription" runat="server"></CTRL:CTRLexpand>
                        </h3>
                    </div>
                    <div class="right">
                        &nbsp;
                    </div>
                </div>
                <div class="cisectioncontent">
                    <div class="description">
                        <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="cisectionfooter">
 
                </div>
            </div>
        </div>
        <div class="section box collapsable collapsed grid_12">
            <div class="sectioninner">
                <div class="cisectionheader clearfix">
                    <div class="left">
                        <h3>
                            <asp:Literal ID="LTextraInfoTile" runat="server">*extra info</asp:Literal>
                            <CTRL:CTRLexpand id="CTRLexpandExtrainfo" runat="server"></CTRL:CTRLexpand>
                        </h3>
                    </div>
                    <div class="right">
                        &nbsp;
                    </div>
                </div>
                <div class="cisectioncontent container_12 clearfix">
                    <div class="section box grid_8 alpha" id="DVotherDetails" runat="server" visible="false">
                        <div class="sectioninner">
                            <div class="cisectionheader clearfix">
                                <div class="left">
                                    <h3>
                                        <asp:Literal ID="LTotherDetailsTitle" runat="server">*other details</asp:Literal>
                                    </h3>
                                </div>
                                <div class="right">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="cisectioncontent">
                                <div class="tablewrapper">
                                    <table class="table minimal fullwidth communitydetails others">
                                        <tbody>
                                            <asp:Repeater ID="RPTotherDetails" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="key"><%#Container.DataItem.Title%></td>
                                                        <td class="value"><%#Container.DataItem.Value%></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="cisectionfooter">                          
                            </div>
                        </div>
                    </div>
                    <div class="section box grid_12 alpha omega" id="DVenrollmentsDetails" runat="server">
                        <div class="sectioninner">
                            <div class="cisectionheader clearfix">
                                <div class="left">
                                    <h3>
                                        <asp:Literal ID="LTenrollmentsDetailsTitle" runat="server">*subscriptions details</asp:Literal>
                                    </h3>
                                </div>
                                <div class="right">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="cisectioncontent">
                                <span class="subscription roles">
                                    <asp:repeater ID="RPTenrollmentsInfo" runat="server">
                                        <ItemTemplate>
                                            <span class="role">
                                                <span class="number"><%#Container.DataItem.Count %></span>
                                                <span class="text"><%#Container.DataItem.Name %></span>
                                                <span class="waiting" id="SPNwaiting" runat="server">
                                                    <asp:Label ID="LBwaitingRoleOpen" runat="server" CssClass="text">*(including</asp:Label>
                                                    <span class="number"><%#Container.DataItem.Waiting %></span>
                                                    <asp:Label ID="LBwaitingRoleClose" runat="server" CssClass="text">in waiting)</asp:Label>
                                                </span>
                                            </span>
                                        </ItemTemplate>
                                    </asp:repeater>
                                </span>
                            </div>
                            <div class="cisectionfooter">
                                <asp:Literal ID="LTenrollmentsDetailsCount_t" runat="server">*total subscriptions:</asp:Literal>
                                <asp:Label ID="LBenrollmentsDetailsCount" runat="server" CssClass="number"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="cisectionfooter">
                    
                </div>
            </div>
        </div>
    </div>
</div>
<asp:Literal id="LTdefaultImg" runat="server" Visible="false">Graphics/Modules/Community/Img/nophoto.png</asp:Literal>
<asp:Literal id="LTavailableSeatsCssClass" runat="server" Visible="false">available</asp:Literal>
<asp:Literal id="LTbusySeatsCssClass" runat="server" Visible="false">busy</asp:Literal>
<asp:Literal id="LTotherSeatsCssClass" runat="server" Visible="false">admins</asp:Literal>
<asp:Literal id="LTotherDetailsCssClass" runat="server" Visible="false">grid_4 omega</asp:Literal>
<asp:Literal id="LTnoOtherDetailsCssClass" runat="server" Visible="false">grid_12 alpha omega</asp:Literal>