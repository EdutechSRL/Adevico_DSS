<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ContentTranslator.ascx.vb" Inherits="Comunita_OnLine.UC_ContentTranslator" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>


<asp:MultiView ID="MLVselector" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server"></asp:View>
    <asp:View ID="VIWactive" runat="server">
        <div class="fieldobject fieldeditor">
            <div class="fieldrow templatename" id="DVname" visible="false" runat="server">
                <asp:Label ID="LBname_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBname">*Name</asp:Label>
                <asp:Label id="LBnameLanguage_t" runat="server" CssClass="templatelanguage" AssociatedControlID="TXBname"></asp:Label>
                <asp:TextBox ID="TXBname" runat="server" CssClass="fieldinput"></asp:TextBox>
            </div>
            <div class="fieldrow subject" id="DVsubject" visible="false" runat="server">
                <asp:Label ID="LBsubject_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBsubject">*Subject</asp:Label>
                <asp:Label id="LBsubjectLanguage_t" runat="server" CssClass="templatelanguage" AssociatedControlID="TXBsubject"></asp:Label>
                <asp:TextBox ID="TXBsubject" runat="server" CssClass="fieldinput"></asp:TextBox>
            </div>
            <div class="fieldrow clearfix"> 
                <div class="varbuttons expandlistwrapper compressed" id="DVplaceholders" runat="server" visible="false">
                    <asp:Label ID="LBplaceholders_t" runat="server" CssClass="fieldlabel" >*Tags</asp:Label>
                    <span class="icons">
                        <asp:Label ID="LBlegend" runat="server" CssClass="more icon help"></asp:Label>
                    </span>
                    <div class="clear"></div>
                    <asp:Repeater ID="RPTplaceHolder" runat="server">
                        <HeaderTemplate>
                            <ul class="expandlist">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li class="listitem">
                                <asp:Button ID="BTNattribute" CssClass="varbutton linkMenu" runat="server" CausesValidation="false" />
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                            <div class="commands">
                                <asp:Label ID="LBdisplayAllPlaceHolders" runat="server" CssClass="command expand">*show all placeholder</asp:Label>
                                <asp:Label ID="LBdisplaySomePlaceHolders"  runat="server" CssClass="command compress">*collapse list</asp:Label>
                            </div>
                            <div class="clear"></div>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Label id="LBmandatorySkipped"  CssClass="required" runat="server"></asp:Label>
                </div>
            </div>
            <div class="fieldrow editor " id="DVbody" runat="server" visible="false">
                <asp:TextBox ID="TXBstandard" runat="server" Rows="15" TextMode="MultiLine" Columns="100" CssClass="addTextToMe fieldinput" Visible="false">
                </asp:TextBox>
                <CTRL:CTRLeditor id="CTRLhtml" runat="server" ContainerCssClass="containerclass" 
                    LoaderCssClass="loadercssclass" EditorHeight="280px" EditorWidth="95%" AllAvailableFontnames="true"  EnabledTags=""
                     UseRealFontSize="true" RealFontSizes="10px,12px,14px, 16px, 18px"
                    AutoInitialize="true" Toolbar="advanced" CurrentType="telerik" DisabledTags="img,latex,youtube,emoticons,wiki">
                </CTRL:CTRLeditor>
            </div>
            <div class="fieldrow shorttext" id="DVshortText" runat="server" visible="false">
                <asp:Label ID="LBshortText_t" runat="server" CssClass="fieldlabel" AssociatedControlID="TXBshortText">*Short text</asp:Label>
                <asp:Label id="LBshortTextLanguage_t" runat="server" CssClass="templatelanguage" AssociatedControlID="TXBshortText"></asp:Label>
                <asp:TextBox ID="TXBshortText" runat="server" CssClass="fieldinput"></asp:TextBox>
            </div>
        </div>

        <div class="dialog dlgkeyword" runat="server" id="DVdialog">
            <asp:Repeater ID="RPTattributes" runat="server">
                <HeaderTemplate>
                     <table class="table minimal fullwidth templatelegend">
                        <thead>
                            <tr>
                                <th class="lgdbutton"><asp:Literal ID="LTtagHeaderTranslatedName" runat="server"></asp:Literal></th>
                                <th class="lgdplaceholder"><asp:Literal ID="LTtagHeaderValue" runat="server"></asp:Literal></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                            <tr>
                                <td class="lgdbutton"><%#Container.DataItem.Name%></td>
                                <td class="lgdplaceholder"><%#Container.DataItem.Tag%></td>
                            </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </asp:View>
</asp:MultiView>