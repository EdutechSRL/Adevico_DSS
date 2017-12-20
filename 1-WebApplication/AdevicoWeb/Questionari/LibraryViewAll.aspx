<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="LibraryViewAll.aspx.vb" Inherits="Comunita_OnLine.LibraryViewAll" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Reference Control="UserControls/ucDomandaMultiplaEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:Panel ID="PNLmenu" runat="server" Width="100%" HorizontalAlign="right">
        <asp:LinkButton ID="LNBCartellaPrincipale" Visible="true" runat="server" CssClass="Link_Menu"
            CausesValidation="false"></asp:LinkButton>&nbsp;
        <asp:LinkButton ID="LNBGestioneQuestionario" Visible="true" runat="server" CssClass="Link_Menu"
            CausesValidation="false"></asp:LinkButton>
    </asp:Panel>
    <br />
    <br />
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <asp:Panel ID="PNLElenco" runat="server">
                <asp:Label ID="LBTitolo" runat="server" Text="" class="NomePagina"></asp:Label>
                <br />
                <asp:Label ID="LBMessage" runat="server" ForeColor="red"></asp:Label>
                <br />
                <asp:DataList ID="DLPagine" runat="server" DataKeyField="id" CellPadding="4" ForeColor="#333333"
                    Width="100%" >
                    <ItemTemplate>
                        <b>
                            <%#Eval("nomePagina")%>
                        </b>
                        <asp:Literal ID="LTpageNumber" runat="server" Text='<%#Container.DataItem.NumeroPagina %>' Visible="false"></asp:Literal>
                        <br />
                        <%#Eval("descrizione")%>
                        <br />
                      
                        <asp:RadioButtonList runat="server" ID="RBLfiltraDomande" AutoPostBack="true" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="FiltraDomande">
                            <asp:ListItem Value="1"></asp:ListItem>
                            <asp:ListItem Value="2"></asp:ListItem>
                            <asp:ListItem Value="3"></asp:ListItem>
                            <asp:ListItem Value="4"></asp:ListItem>
                            <asp:ListItem Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                        <hr />
                        <asp:DataList ID="DLDomande" runat="server" OnItemDataBound="loadDomandeOpzioni"
                            Width="100%" DataKeyField="id">
                            <ItemTemplate>
                                <div style="text-align: right;">
                                    (Cod.<%#Eval("id")%>) Diff.<%#Eval("difficolta")%>
                                </div>
                                <br />
                                <span class="question" title="<%#MandatoryToolTip(Container.Dataitem)%>">
                                    <asp:Label ID="LBremovedQuestion" runat="server" Visible="false" CssClass="removedquestion">*Removed question</asp:Label>
                                    <asp:Label ID="LBremovedQuestionNumber" runat="server" Visible="false" CssClass="number"><%#Eval("VirtualNumber")%></asp:Label>
                                    <asp:Label ID="LBquestionNumber" runat="server"  CssClass="number"><%#Eval("numero")%></asp:Label>
                                    <span class="separator">.</span>
                                    <%#MandatoryDisplay(Container.Dataitem)%>
                                    <span class="name">
                                    <%#me.SmartTagsAvailable.TagAll(Eval("testo"))%>
                                    </span>
                                </span>
                                <br />
                                <br />
                                <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                <br />
                                <br />
                            </ItemTemplate>
                            <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <AlternatingItemStyle BackColor="WHITE" />
                            <ItemStyle BackColor="WHITE" />
                            <HeaderStyle BackColor="#EFF3FB" Font-Bold="True" ForeColor="White" />
                        </asp:DataList>
                    </ItemTemplate>
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <AlternatingItemStyle BackColor="#E3EAEB" />
                    <ItemStyle BackColor="#E3EAEB" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                </asp:DataList>&nbsp;</asp:Panel>
        </asp:View>
           <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server" Visible="false"></asp:Label>
            <asp:Panel ID="PNLisRisposte" runat="server" Visible="false">
                <asp:Label ID="LBIsRisposte" runat="server"></asp:Label>
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Content>