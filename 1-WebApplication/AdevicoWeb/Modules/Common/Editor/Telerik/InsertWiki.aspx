<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InsertWiki.aspx.vb" Inherits="Comunita_OnLine.InsertWiki" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <asp:Literal ID="LTwikiTitle" runat="server"></asp:Literal></title>

    <asp:Literal ID="LTskins" runat="server"></asp:Literal>
    <link rel="stylesheet" href='<%=AppUrl%>Graphics/Generics/css/4_UI_Elements.css' />
    <link rel="stylesheet" href='<%=AppUrl%>Graphics/Generics/Command/css/command.css' />
    <link rel="Stylesheet" href='<%=AppUrl%>Graphics/Modules/Editor/css/popup.css' />
    <link rel="Stylesheet" href='<%=AppUrl%>Graphics/Modules/Wiki/wiki.new.css'/>
    
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        var workLink = null;
        function getRadWindow() //mandatory for the RadWindow dialogs functionality
        {
            if (window.radWindow) {
                return window.radWindow;
            }
            if (window.frameElement && window.frameElement.radWindow) {
                return window.frameElement.radWindow;
            }
            return null;
        }

        function initDialog() //called when the dialog is initialized
        {
            var clientParameters = getRadWindow().ClientParameters;
            workLink = clientParameters;
            txt = document.getElementById('<%=me.TXBsearch.ClientID %>');
            if (txt) {

                if (txt.value == "" && clientParameters.innerText != "") {
                    txt.value = clientParameters.innerText;
                    document.getElementById("BTNsearchWiki").click();
                    var theForm = document.forms['aspnetForm'];
                    if (!theForm) {
                        theForm = document.aspnetForm;
                    }
                    function __doPostBack(eventTarget, eventArgument) {
                        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                            theForm.__EVENTTARGET.value = eventTarget;
                            theForm.__EVENTARGUMENT.value = eventArgument;
                            theForm.submit();
                        }
                    }
                }
            }
//            var HiddenContainer = document.getElementById("HDNdefaultSearch");

