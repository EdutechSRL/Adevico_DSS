<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Edit.aspx.vb" Inherits="Comunita_OnLine.EditSkin" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="CtrlSkinMainLogo" Src="./UC/UC_SkinHeadLogo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinFooterLogo" Src="./UC/UC_SkinFootLogo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinFooterText" Src="./UC/UC_SkinFootText.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CtrlSkinTemplate" Src="./UC/UC_SkinTemplate.ascx" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="SkinManagement.css" />
<style type="text/css">
    
    /* div#cFooter */

    div.template input, div.template label { display: inline-block;}

    .item
    {
        display: inline-block; width: 270px; height: 100px;
        border: 1px solid black;
        vertical-align: top;
        /*margin-left: 1.5em;*/
        margin-bottom: 1em;
        }

     .item * { vertical-align:top; }

    .item ul { list-style: none; }



    /*        HEADER         */
    /* LEFT TOP*/
    .head_left ul, .head_left a, .head_left img, .head_left h1, .head_left span { margin: 0; padding: 0; border: 0; display: inline-block;}
    /* RIGHT TOP*/
    .head_right ul, .head_right a, .head_right img, .head_right h1, .head_right span { margin: 0; padding: 0; border: 0; display: inline-block;}
    .head_right ul, .head_right a, .head_right img { float: right; text-align: right; }
    .head_right span, .head_right h1 { float: left; }


    /*        FOOTER         */
    /* FLOW */
    .flow * { display: inline; }

    /* 50-50 left*/
    .half_left ul, .half_left span { margin: 0; padding: 0; border: 0; width: 50%; display: inline-block;}

    /* LEFT */
    .left ul, .left span { margin: 0; padding: 0; border: 0; display: inline-block;}

    /* 50-50 right */
    .half_right ul, .half_left span { margin: 0; padding: 0; border: 0; width: 50%; display: inline-block;}
    .half_right ul { float: right; text-align: right; }
    .half_right span { float: left; }

    /* Right */
    .right ul, .right span, .right a, .righ img, .right h1 { margin: 0; padding: 0; border: 0; display: inline-block;}
    .right ul, .right img, .right img { float: right; text-align: right; }
    .right { text-align: right; }
    .right span {  }




    /* Top */
    .top ul, .top span { margin: 0; padding: 0; border: 0; width: 100%; display: inline-block; }
    .top li { display: inline-block; }

    /* Bottom */
    .bottom ul, .bottom span { margin: 0; padding: 0; border: 0; width: 100%; display: inline-block; clear:both;}
    .bottom li { display: inline-block; }
    .bottom ul { position:absolute; bottom:0px; }
    /*.half_bottom span { float: left; clear:both; }*/
    .bottom { position: relative; }

</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPback" runat="server" CssClass="Link_Menu" Text="#Back" />
         <asp:Button ID="BTNsaveSkinName" runat="server" CssClass="Link_Menu"/>
    </div>
     <asp:MultiView ID="MLVeditSkin" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBskinMessage" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWdata" runat="server">
            <br />
            <div>
                <asp:Label ID="LBskinName_t" runat="server" CssClass="Titolo_Campo">#Nome:</asp:Label>
                <asp:TextBox ID="TXBskinName" runat="server" CssClass="Testo_Campo" Columns="100" MaxLength="255"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVskinName" ControlToValidate="TXBskinName" runat="server"
                    SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div id="SkinManagement">
                <div class="tab">
                    <telerik:RadTabStrip ID="TBSskinEdit" runat="server" Align="Justify" Width="100%" Height="20px"
                        CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true">
                        <Tabs>
                            <telerik:RadTab Text="#Logo Principale" Value="SkinView.Css" Visible="false" />
                            <telerik:RadTab Text="#Logo Principale" Value="SkinView.Images" Visible="false"/>
                            <telerik:RadTab Text="#Logo Principale" Value="SkinView.HeaderLogo" Visible="false"/>
                            <telerik:RadTab Text="#Loghi Footer" Value="SkinView.FooterLogos" Visible="false"/>
                            <telerik:RadTab Text="#Testo Footer" Value="SkinView.FooterText" Visible="false"/>
                            <telerik:RadTab Text="#Testo Footer" Value="SkinView.Shares" Visible="false"/>
                            <%--<telerik:RadTab Text="#Template" Value="SkinView.Templates" Visible="false"/>--%>
                        </Tabs>
                    </telerik:RadTabStrip>
                </div>
                <div class="edit_content">
                    <asp:MultiView ID="MLVskins" runat="server">
                        <asp:View ID="VIWcss" runat="server">
                        </asp:View>
                        <asp:View ID="VIWimages" runat="server">
                        </asp:View>
                        <asp:View ID="VIWheaderLogo" runat="server">
                            <CTRL:CtrlSkinMainLogo ID="CTRLmainLogo" runat="server" />
                        </asp:View>
                        <asp:View ID="VIWfooterLogos" runat="server">
                            <CTRL:CtrlSkinFooterLogo ID="CTRLfootLogo" runat="server" />
                        </asp:View>
                        <asp:View ID="VIWfooterText" runat="server">
                            <CTRL:CtrlSkinFooterText ID="CTRLfootText" runat="server" />
                        </asp:View>
                        <asp:View ID="VIWshares" runat="server">
                        </asp:View>
<%--                        <asp:View ID="VIWtemplate" runat="server">
                            <CTRL:CtrlSkinTemplate ID="CTRLtemplate" runat="server" />
                        </asp:View>--%>
                    </asp:MultiView>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>