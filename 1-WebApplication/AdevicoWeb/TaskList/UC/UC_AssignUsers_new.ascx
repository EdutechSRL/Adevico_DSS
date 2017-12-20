<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AssignUsers_new.ascx.vb" Inherits="Comunita_OnLine.UC_AssignUsers_new" EnableTheming="true" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>

<div id="DivEsterno" runat="server" style="width: 900px; text-align: left;">
    <div>
        <asp:HyperLink ID="HYPaddResource" runat="server" Text="*Add User" CssClass="Link_Menu"></asp:HyperLink>
    </div>
    <div style="float: left;">
       <asp:Repeater Id="RPText" runat="server">
            <HeaderTemplate>
                <table border="1"> 
                    <tr class="ROW_header_Small_Center">
                        <td style="width:130px;">
                            <b>
                                <asp:Literal ID="LTheaderUsername" Text="*USER NAME" runat="server"></asp:Literal>
                            </b>                                                           
                        </td>
                        
                         <td >                            
                            <b>
                                <asp:Literal ID="LTheaderRoles" Text="*ROLES" runat="server"></asp:Literal>
                            </b>                                    
                        </td>                        
                        
                        <td id="TDheaderCompleteness" runat="server">                            
                            <b>
                                <asp:Literal ID="LTheaderCompleteness" Text="*COMPLETENESS" runat="server"></asp:Literal>
                            </b>                                    
                        </td>                        
                    </tr>       
            </HeaderTemplate>
            
                <ItemTemplate>
                    <tr class="ROW_Normal_Small">
                            <td style="width:130px; text-align:center;" class="ROW_Normal_Small" >
                                <asp:Literal ID="LTuserName" runat="server"></asp:Literal>
                            </td>
                            
                            <td>                                                       
                                <asp:Repeater ID="RPTroles" runat="server" OnItemCommand="RPTroles_ItemCommand">
                                    <HeaderTemplate>
                                        <table border="0">                                 
                                    </HeaderTemplate>             
                                    <ItemTemplate>
                                        <tr>                                                               
                                            <td>
                                                <div>
                                                    <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false" Visible="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CausesValidation="false"></asp:LinkButton>
                                                </div>
                                            </td>
                                            <td style="width:100px; text-align:right">
                                                <div>
                                                    <asp:Literal ID="LTinnerRole" runat="server"></asp:Literal>
                                                </div>                                                
                                            </td>
                                        </tr>               
                                    </ItemTemplate>
                                    <AlternatingItemTemplate >
                                        <tr>                                                               
                                            <td>
                                                <div>
                                                    <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false" Visible="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CausesValidation="false"></asp:LinkButton>
                                                </div>
                                            </td>
                                            <td style="width:100px; text-align:right">
                                                <div>
                                                    <asp:Literal ID="LTinnerRole" runat="server"></asp:Literal>
                                                </div>                                                
                                            </td>
                                        </tr>               
                                    </AlternatingItemTemplate>            
                                    <FooterTemplate>
                                                </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>                                                                                               
                        
                            <td id="TDcompleteness" runat="server">
                                <asp:Panel ID="PNLcompletess" runat="server" style= "border:1">
                                   <div style="float: left; width: 60px; text-align: center; padding: 0px 2px 0px 0px;" class="dettagli_CampoSmall">
                                       <asp:Literal ID="LTtaskCompleteness" runat="server"></asp:Literal>
                                       %
                                   </div>
                                   <div style="float: left; width: 100px;  border: 1px solid black;
                                       height: 15px">
                                       <asp:Image ID="IMtaskCompleteness" runat="server" BackColor="Green" Height="10px" />
                                   </div>                                   
                               </asp:Panel>
                                &nbsp;
                           </td>
                        </tr>   
                </ItemTemplate>                         
                <FooterTemplate>
                  </table>
                </FooterTemplate>
       </asp:Repeater>
       
       <div style="text-align: right; padding-top: 5px; clear: both; height: 22px; position: relative;">
          <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="false"></CTRL:GridPager>
       </div>
             
    </div>
    
</div>