<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ADM_forums.aspx.vb" Inherits="Comunita_OnLine.ADM_forums" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%--OLD radtabstrip TELERIK--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLgestione" Src="UC/UC_GestioneiscrittiForum.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLaggiungi" Src="UC/UC_AggiungiUtenteForum.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <link rel="stylesheet" href="../Graphics/Modules/Forum/forum.new.css" />
    <%--<LINK href="./../Styles.css" type="text/css" rel="stylesheet"/>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="topmenu">
        <asp:Panel ID="PNLmenuIniziale" runat="server" HorizontalAlign="Right">
            <asp:LinkButton ID="LKBindietro" runat="server" CausesValidation="False" Visible="False"
                CssClass="LINK_MENU">Torna a Lista Forum</asp:LinkButton>
            <asp:LinkButton ID="LKBcrea" runat="server" Visible="False" CssClass="LINK_MENU">Aggiungi nuovo Forum</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="PNLmenuInserimento" runat="server" Visible="False" HorizontalAlign="Right">
            <asp:LinkButton ID="LNBindietroFromInserimento" runat="server" CausesValidation="False"
                CssClass="LINK_MENU">Torna a Lista Forum</asp:LinkButton>
            <asp:LinkButton ID="LNBsalva" runat="server" CssClass="LINK_MENU">Crea Forum</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="PNLmenuUtenti" runat="server" HorizontalAlign="Right" Visible="false">
            <asp:LinkButton ID="LNBannullaRuolo" runat="server" CausesValidation="False" CssClass="LINK_MENU"></asp:LinkButton>
            <asp:LinkButton ID="LNBmodificaRuolo" runat="server" CssClass="LINK_MENU">Modifica Ruolo</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="PNLmenuAggiungiUtenti" runat="server" HorizontalAlign="Right" Visible="false">
            <asp:LinkButton ID="LNBindietroToLista" runat="server" CausesValidation="False" CssClass="LINK_MENU">Torna a Lista Forum</asp:LinkButton>
            <asp:LinkButton ID="LNBaggiungiUtente" runat="server" CssClass="LINK_MENU">Aggiungi utente</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="PNLmenuInserimentoUtenti" runat="server" HorizontalAlign="Right" Visible="false">
            <asp:LinkButton ID="LNBindietroToAggiungi" runat="server" CausesValidation="False"
                CssClass="LINK_MENU">Torna a Lista Forum</asp:LinkButton>
            <asp:LinkButton ID="LNBassociaUtenti" runat="server" CssClass="LINK_MENU">Aggiungi utente</asp:LinkButton>
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
            <asp:Panel ID="PNLforums" runat="server" HorizontalAlign="Left">
                <br />
                <asp:Label ID="LBselezioneForum_t" runat="server" CssClass="FiltroCampoSmall"></asp:Label>
                &nbsp;
                <asp:RadioButtonList ID="RBLselezioneForum" CssClass="FiltroCampoSmall" runat="server"
                    RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="True">
                    <asp:ListItem Value="-1">Tutti i forum</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">Forum attivi</asp:ListItem>
                    <asp:ListItem Value="1">Forum archiviati</asp:ListItem>
                </asp:RadioButtonList>
                <asp:DataGrid ID="DGforums" runat="server" DataKeyField="FRUM_id" AllowPaging="true" UseAccessibleHeader="true"
                    AutoGenerateColumns="False" AllowSorting="true" ShowFooter="false" ShowHeader="true" PageSize="15"
                    CssClass="table light fullwidth">
                    <ItemStyle CssClass="ForumNW_RowNormal"></ItemStyle>
                    <AlternatingItemStyle CssClass="ForumNW_RowAlternato"></AlternatingItemStyle>
                    <HeaderStyle CssClass=""></HeaderStyle>
                    <PagerStyle CssClass="Riga_Paginazione" Position="Bottom" Mode="NumericPages" Visible="true"
                        HorizontalAlign="Right" Height="20px" VerticalAlign="Bottom"></PagerStyle>
                    <Columns>
                        <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:ImageButton ID="IMBmodifica" runat="server" CausesValidation="False" CommandName="modifica"
                                    ImageUrl="../images/m.gif"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn runat="server" HeaderText="" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:ImageButton ID="IMBCancella" runat="server" CausesValidation="False" CommandName="elimina"
                                    ImageUrl="../images/x.gif"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Forum nome" SortExpression="FRUM_name">
                            <ItemTemplate>
                                <div class="linewrapper">
                                    <a name="forum_<%# DataBinder.Eval(Container.DataItem, "FRUM_ID") %>"></a>
                                            <%# DataBinder.Eval(Container.DataItem, "FRUM_name") %>
                                </div>
                                <div class="linewrapper commands">
                                    <asp:Label ID="LBinit" runat="server" CssClass="separator start">[</asp:Label>
                                            <asp:LinkButton ID="LNBentra" runat="server" CommandName="Entra" CssClass="ForumNW_CommandPostLinkSmall">Entra</asp:LinkButton>
                                            <asp:Label ID="LBseparatore1" runat="server" CssClass="separator">|</asp:Label>
                                            <asp:LinkButton ID="LNBnascondi" runat="server" CommandName="Hide" CssClass="ForumNW_CommandPostLinkSmall">Hide</asp:LinkButton>
                                            <asp:Label ID="LBseparatore2" runat="server" CssClass="separator">|</asp:Label>
                                            <asp:LinkButton ID="LNBarchivia" runat="server" CommandName="Archivia" CssClass="ForumNW_CommandPostLinkSmall" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FRUM_Id") %>' CausesValidation="False"
                                                        OnClick="LNBarchivia_Click">Archivia</asp:LinkButton>
                                            <asp:Label ID="LBseparatore3" runat="server" CssClass="separator">|</asp:Label>
                                            <asp:LinkButton ID="LNBiscritti" runat="server" CommandName="GestioneIscritti" CssClass="ForumNW_CommandPostLinkSmall">Gestione Iscritti</asp:LinkButton>
                                            <asp:Label ID="LBend" runat="server" CssClass="separator end">]</asp:Label>
                                </div>                                
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FRUM_DateCreated" HeaderText="Creato il" ItemStyle-Width="150px"
                            SortExpression="FRUM_DateCreated" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FRUM_NThread" HeaderText="Topics" ItemStyle-Width="50px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ForumNW_RowHeader">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FRUM_NPost" HeaderText="Posts" ItemStyle-Width="50px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ForumNW_RowHeader">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TotIscritti" HeaderText="Iscritti" ItemStyle-Width="70px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="ForumNW_RowHeader">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="RuoloForum" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FRUM_Hide" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FRUM_Archiviato" Visible="False"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
                <br />
                <asp:Label ID="LBnoforum" runat="server" CssClass="Errore"></asp:Label>
                <br />
                <br />
            </asp:Panel>
            <asp:Panel ID="PNLcreaModifica" runat="server" Visible="False" Width="900px">
                <input id="HDfrum_id" type="hidden" runat="server" name="HDfrum_id" />
                <div class="fieldobject addforum">
                    <div class="fieldrow name">
                        <asp:Label ID="LBnomeforum_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBnomeForum">*Nome Forum:</asp:Label>
                        <asp:TextBox ID="TXBnomeForum" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                            MaxLength="50" Columns="40"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" CssClass="Validatori"
                                            ControlToValidate="TXBnomeForum" Display="static" EnableClientScript="true">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="fieldrow description">
                        <asp:Label ID="LBdescrizione_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBDescrizione">*Descrizione:</asp:Label>
                        <asp:TextBox ID="TXBDescrizione" runat="server" CssClass="Testo_campo_obbligatorioSmall"
                                            TextMode="MultiLine" Rows="7" Columns="52" MaxLength="150"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="Requiredfieldvalidator" runat="server" CssClass="Validatori"
                                            ControlToValidate="TXBDescrizione" Display="static" EnableClientScript="true">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="fieldrow role">
                        <asp:Label ID="LBruoloDefault_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLruoloDefault">*Ruolo Default:</asp:Label>
                        <asp:DropDownList ID="DDLruoloDefault" runat="server" CssClass="Testo_CampoSmall">
                                        </asp:DropDownList>
                    </div>
                    <div class="fieldrow moderated">
                        <asp:Label ID="LBforumModerato_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CHBmoderato">&#160; Forum Moderato:</asp:Label>
                        <asp:CheckBox ID="CHBmoderato" runat="server" Checked="false" Text="Si" CssClass="Testo_CampoSmall"/>
                    </div>
                </div>
                
            </asp:Panel>
            <asp:Panel ID="PNLgestioneIscritti" runat="server" Visible="False">
                <telerik:radtabstrip id="TBSmenu" runat="server" align="Justify" Width="350px" Height="26px" SelectedIndex="0"
                              causesvalidation="false" autopostback="true" skin="Outlook" enableembeddedskins="true">
                                <tabs>
                                    <telerik:RadTab text="Gestione Iscritti" value="TABgestioneIscritti" runat="server"></telerik:RadTab>
                                    <telerik:RadTab text="Aggiungi Utenti" value="TABaggiungiUtente" runat="server" ></telerik:RadTab>
                                </tabs>
                            </telerik:radtabstrip>
                <CTRL:CTRLgestione ID="CTRLgestione" runat="server"></CTRL:CTRLgestione>
                <CTRL:CTRLaggiungi ID="CTRLaggiungi" runat="server" Visible="false"></CTRL:CTRLaggiungi>                
            </asp:Panel>
            <asp:Panel ID="PNLavviso" runat="server" Visible="False">
                <table align="center">
                    <tr>
                        <td align="center">
                            <asp:Label ID="LBavviso" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="BTNok" runat="server" CssClass="pulsante" Text="Ok"></asp:Button>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>
