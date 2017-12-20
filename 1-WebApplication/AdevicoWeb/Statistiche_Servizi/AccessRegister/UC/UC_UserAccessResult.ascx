<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_UserAccessResult.ascx.vb"
    Inherits="Comunita_OnLine.UC_UserAccessResult" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div id="DIVfilterTitle" style="width: 900px; text-align: left; padding-top: 20px; margin: 0px auto; clear: both;" runat="server">
    <asp:Literal runat="server" ID="LTfilterInfo"></asp:Literal>
    <br style="clear: left;" />
</div>
<div id="DIVresultFilters" style="width: 900px; padding-top: 5px;   margin: 0px auto; clear: both;" runat="server">
    <div style="clear: both; text-align: left; padding-bottom: 5px; padding-top: 5px;"
        id="DIVusername" runat="server">
        <asp:Literal ID="LTsearch" runat="server" EnableViewState="false" Text="Name or Surname:"></asp:Literal>&nbsp;
        <asp:TextBox ID="TXBsearch" runat="server" Columns="50" MaxLength="60" SkinID="Filter"></asp:TextBox>
    </div>
    <div style="clear: both;">
        <div style="text-align: left; float: left; width: 40px; vertical-align: middle;">
            <asp:Literal runat="server" ID="LTdataI"></asp:Literal>
        </div>
        <div style="text-align: left; width: 150px; float: left;">
            <telerik:RadDatePicker ID="RDPdataI" runat="server">
            </telerik:RadDatePicker>
        </div>
        <div style="text-align: right; float: left; width: 40px; padding-right:5px; vertical-align: middle;">
            <asp:Literal runat="server" EnableViewState="false" ID="LTdataF"></asp:Literal>
        </div>
        <div style="text-align: left; width: 150px; float: left;">
            <telerik:RadDatePicker ID="RDPdataF" runat="server">
            </telerik:RadDatePicker>
        </div>
        <div style="text-align: left; width: 200px; float: left;">
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="BTNsearch" runat="server" />
        </div>
        <div style="text-align: right; float: left;">
            <asp:HyperLink ID="HYPprint" runat="server" EnableViewState="false" CssClass="Link_Menu"
                Height="18px" Target="_blank"></asp:HyperLink>
        </div>
    </div>
    <div style="clear: both;">
        <asp:RequiredFieldValidator ID="RFVdadaI" ControlToValidate="RDPdataI" runat="Server"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RFVdadaF" ControlToValidate="RDPdataF" runat="Server"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="CMVdate" runat="server" ControlToValidate="RDPdataF" ControlToCompare="RDPdataI"
            Operator="GreaterThan" Type="Date"></asp:CompareValidator>
    </div>
</div>
<div id="DIVdata" style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto; clear: both;" runat="server">
    <div>
        <div style="text-align: left; float: left; width: 50%" id="DVtotalUsage">
            <asp:Label ID="LBtotalTime" runat="server"/>
        </div>
        <div style="text-align: right; float: left; width: 50%" id="DIVpageSize" runat="server">
            <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="75">75</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div style="clear: both;">
        <asp:GridView CssClass="table light" width="900px" runat="server" ID="GDVstatistic" AutoGenerateColumns="false" AllowSorting="true" UseAccessibleHeader="true">
            <Columns>
                <asp:TemplateField HeaderText="Community" SortExpression="Community" Visible="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Container.DataItem.CommunityName%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User" SortExpression="Owner" ItemStyle-Width="220px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Container.DataItem.PersonName%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Day" SortExpression="Day" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#FormatDateTime(Container.DataItem.Day, DateFormat.ShortDate)%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hours" SortExpression="Hour" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Container.DataItem.HourToInterval%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Time" SortExpression="UsageTime" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#GetTimeTranslatedString(Container.DataItem.ToTimeSpan)%>&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="LBguidID" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;">
        <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
    </div>
</div>