//            if (TextContainer) {
//                if (TextContainer.value == "" && clientParameters.innerHTML != "") {
//                    TextContainer.value = clientParameters.innerHTML
//                    HiddenContainer.value = clientParameters.innerHTML
//                    document.getElementById("BTN_search").click();
//                    var theForm = document.forms['aspnetForm'];
//                    if (!theForm) {
//                        theForm = document.aspnetForm;
//                    }
//                    function __doPostBack(eventTarget, eventArgument) {
//                        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
//                            theForm.__EVENTTARGET.value = eventTarget;
//                            theForm.__EVENTARGUMENT.value = eventArgument;
//                            theForm.submit();
//                        }
//                    }
//                }
//            }
        }
        if (window.attachEvent) {
            window.attachEvent("onload", initDialog);
        }
        else if (window.addEventListener) {
            window.addEventListener("load", initDialog, false);
        }

        function insertLink(linkName, linkUrl) {
            workLink.href = linkUrl;
            workLink.target = "_self";
            workLink.name = linkName;
            workLink.className = "wikilink";

            getRadWindow().close(workLink); //use the close function of the getRadWindow to close the dialog and pass the arguments from the dialog to the callback function on the main page.

            return false;
        }
    </script>
    <asp:Panel ID="PNLsearch" runat="server" BorderWidth="0">
        <div class="fieldobject search">
            <div class="fieldrow">
                <asp:Label ID="LBsearchBy" runat="server" CssClass="Testo_campoSmall"></asp:Label>
            <asp:DropDownList ID="DDLsearchBy" runat="server">
                <asp:ListItem Value="1">(Topic)</asp:ListItem>
                <asp:ListItem Value="2">Inizia per</asp:ListItem>
                <asp:ListItem Value="3">Finisce per</asp:ListItem>
                <asp:ListItem Value="4">Contiene</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="TXBsearch" runat="server" CssClass="Testo_campoSmall" MaxLength="60">
            </asp:TextBox>
            <asp:Button ID="BTNsearchWiki" runat="server" CommandArgument="TXB_search.text" CommandName="Cerca"
                CssClass="PulsanteFiltro" Height="21px" Text="Cerca" />
            </div>                    
            <div class="fieldrow">
                <asp:Label ID="LBsearchInto" runat="server" CssClass="Testo_campoSmall">Into:</asp:Label>
                <asp:CheckBox runat="server" ID="CBXcurrentCommunity" Text="*Current community" Checked="true" />
                <asp:CheckBox runat="server" ID="CBXpublicWiki" Text="*Public" Checked="false" />
            </div>
        </div>
    </asp:Panel>
    <input type="hidden" id="HDNdefaultSearch" runat="server" />
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <div class="TestoInfo">
                <div class="message info">
                    <asp:Label ID="LBnotopicFound" runat="server" CssClass="Testo_campoSmall">* No topic found</asp:Label>                    
                </div>                
            </div>
        </asp:View>
        <asp:View ID="VIWnoPermission" runat="server">

            <div class="messages">
                <div class="message error">
                    <span class="icons"><span class="icon">&nbsp;</span></span>
                    <asp:Label ID="LBnoPermission" runat="server">
                     Non si dispone dei permessi necessari per visualizzare la pagina.
                </asp:Label>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWresults" runat="server">
            <asp:DataList ID="DLSresult" runat="server" CellPadding="4" ForeColor="#333333"
                DataKeyField="id" UseAccessibleHeader="true" ShowFooter="true" ShowHeader="true" Width="100%">
                <HeaderStyle CssClass="" />
                <HeaderTemplate>
                    <table class="table minimal fullwidth wikitable wikilink">
                        <tr class="tableheader tablerow">
                            <th class="topic">
                                <asp:Label ID="LBwikiItem_t" runat="server" Text="Voce"></asp:Label>
                            </th>
                            <th class="section">
                                <asp:Label ID="LBwikiSection_t" runat="server" Text="Sezione"></asp:Label>
                            </th>
                            <th class="community" runat="server" id="THcommunity">
                                <asp:Label ID="LBwikiCommunity_t" runat="server" Text="Comunità"></asp:Label>
                            </th>
                        </tr>                    
                </HeaderTemplate>
                <ItemTemplate>
                    
                        <tr class="tablerow">
                            <td class="topic">
                                <span class="Titolo_Campo"><a href="#" onclick="return insertLink('<%#DataBinder.Eval(Container.DataItem, "Nome")%>','<%=Me.AppUrl %>wiki/wiki_comunita.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>');">
                                    <%#DataBinder.Eval(Container.DataItem, "Nome")%></a> </span>
                                </td>
                                <td class="section">
                                    <span class=""><a name="<%#DataBinder.Eval(Container.DataItem, "ID")%>"></a>
                                        <%#DataBinder.Eval(Container.DataItem, "Sezione.NomeSezione")%> </span>
                                </td>
                                <td class="community" runat="server" id="TDcommunity">
                                    <span class=""><a name="<%#DataBinder.Eval(Container.DataItem, "ID")%>"></a>
                                        <%#DataBinder.Eval(Container.DataItem, "Comunita")%> </span>
                                </td>
                        </tr>
                    
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
            <div>
                <table align="right" width="35px">
                    <tr align="right">
                        <td align="right">
                            <asp:ImageButton ID="IMG_indietro" runat="server" CssClass="PulsanteFiltro" ImageUrl="~/questionari/img/indietro.gif"
                                AlternateText="Pagina precedente" />
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="IMG_avanti" runat="server" CssClass="PulsanteFiltro" ImageUrl="~/questionari/img/avanti.gif"
                                AlternateText="Pagina successiva" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
        </asp:View>
    </asp:MultiView>
    </form>
</body>
</html>
