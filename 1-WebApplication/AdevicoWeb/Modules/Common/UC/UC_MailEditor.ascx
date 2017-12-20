<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailEditor.ascx.vb"
    Inherits="Comunita_OnLine.UC_MailEditor" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<script language="javascript" type="text/javascript">

    $.fn.insertAtCaret = function (tagName) {
        return this.each(function () {
            if (document.selection) {
                //IE support
                this.focus();
                sel = document.selection.createRange();
                sel.text = tagName;
                this.focus();
            } else if (this.selectionStart || this.selectionStart == '0') {
                //MOZILLA/NETSCAPE support
                startPos = this.selectionStart;
                endPos = this.selectionEnd;
                scrollTop = this.scrollTop;
                this.value = this.value.substring(0, startPos) + tagName + this.value.substring(endPos, this.value.length);
                this.focus();
                this.selectionStart = startPos + tagName.length;
                this.selectionEnd = startPos + tagName.length;
                this.scrollTop = scrollTop;
            } else {
                this.value += tagName;
                this.focus();
            }
        });
    };

    $(document).ready(function () {

        $(".addTextTelerik").click(function () {

            //var editor = $find("<%=CTRLhtmlMail.EditorClientId%>");
            //var editor = $("#" + "<%=CTRLhtmlMail.EditorClientId%>");
            var editor =  $find("<%=CTRLhtmlMail.EditorClientId%>");
            var add = $(this).attr("rel");
            //editor.pasteHtml(add);
            editor.pasteHtml(add);
            <% = HTMLMailOnKeyUpScript%>
            return false;
        });
        $(".addTextTextarea").click(function () {
            var textarea = $(".addTextToMe");
            var add = $(this).attr("rel");
            textarea.insertAtCaret(add);
            <% = StandardMailOnKeyUpScript%>
            return false;
        });
    });


</script>
<div class="RowMail" id="DVsender" runat="server">
    <div class="RowCellLeft">
        <asp:Label ID="LBsender_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="DDLsender">Mittente:</asp:Label>
    </div>
    <div class="RowCellRight">
        <asp:DropDownList ID="DDLsender" runat="server" CssClass="Testo_Campo">
            <asp:ListItem Value="LoggedUser" Text=" " Selected="True"></asp:ListItem>
            <asp:ListItem Value="System" Text=""></asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
<div class="RowMail" id="DVsubject" runat="server">
    <div class="RowCellLeft">
        <asp:Label ID="LBsubjectType_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="DDLsubject">Oggetto inizia con:</asp:Label>
    </div>
    <div class="RowCellRight">
        <asp:DropDownList ID="DDLsubject" runat="server" CssClass="Testo_Campo" AutoPostBack="true">
            <asp:ListItem Value="SystemConfiguration" Text=" " Selected="True"></asp:ListItem>
            <asp:ListItem Value="None" Text=""></asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
<div class="RowMail">
    <div class="RowCellLeft">
        <asp:Label ID="LBsubject_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="TXBsubject">Oggetto:</asp:Label>
    </div>
    <div class="RowCellRight">
        <asp:Label ID="LBpreSubject" runat="server"></asp:Label><asp:TextBox ID="TXBsubject" runat="server" CssClass="Testo_Campo" Columns="80"></asp:TextBox>
    </div>
</div>
<div class="RowMail" runat="server" id="DVmailType">
    <div class="RowCellLeft">
        <asp:Label ID="LBmailType_t" runat="server" CssClass="Titolo_Campo" AssociatedControlID="RBLmailType">Tipo mail:</asp:Label>
    </div>
    <div class="RowCellRight">
        <asp:RadioButtonList ID="RBLmailType" runat="server" CssClass="Testo_Campo rbl_MultiElement_small" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true">
            <asp:ListItem Value="0" Text="HTML"></asp:ListItem>
            <asp:ListItem Value="1" Text="Standard" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
</div>
<div runat="server" id="DVattributes" class="RowMail">
    <div class="RowCellLeft">
        <asp:Label ID="LBtags_t" CssClass="Titolo_Campo" runat="server">Tags:</asp:Label>
    </div>
    <div class="RowCellRight placeholderslist">
         <asp:Repeater ID="RPTplaceHolder" runat="server">
            <HeaderTemplate>
                <asp:Label ID="LBattributesInfo" CssClass="Testo_Campo" runat="server" Visible="false"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Button ID="BTNattribute" runat="server" CausesValidation="false" />
            </ItemTemplate>
            <FooterTemplate>
                <span class="icons">
                    <asp:Label ID="LBlegend" runat="server" CssClass="more icon help"></asp:Label>
                </span>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Label id="LBmandatorySkipped"  CssClass="required" runat="server"></asp:Label>
    </div>
</div>
<div class="RowMail">
    <div class="RowCellLeft">
        <asp:Label ID="LBmailMessage_t" runat="server" CssClass="Titolo_Campo"></asp:Label>
    </div>
    <div class="RowCellRight">
        <asp:TextBox ID="TXBstandardMail" runat="server" Rows="15" TextMode="MultiLine" Columns="100" CssClass="addTextToMe Testo_Campo" Visible="false">
        </asp:TextBox>
        <CTRL:CTRLeditor id="CTRLhtmlMail" runat="server" ContainerCssClass="containerclass" 
            LoaderCssClass="loadercssclass" EditorHeight="280px" EditorWidth="95%" AllAvailableFontnames="true"  EnabledTags=""
             UseRealFontSize="true" RealFontSizes="10px,12px,14px, 16px, 18px" NewLineMode="Br"
            AutoInitialize="true" Toolbar="advanced" CurrentType="telerik" DisabledTags="img,latex,youtube,emoticons,wiki">
        </CTRL:CTRLeditor>
    </div>
</div>
<div class="RowMail" id="DVnotificationCopy" runat="server" visible="false">
    <div class="RowCellLeft">
        &nbsp;
    </div>
    <div class="RowCellRight">
        <asp:CheckBox ID="CBXcopyToSender" CssClass="Testo_Campo" runat="server" />
        <asp:CheckBox ID="CBXnotifyToSender" CssClass="Testo_Campo" runat="server" />
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
                        <td class="lgdbutton"><%#Container.DataItem.Translation%></td>
                        <td class="lgdplaceholder"><%=ContainerLeft%><%#Container.DataItem.Id.ToString%><%=ContainerRight%></td>
                    </tr>
        </ItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>