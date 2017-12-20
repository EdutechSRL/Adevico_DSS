<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="MyCommunityStatistics.aspx.vb" Inherits="Comunita_OnLine.MyCommunityStatistics" Theme="Materiale" EnableTheming="true"%>
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
    <div id="DVmenu" style="width: 900px; text-align: right;" align="center" runat="server" visible="false">
        <asp:HyperLink ID="HYPbackHistory" runat="server" CssClass="Link_Menu" Text="Back" Height="18px" CausesValidation="false"></asp:HyperLink>&nbsp;
    </div>
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
			 <telerik:RadTab Text="Personali" Value="MyCommunity" Selected="true" Visible="false"></telerik:RadTab>
			 <telerik:RadTab Text="Altri utenti" Value="UsersCommunity" Visible="false"></telerik:RadTab>
			 <telerik:RadTab Text="Comunita'" Value="Community" Visible="false"></telerik:RadTab>
		  </Tabs>
	   </telerik:RadTabStrip>
    </div>			 
     <asp:MultiView ID="MLVuser" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstatistics" runat="server">
            <div id="Div1" style="width:900px; text-align:center; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
		        <asp:GridView CssClass="table light" Width="900px" runat="server" ID="GDVstatistic" AutoGenerateColumns="false" AllowSorting="true" UseAccessibleHeader="true">
		          <Columns>
			         <asp:TemplateField HeaderText="ModuleName" SortExpression="ModuleName" ItemStyle-HorizontalAlign="Left">
				        <ItemTemplate>
				            <%# Container.DataItem.Name%>
	   			        </ItemTemplate>
			         </asp:TemplateField>
			         <asp:TemplateField HeaderText="Time" SortExpression="UsageTime" ItemStyle-HorizontalAlign="right">
				        <ItemTemplate>
	   				        <%# GetTimeTranslatedString(Container.DataItem.ToTimeSpan)%>&nbsp;
				        </ItemTemplate>
			         </asp:TemplateField>
			         <asp:TemplateField HeaderText="N° accessi" SortExpression="AccessNumber" ItemStyle-HorizontalAlign="right">
				        <ItemTemplate>
				            <%# Container.DataItem.nAccesses%>&nbsp;
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
            <div style="width:900px; text-align:right; padding-top:5px; clear:both; height:22px;">
	           <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
            </div>
        </asp:View>
        <asp:View ID="VIWunknown" runat="server">
            <br /><br /><br /><br /><br />
            <asp:Label ID="LBinfo" runat="server"></asp:Label>
            <br /><br /><br /><br /><br />
        </asp:View>
    </asp:MultiView>
</asp:Content>