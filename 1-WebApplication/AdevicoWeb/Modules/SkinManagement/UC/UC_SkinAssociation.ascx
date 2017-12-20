<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinAssociation.ascx.vb" Inherits="Comunita_OnLine.UC_SkinAssociation" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="../../../UC/UC_SearchCommunityByService.ascx" %>

<div class="Association">

    <asp:MultiView ID="MLVassociation" runat="server" EnableViewState="true">
        <asp:View ID="V_List" runat="server">
            <div class="buttons">
                <asp:Label ID="Lbl_AssPortale" runat="server">Associato a portale.</asp:Label>
                <asp:LinkButton ID="Lkb_AddPortal" runat="server" CssClass="Link_Menu">Associa Portale</asp:LinkButton>
                <asp:LinkButton ID="Lkb_RemPortal" runat="server" CssClass="Link_Menu">DeAssocia Portale</asp:LinkButton>
            </div>
            <hr />
            <div class="FixedCol">
                <span>
                    <asp:Label ID="Lbl_Orgn_t" runat="server" CssClass="Titolo_Campo">Organizzazioni</asp:Label>
                    <asp:LinkButton ID="Lkb_ModOrgn" runat="server" CssClass="Link_Menu">Aggiungi/Modifica</asp:LinkButton>
                </span>
                <asp:Repeater ID="Rpt_OrgnList" runat="server">
                    <HeaderTemplate>
                        <div class="FixedSize">
                        <ul class="Item">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:LinkButton ID="Lkb_DelOrgnAss" runat="server" CssClass="DeleteSmall">x</asp:LinkButton>
                            <asp:Label ID="Lbl_OrgnName" runat="server">ORGN</asp:Label>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="FixedCol">
                <span>
                    <asp:Label ID="Lbl_Com_t" runat="server" CssClass="Titolo_Campo">Comunità</asp:Label>
                    <asp:LinkButton ID="Lkb_ModCom" runat="server" CssClass="Link_Menu">Aggiungi/Modifica</asp:LinkButton>
                </span>
                <asp:Repeater ID="Rpt_ComList" runat="server">
                    <HeaderTemplate>
                        <div class="FixedSize">
                        <ul class="Item">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:LinkButton ID="Lkb_DelComAss" runat="server" CssClass="DeleteSmall">x</asp:LinkButton>
                            <asp:Label ID="Lbl_ComName" runat="server">Community</asp:Label>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div style="clear:both;"></div>
        </asp:View>
        <asp:View ID="V_SetCom" runat="server">
            <div class="buttons">
                <asp:LinkButton ID="Lkb_SaveCom" runat="server" CssClass="Link_Menu">Salva</asp:LinkButton>
                <asp:LinkButton ID="Lkb_BackCom" runat="server" CssClass="Link_Menu">Torna alla lista</asp:LinkButton>
            </div>
        
            <CTRL:CTRLsearch ID="UC_SearchCommunity" runat="server" />
        </asp:View>

        <asp:View ID="V_SetOrgn" runat="server" EnableViewState="true">
        
            <div class="buttons">
                <asp:LinkButton ID="Lkb_SaveOrgn" runat="server" CommandName="Save" CssClass="Link_Menu">Salva</asp:LinkButton>
                <asp:LinkButton ID="Lkb_BackOrgn" runat="server" CssClass="Link_Menu">Torna alla lista</asp:LinkButton>
            </div>
            <asp:Repeater ID="Rpt_OrgnMod" runat="server" EnableViewState="true">
                <HeaderTemplate>
                    <ul class="UL_Orgn">
                </HeaderTemplate>
                <ItemTemplate>
                        <li>
                            <asp:CheckBox ID="Cbx_Orgn" runat="server" Text="OrgnName" CssClass="Titolo_Campo"/>
                            <asp:HiddenField ID="Hid_OrgnId" runat="server" />
                            <br />
                            <asp:HyperLink ID="Hyp_Orgn" runat="server" CssClass="Testo_campoSmall" Target="_blank">Orgn</asp:HyperLink>
                        </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </asp:View>
    </asp:MultiView>
</div>