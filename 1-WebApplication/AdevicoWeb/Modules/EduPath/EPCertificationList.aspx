<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EPCertificationList.aspx.vb" Inherits="Comunita_OnLine.EPCertificationList" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
<asp:Label runat="server" ID="LBserviceCertificationList">**Certification List</asp:Label>   
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<asp:MultiView runat="server" ID="MLVcertificationList">
<asp:View runat="server" ID="VIWcertificationList">

<div class="pathcertification epsummary pathcertificationlist">
<div class="fieldobject filters">
    <div class="fieldrow filterstartdate">                        
        <asp:Label runat="server" ID="LBfilterStartDateTitle" CssClass="fieldlabel">**Start date</asp:Label>
        <telerik:RadDatePicker ID="RDPStartDate" runat="server">
            <Calendar ID="Calendar1" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x"
                runat="server">
            </Calendar>
            <DateInput ID="DateInput1" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" Width="" runat="server">
            </DateInput>
            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
        </telerik:RadDatePicker>
    </div>
    <div class="fieldrow filterenddate">
        <asp:Label runat="server" ID="LBfilterEndDateTitle" CssClass="fieldlabel">**End date</asp:Label>
        <telerik:RadDatePicker ID="RDPEndDate" runat="server">
            <Calendar ID="Calendar2" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x"
                runat="server">
            </Calendar>
            <DateInput ID="DateInput2" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" Width="" runat="server">
            </DateInput>
            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
        </telerik:RadDatePicker>
    </div>
    <div class="fieldrow">        
        <asp:Label runat="server" ID="LBfilterCertificateTitle" CssClass="fieldlabel">**Certificate</asp:Label>
        <asp:DropDownList runat="server" id="DDLCertificateFilters">        
        </asp:DropDownList>
    </div>
    <%--<div class="fieldrow">        
        <asp:Label runat="server" ID="LBfilterUserTitle" CssClass="fieldlabel">**User</asp:Label>
        <select class="">
            <option>*First Name</option>
            <option>*Last Name</option>
        </select>
        <asp:TextBox runat="server" ID="TXBuserFilter"></asp:TextBox>
    </div>--%>
    <div class="fieldrow">        
        <asp:Label runat="server" ID="LBfilterCommunityRoleTitle" CssClass="fieldlabel">**Community Role</asp:Label>
        <asp:DropDownList runat="server" id="DDLRolesFilters">        
        </asp:DropDownList>
    </div>
    <div class="fieldrow">        
        <asp:Button runat="server" ID="BTNupdate" Text="**update" />
    </div>
</div>
<asp:CheckBox runat="server" ID="CHBshowall" Text="**all statistics" AutoPostBack="true" />
<table class="table light certificateslist fullwidth">
    <thead>
    <tr>
        <th class="certificatename"><asp:Label runat="server" ID="LBcertificateNameHeader">**Certificate Name</asp:Label></th>
        <th class="stats"><asp:Label runat="server" ID="LBparticipantsStatHeader">**Stats</asp:Label></th>
    </tr>
    </thead>
    <tbody>
    <asp:repeater runat="server" ID="RPTcertifications">
    <ItemTemplate>
    <tr>
        <td class="certificatename">
            <asp:Label runat="server" ID="LBcertificateName" Visible="false">**Certificate</asp:Label>
            <asp:HyperLink runat="server" ID="HYPcertificateName">**Certificate</asp:HyperLink>
        </td>
        <td class="stats"><asp:Label runat="server" ID="LBparticipants">**0 / 100 participants</asp:Label></td>
    </tr>
    </ItemTemplate>
    </asp:repeater>
    <tr runat="server" id="TRempty" visible="false" class="norecordrow">
        <td colspan="2"><asp:Label runat="server" ID="LBLempty">**empty</asp:Label></td>
    </tr>

    </tbody>
</table>
</div>


</asp:View>
<asp:View runat="server" ID="VIWerror">
    <div id="DVerror" align="center">                
        <div align="center">
            <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
        </div>
    </div>
</asp:View>
</asp:MultiView>
</asp:Content>
