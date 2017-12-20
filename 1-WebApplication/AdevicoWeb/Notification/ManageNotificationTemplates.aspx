<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ManageNotificationTemplates.aspx.vb" Inherits="Comunita_OnLine.ManageNotificationTemplates" Theme="Materiale" EnableTheming="true" ValidateRequest="false" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div align="center" style="clear:both; width:800px; text-align:center; padding-top:10px; ">
        <div style="float:right; text-align: right;">
		    <asp:Button ID="BTNsave" runat="server"  CssClass="Link_Menu" UseSubmitBehavior="true" Text="Update"/> 
		</div>
	</div>
	<div align="center" style="clear:both; width:800px; text-align:center; padding-top:10px; ">
	    <div style="text-align:left; padding-left:20px;">
	        <asp:Literal runat="server" EnableViewState="false" ID="LTmodule">Module:</asp:Literal>
            <asp:DropDownList ID="DDLmodule" runat="server" AutoPostBack="true"></asp:DropDownList>
            &nbsp;&nbsp;
            <asp:Literal runat="server" EnableViewState="false" ID="LTmoduleAction">Action:</asp:Literal>
            <asp:DropDownList ID="DDLmoduleAction" runat="server" AutoPostBack="true"></asp:DropDownList>
             &nbsp;&nbsp;
            <asp:Literal runat="server" EnableViewState="false" ID="LTtemplateType">Type:</asp:Literal>
            <asp:DropDownList ID="DDLtemplateType" runat="server" AutoPostBack="true">
                <asp:ListItem Value="1">Sommario</asp:ListItem>
                <asp:ListItem Value="2" Selected="True">Notifica generica</asp:ListItem>
            </asp:DropDownList>
	    </div>
	    <div style="text-align:left; padding-left:20px; padding-top:10px; clear:both;">
	       <div style="clear:both;">
                <div style="float:left;">
                    <table border="1" cellspacing="0">
                        <asp:Repeater ID="RPTtemplates" runat="server">
                            <HeaderTemplate>
                                <tr class="ROW_header_Small_Center">
                                    <td>
                                        <asp:Literal id="LTheaderLanguage" runat="server" Visible="true" Text="Language"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal id="LTheaderMessages" runat="server" Visible="true" Text="Messages"></asp:Literal>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="ROW_Normal_Small">
                                    <td valign="top">
                                        <asp:Literal id="LTlanguageName" runat="server" Visible="true" Text='<%#Container.DataItem.LanguageName%>'></asp:Literal>
                                        <asp:Literal id="LTlanguageID" runat="server" Visible="false" Text='<%#Container.DataItem.LanguageID%>'></asp:Literal>
                                        <asp:Literal id="LTid" runat="server" Visible="false" Text='<%#Container.DataItem.ID%>'></asp:Literal>
                                    </td>
                                    <td valign="top">
                                        <div>
                                            <div style="width:100px; float:left;">
                                                <asp:Literal ID="LTtemplateName" runat="server">Name:</asp:Literal>
                                            </div>
                                            <div style="float:left;">
                                                <asp:TextBox ID="TXBtemplate" Columns="70" runat="server" Width="600px" Text='<%#Container.DataItem.TemplateName%>' SkinID="inputsmall"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div>
                                            <div style="width:100px; float:left;">
                                                <asp:Literal ID="LTmessage" runat="server">Message:</asp:Literal>
                                            </div>
                                            <div style="float:left;">
                                                <asp:TextBox ID="TXBmessage" Columns="70" runat="server" Height="50px" Rows="1" TextMode="MultiLine" Width="600px" Text='<%#Container.DataItem.Message%>' SkinID="inputsmall"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="ROW_Alternate_Small">
                                    <td valign="top">
                                        <asp:Literal id="LTlanguageName" runat="server" Visible="true" Text='<%#Container.DataItem.LanguageName%>'></asp:Literal>
                                        <asp:Literal id="LTlanguageID" runat="server" Visible="false" Text='<%#Container.DataItem.LanguageID%>'></asp:Literal>
                                        <asp:Literal id="LTid" runat="server" Visible="false" Text='<%#Container.DataItem.ID%>'></asp:Literal>
                                    </td>
                                    <td valign="top">
                                        <div>
                                            <div style="width:100px; float:left;">
                                                <asp:Literal ID="LTtemplateName" runat="server">Name:</asp:Literal>
                                            </div>
                                            <div style="float:left;">
                                                <asp:TextBox ID="TXBtemplate" Columns="70" runat="server" Width="600px" Text='<%#Container.DataItem.TemplateName%>' SkinID="inputsmall"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div>
                                            <div style="width:100px; float:left;">
                                                <asp:Literal ID="LTmessage" runat="server">Message:</asp:Literal>
                                            </div>
                                            <div style="float:left;">
                                                <asp:TextBox ID="TXBmessage" Columns="70" runat="server" Height="50px" Width="600px" Rows="1" TextMode="MultiLine" Text='<%#Container.DataItem.Message%>' SkinID="inputsmall"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
	    </div>
	</div>    
</asp:Content>