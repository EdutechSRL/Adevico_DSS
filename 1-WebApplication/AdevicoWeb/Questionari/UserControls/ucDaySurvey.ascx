<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucDaySurvey.ascx.vb" Inherits="Comunita_OnLine.ucDaySurvey" %>
      <link media="screen" href="<%#ResolveUrl("~/Questionari/stile.css?v=201604071200lm")%>" type="text/css" rel="StyleSheet" />

      <div runat="server" id="divDaySurvey">
                 <asp:MultiView runat="server" ID="MLVquestionari">
                        <asp:View ID="VIWlink" runat="server">
                        <asp:DataList CssClass="Filtro_CellFiltri" ID="DLLinks" ShowFooter="true" runat="server" DataKeyField="id" CellPadding="4" ForeColor="#333333" BorderWidth="1" Width="100%" OnItemCommand="DLLinksCommand">
                        <ItemTemplate>  
                        <img src="<%#ResolveUrl("~/images/HasNews.gif")%>" alt="" />
                        <asp:LinkButton  runat="server" ID="LNBViewSurvey" text='<%# me.LinkText +  Eval("nome") %>' CommandName="OtherSurveys"></asp:LinkButton>
                        </ItemTemplate>
                        </asp:DataList>
                        <asp:label ID="LBDescrizione" runat="server"></asp:label>
                        </asp:View>
                         <asp:View ID="VIWdatalist" runat="server">
                         <asp:Label runat="server" ID="LBTitolo"></asp:Label><br />
                         <asp:DataList ID="DLPagine" ShowFooter="true" runat="server" DataKeyField="id" CellPadding="4" ForeColor="#333333" BorderWidth="1" Width="100%">
                            <ItemTemplate>  
                                <asp:DataList ID="DLDomande" runat="server" DataKeyField="id" OnItemDataBound="loadDomandeOpzioni"
                                    Width="100%">
                                    <ItemTemplate>
                                        <div class="ContenitoreDomanda0">
                                            <div class="TestoDomanda">
                                                <div>                                                
                                                <%#me.SmartTagsAvailable.TagAll(Eval("testo"))%>
                                            </div>
                                            <br />
                                            <div class="Risposte">
                                                <asp:PlaceHolder ID="PHOpzioni" runat="server" Visible="true"></asp:PlaceHolder>
                                            </div> 
                                        </div>
                                        <asp:Label runat="server" ID="LBsuggerimentoOpzione" Font-Italic="true" Visible="false" ></asp:Label>
                                        <asp:Label runat="server" ID="LBSuggerimento" text='<%#Eval("suggerimento")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle BackColor="WHITE" Font-Bold="True" ForeColor="White" />
                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <AlternatingItemStyle BackColor="#EFF3FB" />
                                    <ItemStyle BackColor="WHITE" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                </asp:DataList>                        
                            </ItemTemplate>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <AlternatingItemStyle BackColor="#507CD1" />
                            <ItemStyle BackColor="#EFF3FB" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        </asp:DataList>
                        <br /><div style="text-align: right;">   
                            <asp:Button runat="server" ID="BTNFine" EnableViewState="False" Visible="true" /><br />
                            </div>
                            <asp:LinkButton runat="server" ID="LNBOtherSurveys1"></asp:LinkButton><br />
                        </asp:View>
                        <asp:View runat="server" ID="VIWRisultati">
                        <br />
                        <asp:DataList ID="DLDomandeRisultati" runat="server" DataKeyField="id" OnItemDataBound="loadRisposteOpzioni"
                                BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" GridLines="Horizontal" >
                                <ItemTemplate>
                                   <table>
                                   <tr>
                                   <td align="left">
                                    <b><%#DataBinder.Eval(Container, "DataItem.numero")%>.  <%#Me.SmartTagsAvailable.TagAll(DataBinder.Eval(Container, "DataItem.testo"))%> </b>    <br />
                                    <asp:Label runat="server" ID="LBLnumeroRisposte"></asp:Label> <%#DataBinder.Eval(Container, "DataItem.numeroRisposteDomanda")%>
                                   </td>
                                   
                                   </tr>
                                   <tr>
                                   <td>
                                        <asp:placeholder ID="PHOpzioni" runat="server" Visible="true"></asp:placeholder>
                                   </td>
                                 
                                   </tr>
                                   </table>
                                     
                                         </ItemTemplate>
                                           <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                         <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                         <AlternatingItemStyle BackColor="#F7F7F7"   />
                                         <ItemStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                         <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                </asp:DataList>
                                <br /><asp:LinkButton runat="server" ID="LNBOtherSurveys2"></asp:LinkButton><br />

                        </asp:View>
                 </asp:MultiView>
        </div>
