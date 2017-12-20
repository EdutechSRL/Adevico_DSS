<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProjectDateInfo.ascx.vb" Inherits="Comunita_OnLine.UC_ProjectDateInfo" %>
<%@ Register TagPrefix="CTRL" TagName="input" Src="~/Modules/Common/UC/UC_InLineInput.ascx" %>
<asp:MultiView ID="MLVproject" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWdefault" runat="Server">
        <div class="fieldobjectgroup clearfix">
            <div class="fieldobject projectdetails left details clearfix">
                <div class="fieldrow projectstart">
                    <asp:Label ID="LBprojectStartDate_t" runat="server" AssociatedControlID="CTRLstartDateInput">*Start:</asp:Label>
                    <CTRL:input runat="server" ID="CTRLstartDateInput" ContainerCssClass="editable" EditCssClass="datepicker" InputMaxChar="10" DataType="Date" ValidationEnabled="false"></CTRL:input>
                </div>
                <div class="fieldrow projectend">
                    <asp:Label ID="LBprojectEndDate_t" runat="server" AssociatedControlID="CTRLstartDateInput">*End:</asp:Label>
                    <CTRL:input runat="server" ID="CTRLendDateInput" ContainerCssClass="editable disabled" EditCssClass="datepicker" ReadOnlyInput="true" InputMaxChar="10" DataType="Date" ValidationEnabled="false"></CTRL:input>
                </div>
                <div class="fieldrow projectdeadline">
                    <asp:Label ID="LBprojectDeadline_t" runat="server" AssociatedControlID="CTRLdeadlineInput">*Deadline:</asp:Label>
                    <CTRL:input runat="server" ID="CTRLdeadlineInput" ContainerCssClass="editable" EditCssClass="datepicker" InputMaxChar="10" DataType="Date" ValidationEnabled="false"></CTRL:input>
                </div>
            </div>
            <div class="fieldobject projectdetails right details clearfix">
                <div class="fieldrow projecttype">
                    <asp:Label ID="LBprojectType_t" runat="server" AssociatedControlID="LBprojectType">*Project type:</asp:Label>
                    <asp:Label ID="LBprojectType" runat="server" CssClass="item">*Project type:</asp:Label>
                </div>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
</asp:MultiView>