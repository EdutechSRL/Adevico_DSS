<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="forums.aspx.vb" Inherits="Comunita_OnLine.forums" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="./../jscript/generali.js" language="Javascript"></script>
    <link rel="stylesheet" href="../Graphics/Modules/Forum/forum.new.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="topmenu">
        <asp:Panel ID="PNLmenu" CssClass="pnlmenu" runat="server" HorizontalAlign="right">
            <asp:Label ID="LBselezioneForum_t" runat="server" CssClass="FiltroCampoSmall"></asp:Label>&nbsp;&nbsp;
            <asp:RadioButtonList ID="RBLselezioneForum" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                AutoPostBack="True" CssClass="FiltroCampoSmall">
                <asp:ListItem Value="0" Selected="True">Forum attivi</asp:ListItem>
                <asp:ListItem Value="1">Forum archiviati</asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel>
    </div>
    <div class="forumcontent">
        <asp:Panel ID="PNLpermessi" runat="server" Visible="False">
            <div class="messages">
                <div class="message error">
                    <span class="icons"><span class="icon">&nbsp;</span></span><asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>            
        </asp:Panel>
        <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center">
            <div class="tablewrapper">
            
                        <asp:DataGrid ID="DGforum" runat="server" DataKeyField="FRUM_ID" CellPadding="0" BorderWidth="0" BorderStyle="None"
                            CellSpacing="0" HorizontalAlign="Center" AutoGenerateColumns="False" ShowFooter="true"
                            CssClass="datagridforum">
                            <ItemStyle CssClass="ForumNW_RowNormal"></ItemStyle>
                            <AlternatingItemStyle CssClass="ForumNW_RowAlternato"></AlternatingItemStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <table class="table light fullwidth forumtable forums">
                                            <tr class="tableheader">
                                                <th class="indicators">
                                                    &nbsp;
                                                </th>
                                                <th class="forumname">
                                                    <asp:Label ID="LBforum_t" runat="server">Forum</asp:Label>
                                                </th>
                                                <th class="topics">
                                                    <asp:Label ID="LBthreads_t" runat="server">Topics</asp:Label>
                                                </th>
                                                <th class="posts">
                                                    <asp:Label ID="LBposts_t" runat="server">Posts</asp:Label>
                                                </th>
                                                <th class="lastpost">
                                                    <asp:Label ID="LBlastPost_t" runat="server">Last Post</asp:Label>
                                                </th>
                                            </tr>                                                                                    
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        
                                            <tr class="tablerow">
                                                <td class="indicators">
                                                    <img runat="server" src='<%# DataBinder.Eval(Container.DataItem, "proprieta") %>'
                                                        alt='<%# DataBinder.Eval(Container.DataItem, "alternative") %>' id="IMchiuso" />
                                                </td>
                                                <td class="forumname">
                                                    <div class="linewrapper title clearfix">
                                                        <span class="name">
                                                            <a name="forum_<%# DataBinder.Eval(Container.DataItem, "FRUM_ID") %>"></a>
                                                            <asp:LinkButton ID="LNBtitolo" runat="server" CssClass="ForumNW_LinkButton" Visible="False"
                                                        CommandName="normale"></asp:LinkButton>
                                                        
                                                        <asp:Label ID="LBtitoloForum" runat="server" CssClass="Forum_TableTextBold"></asp:Label>
                                                        </span>
                                                        <span class="mark">
                                                    <asp:Label ID="LBvoto_t" runat="server" Visible="False" CssClass="ForumNW_VotaTestoValue"></asp:Label>
                                                    <asp:Label ID="LBvotoTesto" runat="server" Visible="False" CssClass="ForumNW_VotaTesto"></asp:Label>
                                                    <select id="SL_voto_" runat="server" class="ForumNW_VotaTestoDrop">
                                                        <option value="0">0</option>
                                                        <option value="1">1</option>
                                                        <option value="2">2</option>
                                                        <option value="3">3</option>
                                                        <option value="4">4</option>
                                                        <option value="5">5</option>
                                                    </select>
                                                    <asp:ImageButton ID="IMGvota" runat="server" ImageUrl="./../images/forum/ok2005.gif"
                                                        CommandName="Vota" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FRUM_ID") %>'
                                                        ImageAlign="AbsMiddle"></asp:ImageButton>
                                                        </span>
                                                    </div>
                                                    <div class="linewrapper description">
                                                    <asp:Label ID="LBdescrizione" runat="server" CssClass="Forum_TableText"></asp:Label>
                                                    </div>
                                                    <div class="linewrapper commands">
                                                    <asp:Label ID="LBinit" runat="server" Font-Size="11px" CssClass="separator start">[</asp:Label>
                                                    <asp:LinkButton ID="LNBArchivia" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FRUM_Id") %>' CausesValidation="False"
                                                        OnClick="LNBarchivia_Click">Archivia</asp:LinkButton>
                                                    <asp:Label ID="LBhasModeratori" runat="server" CssClass="separator">|</asp:Label>
                                                    <asp:LinkButton ID="LNBmoderatori" runat="server" CssClass="ForumNW_CommandPostLinkSmall">Visualizza Moderatori</asp:LinkButton>
                                                    <asp:Label ID="LBhasNotifica" runat="server" CssClass="separator">|</asp:Label>
                                                    <asp:LinkButton ID="LNBnotifica" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FRUM_Id") %>' CausesValidation="False"
                                                        OnClick="LNBnotifica_Click">Attiva notifica risposte via Email</asp:LinkButton>
                                                    <asp:Label ID="LBhasSegnala" runat="server" CssClass="separator">|</asp:Label>
                                                    <asp:LinkButton ID="LNBsegnala" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FRUM_Id") %>' CausesValidation="False">Segnala A..</asp:LinkButton>
                                                    <asp:Label ID="LBend" runat="server" CssClass="separator end">]</asp:Label>
                                                    </div>
                                                </td>
                                                <td class="topics">
                                                    <span class="ForumNW_TableCellSmall">
                                                        <%# DataBinder.Eval(Container.DataItem, "FRUM_NThread") %>
                                                    </span>
                                                </td>
                                                <td class="posts">
                                                    <span class="ForumNW_TableCellSmall">
                                                        <%# DataBinder.Eval(Container.DataItem, "FRUM_NPost") %></span>
                                                </td>
                                                <td class="lastpost ">
                                                    <div class="wrapper clearfix">
                                                    <div class="left">
                                                    <div class="linewrapper">
                                                    <asp:Label ID="LBlastPostData" runat="server" CssClass="ForumNW_TableCellSmall"><%# DataBinder.Eval(Container.DataItem, "DataUltimoPost") %></asp:Label>
                                                    </div>
                                                    <div class="linewrapper">
                                                    <asp:Label ID="LBlastPostPerson" runat="server" CssClass="ForumNW_TableCellSmall"><%# DataBinder.Eval(Container.DataItem, "AnagraficaLastPost") %></asp:Label>
                                                    </div>
                                                        </div>
                                                    <div class="right">
                                                    <asp:ImageButton ID="IMBgoToPost" ImageUrl="./../images/forum/right_arrow.gif" runat="server"
                                                        CommandName="vaiApost" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FRUM_LastPost_ID") %>'
                                                        CausesValidation="False" OnClick="IMBgoToPost_Click"></asp:ImageButton>
                                                    </div>
                                                    </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        
                                    </ItemTemplate>                                       
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="FRUM_Id" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FRUM_Name" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Iscritto" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="RuoloForum" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FRUM_Moderated" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="DataUltimoPost" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FRUM_LastPost_ID" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Abilitato" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FRUM_Archiviato" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="VotoMedio" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="HasVotato" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="posizione" Visible="False"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                
                        <asp:Label runat="server" CssClass="avviso" ID="LBavviso" Width="100%"></asp:Label>
            </div>
            
            <asp:Panel ID="PNLlegenda" runat="server" HorizontalAlign="Center">
                <div class="fieldobject">
                    <span class="fieldrow legend hor">        	            
        	            <span class="group first">
        	                <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/no_new_posts_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona4" runat="server">Open Forum [no new posts]</asp:Label>
        	                    </span>
        	                </span>
                            <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/new_posts_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona5" runat="server">Open Forum [new posts]</asp:Label>
        	                    </span>
        	                </span>
                        </span>
                        <span class="group">
                            <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/closed_topic_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona2" runat="server">Moderated Forum [no new replies]</asp:Label>
        	                    </span>
        	                </span>
                            <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/locked_new_posts_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona3" runat="server">Moderated Forum [new replies]</asp:Label>
        	                    </span>
        	                </span>
                        </span>
                        <span class="group last">
                            <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/forum_no_entry_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona1" runat="server">No Access</asp:Label>
        	                    </span>
        	                </span>
                        </span>
                            
        	             </span>
                       
                    
                </div>

                
            </asp:Panel>
            
            <asp:Panel ID="PNLstatistiche" runat="server">
                <div class="fieldobject stats">
                    <div class="fieldrow">
                        <label class="fieldlabel">
                            <asp:Label ID="LBLStatistiche" runat="server"></asp:Label>
                        </label>
                        <div class="inlinewrapper maxheight">
                            <asp:Repeater ID="RPTAccessiForum" runat="server">
                                    <ItemTemplate>
                                        <div class="statitem">
                                            <span class="name">
                                                <span class="firstname"><%#DataBinder.Eval(Container.DataItem, "PRSN_nome")%></span>
                                                <span class="lastname"><%#DataBinder.Eval(Container.DataItem, "PRSN_cognome")%></span>
                                            </span>
                                            <span class="stat">(<%#DataBinder.Eval(Container.DataItem, "PSTT_Totale")%>) &nbsp;</span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                        </div>
                    </div>
                </div>                
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>
