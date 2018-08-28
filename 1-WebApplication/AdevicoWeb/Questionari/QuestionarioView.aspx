<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="QuestionarioView.aspx.vb" Inherits="Comunita_OnLine.QuestionarioView" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link href="./../Styles.css?v=201604071200lm" type="text/css" rel="stylesheet" />
   <link href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link href="stileResponseCompile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
   <link media="screen" href="./../Graphics/Modules/Editor/ContentArea/EditorContent_LV.css?v=20180413Adv" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <div id="QuestionarioView">
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <asp:Panel ID="PNLmenu" runat="server" CssClass="panelMenu">
                <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"/>
                <asp:LinkButton ID="LNBGestioneDomande" Visible="false" runat="server" CssClass="Link_Menu"/>
                <asp:LinkButton ID="LNBCestino" Visible="false" runat="server" CssClass="Link_Menu"/>
                <asp:LinkButton ID="LNBImportaModello" Visible="true" runat="server" CssClass="Link_Menu">
                </asp:LinkButton>
            </asp:Panel>
            <asp:Panel ID="PNLElenco" runat="server">
                <br /> 
                <asp:Label ID="LBTitolo" runat="server" Text="" CssClass="RigaTitolo"></asp:Label>
                <br /><br /><br />
                <asp:DataList ID="DLPagine" runat="server" DataKeyField="id" 
                    CssClass="datalistPagine" RepeatLayout="Flow">
                    <ItemTemplate>
                        <div class="NomePagina">
                            <%#Eval("nomePagina")%>
                        </div>
                        <div class="TestoDomanda">
                            <%#Eval("descrizione")%>
                        </div>
                        <asp:DataList ID="DLDomande" runat="server" DataKeyField="id" 
                            OnItemDataBound="loadDomandeOpzioni" 
                            CssClass="datalistDomande" RepeatLayout="Flow">
                            <ItemTemplate>
                                <div class="ContenitoreDomanda0">
                                   <div class="TestoDomanda">
                                        <div class='<%# me.displayDifficulty %> difficultyInfo'>
                                            (Cod.<%#Eval("id")%>
                                            <asp:Label runat="server" ID="LBDifficoltaTesto" Text='<%#Eval("difficoltaTesto")%>'></asp:Label>)
                                        </div>
                                        <div class="question-number">
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("numero") & "."%>'></asp:Label>
                                            <%--Visible='<%# me.showDifficulty %>'--%>
                                        </div>
                                        <div class="question-name"><span class="name"><%#me.SmartTagsAvailable.TagAll(Eval("testo"))%></div>
                                    </div>
                                
                                    <div class="Risposte">
                                        <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                    </div>
                                    <div class="suggestion">
                                        <asp:Label runat="server" ID="LBSuggerimento" Text='<%#Eval("suggerimento")%>' Visible="false" CssClass="global"></asp:Label>
                                    </div>
                                </div>
                            </ItemTemplate>
                           <FooterStyle CssClass="footer"/>
                            <SelectedItemStyle CssClass="item-question Selected"/>
                            <AlternatingItemStyle CssClass="item-question Alternate"/>
                            <ItemStyle CssClass="item-question"/>
                            <HeaderStyle CssClass="header"/>
                        </asp:DataList>
                        <div class="NomePaginaFooter">
                            <%#Eval("nomePagina")%>
                        </div>
                    </ItemTemplate>
                    <FooterStyle CssClass="footer"/>
                    <SelectedItemStyle CssClass="item-page Selected"/>
                    <AlternatingItemStyle CssClass="item-page alternate" />
                    <ItemStyle CssClass="item-page"/>
                    <HeaderStyle CssClass="header"/>
                </asp:DataList>
            </asp:Panel>
            <div class="numeriPagina">
                <asp:Label ID="LBpagina" runat="server"></asp:Label>
                <%--<asp:ImageButton ID="IMBprima" ImageUrl="img/indietro.gif" runat="server" Visible="False" CssClass="button back" ></asp:ImageButton>--%>
                <asp:LinkButton ID="LkbBack" runat="server" CssClass="button prev" Visible="false"></asp:LinkButton>
                <asp:PlaceHolder ID="PHnumeroPagina" runat="server"></asp:PlaceHolder>
                <asp:LinkButton ID="LkbNext" runat="server" CssClass="button next" Visible="false"></asp:LinkButton>
                <%--<asp:ImageButton ID="IMBdopo" runat="server" ImageUrl="img/avanti.gif" Visible="False" CssClass="button next"></asp:ImageButton>--%>
            </div>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server" CssClass="errore"></asp:Label>
        </asp:View>
    </asp:MultiView>
    </div>
</asp:Content>
