<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GlossarySettings.ascx.vb" Inherits="Comunita_OnLine.UC_GlossarySettings" %>
<%@ Register TagPrefix="CTRL" TagName="Switch" Src="~/Modules/Common/UC/UC_Switch.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<div class="content">

    <asp:Literal ID="LTmessageheaderCss" runat="server" Visible="false">fieldobject _fieldgroup hide</asp:Literal>

    <div id="DVmessages" class="fieldobject fieldgroup hide" runat="server">
        <div class="fieldrow">
            <CTRL:Messages ID="CTRLmessagesInfo" runat="server" Visible="false"/>
        </div>
    </div>

    <div class="fieldobject box basicinfo first">
        <div class="fieldrow objectheader">
            <h4 class="title">
                <asp:Literal ID="LTbasicGlossarySettings" runat="server">*Basic info</asp:Literal>
            </h4>
        </div>
        <div class="fieldrow fieldlongtext">
            <label class="fieldlabel">
                <asp:Literal ID="LTglossaryName_t" runat="server">*Glossary Name</asp:Literal><span class="mandatory">*</span>

            </label>
            <asp:TextBox ID="TXBname" runat="server" CssClass="inputtext big"></asp:TextBox>
        </div>
        <div class="fieldrow fielddescription">
            <label class="fieldlabel">
                <asp:Literal ID="LTglossaryDescription_t" runat="server">*Description</asp:Literal><%--<span class="mandatory">*</span>--%>
            </label>
            <asp:TextBox ID="TXBdescription" runat="server" CssClass="textarea big" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div class="fieldrow defaultglossary">
            <asp:Label ID="LBdefaultGlossary_t" runat="server" AssociatedControlID="CBXisDefault" CssClass="fieldlabel">*Default Glossary</asp:Label>
            <span class="inputgroup">
                <asp:CheckBox ID="CBXisDefault" runat="server" CssClass="fieldinput"></asp:CheckBox>
                <label for="">default</label>
            </span>
        </div>

        <div class="fieldrow fieldlanguage">
            <asp:Label ID="LBlanguage_t" runat="server" AssociatedControlID="RBLanguages" CssClass="fieldlabel">*Language</asp:Label>
            <div class="inlinewrapper">
                <asp:RadioButtonList ID="RBLanguages" CssClass="radiobuttonlist" AutoPostBack="false" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                </asp:RadioButtonList>
            </div>
        </div>

        <div class="fieldrow status">
            <asp:Label ID="LBstatus_t" runat="server" AssociatedControlID="SWHpublishGlossary" CssClass="fieldlabel">*Status</asp:Label>
            <CTRL:Switch ID="SWHpublishGlossary" runat="server" Status="true"/>
        </div>

        <div class="fieldrow fieldlongtext defaultview">
            <label class="fieldlabel">
                <asp:Literal ID="LTdefaultview_t" runat="server">*Default view</asp:Literal>
            </label>
            <div class="inlinewrapper">
                <span class="inputgroup">
                    <asp:RadioButton name="defaultview" GroupName="defaultview" ID="RBpagedList" runat="server" />
                    <asp:Label runat="server" ID="LBpagedList_t" AssociatedControlID="RBpagedList">*Paged list</asp:Label>

                    <span class="activeonly">
                        <asp:Label runat="server" ID="LBpageSize" AssociatedControlID="TXBpageSize">*page size</asp:Label>
                        <asp:TextBox ID="TXBpageSize" runat="server"></asp:TextBox>
                    </span>
                </span>
                <span class="inputgroup">
                    <asp:RadioButton name="defaultview" GroupName="defaultview" ID="RBthreeColumn" runat="server" />
                    <asp:Label runat="server" ID="LBthreeColumn_t" AssociatedControlID="RBthreeColumn">*3 columns list</asp:Label>
                </span>

            </div>
        </div>

        <div class="fieldrow mandatorylegend">
            <asp:Literal ID="LTmarkedFields_t" runat="server"></asp:Literal>
            <span class="mandatory">* </span>
            <asp:Literal ID="LTareMandatory_t" runat="server"></asp:Literal>
        </div>
    </div>

</div>
<div class="footer">
    <div class="DivEpButton">
        <asp:LinkButton ID="LNBback" runat="server" CssClass="linkMenu">*Torna</asp:LinkButton>
        <asp:LinkButton ID="LNBsaveGlossary" runat="server" CssClass="linkMenu">*Salva Glossario</asp:LinkButton>
    </div>
</div>