<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="OnLineUsers.aspx.vb" Inherits="Comunita_OnLine.OnLineUsers" Theme="Materiale" EnableTheming="true"%>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.UserActions.DomainModel" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 900px; text-align:right; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <asp:HyperLink id="HYPbackHistory" runat="server"  CssClass="Link_Menu" Visible="false" Text="Back" Height="18px"></asp:HyperLink>
    </div>
    <div style="width: 900px; text-align:left; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <telerik:RadTabStrip ID="TBSusageTime" runat="server" Align="Justify" Height="20px"
	    CausesValidation="False" AutoPostBack="True"  Skin="Outlook" SelectedIndex="0" Width="200px">
		  <Tabs>
			 <telerik:RadTab Text="Users On Line" Value="7" NavigateUrl="OnLineUsers.aspx?View=UserOnLine&Page=0&Dir=desc&Order=LastAction"></telerik:RadTab>
			 <telerik:RadTab Text="Users On Line" Value="8" NavigateUrl="OnLineUsers.aspx?View=UserOnLine&Page=0&Dir=desc&Order=LastAction"></telerik:RadTab>
		  </Tabs>
	   </telerik:RadTabStrip>
    </div>
     <div style="width: 900px; text-align:right; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <div style="text-align:left; float:left;">
		  <asp:Literal runat="server"  ID="LTtotalUsageTime"></asp:Literal>
	   </div> 
    </div>		
    <div id="Div2" style="width:900px; text-align:left; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
        <div style="padding-top:5px;">
            <asp:Literal ID="LTsearch" runat="server"  Text="Name or Surname:"></asp:Literal>
            <asp:TextBox ID="TXBsearch" runat="server" Columns="50" MaxLength="60" SkinId="find"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="BTNsearch" runat="server" CssClass="Link_Menu" UseSubmitBehavior="true"/>
        </div>
        <div style="text-align:right;">
            <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="75">75</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div id="Div1" style="width:900px; text-align:center; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
		<asp:GridView cssclass="table light" Width="900px" runat="server" ID="GDVstatistic" AutoGenerateColumns="false" AllowSorting="true" UseAccessibleHeader="true">
		  <Columns>
			 <asp:TemplateField HeaderText="User" SortExpression="Owner" ItemStyle-HorizontalAlign="left">
				<ItemTemplate>
				    <%# Container.DataItem.Owner%>
	   			</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="Community" SortExpression="Community" ItemStyle-HorizontalAlign="left">
				<ItemTemplate>
	   			    <%# Container.DataItem.CommunityName%>
				</ItemTemplate>
			 </asp:TemplateField>
			  <asp:TemplateField HeaderText="Access" SortExpression="FirstAction" ItemStyle-HorizontalAlign="right">
				<ItemTemplate>
				   <%#Container.DataItem.FirstAction%>
				</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="Module" SortExpression="ModuleName" ItemStyle-HorizontalAlign="right">
				<ItemTemplate>
				   <%# Container.DataItem.ModuleName%>
				</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="LastAction" SortExpression="LastAction" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
				    <%# GetLastActionTranslated(Container.DataItem.LAstAction)%>
				</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="ActionName" SortExpression="ActionName" ItemStyle-HorizontalAlign="Center" Visible=false >
				<ItemTemplate>
				    <%# Container.DataItem.ActionName%>
				</ItemTemplate>
			 </asp:TemplateField>
		  </Columns>
	   </asp:GridView>	   
    </div>
    <div style="width:900px; text-align:right; padding-top:5px; clear:both; height:22px;">
	   <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
    </div>
</asp:Content>