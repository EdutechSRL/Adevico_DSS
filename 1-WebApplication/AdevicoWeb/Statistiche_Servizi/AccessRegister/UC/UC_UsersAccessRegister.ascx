<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_UsersAccessRegister.ascx.vb" Inherits="Comunita_OnLine.UC_UsersAccessRegister" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../../UC/UC_PagerControl.ascx" %>
   	<div id="DIVfilterTitle" style="width: 900px; text-align: left; padding-top: 20px; margin: 0px auto; clear: both;" runat="server">
   	    <asp:Literal runat="server" EnableViewState="false" ID="LTfilterInfo"></asp:Literal>
   	    <br style="clear:left;"/>
   	</div>
   	 
   <div id="Div2" style="width: 900px; padding-top: 5px;   margin: 0px auto; clear: both;" runat="server">
        <div style="padding-top:5px;">
            <asp:Literal ID="LTsearch" runat="server" EnableViewState="false" Text="Name or Surname:"></asp:Literal>&nbsp;
            <asp:TextBox ID="TXBsearch" runat="server" Columns="50" MaxLength="60" SkinId="Filter"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="BTNsearch" runat="server" UseSubmitBehavior="true"/>
        </div>
    </div>
    <div id="Div1" style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto; clear: both;" runat="server">
		<br />
		<div style="text-align:right;" id="DIVpageSize" runat="server">
            <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="75">75</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
            </asp:DropDownList>
        </div>
		<asp:GridView CssClass="table light" width="900px"  runat="server" ID="GDVstatistic" AutoGenerateColumns="false" AllowSorting="true" UseAccessibleHeader="true">
		  <Columns>
			  <asp:TemplateField HeaderText="User" SortExpression="Owner" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
				<ItemTemplate>
				    <%#Container.DataItem.PersonName%>
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
        <div style="width:900px; text-align:right; padding-top:5px; clear:both; height:22px;" >
            <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
        </div>
    </div>