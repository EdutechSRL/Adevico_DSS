<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GenericCSVuploader.ascx.vb" Inherits="Comunita_OnLine.UC_GenericCSVuploader" %>
    <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
           <span class="Fieldrow">
                <br /><br /><br /><br />
                <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                <br /><br /><br /><br />
            </span>
        </asp:View>
        <asp:View ID="VIWselectCSV" runat="server">
             <span class="Fieldrow" id="SPNselectFile" runat="server">
                <asp:Label ID="LBsourceFile_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="INFcsv">Select file: </asp:Label>
                <input type="file" id="INFcsv" runat="server" size="60" />
                <asp:Button ID="BTNupload" runat="server" Text="Upload" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
            </span>
            <span class="Fieldrow" id="SPNdisplayFile" runat="server">
                <asp:Label ID="LBuploadedFile_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBuploadedFile">Selectd file: </asp:Label>
                <asp:Label ID="LBuploadedFile" runat="server" CssClass="Testo_Campo"></asp:Label>
                <asp:Button ID="BTNremoveFile" runat="server" CssClass="img_btn ico_delete_s" CausesValidation="false" />
            </span>
            <span class="Fieldrow" id="SPNfirstRowColumnNames" runat="server">
                 <asp:Label ID="LBfirstRowColumnNames_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXfirstRowColumnNames">Column names on first row:</asp:Label>
                 <asp:CheckBox ID="CBXfirstRowColumnNames" runat="server" CssClass="Testo_Campo" />
            </span>
            <span class="Fieldrow" id="SPNcolumnDelimiter" runat="server">
                <asp:Label ID="LBcolumnDelimiter_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLcolumnDelimiter">Column delimiter:</asp:Label>
                <asp:DropDownList ID="DDLcolumnDelimiter" runat="server" CssClass="Testo_Campo">
                    <asp:ListItem Value="Semicolon">Punto e virgola {;}</asp:ListItem>
                    <asp:ListItem Value="Colon">Due punti {:}</asp:ListItem>
                    <asp:ListItem Value="Comma">Virgola {,}</asp:ListItem>
                    <asp:ListItem Value="Tab">Tabulazione {t}</asp:ListItem>
                    <asp:ListItem Value="VerticalBar">Barra verticale {|}</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="Fieldrow" id="SPNrowDelimiter" runat="server">
                <asp:Label ID="LBrowDelimiter_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLrowDelimiter">Row delimiter:</asp:Label>
                <asp:DropDownList ID="DDLrowDelimiter" runat="server" CssClass="Testo_Campo">
                    <asp:ListItem Value="CrLf">{CR}{LF}</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="Fieldrow" id="SPNrowToSkip" runat="server">
                <asp:Label ID="LBrowToSkip_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLrowsToSkip">Row to skip:</asp:Label>
                <asp:DropDownList ID="DDLrowsToSkip" runat="server" CssClass="Testo_Campo">

                </asp:DropDownList>
            </span>
            <span class="Fieldrow" id="SPNpreview" runat="server" visible="false">
                <asp:Button ID="BTNpreviewCSV" runat="server" CausesValidation="false" />
                <span class="tbl_wizdata" id="SPNpreviewTable" runat="server" visible="false">
                <asp:Repeater ID="RPTpreview" runat="server">
                      <ItemTemplate>
                         <table border="1" cellpadding="0" cellspacing="0">
                            <tr class="ROW_header_Small_Center">
                                <asp:Repeater ID="RPTcsvHeader" runat="server" DataSource="<%#Container.DataItem.ColumHeader%>" OnItemDataBound="RPTcsvHeader_ItemDataBound">
                                    <ItemTemplate>
                                        <th><asp:literal ID="LTcolumn" runat="server"></asp:literal></th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                            <asp:Repeater ID="RPTitemRow" runat="server" DataSource="<%#Container.DataItem.Rows%>">
                                <ItemTemplate>
                                    <tr>
                                        <asp:Repeater ID="RPTitems" runat="server" DataSource="<%#Container.DataItem%>" OnItemDataBound="RPTitems_ItemDataBound">
                                            <ItemTemplate>
                                                <td>&nbsp;<asp:literal ID="LTitem" runat="server"></asp:literal>&nbsp;</td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                         </table>
                    </ItemTemplate>
                    <FooterTemplate>
                       
                    </FooterTemplate>
                </asp:Repeater>
                </span>
            </span>
        </asp:View>
    </asp:MultiView>
    <br /><br />