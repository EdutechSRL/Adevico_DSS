<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="UsageModule.aspx.vb" Inherits="Comunita_OnLine.UsageModule" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>

<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.UsageResults.DomainModel" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DIVfilterTitle" style="width: 900px; text-align: center; padding-top: 5px;
        margin: 0px auto; clear: both;" runat="server">
        <div>
            <div style="text-align: left; float: left; width: 600px;">
                <p>
                    <asp:Literal runat="server"  ID="LTfilterInfo"></asp:Literal></p>
            </div>
            <div style="text-align: right; float: left; width: 100px;">
                <p>
                    <asp:HyperLink ID="HYPbackHistory" runat="server"  CssClass="Link_Menu"
                        Visible="false" Text="Back" Height="18px"></asp:HyperLink></p>
            </div>
        </div>
        <br style="clear: left;" />
    </div>
    <div id="DIVresultFilters" style="width: 700px; text-align: center; padding-top: 5px;
        margin: 0px auto; clear: both;" runat="server">
          <div style="padding-top:5px;">
            <asp:label ID="LBsearch" runat="server"  Text="Name or Surname:"></asp:label>
            <asp:TextBox ID="TXBsearch" runat="server" Columns="50" MaxLength="60" SkinId="find"></asp:TextBox>
        </div>
        <div style="clear: both;">
            <div style="text-align: left; float: left; width: 40px;">
                <asp:Literal runat="server"  ID="LTdataI"></asp:Literal>
            </div>
            <div style="text-align: left; width: 150px; float: left;">
                <telerik:RadDatePicker ID="RDPdataI" runat="server">
                </telerik:RadDatePicker>
            </div>
            <div style="text-align: right; float: left; width: 40px;">
                <asp:Literal runat="server"  ID="LTdataF"></asp:Literal>
            </div>
            <div style="text-align: left; width: 150px; float: left;">
                <telerik:RadDatePicker ID="RDPdataF" runat="server">
                </telerik:RadDatePicker>
            </div>
            <div style="text-align: left; width: 150px; float: left;">
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="BTNsearch" runat="server" CssClass="Link_Menu" />
            </div>
            <div style="text-align: right; float: left;">
                <asp:HyperLink ID="HYPprint" runat="server"  CssClass="Link_Menu"
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
    <div id="Div1" style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <br />
        <asp:UpdatePanel ID="UPDresults" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:GridView SkinID="griglia900" runat="server" ID="GDVmoduleUsage" AutoGenerateColumns="false"
                    AllowSorting="true" UseAccessibleHeader="true">
                    <Columns>
                        <asp:TemplateField HeaderText="User" SortExpression="Owner">
                            <ItemTemplate>
                                <%#Container.DataItem.PersonName%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Day" SortExpression="Day">
                            <ItemTemplate>
                                <%#FormatDateTime(Container.DataItem.Day, DateFormat.ShortDate)%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time" SortExpression="UsageTime">
                            <ItemTemplate>
                                <%# GetTimeTranslatedString(Container.DataItem.ToTimeSpan)%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="N° accessi" SortExpression="AccessNumber">
                            <ItemTemplate>
                                <%# Container.DataItem.nAccesses%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LBguidID" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;">
                    <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
