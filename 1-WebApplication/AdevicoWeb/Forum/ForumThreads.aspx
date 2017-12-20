<%@ Page Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ForumThreads.aspx.vb" Inherits="Comunita_OnLine.ForumThreads" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--
--%>
<%@ Register TagPrefix="CTRL" TagName="CTRLaggiungiThread" Src="./UC/UC_AggiungiThread.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../Graphics/Modules/Forum/forum.new.css" />
    <%--<link href="./../Styles.css" type="text/css" rel="stylesheet"/>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="topmenu">
        <asp:Panel ID="PNLmenu" CssClass="pnlmenu"  runat="server" HorizontalAlign="Right">
            <asp:Label ID="LBselezioneForum_t" runat="server" CssClass="FiltroCampoSmall"></asp:Label>&nbsp;&nbsp;
            <asp:RadioButtonList ID="RBLselezioneTopic" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                AutoPostBack="True" CssClass="FiltroCampoSmall">
                <asp:ListItem Value="False" Selected="True">Topics attivi</asp:ListItem>
                <asp:ListItem Value="True">Topics archiviati</asp:ListItem>
            </asp:RadioButtonList>
            &nbsp;
            <asp:LinkButton ID="LNBnewThread" runat="server" CausesValidation="False" CssClass="Link_Menu">New Topic</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="PNLmenuInserimento" runat="server" HorizontalAlign="Right" Visible="False">
            <asp:LinkButton ID="LNBannullaInserimento" runat="server" CausesValidation="False"
                CssClass="Link_Menu">Undo insert</asp:LinkButton>
            <asp:LinkButton ID="LNBrispondi" runat="server" CausesValidation="true" CssClass="Link_Menu">Insert</asp:LinkButton>
        </asp:Panel>
    </div>
    <div class="forumcontent">
        <asp:Panel ID="PNLpermessi" runat="server" Visible="False">
            <table align="center">
                <div class="messages">
                <div class="message error">
                    <span class="icons"><span class="icon">&nbsp;</span></span><asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>    
            </table>
        </asp:Panel>
        <asp:Panel ID="PNLcontenuto" runat="server" HorizontalAlign="Center">
            
            
            <asp:Table ID="TBLprincipale" HorizontalAlign="Center" Width="100%" runat="server">
                
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Panel ID="PNLthread" runat="server">
                            <div class="fieldobject toolbar top clearfix" ID="TBRmovimento0" runat="server">
                <div class="fieldrow left">
                    <asp:LinkButton ID="LNBforum" runat="server" CssClass="ForumNW_History" CausesValidation="False">Forum</asp:LinkButton>
                </div>
                <div class="fieldrow right">
                    <asp:Label ID="LBjumpToforum" runat="server" AssociatedControlID="DDLforum" CssClass="ForumNW_History_text">Jump to:&nbsp;</asp:Label>
                    <asp:DropDownList ID="DDLforum" runat="server" AutoPostBack="True" CssClass="ForumNW_History_dropForum">
                    </asp:DropDownList>
                </div>
            </div>
                            <div class="tablewrapper">

                            <asp:DataGrid ID="DGthread" runat="server" DataKeyField="THRD_ID" CellPadding="0"
                                CellSpacing="0" HorizontalAlign="Center" AutoGenerateColumns="False" ShowFooter="true"
                                CssClass="datagridforum">
                                <ItemStyle CssClass="ForumNW_RowNormal"></ItemStyle>
                                <AlternatingItemStyle CssClass="ForumNW_RowAlternato"></AlternatingItemStyle>
                                <Columns>
                                    <asp:TemplateColumn>
                                        <HeaderTemplate>
                                            <table class="table light fullwidth forumtable threads">
                                                <tr class="tableheader">
                                                    <th class="indicators">
                                                        &nbsp;
                                                    </th>
                                                    <th class="topic">
                                                        <asp:Label ID="LBthreads_t" runat="server">Topics</asp:Label>
                                                    </th>
                                                    <th class="author">
                                                        <asp:Label ID="LBauthor_t" runat="server">Topic Starter</asp:Label>
                                                    </th>
                                                    <th class="replies">
                                                        <asp:Label ID="LBreplies_t" runat="server">Replies</asp:Label>
                                                    </th>
                                                    <th class="views">
                                                        <asp:Label ID="LBviews_t" runat="server">Views</asp:Label>
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
                                                    <td class="topic">
                                                        <div class="linewrapper title clearfix">
                                                            <span class="name">
                                                        <a name="topic_<%# DataBinder.Eval(Container.DataItem, "THRD_ID") %>"></a>
                                                        <asp:LinkButton ID="LNBtitolo" runat="server" CssClass="ForumNW_LinkButton" Visible="False"
                                                            CommandName="entra"></asp:LinkButton>
                                                        <asp:Label ID="LBtitoloTopic" runat="server" CssClass="Forum_TableTextBold"></asp:Label>
                                                            </span>
                                                            <span class="mark">
                                                        <asp:Label ID="LBvoto_t" runat="server" Visible="False" CssClass="ForumNW_VotaTestoValue"></asp:Label>
                                                        <asp:Label ID="LBvotoTesto" runat="server" Visible="False" CssClass="ForumNW_VotaTesto"></asp:Label>
                                                        <select id="SL_voto_" runat="server" class="ForumNW_VotaTestoDrop" name="SL_voto_">
                                                            <option value="0">0</option>
                                                            <option value="1">1</option>
                                                            <option value="2">2</option>
                                                            <option value="3">3</option>
                                                            <option value="4">4</option>
                                                            <option value="5">5</option>
                                                        </select>
                                                        <asp:ImageButton ID="IMGvota" runat="server" ImageUrl="./../images/forum/ok2005.gif"
                                                            CommandName="Vota" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "THRD_ID") %>'
                                                            ImageAlign="AbsMiddle"></asp:ImageButton>
                                                        </div>
                                                        </span>
                                                        <div class="linewrapper commands">
                                                        <asp:Panel ID="PNLmenuTopic" HorizontalAlign="Left" runat="server" Width="100%">
                                                            <div class="linewrapper commands">
                                                            <span id="TBRcomandi0" runat="server">
                                                                <asp:Label ID="LBinit" runat="server" CssClass="separator start">[</asp:Label>
                                                                        <asp:LinkButton ID="LNBeliminaThread" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "THRD_Id") %>' CausesValidation="False"
                                                                            OnClick="LNBeliminaThread_Click">Elimina</asp:LinkButton>
                                                                        <asp:Label ID="LBhasArchivia" runat="server" CssClass="separator">|</asp:Label>
                                                                        <asp:LinkButton ID="LNBArchivia" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "THRD_Id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "THRD_Archiviato") %>'
                                                                            CausesValidation="False" OnClick="LNBarchivia_Click">Archivia</asp:LinkButton>
                                                                        <asp:Label ID="LBhasCensura" runat="server" CssClass="separator">|</asp:Label>
                                                                        <asp:LinkButton ID="LNBcensura" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "THRD_Id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "THRD_Hide") %>'
                                                                            CausesValidation="False" OnClick="LNBcensura_Click">Censura/Nascondi</asp:LinkButton>
                                                                        <asp:Label ID="LBend" runat="server" CssClass="separator end">]</asp:Label>
                                                            </span>
                                                            <span id="TBRcomandi1" runat="server">
                                                                <asp:Label ID="LBinit_1" runat="server" CssClass="separator start">[</asp:Label>
                                                                        <asp:Label ID="LBhasNotifica" runat="server" CssClass="separator">|</asp:Label>
                                                                        <asp:LinkButton ID="LNBnotifica" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "THRD_Id") %>' CausesValidation="False"
                                                                            OnClick="LNBnotifica_Click">Attiva notifica risposte via Email</asp:LinkButton>
                                                                        <asp:Label ID="LBhasSegnala" runat="server" CssClass="separator">|</asp:Label>
                                                                        <asp:LinkButton ID="LNBsegnala" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "THRD_Id") %>' CausesValidation="False">Segnala A..</asp:LinkButton>
                                                                        <asp:Label ID="LBend_1" runat="server"  CssClass="separator end">]</asp:Label>
                                                            </span>
                                                            </div>
                                                        </asp:Panel>
                                                        </div>
                                                    </td>
                                                    <td class="author">
                                                        <asp:Label ID="LBlogin" runat="server" CssClass="ForumNW_TableCellSmall"><%# DataBinder.Eval(Container.DataItem, "PRSN_Anagrafica") %></asp:Label>
                                                    </td>
                                                    <td class="replies">
                                                        <asp:Label ID="LBnReply" runat="server">
														<span class="ForumNW_TableCellSmall"><%# DataBinder.Eval(Container.DataItem, "THRD_NReply") %></span>
                                                        </asp:Label>
                                                    </td>
                                                    <td class="views">
                                                        <asp:Label ID="LBnView" runat="server">
														<span class="ForumNW_TableCellSmall"><%# DataBinder.Eval(Container.DataItem, "THRD_NView") %></span>
                                                        </asp:Label>
                                                    </td>
                                                    <td class="lastpost">
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
                                                            CommandName='<%# DataBinder.Eval(Container.DataItem, "THRD_ID") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "THRD_LastPost_ID") %>'
                                                            CausesValidation="False" OnClick="IMBgoToPost_Click"></asp:ImageButton>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="oArchiviazione" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="THRD_Archiviato" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="THRD_Hide" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="THRD_LastPost_ID" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="THRD_Subject" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="THRD_ID" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="VotoMedio" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="HasVotato" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="posizione" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="isWaiting" Visible="False"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                            </div>
                            <div class="fieldobject toolbar bottom clearfix" ID="TBRmovimento1" runat="server">
                <div class="fieldrow left">
                    <asp:LinkButton ID="LNBforum_bottom" runat="server" CssClass="ForumNW_History" OnClick="LNBforum_Click"
                                        CausesValidation="False">Forum</asp:LinkButton>
                </div>
                <div class="fieldrow right">
                    <asp:Label ID="LBjumpToforum_bottom" runat="server" AssociatedControlID="DDLforum_bottom" CssClass="ForumNW_History_text">Jump to:&nbsp;</asp:Label>
                                    <asp:DropDownList ID="DDLforum_bottom" runat="server" AutoPostBack="True" CssClass="ForumNW_History_dropForum"
                                        OnSelectedIndexChanged="DDLforum_SelectedIndexChanged">
                                    </asp:DropDownList>
                </div>
            </div>
                        </asp:Panel>
                        <asp:Panel ID="PNLnewThread" runat="server" Visible="False" HorizontalAlign="Center">
                            <asp:Table ID="Table2" HorizontalAlign="Center" runat="server" Width="900px">
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                        <CTRL:CTRLaggiungiThread ID="CTRLaggiungiThread" runat="server"></CTRL:CTRLaggiungiThread>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2">
										&nbsp;
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>                        
                        <asp:Label runat="server" CssClass="avviso" ID="LBavviso" Width="100%"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                
                <asp:TableRow ID="TBRlegenda">
                    <asp:TableCell>
                        
                        <asp:Panel ID="PNLlegenda" runat="server" HorizontalAlign="Center">

                            <div class="fieldobject">
                    <span class="fieldrow legend hor">        	            
        	            <span class="group first">
        	                <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/no_new_posts_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona2" runat="server">Topic [no new posts]</asp:Label>
        	                    </span>
        	                </span>
                            <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/new_posts_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona3" runat="server">Topic [new posts]</asp:Label>
        	                    </span>
        	                </span>
                        </span>
                        <span class="group last">
                            <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/priority_post_locked_icon.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona1" runat="server">Censored</asp:Label>
        	                    </span>
        	                </span>
                            <span class="legenditem">
        	                    <span class="legendicon img">
                                    <img src="./../images/forum/waitingPost.gif" alt="" border="0" />
        	                    </span>
        	                    <span class="legendtext">
                                    <asp:Label ID="LBicona4" runat="server">Waiting for confirm</asp:Label>
        	                    </span>
        	                </span>
                        </span>
                        
                            
        	             </span>                       
                    
                </div>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <asp:Panel ID="PNLstatistiche" runat="server" HorizontalAlign="Center">
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
