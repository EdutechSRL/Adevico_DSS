<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="UsageReport.aspx.vb" Inherits="Comunita_OnLine.UsageReport" Theme="Materiale" EnableTheming="true" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.UsageResults.DomainModel" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 700px; text-align:center; margin:0,auto; padding-top:5px; clear:both;" align="center">
	   <telerik:RadTabStrip ID="TBSusageTime" runat="server" Align="Justify" 
            Width="100%" Height="20px"
	    CausesValidation="False" AutoPostBack="True"  Skin="Outlook" SelectedIndex="4">
		  <Tabs>
			 <telerik:RadTab Text="My Portal Presence" Value="0" 
                  NavigateUrl="UsageReport.aspx?View=MyPortalPresence&SubView=UserReport&Page=0&Dir=asc&Order=Hour" ></telerik:RadTab>
             <telerik:RadTab Text="My communities Presence" Value="1" NavigateUrl="UsageReport.aspx?View=MyCommunitiesPresence&SubView=MyCommunityList&Page=0&Dir=asc&Order=Community"></telerik:RadTab>
			 <telerik:RadTab Text="Users Portal Presence" Value="2" NavigateUrl="UsageReport.aspx?View=UsersPortalPresence&SubView=UsersList&Page=0&Dir=asc&Order=Owner"></telerik:RadTab>
			 <telerik:RadTab Text="Current community Presence" Value="3" NavigateUrl="UsageReport.aspx?View=CurrentCommunityPresence&SubView=UserReport&Page=0&Dir=asc&Order=Hour"></telerik:RadTab>
			 <telerik:RadTab Text="Users community Presence" Value="4" 
                  NavigateUrl="UsageReport.aspx?View=UsersCurrentCommunityPresence&SubView=UsersList&Page=0&Dir=asc&Order=Owner" 
                  Selected="True"></telerik:RadTab>
                  
                <telerik:RadTab Text="Users reports between date" Value="5" 
                  NavigateUrl="UsageReport.aspx?View=BetweenDateUsersPortal&Page=0&Dir=asc&Order=Owner" 
                  ></telerik:RadTab>
          <telerik:RadTab Text="Users reports between date" Value="6" 
                  NavigateUrl="UsageReport.aspx?View=BetweenDateUsersCommunity&Dir=asc&Order=Owner" 
                  ></telerik:RadTab>
	      </Tabs>
	   </telerik:RadTabStrip>
    </div>
   	<div id="DIVfilterTitle" style="width:700px; text-align:center; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
   	    <div>
   	        <div style="text-align:left; float:left; width:600px;">
		        <p><asp:Literal runat="server"  ID="LTfilterInfo"></asp:Literal></p>
            </div>
            <div style="text-align:right; float:left; width:100px;">
                <p><asp:HyperLink id="HYPbackHistory" runat="server"  CssClass="Link_Menu" Visible="false" Text="Back" Height="18px"></asp:HyperLink></p>
            </div> 
   	    </div>
   	    <br style="clear:left;">
   	</div>		
   	<div id="DIVresultFilters" style="width:700px; text-align:center; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">  
        <div style="clear:both;">
            <div style="text-align:left;float:left; width: 40px;">
                <asp:Literal runat="server"  ID="LTdataI"></asp:Literal>
            </div>
            <div style="text-align:left; width:150px; float:left;">
                <telerik:RadDatePicker ID="RDPdataI" runat="server"></telerik:RadDatePicker>           
            </div> 
            <div style="text-align:right; float:left; width: 40px;">
                <asp:Literal runat="server"  ID="LTdataF"></asp:Literal>
            </div>
            <div style="text-align:left; width:150px; float:left;">
                <telerik:RadDatePicker ID="RDPdataF" runat="server">
                </telerik:RadDatePicker>
            </div>
            <div style="text-align:left; width:150px; float:left;">
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="BTNsearch" runat="server"  CssClass="Link_Menu"/>
            </div>
            <div style="text-align:right; float:left;">
                <asp:HyperLink id="HYPprint" runat="server"  CssClass="Link_Menu" Height="18px" Target="_blank"></asp:HyperLink>
            </div>
        </div>
       
        <div style="clear:both;">
            <asp:RequiredFieldValidator ID="RFVdadaI" ControlToValidate="RDPdataI" runat="Server"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RFVdadaF" ControlToValidate="RDPdataF" runat="Server"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CMVdate" runat="server" ControlToValidate="RDPdataF"
            ControlToCompare="RDPdataI" Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>         
        </div>
    </div>
    <div id="Div1" style="width:700px; text-align:center; padding-top:5px; margin: 0px auto;  clear:both;" runat="server">
		<br />
		<asp:GridView SkinID="griglia700" runat="server" ID="GDVstatistic" AutoGenerateColumns="false" AllowSorting="true" UseAccessibleHeader="true">
		  <Columns>
			 <asp:TemplateField HeaderText="Community" SortExpression="Community">
				<ItemTemplate>
				    <%#Container.DataItem.CommunityName%>
	   			</ItemTemplate>
			 </asp:TemplateField>
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
			 <asp:TemplateField HeaderText="Hours" SortExpression="Hour">
				<ItemTemplate>
				    <%#Container.DataItem.HourToInterval%>
				</ItemTemplate>
			 </asp:TemplateField>
			 <asp:TemplateField HeaderText="Time" SortExpression="UsageTime">
				<ItemTemplate>
	   				<%# GetTimeTranslatedString(Container.DataItem.ToTimeSpan)%>
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
        <div style="width:700px; text-align:right; padding-top:5px; clear:both; height:22px;">
            <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
        </div>
    </div>
   
</asp:Content>