<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ContentTranslationSelector.ascx.vb" Inherits="Comunita_OnLine.UC_ContentTranslationSelector" %>
<asp:MultiView ID="MLVselector" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
    <!--
    <a class="btnswitch first" href="" title="international">
                <span class="statuslight green">&nbsp;</span>
                <span class="langcode">INT</span>
            </a>

           <!--<a class="linkMenu" href="#en">
                    <span class="langcode">EN</span>
                    <span class="langname">english</span>
                </a> -->
    </asp:View>
    <asp:View ID="VIWactive" runat="server">
        <div class="languagegroup clearfix">
            <asp:Label ID="LBusedLanguage_t" runat="server">*Internazionalizzazioni attivate</asp:Label>

            <asp:Repeater ID="RPTtranslations" runat="server">
                <HeaderTemplate>
                    <span class="btnswitchgroup"><!--
                --></HeaderTemplate>
                <ItemTemplate><!--
                --><asp:LinkButton ID="LNBtranslation" runat="server" CssClass="btnswitch" CausesValidation="false"></asp:LinkButton><!--
                --></ItemTemplate>
                <FooterTemplate><!--
                --></span>
                </FooterTemplate>
            </asp:Repeater>
            <span class="languageadd" id="SPNlanguageAdd" runat="server">
                <asp:Label ID="LBlanguageAdd_t" runat="server">*Add</asp:Label>
                 <div class="ddbuttonlist enabled" id="DVaddLanguage" runat="server"><!--
                    --><asp:Repeater ID="RPTlanguages" runat="server">
                        <ItemTemplate><!--
                        --><asp:linkbutton id="LNBlanguage" runat="server" CssClass="linkMenu" CausesValidation="false" CommandName="add"></asp:linkbutton><!--    
                        --></ItemTemplate>
                    </asp:Repeater><!--
                --></div>
            </span>

        </div>
    </asp:View>
</asp:MultiView>

<asp:Literal ID="LTaddLanguage" runat="server" Visible="false"><span class="langcode">{0}</span>&nbsp;<span class="langname">{1}</span></asp:Literal>
<asp:Literal ID="LTtranslation" runat="server" Visible="false"><span class="statuslight {0}">&nbsp;</span><span class="langcode">{1}</span></asp:Literal>