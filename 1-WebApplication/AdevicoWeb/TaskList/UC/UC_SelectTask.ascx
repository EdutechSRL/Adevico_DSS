<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectTask.ascx.vb" Inherits="Comunita_OnLine.UC_SelectTask" %>
<style type="text/css">
.ROW_Selected
{
	font-size: 11px;
	background: Aquamarine;
	color: black;	
	text-align: left;
}
.CellSelect
{
width:50px;
}
.CellTaskName
{
width:auto;
}
.Table
{
    width:880;
}

</style>

        <div style="text-align:center;">
            <asp:Repeater ID="RPlistOfTask" runat="server" EnableViewState="false"  >
            <HeaderTemplate>    
                <table id="tableMap" class="Table"  align="center"  cellspacing="0" border="1" >
                    <tr class="ROW_header_Small_Center">
                        <td class="CellSelect"></td>
                        <td class="CellTaskName"><asp:Label ID="LBtaskNameTitle" runat="server" >TASK</asp:Label></td>                                    
                    </tr>  
            </HeaderTemplate>
            <ItemTemplate>
            <tr  class="<%# me.SelectStyle(False,(Container.DataItem).TaskID) %>" > 
                <td class="CellSelect" >                    
                    <asp:LinkButton id="LBTselectTask" runat="server" Text="Select" CommandName="select"  CssClass="ROW_ItemLink_Small"></asp:LinkButton>        
                </td>
                <td class="CellTaskName">
                    <asp:Literal ID="LTspaceWBS" runat="server"></asp:Literal>
                    <asp:Label ID="LBwbs" runat="server" ></asp:Label>           
                    <asp:Label ID="LBtaskName" runat="server"  ></asp:Label>            
                </td>                    
            </tr>
            </ItemTemplate>
            <AlternatingItemTemplate >
            <tr   class="<%# me.SelectStyle(True,(Container.DataItem).TaskID) %>">
                <td class="CellSelect">  
                     <asp:LinkButton id="LBTselectTask" runat="server" Text="Select" CommandName="select"  CssClass="ROW_ItemLink_Small"></asp:LinkButton>        
                </td>
                <td class="CellTaskName">
                    <asp:Literal ID="LTspaceWBS" runat="server"></asp:Literal>
                    <asp:Label ID="LBwbs" runat="server" ></asp:Label>           
                    <asp:Label ID="LBtaskName" runat="server"  ></asp:Label>            
                </td>       
            </tr>           
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table> 
                <br />
            </FooterTemplate>
            </asp:Repeater> 
        </div>