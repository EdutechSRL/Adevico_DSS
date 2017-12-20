<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TimeStat.ascx.vb"
    Inherits="Comunita_OnLine.UC_TimeStat" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div class="DetailItem DateSelector" id="DIVselect" runat="server">
    <div class="DetailLeftItem" style="padding: 0px;">
        <asp:Label runat="server" ID="LBviewStat" Text="AAAAA*" CssClass="Titolo_campoSmall"></asp:Label>
    </div>
    <div class="DetailRightItem inline">
        <telerik:RadDatePicker ID="RDPviewStat" runat="server">
            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x"
                runat="server">
            </Calendar>
            <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" Width="" runat="server">
            </DateInput>
            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
        </telerik:RadDatePicker>
    </div>
    <div class="DetailRightItem inline">
        <telerik:RadAjaxPanel runat="server" ID="Radajaxpanel2">
            <telerik:RadNumericTextBox DecimalDigits="0" Type="number" ShowSpinButtons="true"
                IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                Label="" runat="server" ID="TXBhour" Width="50px" MaxValue="100" MinValue="0">
            </telerik:RadNumericTextBox>:<telerik:RadNumericTextBox DecimalDigits="0" Type="number"
                ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                Label="" runat="server" ID="TXBminuts" Width="50px" MaxValue="59" MinValue="0">
            </telerik:RadNumericTextBox>
        </telerik:RadAjaxPanel>
    </div>
    <div class="DetailRightItem inline">
        <asp:LinkButton ID="LKBviewStat" runat="server" Text="go*" CssClass="Link_Menu"></asp:LinkButton>
    </div>
</div>
<div id="DIVendDate" runat="server" class="DetailItem DateEnd" >
    <asp:Label ID="LBendDate" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
    <asp:LinkButton ID="LKBviewCertified" runat="server" Text="go*" CssClass="Link_Menu"></asp:LinkButton>
    <asp:LinkButton ID="LKBviewNow" runat="server" Text="go*" CssClass="Link_Menu"></asp:LinkButton>
</div>
<div id="DIVoverTime" runat="server" class="DetailItem DateOver">
    <asp:Label ID="LBoverTime" runat="server" >&nbsp;</asp:Label>
</div>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="DropDownList4">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="TXBhEnd" />
                <telerik:AjaxUpdatedControl ControlID="TXBmEnd" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
