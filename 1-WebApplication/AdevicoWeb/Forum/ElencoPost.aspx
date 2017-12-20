<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ElencoPost.aspx.vb" Inherits="Comunita_OnLine.ElencoPost" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaggiungiPost" Src="UC/UC_AggiungiPost.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="../Graphics/Modules/Forum/forum.new.css" />
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="topmenu">
        <asp:Panel ID="PNLmenu" runat="server" HorizontalAlign="left">
            <asp:CheckBox runat="server" ID="CHBnotificaMail" Text="Notifica le risposte a questi thread via e-mail"
                AutoPostBack="true" CssClass="FiltroCampoSmall"></asp:CheckBox>
            &nbsp;
            <asp:RadioButtonList ID="RBLordinamentoPost" RepeatDirection="Horizontal" CssClass="FiltroCampoSmall"
                RepeatLayout="Flow" runat="server" AutoPostBack="True">
                <asp:ListItem Value="0" Selected="True">Dal primo all'ultimo</asp:ListItem>
                <asp:ListItem Value="1">Dall'ultimo post al primo</asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel>
        <asp:Panel ID="PNLmenuInserimento" runat="server" Visible="False" HorizontalAlign="right">
            <asp:LinkButton ID="LNBannullaInserimento" runat="server" CausesValidation="False"
                CssClass="Link_Menu">Undo insert</asp:LinkButton>
            <asp:LinkButton ID="LNBrispondi" runat="server" CausesValidation="true" CssClass="Link_Menu">Insert</asp:LinkButton>
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
            <div class="fieldobject toolbar top clearfix" ID="TBRmovimento0" runat="server">
                <div class="fieldrow left">
                    <asp:LinkButton ID="LNBforum" runat="server" CssClass="ForumNW_History" CausesValidation="False">Forum</asp:LinkButton>
                                &nbsp;<img src="./../images/freccia_dx.gif" border="0" alt="" />
                                <asp:LinkButton ID="LNBtopics" runat="server" CssClass="ForumNW_History" CausesValidation="False">Topic</asp:LinkButton>
                </div>
                <div class="fieldrow right">
                    <asp:Label ID="LBjumpToforum" runat="server" CssClass="ForumNW_History_text" AssociatedControlID="DDLforum">Jump to:&nbsp;</asp:Label>
                    <asp:DropDownList ID="DDLforum" runat="server" AutoPostBack="True" CssClass="ForumNW_History_dropForum">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="tablewrapper">
                            <asp:DataGrid ID="DGPost" runat="server" DataKeyField="POST_Id" AutoGenerateColumns="False"
                                        CellPadding="0" CellSpacing="0" BorderStyle="None" GridLines="None" CssClass="datagridforum" ShowFooter="true">
                                        <ItemStyle CssClass="ForumNW_RowNormal"></ItemStyle>
                                        <AlternatingItemStyle CssClass="ForumNW_RowAlternato"></AlternatingItemStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderTemplate>
                                                    <table class="table light fullwidth forumtable posts">
                                                        <tr class="tableheader">
                                                            <th class="author">
                                                                <asp:Label ID="LBautore_t" runat="server">Author</asp:Label>
                                                            </th>
                                                            <th class="message">
                                                                <asp:Label ID="LBmessaggio_t" runat="server">Message</asp:Label>
                                                            </th>
                                                        </tr>
                                                    
                                                </HeaderTemplate>
                                                <ItemTemplate>                                                    
                                                        <tr class="tablerow <%# DataBinder.Eval(Container.DataItem, "stile") %>">
                                                            <td class="author <%# DataBinder.Eval(Container.DataItem, "stile2") %>">
                                                                <div class="authorcontent">
                                                                    <div class="linewrapper name">
                                                                        <asp:Label ID="LBautore" runat="server" CssClass="ForumNW_Autore"><%# DataBinder.Eval(Container.DataItem, "PRSN_Anagrafica") %></asp:Label>
                                                                    </div>
                                                                    <div class="linewrapper role">
                                                                        <asp:Label ID="LBruolo" runat="server" CssClass="ForumNW_Ruolo"><%# DataBinder.Eval(Container.DataItem, "TPRF_nome") %></asp:Label>
                                                                    </div>
                                                                    <div class="linewrapper image">
                                                                        <asp:Image ID="IMGprofilo" runat="server" Width="80px" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "PRSN_FotoPath") %>'>
                                                                            </asp:Image>
                                                                    </div>
                                                                    <div class="linewrapper stats">
                                                                        <asp:Label ID="LBtotalePost_t" runat="server" CssClass="ForumNW_Ruolo">Post inseriti:</asp:Label>&nbsp;
                                                                            <span class="ForumNW_Ruolo">
                                                                                <%# DataBinder.Eval(Container.DataItem, "TotalePost") %></span>
                                                                    </div>
                                                                </div>
                                                                
                                                            </td>
                                                            <td class="message <%# DataBinder.Eval(Container.DataItem, "stile2") %>">
                                                                <div class="messagecontent clearfix">
                                                                    <div class="header">
                                                                        <div class="linewrapper title clearfix">
                                                                            <span class="name">
                                                                                <span class="title">
                                                                        <asp:Image ID="IMGmessaggio" runat="server" ImageAlign="Middle" BorderStyle="None">
                                                                            </asp:Image>
                                                                            <a name="post_<%# DataBinder.Eval(Container.DataItem, "POST_ID") %>">
                                                                                <asp:Label ID="LBtitoloMessaggio" runat="server" CssClass="ForumNW_SubjectPost"><%# DataBinder.Eval(Container.DataItem, "POST_Subject") %></asp:Label>
                                                                            </a>
                                                                                    </span>
                                                                                <span class="separator">|</span>
                                                                                <span class="details">
                                                                            <asp:Label ID="LBdataPost" runat="server" CssClass="ForumNW_datapost"></asp:Label>
                                                                            <asp:Label ID="LBdataPostModificato" runat="server" CssClass="ForumNW_datapost"></asp:Label>
                                                                            <asp:HyperLink ID="HYLparent" runat="server" CssClass="ForumNW_CommandPostLinkSmall">(vai al post padre)</asp:HyperLink>
                                                                            &nbsp;<asp:Label ID="LBapprovato" runat="server"></asp:Label>
                                                                                </span>
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
                                                                                CommandName="Vota" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "POST_ID") %>'
                                                                                ImageAlign="AbsMiddle"></asp:ImageButton>
                                                                        </span>
                                                                        </div>
                                                                        
                                                                    </div>
                                                                    <div class="body">
                                                                        <div class="post">
                                                                            <asp:Label ID="LBmessaggio" runat="server" CssClass="Testo_campoSmall renderedtext"><%# DataBinder.Eval(Container.DataItem, "POST_Body") %></asp:Label>                                                                        
                                                                        </div>
                                                                    </div>
                                                                    <div class="footer">

                                                                    </div>
                                                                </div>
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr class="tablerow jumper">
                                                            <td class="author <%# DataBinder.Eval(Container.DataItem, "stile") %>">
                                                                <div class="linewrapper commands">
                                                                <a href="#top" class="ForumNW_BackToTop" onmouseover="window.status='';return true;"
                                                                    onmouseout="window.status='';return true;" onclick="window.status='';return true;"
                                                                    onfocus="window.status='';return true;">Back To Top</a>
                                                                </div>
                                                            </td>
                                                            <td class="message <%# DataBinder.Eval(Container.DataItem, "stile") %>">
                                                                <div class="linewrapper commands">
                                                                <asp:LinkButton ID="LNBreply" CssClass="ForumNW_CommandPostLinkSmall" runat="server"
                                                                    CausesValidation="False" CommandName="reply">Reply</asp:LinkButton>
                                                                <asp:Label ID="LBreplyQuote" runat="server" CssClass="separator">|</asp:Label>
                                                                <asp:LinkButton ID="LNBreplyQuote" CssClass="ForumNW_CommandPostLinkSmall" runat="server"
                                                                    CausesValidation="False" CommandName="replyQuota">Reply e quota</asp:LinkButton>
                                                                <asp:Label ID="LBmodifica" runat="server" CssClass="separator">|</asp:Label>
                                                                <asp:LinkButton ID="LNBmodifica" CssClass="ForumNW_CommandPostLinkSmall" runat="server"
                                                                    CausesValidation="False" CommandName="modifica">Modifica</asp:LinkButton>
                                                                <asp:Label ID="LBhasSegnala" runat="server" CssClass="separator">|</asp:Label>
                                                                <asp:LinkButton ID="LNBsegnala" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "POST_ID") %>' CausesValidation="False">Segnala A..</asp:LinkButton>
                                                                <asp:Label ID="LBcensura" runat="server" CssClass="separator">|</asp:Label>
                                                                <asp:LinkButton ID="LNBcensura" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                    CausesValidation="False" CommandName="censura">Censura</asp:LinkButton>
                                                                <asp:Label ID="LBaccetta" runat="server" CssClass="separator">|</asp:Label>
                                                                <asp:LinkButton ID="LNBaccetta" runat="server" CssClass="ForumNW_CommandPostLinkSmall"
                                                                    CausesValidation="False" CommandName="accetta">Accetta</asp:LinkButton>
                                                                <asp:Label ID="LBcancellaPost" runat="server" CssClass="separator">|</asp:Label>
                                                                <asp:ImageButton ID="IMBcancellaPost" runat="server" CausesValidation="False" OnClick="IMBcancellaPost_Click"
                                                                    ImageUrl="../images/x.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "POST_ID") %>'
                                                                    ImageAlign="AbsMiddle"></asp:ImageButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="tablerow separator">
                                                            <td colspan="2" class="ForumNW_separatore">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="POST_Approved" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="stile" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="stile2" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="TPRF_nome" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="TotalePost" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="POST_Approved" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Post_parentId" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="POST_IdRuolo" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="POST_PRSN_Id" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="POST_Id" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="POST_Body" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="HasReplies" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="HasVotato" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="VotoMedio" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="FRIM_id" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="posizione" Visible="False"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                        </div>

            <div class="fieldobject toolbar bottom clearfix" ID="TBRmovimento1" runat="server">
                <div class="fieldrow left">
                    <asp:LinkButton ID="LNBforum_bottom" runat="server" CssClass="ForumNW_History" OnClick="LNBforum_Click"
                                        CausesValidation="False">Forum</asp:LinkButton>
                                    &nbsp;<img src="./../images/freccia_dx.gif" border="0" alt="" />
                                    <asp:LinkButton ID="LNBtopics_bottom" runat="server" CssClass="ForumNW_History" CausesValidation="False"
                                        OnClick="LNBtopics_Click">Topic</asp:LinkButton>
                </div>
                <div class="fieldrow right">
                    <asp:Label ID="LBjumpToforum_bottom" runat="server" CssClass="ForumNW_History_text">Jump to:&nbsp;</asp:Label>
                                    <asp:DropDownList ID="DDLforum_bottom" runat="server" AutoPostBack="True" CssClass="ForumNW_History_dropForum"
                                        OnSelectedIndexChanged="DDLforum_SelectedIndexChanged">
                                    </asp:DropDownList>
                </div>
            </div>
           

        </asp:Panel>
        <asp:Panel ID="PNLnewMessage" runat="server" HorizontalAlign="Center" Visible="False">
            <asp:Table ID="Table1" runat="server" HorizontalAlign="Center" Width="900px">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2" HorizontalAlign="center">
                        <CTRL:CTRLaggiungiPost ID="CTRLaggiungiPost" runat="server"></CTRL:CTRLaggiungiPost>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
						&nbsp;
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>
