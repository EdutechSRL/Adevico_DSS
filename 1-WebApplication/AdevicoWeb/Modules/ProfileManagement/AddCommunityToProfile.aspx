<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="AddCommunityToProfile.aspx.vb" Inherits="Comunita_OnLine.AddCommunityToProfile" %>

<%@ Register TagPrefix="CTRL" TagName="CommunitiesTree" Src="./UC/UC_CommunitiesTree.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Subscriptions" Src="./UC/UC_CommunitySubscriptions.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Unsubscriptions" Src="./UC/UC_CommunityUnsubscription.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Template/Wizard/css/wizard.css?v=201604071200lm" type="text/css" rel="stylesheet" />
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201604071200lm" rel="Stylesheet" type="text/css" />
    <style type="text/css">
    .rtChk
    {
        font-size: 18px;
    }

    /* Override Wizard */
    /*div#Wizard div.StepContent div.StepData .Titolo_campo { width: 200px; }*/
</style>
    <script language="javascript" type="text/javascript">
    function UpdateAllChildren(nodes, checked) {
        var i;
        for (i = 0; i < nodes.get_count(); i++) {
            if (checked) {
                nodes.getNode(i).check();
            }
            else {
                nodes.getNode(i).set_checked(false);
            }

            if (nodes.getNode(i).get_nodes().get_count() > 0) {
                UpdateAllChildren(nodes.getNode(i).get_nodes(), checked);
            }
        }
    }

    function CheckChildNodesImport(sender, eventArgs) {
        var node = eventArgs.get_node();
        var childNodes = eventArgs.get_node().get_nodes();
        var isChecked = eventArgs.get_node().get_checked();
        UpdateAllChildren(childNodes, isChecked);


        if (!node.get_checked()) {
            while (node.get_parent().set_checked != null) {
                node.get_parent().set_checked(false);
                node = node.get_parent();
            }
        }
        //UpdateAllChildrenImport(node.Nodes, node.Checked);
    }
			
    </script>
    <style type="text/css" type="text/css">
        div.stepdata
        {
            padding: 15px 15px 15px 15px;
        }
        /*div#Wizard div.StepContent div.StepData .Titolo_campo
        {
            display: inline-block;
            min-width: 150px;
            width: 150px;
        }
        
        div#Wizard div.StepContent div.StepData .Testo_Campo
        {
            clear: right;
            }
        div.StepContent
        {
            min-height: 250px;
        }
            
        span.Fieldrow
        {
            display:block;
            clear: both;
        }
        
        div#Wizard div.StepContent div.StepData span.full
        {
            width: 100%
            }
            
        div#Wizard div.StepContent div.StepData label.large
        {
            min-width: 300px;
        }
            */
      /*div#wizard div.StepContent span.Fieldrow, 
        div#wizard div.StepContent div.StepData .Titolo_campo { vertical-align: baseline; }
        div#wizard div.StepContent span.Fieldrow .Titolo_campo { width: auto; }
        div#wizard div.StepContent .RadTreeView input { border: 0; }*/
</style>
    <script language="javascript" type="text/javascript">
        function AfterCheckHandler(sender, eventArgs) {
            var tree = $find("<%=TreeViewClientID %>");
            var nodes = tree.get_allNodes();
            var check = false;
            var node = eventArgs.get_node();
            if (node.get_checked()) {
                check = true;
            }
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].get_checked() != check && nodes[i] != node && nodes[i].get_value() == node.get_value()) {
                    (check) ? nodes[i].check() : nodes[i].uncheck();
                }
            }

            //tree.UpdateState();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="middle-content">
        <div id="data_content">
            <div class="button" id="DVmenu" runat="server">
                <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="linkMenu" Text="Back" Height="18px" CausesValidation="false"></asp:HyperLink>
            </div>
        <asp:MultiView ID="MLVprofiles" runat="server" ActiveViewIndex="1">
            <asp:View ID="VIWdefault" runat="server">
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="LBmessage" runat="server"></asp:Label>
                <br />
                <br />
                <br />
                <br />
                <br />
            </asp:View>
            <asp:View ID="VIWedit" runat="server">
                <div id="wizard">
                    <div class="wiz_header">
                        <div class="wiz_top_nav">
                            <div class="stepButton">
                                <asp:Button ID="BTNbackTop" runat="server" Text="Back" Visible="false" />
                                <asp:Button ID="BTNnextTop" runat="server" Text="Next" CausesValidation="true" />
                                <asp:Button ID="BTNcompleteTop" runat="server" Text="Next" Visible="false" />
                            </div>
                        </div>
                        <div class="wiz_top_info ">
                            <div class="wiz_top_desc clearfix">
                                <h2>
                                    <asp:Label ID="LBstepTitle" runat="server" CssClass="Titolo_Campo"></asp:Label>
                                </h2>
                                <asp:Label ID="LBstepDescription" runat="server" CssClass="Testo_Campo"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="wiz_content">
                        <div class="stepdata">
                            <asp:MultiView ID="MLVwizard" runat="server" ActiveViewIndex="0">
                                <asp:View ID="VIWcommunities" runat="server">
                                    <div style="text-align: left;">
                                        <CTRL:CommunitiesTree ID="CTRLcommunities" runat="server"></CTRL:CommunitiesTree>
                                    </div>
                                </asp:View>
                                <asp:View ID="VIWsubscriptionsSettings" runat="server">
                                    <CTRL:Subscriptions ID="CTRLsubscriptions" runat="server"></CTRL:Subscriptions>
                                </asp:View>
                                <asp:View ID="VIWremoveSubscription" runat="server">
                                    <CTRL:Unsubscriptions ID="CTRLunsubscriptions" runat="server"></CTRL:Unsubscriptions>
                                </asp:View>
                                <asp:View ID="VIWcomplete" runat="server">
                                    <div>
                                        <asp:Label ID="LBcommunityToSubscribe" runat="server" CssClass="Testo_Campo" Visible="false"></asp:Label>
                                        <asp:Label ID="LBsubscriptionsEdited" runat="server" CssClass="Testo_Campo" Visible="false"></asp:Label>
                                        <asp:Label ID="LBsubscriptionsToDelete" runat="server" CssClass="Testo_Campo" Visible="false"></asp:Label>
                                    </div>
                                </asp:View>
                                <asp:View ID="VIWerror" runat="server">
                                    <span class="LIT_Error">
                                        <asp:Literal ID="LTerrors" runat="server"></asp:Literal>
                                    </span>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>
                    <div class="wiz_bot_nav clearfix">
                        <div class="stepButton">
                            <asp:Button ID="BTNbackBottom" runat="server" Text="Back" Visible="false" />
                            <asp:Button ID="BTNnextBottom" runat="server" Text="Next" CausesValidation="true" />
                            <asp:Button ID="BTNcompleteBottom" runat="server" Text="Next" Visible="false" OnClientClick="$('#DVprogress').show();return true;" />
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
        </div>
    </div>
    <div id="DVprogress" style="display: none;">
        <div id="progressBackgroundFilter">
        </div>
        <div id="processMessage"> <asp:Literal ID="LTprogress" runat="server"/> <br /> <asp:Image
        ID="imgLoading" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" /> </div>
    </div>
</asp:Content>