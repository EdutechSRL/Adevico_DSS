<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TaskDetail.aspx.vb" Inherits="Comunita_OnLine.TaskDetail" ValidateRequest="False"
MasterPageFile="~/AjaxPortal.Master"  %>

<%@ Register TagPrefix="CTRL" TagName="User" Src="UC/UC_AssignUsers_new.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Detail" Src="UC/UC_TaskDetail.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TaskManagementFile" Src="~/Modules/Common/UC/UC_OtherModuleItemFiles.ascx"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">

 <style type="text/css">
 /*FireFox*/.TaskDetail
 {
    
 }
 
</style>

<asp:MultiView ID="MLVtaskDetail" runat="server">
    <asp:View  ID="VIWtaskDetail" runat="server">
       <div id="DIVtaskDetail" runat="server"  style="display:block;">
             <CTRL:Detail ID="CTRLdetail" runat="server"  />
        </div>      
        <div id="DIVassignedPerson" runat="server" style=" clear:both;  padding-top:5px; ">
            <CTRL:User ID="CTRLuser" runat="server" />
        </div>
        <div id="DIVfiles" runat="server" style="clear: both;"> 
            <div id="DIVaddFileButton" runat="server">
                <asp:HyperLink ID="HypFileManagement" runat="server" Text="*Aggiungi File" CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <div id="DIVfileSummary" runat="server">
                <CTRL:TaskManagementFile ID="CTRLtaskManagementFile" runat="server" />
            </div>
       </div>    
    </asp:View>
    <asp:View ID="VIWerror" runat="server">
      <div id="DVerror" align="center">
         <div align="right" style="text-align:right; clear:right;">     
            <asp:HyperLink ID="HYPreturnToTaskList" runat="server" Text="Torna all'elenco" CssClass="Link_Menu"/>
         </div>
         <div align="center">
            <asp:Label ID="LBerror" runat="server" CssClass= "messaggio"></asp:Label>          
         </div>
      </div>   
    </asp:View>
</asp:MultiView>
</asp:Content>
