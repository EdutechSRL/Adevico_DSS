<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ManageTaskAssignment.aspx.vb"
    Inherits="Comunita_OnLine.ManageTaskAssignment" MasterPageFile="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="AssignedUser" Src="UC/UC_AssignedUser.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AddAssignment" Src="UC/UC_AddTaskAssignment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="QuickSelectionTaskUsers" Src="UC/UC_QuickSelectionTaskUsers.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVtitle" style="width: 900px; text-align: left;" class="RigaTitolo" align="center">
        <asp:Label ID="LBtitolo" runat="server">**Add Task Assignments</asp:Label>
    </div>
    <div>
        <asp:Button ID="BTNswitchUserSelection" runat="server" CssClass="Link_Menu" Text="**QuickSearch/Classic" />
        <asp:HyperLink ID="HYPreturn" runat="server" CssClass="Link_Menu" Text="**return"></asp:HyperLink>
    </div>
    <asp:MultiView ID="MLVtaskAssignment" runat="server">

        <asp:View ID="VIWaddAssignmment" runat="server">
        <div style="text-align: right;padding:5px">
                <asp:Button ID="BTNaddTaskAssignments" runat="server" CssClass="Link_Menu" Text="**Add Classic Task Assignments" />
        </div>
            <div style="width: 900px; text-align: left; height: auto" class="RigaTitolo" align="center" >
                <asp:Label ID="LBwbs" runat="server"></asp:Label>
                <asp:Label ID="LBtaskName" runat="server"></asp:Label>
            </div>            
            <div style="padding: 5px;">
                <CTRL:AddAssignment ID="CTRLaddUser" runat="server" />
            </div>             
        </asp:View>
        
        <asp:View ID="VIWquickSelection" runat="server">
           <div>
                <asp:Button ID="BTNsaveQuickLoad" runat="server"  CssClass="Link_Menu"  Text="**Save quick Assignment"/>
                <CTRL:QuickSelectionTaskUsers ID="CTRLquickUsersSelection" runat="server" />
           </div> 
        </asp:View>
       
        <asp:View runat="server" ID="VIWassignedPersons">
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div align="right" style="text-align: right; clear: right">
                    <asp:HyperLink ID="HYPreturnError" runat="server" Text="**Return" CssClass="Link_Menu" />
                </div>
                <div align="center" style="padding: 5px;">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWquickAddTaskAssignments" runat="server">
            <div id="DVquickTA">
                <asp:Repeater ID="RPuserPermission" runat="server" EnableViewState="false">
                                <HeaderTemplate>
                                    <table id="tableMap" border="1" width="880px" cellspacing="0">
                                        <tr class="ROW_header_Small_Center">
                                            <td>
                                                <asp:Label ID="LBpersonTitle" runat="server" Text="*Ruolo" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBmanagerTitle" runat="server" Text="*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBresourceTitle" runat="server" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LBvisitorTitle" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="ROW_Normal_Small">
                                        
                                        <td class="TableItem">
                                            <asp:Label ID="LBperson" runat="server" Text="*Nomeutente" CssClass="Titolo_campoSmall"></asp:Label>
                                        </td>
                                        
                                        <td class="TableItem">
                                            <asp:CheckBox ID="CKBmanager" runat="server" Text="*Manager" CssClass="dettagli_CampoSmall" />
                                        </td>
                                        <td class="TableItem">
                                            <asp:CheckBox ID="CKBresource" runat="server" Text="*Resource" CssClass="dettagli_CampoSmall" />
                                        </td>
                                         <td class="TableItem">
                                            <asp:CheckBox ID="CBXvisitor" runat="server" Text="*Visitor" CssClass="dettagli_CampoSmall" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="ROW_Alternate_Small">
                                        
                                        <td class="TableItem">
                                            <asp:Label ID="LBperson" runat="server" Text="*Nomeutente" CssClass="Titolo_campoSmall"></asp:Label>
                                        </td>
                                        
                                        <td class="TableItem">
                                            <asp:CheckBox ID="CKBmanager" runat="server" Text="*Manager" CssClass="dettagli_CampoSmall" />
                                        </td>
                                        <td class="TableItem">
                                            <asp:CheckBox ID="CKBresource" runat="server" Text="*Resource" CssClass="dettagli_CampoSmall" />
                                        </td>
                                         <td class="TableItem">
                                            <asp:CheckBox ID="CBXvisitor" runat="server" Text="*Visitor" CssClass="dettagli_CampoSmall" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>    
            </div>
        </asp:View>      
    </asp:MultiView>
</asp:Content>
