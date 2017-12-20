<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMsourceCSV.ascx.vb" Inherits="Comunita_OnLine.UC_IMsourceCSV" %>
<div class="StepData IMsourceCSV">
    <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <div class="fieldobject">
                <div class="fieldrow">
                    <br /><br /><br /><br />
                    <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                    <br /><br /><br /><br />
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWselectCSV" runat="server">
            <div class="fieldobject">
                <div class="fieldrow" runat="server" id="DVselectFile">
                    <asp:Label ID="LBsourceFile_t" runat="server" CssClass="fieldlabel" AssociatedControlID="INFcsv">Select file: </asp:Label>
                    <input type="file" id="INFcsv" runat="server" size="60" />
                    <asp:Button ID="BTNupload" runat="server" Text="Upload" CssClass="Testo_Campo" CausesValidation="false"></asp:Button>
                </div>
                <div class="fieldrow" runat="server" id="DVdisplayFile">
                    <asp:Label ID="LBuploadedFile_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBuploadedFile">Selectd file: </asp:Label>
                    <asp:Label ID="LBuploadedFile" runat="server" CssClass="Testo_Campo"></asp:Label>
                    <asp:Button ID="BTNremoveFile" runat="server" CssClass="img_btn ico_delete_s" CausesValidation="false" />
                </div>
                <div class="fieldrow" runat="server" id="DVfirstRowColumnNames">
                    <asp:Label ID="LBfirstRowColumnNames_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXfirstRowColumnNames">Column names on first row:</asp:Label>
                    <asp:CheckBox ID="CBXfirstRowColumnNames" runat="server" CssClass="Testo_Campo" />
                </div>
                <div class="fieldrow" runat="server" id="DVcolumnDelimiter">
                    <asp:Label ID="LBcolumnDelimiter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLcolumnDelimiter">Column delimiter:</asp:Label>
                    <asp:DropDownList ID="DDLcolumnDelimiter" runat="server" CssClass="Testo_Campo">
                        <asp:ListItem Value="Semicolon">Punto e virgola {;}</asp:ListItem>
                        <asp:ListItem Value="Colon">Due punti {:}</asp:ListItem>
                        <asp:ListItem Value="Comma">Virgola {,}</asp:ListItem>
                        <asp:ListItem Value="Tab">Tabulazione {t}</asp:ListItem>
                        <asp:ListItem Value="VerticalBar">Barra verticale {|}</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="fieldrow" runat="server" id="DVrowDelimiter">
                    <asp:Label ID="LBrowDelimiter_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLrowDelimiter">Row delimiter:</asp:Label>
                    <asp:DropDownList ID="DDLrowDelimiter" runat="server" CssClass="Testo_Campo">
                        <asp:ListItem Value="CrLf">{CR}{LF}</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="fieldrow" runat="server" id="DVrowToSkip">
                    <asp:Label ID="LBrowToSkip_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLrowsToSkip">Row to skip:</asp:Label>
                    <asp:DropDownList ID="DDLrowsToSkip" runat="server" CssClass="Testo_Campo">

                    </asp:DropDownList>
                </div>
                <div class="fieldrow" runat="server" id="DVpreview">
                    <asp:Button ID="BTNpreviewCSV" runat="server" CausesValidation="false" />
                    <span class="tbl_wizdata" id="SPNpreviewTable" runat="server" visible="false">
                    <asp:Repeater ID="RPTpreview" runat="server">
                          <ItemTemplate>
                             <table class="table light fullwidth">
                                <tr>
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
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <br /><br />
</div>