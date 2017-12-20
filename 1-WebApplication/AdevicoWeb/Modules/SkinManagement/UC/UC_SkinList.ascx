<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SkinList.ascx.vb" Inherits="Comunita_OnLine.UC_SkinList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div class="tab">
    <telerik:RadTabStrip ID="TBSSkinEdit" runat="server" Align="Justify" Width="100%" Height="20px"
        CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true">
    
        <Tabs>
            <telerik:RadTab Text="#UnAssigned" Value="-2" Selected="true" />
            <telerik:RadTab Text="#Community" Value="1" />
            <telerik:RadTab Text="#Organization" Value="2" />
            <telerik:RadTab Text="#Portal" Value="0" />
            <telerik:RadTab Text="#ALL" Value="-1" />
            <telerik:RadTab Text="#Test" Value="-15" />
        </Tabs>
    
    </telerik:RadTabStrip>
</div>
<div class="edit_content">
<asp:Repeater ID="Rpt_List" runat="server">
    <HeaderTemplate>
        <table class="SkinTable" cellspacing="0">
            <thead>
                <tr class="ROW_header_Small">
                    <th class="skinid">
                        <asp:Label runat="server" ID="LBskinId_t">*ID</asp:Label>
                    </th>
                    <th class="Name">
                        <asp:Label ID="Lbl_Name_t" runat="server" CssClass="Table_Title">#Nome</asp:Label>
                    </th>
                    <th class="Shares">
                        <asp:Label ID="Lbl_Association_t" runat="server" CssClass="Table_Title" Enabled="false">#Associata a</asp:Label>
                    </th>
                    <th class="actions">
                        <span class="icons">
                            <span class="icon actions" title="<%=GetActionTitle() %>"></span>
                        </span>
                    </th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
                <tr>
                    <td class="skinid">
                        <asp:Label runat="server" ID="LBskinId">##</asp:Label>
                    </td>
                    <td class="Name">
                        <asp:Label ID="Lbl_SkinName" runat="server" CssClass="Table_Field">Skin xyz della comunità degli ascenti intraprendenti della Valle del Chiese nell'epoca dei porcospini in calore </asp:Label>
                    </td>
                    <td class="Shares">
                        <span class="Portal">
                            <asp:LinkButton ID="Lkb_RemPortal" runat="server" CssClass="DeleteSmall">X</asp:LinkButton>
                            <asp:Label ID="Lbl_Portal" runat="server" CssClass="Table_Field important">Portale</asp:Label>
                        </span>
                        <asp:Repeater ID="Rpt_AssOrgn" runat="server" OnItemCommand="Rpt_AssOrgn_ItemCommand">
                            <HeaderTemplate>
                            <span class="Organization">
                                <ul class="trecol">    
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="Lkb_RemOrgn" runat="server" CssClass="DeleteSmall">X</asp:LinkButton>
                                    <asp:Label ID="Lbl_Orgn" runat="server" CssClass="Table_Field">Organizzazione XYZ</asp:Label>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                                </span>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="Rpt_AssCom" runat="server" OnItemCommand="Rpt_AssCom_ItemCommand">
                            <HeaderTemplate>
                             <span class="Community">
                                <ul class="trecol">    
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="Lkb_RemCom" runat="server" CssClass="DeleteSmall">X</asp:LinkButton>
                                    <asp:Label ID="Lbl_Com" runat="server" CssClass="Table_Field">Comunità XYZ</asp:Label>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </span>
                                </ul>    
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                    <td class="actions">
                        <span class="icons">
                            <asp:LinkButton ID="Lkb_Modify" runat="server" CssClass="icon edit" CommandName="EditSkin">M</asp:LinkButton>
                            <asp:LinkButton ID="Lkb_Delete" runat="server" CssClass="icon delete">X</asp:LinkButton>
                            <asp:LinkButton ID="Lkb_Copy" runat="server" CssClass="icon copy">*C</asp:LinkButton>
                        </span>
                    </td>
                </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:Label ID="Lbl_NoData" CssClass="No_Data" runat="server"></asp:Label>


