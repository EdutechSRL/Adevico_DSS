<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="UsageGlobal.aspx.vb" Inherits="Comunita_OnLine.UsageGlobal" Theme="Materiale" EnableTheming="true" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.UserActions.DomainModel" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 900px; text-align:right; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <asp:HyperLink id="HYPbackHistory" runat="server"  CssClass="Link_Menu" Visible="false" Text="Back" Height="18px"></asp:HyperLink>
    </div>
    <div style="width: 900px; text-align:right; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <div style="text-align:left; float:left;">
		  <asp:Literal runat="server"  ID="LTtotalUsageTime"></asp:Literal>
	   </div> 
	   <div style="text-align:right; float:left;">
		  <asp:HyperLink id="HYPdetails" runat="server"  ImageUrl="~/images/Grid/preview.gif"></asp:HyperLink>
	   </div>
    </div>
    <div style="width: 900px; text-align:center; margin:0,auto; padding-top:5px; padding-left:5px; clear:both;" align="center">
	   <telerik:RadTabStrip ID="TBSusageTime" runat="server" Align="Justify" Width="100%" Height="20px"
	    CausesValidation="False" AutoPostBack="True"  Skin="Outlook" SelectedIndex="2">
		  <Tabs>
			 <telerik:RadTab Text="Personali" Value="0" NavigateUrl="UsageGlobal.aspx?View=Personal&Page=0&Dir=desc&Order=UsageTime" Selected="True" ></telerik:RadTab>
			 <telerik:RadTab Text="Singolo utente" Value="3" NavigateUrl="UsageGlobal.aspx?View=GenericUser&Page=0&Dir=asc&Order=Owner"></telerik:RadTab>
			 <telerik:RadTab Text="Altri utenti" Value="1" NavigateUrl="UsageGlobal.aspx?View=SystemUsers&Page=0&Dir=asc&Order=Owner"></telerik:RadTab>
			 <telerik:RadTab Text="Personal Community" Value="4" NavigateUrl="UsageGlobal.aspx?View=PersonalCommunity&Page=0&Dir=asc&Order=ModuleName"></telerik:RadTab>
			 <telerik:RadTab Text="Utenti Comunità" Value="2" NavigateUrl="UsageGlobal.aspx?View=CommunityUsers&Page=0&Dir=asc&Order=Owner"></telerik:RadTab>
			 <telerik:RadTab Text="Generic Community" Value="6" NavigateUrl="UsageGlobal.aspx?View=GenericCommunity&Page=0&Dir=desc&Order=UsageTime"></telerik:RadTab>
			 <telerik:RadTab Text="Generic Portal" Value="5" NavigateUrl="UsageGlobal.aspx?View=GenericSystem&Page=0&Dir=desc&Order=UsageTime"></telerik:RadTab>
			 <telerik:RadTab Text="Users On Line" Value="7" NavigateUrl="OnLineUsers.aspx?View=UserOnLine&Page=0&Dir=desc&Order=LastAction"></telerik:RadTab>
			 <telerik:RadTab Text="Users On Line" Value="8" NavigateUrl="OnLineUsers.aspx?View=UserOnLine&Page=0&Dir=desc&Order=LastAction"></telerik:RadTab>
		  </Tabs>
	   </telerik:RadTabStrip>
    </div>			 
    <div style="width:900px; text-align:center; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
		<asp:GridView SkinID="griglia900" runat="server" ID="GDVstatistic" AutoGenerateColumns="false" AllowSorting="true" UseAccessibleHeader="true">
		  <Columns>
			 <asp:TemplateField HeaderText="Community" SortExpression="Community">
				<ItemTemplate>
				    <%# Container.DataItem.Name%>
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
			 <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
				    <asp:HyperLink ID="HYPdetails" runat="server" Target="_self" ImageUrl="~/images/Grid/preview.gif"></asp:HyperLink>
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
    <div style="width:900px; text-align:right; padding-top:5px; clear:both; height:22px;">
	   <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
    </div>
</asp:Content>