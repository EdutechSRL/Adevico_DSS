<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="MySystemStatistics.aspx.vb" Inherits="Comunita_OnLine.MySystemStatistics" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.UserActions.DomainModel" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 900px; text-align:right; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <div style="text-align:left; float:left;">
		  <asp:Literal runat="server"  ID="LTtotalUsageTime"></asp:Literal>
	   </div> 
	   <div style="text-align:right; float:left;">
            <span class="icons">
		        <asp:HyperLink id="HYPdetails" runat="server" CssClass="icon view"></asp:HyperLink>
            </span>
	   </div>
    </div>
    <div style="width: 900px; text-align:center; margin:0,auto; padding-top:5px; padding-left:5px; clear:both;" align="center">
	   <telerik:RadTabStrip ID="TBSusageTime" runat="server" Align="Justify" Width="100%" Height="20px"
	    CausesValidation="False" AutoPostBack="True"  Skin="Outlook" SelectedIndex="2">
		  <Tabs>
			 <telerik:RadTab Text="Personali" Value="MySystem" Selected="true" Visible="false"></telerik:RadTab>
			 <telerik:RadTab Text="Altri utenti" Value="UsersSystem" Visible="false"></telerik:RadTab>
			 <telerik:RadTab Text="Generic Portal" Value="System" Visible="false"></telerik:RadTab>
		  </Tabs>
	   </telerik:RadTabStrip>
    </div>
    <div id="Div1" runat="server" style="width: 900px; padding-top: 5px; margin: 0px auto;  clear: both;">
        <br />
		<asp:GridView CssClass="table light" Width="900px" runat="server" ID="GDVstatistic" AutoGenerateColumns="false" AllowSorting="true" UseAccessibleHeader="true">
		  <Columns>
			 <asp:TemplateField HeaderText="Community" SortExpression="Community" ItemStyle-HorizontalAlign="left">
				<ItemTemplate>
				    <span><%# Container.DataItem.Name%></span>
                    <asp:Literal ID="LTinfo" runat="server" Visible="false"></asp:Literal>
	   			</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="Time" SortExpression="UsageTime" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
	   				<%# GetTimeTranslatedString(Container.DataItem.ToTimeSpan)%>
				</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="N° accessi" SortExpression="AccessNumber" ItemStyle-HorizontalAlign="Right">
				<ItemTemplate>
				    <%# Container.DataItem.nAccesses%>
				</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
                    <span class="icons">
                        <asp:HyperLink ID="HYPdetails" runat="server" Target="_self" CssClass="icon view"></asp:HyperLink>    
                    </span>
				</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField Visible="false">
				<ItemTemplate>
				 <asp:label ID="LBguidID" runat="server"></asp:label>
				  </ItemTemplate> 
			 </asp:TemplateField>
		  </Columns>
	   </asp:GridView>	   
    </div>
    <div style="width:900px; text-align:right; padding-top:5px; margin: 0px auto; clear:both; height:22px;">
	   <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
    </div>
</asp:Content>