<asp:Panel ID="Pnl_Test" runat="server" Visible="false" CssClass="test">
    <asp:Label ID="Lbl_TestCom_t" runat="server" CssClass="Titolo_Campo">#Community ID: </asp:Label>
    <asp:TextBox ID="Txb_TestComId" runat="server" CssClass="Testo_campo"></asp:TextBox>
    <br />
    <asp:Label ID="Lbl_TestOrgn_t" runat="server" CssClass="Titolo_Campo">#Organization ID: </asp:Label>
    <asp:TextBox ID="Txb_TestOrgnId" runat="server" CssClass="Testo_campo"></asp:TextBox>
    <br />
    <asp:Label ID="Lbl_TestLang_t" runat="server" CssClass="Titolo_Campo">#Codice lingua: </asp:Label>
    <asp:DropDownList ID="DDL_TestLang" runat="server" CssClass="Testo_campo">
        <asp:ListItem Value="it-IT" Selected="True"></asp:ListItem>
        <asp:ListItem Value="en-US"></asp:ListItem>
        <asp:ListItem Value="de-DE"></asp:ListItem>
        <asp:ListItem Value="fr-FR"></asp:ListItem>
        <asp:ListItem Value="es-ES"></asp:ListItem>
    </asp:DropDownList>
    <br />

    <asp:LinkButton ID="Lkb_TEST" runat="server" CssClass="Link_Menu">TEST</asp:LinkButton>
    <asp:LinkButton ID="Lkb_ClearTest" runat="server" CssClass="Link_Menu">CLEAR CACHE</asp:LinkButton>
    
    <br /><br />
    <asp:Label ID="Lbl_Time_t" runat="server" CssClass="Titolo_Campo">#Get and render time:</asp:Label>
    <asp:Label ID="Lbl_Time" runat="server" CssClass="Testo_campo"></asp:Label>
    <br />    <hr />
    <asp:Label ID="Lbl_MainLogo_t" runat="server" CssClass="Titolo_Campo">#Main logo:</asp:Label>
    <br />
    <asp:Label ID="Lbl_MainLogo" runat="server" CssClass="Testo_campo"></asp:Label>
    <%--<asp:Literal ID="Lit1" runat="server" CssClass="Testo_campo"></asp:Literal>--%>
    <hr />
    <asp:Label ID="Lbl_FooterLogos_t" runat="server" CssClass="Titolo_Campo">#Footer logos:</asp:Label>
    <br />
    <ul class="Logos">
        <%--<asp:Label ID="Lbl_FooterLogos" runat="server" CssClass="Testo_campo"></asp:Label>--%>
        <asp:Literal ID="Lit_FooterLogos" runat="server"></asp:Literal>
    </ul>
    <%----%>
    <hr />
    <asp:Label ID="Lbl_FooterText_t" runat="server" CssClass="Titolo_Campo">#Footer text:</asp:Label>
    <br />
    <div class="renderedtext">
        <asp:Label ID="Lbl_FooterText" runat="server" CssClass="Testo_campo"></asp:Label>
    </div>
    <%--<asp:Literal ID="Lit3" runat="server" CssClass="Testo_campo"></asp:Literal>--%>
    <hr />
    <asp:Label ID="Lbl_Styles_t" runat="server" CssClass="Titolo_Campo">#Style code:</asp:Label>
    <br />
    <asp:Label ID="Lbl_Styles" runat="server" CssClass="Testo_campo"></asp:Label>
    <%--<asp:Literal ID="Lit4" runat="server" CssClass="Testo_campo"></asp:Literal>--%>
    <br />
    <asp:HyperLink ID="Hyp_Preview" runat="server" CssClass="Link_Menu" Visible="false" Target="_blank">#Preview</asp:HyperLink>
</asp:Panel>
</div>